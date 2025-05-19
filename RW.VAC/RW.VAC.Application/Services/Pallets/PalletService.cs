using RW.VAC.Domain.Location;
using RW.VAC.Domain.Pallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Application.Services.Pallets
{
    public class PalletService : IPalletService
    {
        private readonly IPalletRepository _palletRepository;
        private readonly ILocationRepository _locationRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="palletRepository">托盘仓储</param>
        /// <param name="locationRepository">库位仓储</param>
        public PalletService( IPalletRepository palletRepository , ILocationRepository locationRepository )
        {
            _palletRepository = palletRepository ?? throw new ArgumentNullException( nameof( palletRepository ) );
            _locationRepository = locationRepository ?? throw new ArgumentNullException( nameof( locationRepository ) );
        }

        /// <summary>
        /// 获取所有托盘
        /// </summary>
        /// <returns>托盘列表</returns>
        public async Task<IEnumerable<Pallet>> GetAllAsync( )
        {
            // 调用仓储接口获取所有托盘
            return await _palletRepository.GetAllAsync();
        }

        /// <summary>
        /// 创建新托盘
        /// </summary>
        /// <param name="palletType">托盘类型</param>
        /// <returns>创建的托盘</returns>
        public async Task<Pallet> CreatePalletAsync( PalletType palletType )
        {
            // 创建托盘实体
            var pallet = new Pallet
            {
                PalletId = GeneratePalletId( palletType ) ,
                PalletType = palletType ,
                Status = "空闲" ,
                CreateTime = DateTime.Now
            };

            // 保存到数据库
            await _palletRepository.AddAsync( pallet );

            return pallet;
        }

        /// <summary>
        /// 更新托盘状态
        /// </summary>
        /// <param name="palletId">托盘ID</param>
        /// <param name="status">新状态</param>
        /// <returns>更新结果</returns>
        public async Task<bool> UpdatePalletStatusAsync( string palletId , string status )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( palletId ))
            {
                throw new ArgumentException( "托盘ID不能为空" , nameof( palletId ) );
            }

            if (string.IsNullOrWhiteSpace( status ))
            {
                throw new ArgumentException( "状态不能为空" , nameof( status ) );
            }

            // 获取托盘
            var pallet = await _palletRepository.GetByIdAsync( palletId );
            if (pallet == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{palletId}的托盘" );
            }

            // 更新状态
            pallet.Status = status;
            pallet.LastUpdate = DateTime.Now;

            // 保存到数据库
            return await _palletRepository.UpdateAsync( pallet );
        }

        /// <summary>
        /// 更新托盘位置
        /// </summary>
        /// <param name="palletId">托盘ID</param>
        /// <param name="locationId">位置ID</param>
        /// <returns>更新结果</returns>
        public async Task<bool> UpdatePalletLocationAsync( string palletId , string locationId )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( palletId ))
            {
                throw new ArgumentException( "托盘ID不能为空" , nameof( palletId ) );
            }

            if (string.IsNullOrWhiteSpace( locationId ))
            {
                throw new ArgumentException( "位置ID不能为空" , nameof( locationId ) );
            }

            // 获取托盘
            var pallet = await _palletRepository.GetByIdAsync( palletId );
            if (pallet == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{palletId}的托盘" );
            }

            // 获取位置
            var location = await _locationRepository.GetByIdAsync( locationId );
            if (location == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{locationId}的库位" );
            }

            // 检查库位是否被占用
            if (location.IsOccupied == true && location.CurrentPalletId != palletId)
            {
                throw new InvalidOperationException( $"库位{locationId}已被占用" );
            }

            // 如果托盘当前有位置，需要更新之前的位置状态
            if (!string.IsNullOrEmpty( pallet.LocationId ))
            {
                var oldLocation = await _locationRepository.GetByIdAsync( pallet.LocationId );
                if (oldLocation != null)
                {
                    oldLocation.IsOccupied = false;
                    oldLocation.CurrentPalletId = null;
                    oldLocation.LastUpdate = DateTime.Now;
                    await _locationRepository.UpdateAsync( oldLocation );
                }
            }

            // 更新托盘位置
            pallet.LocationId = locationId;
            pallet.LastUpdate = DateTime.Now;

            // 更新库位状态
            location.IsOccupied = true;
            location.CurrentPalletId = palletId;
            location.LastUpdate = DateTime.Now;

            // 保存到数据库
            var palletResult = await _palletRepository.UpdateAsync( pallet );
            var locationResult = await _locationRepository.UpdateAsync( location );

            return palletResult && locationResult;
        }

        /// <summary>
        /// 获取托盘详情
        /// </summary>
        /// <param name="palletId">托盘ID</param>
        /// <returns>托盘详情</returns>
        public async Task<Pallet> GetPalletDetailsAsync( string palletId )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( palletId ))
            {
                throw new ArgumentException( "托盘ID不能为空" , nameof( palletId ) );
            }

            // 获取托盘
            var pallet = await _palletRepository.GetByIdAsync( palletId );
            if (pallet == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{palletId}的托盘" );
            }

            return pallet;
        }

        /// <summary>
        /// 生成托盘ID
        /// </summary>
        /// <param name="palletType">托盘类型</param>
        /// <returns>托盘ID</returns>
        private string GeneratePalletId( PalletType palletType )
        {
            // 生成规则：类型前缀 + 年月日 + 6位随机数
            string prefix = palletType == PalletType.BrakeDevicePallet ? "BDP" : "ADP";
            string dateStr = DateTime.Now.ToString( "yyyyMMdd" );
            string randomStr = new Random().Next( 100000 , 999999 ).ToString();

            return $"{prefix}{dateStr}{randomStr}";
        }



        /// <summary>
        /// 获取所有托盘
        /// </summary>
        /// <returns>托盘列表</returns>

    }
}