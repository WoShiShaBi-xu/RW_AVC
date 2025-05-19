using RW.VAC.Domain.Location;
using RW.VAC.Domain.Pallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Application.Services.Locations
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IPalletRepository _palletRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="locationRepository">库位仓储</param>
        /// <param name="palletRepository">托盘仓储</param>
        public LocationService( ILocationRepository locationRepository , IPalletRepository palletRepository )
        {
            _locationRepository = locationRepository ?? throw new ArgumentNullException( nameof( locationRepository ) );
            _palletRepository = palletRepository ?? throw new ArgumentNullException( nameof( palletRepository ) );
        }
        /// <summary>
        /// 获取所有库位
        /// </summary>
        /// <returns>库位列表</returns>
        public async Task<IEnumerable<Location>> GetAllLocationsAsync( )
        {
            // 调用仓储接口获取所有库位
            return await _locationRepository.GetAllAsync();
        }
        /// <summary>
        /// 创建新库位
        /// </summary>
        /// <param name="locationType">库位类型</param>
        /// <param name="locationName">库位名称</param>
        /// <returns>创建的库位</returns>
        public async Task<Location> CreateLocationAsync( LocationType locationType , string locationName )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( locationName ))
            {
                throw new ArgumentException( "库位名称不能为空" , nameof( locationName ) );
            }

            // 创建库位实体
            var location = new Location
            {
                LocationId = GenerateLocationId( locationType ) ,
                LocationType = locationType ,
                LocationName = locationName ,
                IsOccupied = false ,
                CurrentPalletId = null ,
                LastUpdate = DateTime.Now
            };

            // 保存到数据库
            await _locationRepository.AddAsync( location );

            return location;
        }

        /// <summary>
        /// 更新库位状态
        /// </summary>
        /// <param name="locationId">库位ID</param>
        /// <param name="isOccupied">是否被占用</param>
        /// <returns>更新结果</returns>
        public async Task<bool> UpdateLocationStatusAsync( string locationId , bool isOccupied )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( locationId ))
            {
                throw new ArgumentException( "库位ID不能为空" , nameof( locationId ) );
            }

            // 获取库位
            var location = await _locationRepository.GetByIdAsync( locationId );
            if (location == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{locationId}的库位" );
            }

            // 如果状态相同，不需要更新
            if (location.IsOccupied == isOccupied)
            {
                return true;
            }

            // 如果设置为未占用，需要清除当前托盘ID
            if (!isOccupied)
            {
                location.CurrentPalletId = null;
            }
            else if (location.CurrentPalletId == null)
            {
                // 如果设置为占用但没有托盘ID，抛出异常
                throw new InvalidOperationException( "设置库位为占用状态时，必须分配托盘" );
            }

            // 更新状态
            location.IsOccupied = isOccupied;
            location.LastUpdate = DateTime.Now;

            // 保存到数据库
            return await _locationRepository.UpdateAsync( location );
        }

        /// <summary>
        /// 分配托盘到库位
        /// </summary>
        /// <param name="locationId">库位ID</param>
        /// <param name="palletId">托盘ID</param>
        /// <returns>分配结果</returns>
        public async Task<bool> AssignPalletToLocationAsync( string locationId , string palletId )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( locationId ))
            {
                throw new ArgumentException( "库位ID不能为空" , nameof( locationId ) );
            }

            if (string.IsNullOrWhiteSpace( palletId ))
            {
                throw new ArgumentException( "托盘ID不能为空" , nameof( palletId ) );
            }

            // 获取库位
            var location = await _locationRepository.GetByIdAsync( locationId );
            if (location == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{locationId}的库位" );
            }

            // 检查库位是否已被占用
            if (location.IsOccupied != true && location.CurrentPalletId != palletId)
            {
                throw new InvalidOperationException( $"库位{locationId}已被托盘{location.CurrentPalletId}占用" );
            }
            
            // 获取托盘
            var pallet = await _palletRepository.GetByIdAsync( palletId );
            if (pallet == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{palletId}的托盘" );
            }

            // 检查托盘当前位置
            if (!string.IsNullOrEmpty( pallet.LocationId ) && pallet.LocationId != locationId)
            {
                // 更新托盘原位置
                var oldLocation = await _locationRepository.GetByIdAsync( pallet.LocationId );
                if (oldLocation != null)
                {
                    oldLocation.IsOccupied = false;
                    oldLocation.CurrentPalletId = null;
                    oldLocation.LastUpdate = DateTime.Now;
                    await _locationRepository.UpdateAsync( oldLocation );
                }
            }

            // 更新库位状态
            location.IsOccupied = true;
            location.CurrentPalletId = palletId;
            location.LastUpdate = DateTime.Now;

            // 更新托盘位置
            pallet.LocationId = locationId;
            pallet.LastUpdate = DateTime.Now;

            // 保存到数据库
            var locationResult = await _locationRepository.UpdateAsync( location );
            var palletResult = await _palletRepository.UpdateAsync( pallet );

            return locationResult && palletResult;
        }

        /// <summary>
        /// 从库位移除托盘
        /// </summary>
        /// <param name="locationId">库位ID</param>
        /// <returns>移除结果</returns>
        public async Task<bool> RemovePalletFromLocationAsync( string locationId )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( locationId ))
            {
                throw new ArgumentException( "库位ID不能为空" , nameof( locationId ) );
            }

            // 获取库位
            var location = await _locationRepository.GetByIdAsync( locationId );
            if (location == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{locationId}的库位" );
            }

            // 如果库位没有托盘，不需要处理
            if (location.IsOccupied != true || string.IsNullOrEmpty( location.CurrentPalletId ))
            {
                return true;
            }

            // 获取托盘
            var palletId = location.CurrentPalletId;
            var pallet = await _palletRepository.GetByIdAsync( palletId );

            // 更新库位状态
            location.IsOccupied = false;
            location.CurrentPalletId = null;
            location.LastUpdate = DateTime.Now;

            // 如果找到托盘，更新托盘位置
            if (pallet != null && pallet.LocationId == locationId)
            {
                pallet.LocationId = null;
                pallet.LastUpdate = DateTime.Now;
                await _palletRepository.UpdateAsync( pallet );
            }

            // 保存到数据库
            return await _locationRepository.UpdateAsync( location );
        }

        /// <summary>
        /// 查找可用的库位
        /// </summary>
        /// <param name="locationType">库位类型</param>
        /// <returns>可用库位列表</returns>
        public async Task<IEnumerable<Location>> FindAvailableLocationsAsync( LocationType locationType )
        {
            // 获取所有指定类型的库位
            var locations = await _locationRepository.GetByTypeAsync( locationType );

            // 过滤出未被占用的库位
            return locations.Where( l => l.IsOccupied != true );
        }

        /// <summary>
        /// 获取库位详情
        /// </summary>
        /// <param name="locationId">库位ID</param>
        /// <returns>库位详情</returns>
        public async Task<Location> GetLocationDetailsAsync( string locationId )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( locationId ))
            {
                throw new ArgumentException( "库位ID不能为空" , nameof( locationId ) );
            }

            // 获取库位
            var location = await _locationRepository.GetByIdAsync( locationId );
            if (location == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{locationId}的库位" );
            }

            return location;
        }

        /// <summary>
        /// 生成库位ID
        /// </summary>
        /// <param name="locationType">库位类型</param>
        /// <returns>库位ID</returns>
        private string GenerateLocationId( LocationType locationType )
        {
            // 生成规则：类型前缀 + 年月日 + 4位随机数
            string prefix;

            switch (locationType)
            {
                case LocationType.BufferWaitingTest:
                    prefix = "BWT";
                    break;
                case LocationType.BufferTested:
                    prefix = "BTD";
                    break;
                case LocationType.TestAreaDock:
                    prefix = "TAD";
                    break;
                case LocationType.FinishedProductInspectionDock:
                    prefix = "FPI";
                    break;
                case LocationType.ProtectiveBoxPreparationArea:
                    prefix = "PBP";
                    break;
                case LocationType.ProductionLineDock:
                    prefix = "PLD";
                    break;
                default:
                    prefix = "LOC";
                    break;
            }

            string dateStr = DateTime.Now.ToString( "yyMMdd" );
            string randomStr = new Random().Next( 1000 , 9999 ).ToString();

            return $"{prefix}{dateStr}{randomStr}";
        }
    }
}
