using RW.VAC.Domain.DockingPosition;
using RW.VAC.Domain.Pallet;
using RW.VAC.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Application.Services.DockingPositionService
{
    public class DockingPositionService : IDockingPositionService
    {
        private readonly IDockingPositionRepository _dockingPositionRepository;
        private readonly IPalletRepository _palletRepository;
        private readonly IProductRepository _productRepository;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dockingPositionRepository">接驳位仓储</param>
        /// <param name="palletRepository">托盘仓储</param>
        public DockingPositionService(
            IDockingPositionRepository dockingPositionRepository ,
            IPalletRepository palletRepository ,
            IProductRepository productRepository )
        {
            _dockingPositionRepository = dockingPositionRepository ?? throw new ArgumentNullException( nameof( dockingPositionRepository ) );
            _palletRepository = palletRepository ?? throw new ArgumentNullException( nameof( palletRepository ) );
            _productRepository = productRepository ?? throw new ArgumentNullException( nameof( productRepository ) );
        }


        /// <summary>
        /// 获取所有接驳位
        /// </summary>
        /// <returns>接驳位列表</returns>
        public async Task<IEnumerable<DockingPosition>> GetAllDockingPositionsAsync( )
        {
            // 调用仓储接口获取所有接驳位
            return await _dockingPositionRepository.GetAllAsync();
        }

        /// <summary>
        /// 创建新接驳位
        /// </summary>
        /// <param name="positionType">接驳位类型</param>
        /// <param name="stationId">关联的试验台ID</param>
        /// <returns>创建的接驳位</returns>
        public async Task<DockingPosition> CreateDockingPositionAsync( string positionType , string stationId )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( positionType ))
            {
                throw new ArgumentException( "接驳位类型不能为空" , nameof( positionType ) );
            }

            // 创建接驳位实体
            var dockingPosition = new DockingPosition
            {
                PositionId = GeneratePositionId( positionType ) ,
                PositionType = positionType ,
                StationId = stationId ,
                Status = DockingPositionStatus.空闲 ,
                CurrentPalletId = null ,
                LastUpdate = DateTime.Now
            };

            // 保存到数据库
            await _dockingPositionRepository.AddAsync( dockingPosition );

            return dockingPosition;
        }
        /// <summary>
        /// 从接驳位移除产品
        /// </summary>
        /// <param name="positionId">接驳位ID</param>
        /// <returns>移除结果</returns>
        public async Task<bool> RemoveProductFromPositionAsync( string positionId )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( positionId ))
            {
                throw new ArgumentException( "接驳位ID不能为空" , nameof( positionId ) );
            }

            // 获取接驳位
            var position = await _dockingPositionRepository.GetByIdAsync( positionId );
            if (position == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{positionId}的接驳位" );
            }

            // 如果接驳位没有产品，不需要处理
            if (string.IsNullOrEmpty( position.CurrentProductld ))
            {
                return true;
            }

            // 获取产品
            var productId = position.CurrentProductld;
            var product = await _productRepository.GetByIdAsync( productId );

            // 更新接驳位状态
            position.CurrentProductld = null;
            position.LastUpdate = DateTime.Now;

            // 如果找到产品，更新产品位置
            if (product != null && product is ILocationTrackable trackableProduct && trackableProduct.LocationId == positionId)
            {
                trackableProduct.LocationId = null;
                trackableProduct.LastUpdate = DateTime.Now;
                await _productRepository.UpdateAsync( product );
            }

            // 保存到数据库
            return await _dockingPositionRepository.UpdateAsync( position );
        }
        /// <summary>
        /// 更新接驳位信息
        /// </summary>
        /// <param name="positionId">接驳位ID</param>
        /// <param name="positionType">接驳位类型</param>
        /// <param name="stationId">关联的试验台ID</param>
        /// <returns>更新结果</returns>
        public async Task<bool> UpdateDockingPositionAsync( string positionId , string positionType , string stationId )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( positionId ))
            {
                throw new ArgumentException( "接驳位ID不能为空" , nameof( positionId ) );
            }

            if (string.IsNullOrWhiteSpace( positionType ))
            {
                throw new ArgumentException( "接驳位类型不能为空" , nameof( positionType ) );
            }

            // 获取接驳位
            var position = await _dockingPositionRepository.GetByIdAsync( positionId );
            if (position == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{positionId}的接驳位" );
            }

            // 更新接驳位信息
            position.PositionType = positionType;
            position.StationId = stationId;
            position.LastUpdate = DateTime.Now;

            // 保存到数据库
            return await _dockingPositionRepository.UpdateAsync( position );
        }

        /// <summary>
        /// 删除接驳位
        /// </summary>
        /// <param name="positionId">接驳位ID</param>
        /// <returns>删除结果</returns>
        public async Task<bool> DeleteDockingPositionAsync( string positionId )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( positionId ))
            {
                throw new ArgumentException( "接驳位ID不能为空" , nameof( positionId ) );
            }

            // 获取接驳位
            var position = await _dockingPositionRepository.GetByIdAsync( positionId );
            if (position == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{positionId}的接驳位" );
            }

            // 检查是否可以删除
            if (!await CanDeletePositionAsync( positionId ))
            {
                throw new InvalidOperationException( "接驳位当前被占用，无法删除" );
            }

            // 删除接驳位
            return await _dockingPositionRepository.DeleteAsync( positionId );
        }

        /// <summary>
        /// 检查接驳位是否可以删除
        /// </summary>
        /// <param name="positionId">接驳位ID</param>
        /// <returns>是否可以删除</returns>
        public async Task<bool> CanDeletePositionAsync( string positionId )
        {
            if (string.IsNullOrWhiteSpace( positionId ))
            {
                return false;
            }

            var position = await _dockingPositionRepository.GetByIdAsync( positionId );
            if (position == null)
            {
                return false;
            }

            // 只有空闲状态的接驳位才能删除
            return position.Status == DockingPositionStatus.空闲 && string.IsNullOrEmpty( position.CurrentPalletId );
        }

        /// <summary>
        /// 根据试验台ID获取关联的接驳位
        /// </summary>
        /// <param name="stationId">试验台ID</param>
        /// <returns>关联的接驳位列表</returns>
        public async Task<IEnumerable<DockingPosition>> GetPositionsByStationIdAsync( string stationId )
        {
            if (string.IsNullOrWhiteSpace( stationId ))
            {
                return Enumerable.Empty<DockingPosition>();
            }

            var allPositions = await _dockingPositionRepository.GetAllAsync();
            return allPositions.Where( p => p.StationId == stationId );
        }

        /// <summary>
        /// 批量更新接驳位状态
        /// </summary>
        /// <param name="positionIds">接驳位ID列表</param>
        /// <param name="status">新状态</param>
        /// <returns>更新结果</returns>
        public async Task<bool> BatchUpdatePositionStatusAsync( IEnumerable<string> positionIds , DockingPositionStatus status )
        {
            if (positionIds == null || !positionIds.Any())
            {
                return true;
            }

            var results = new List<bool>();
            foreach (var positionId in positionIds)
            {
                try
                {
                    var result = await UpdatePositionStatusAsync( positionId , status );
                    results.Add( result );
                }
                catch
                {
                    results.Add( false );
                }
            }

            return results.All( r => r );
        }

        /// <summary>
        /// 更新接驳位状态
        /// </summary>
        /// <param name="positionId">接驳位ID</param>
        /// <param name="status">新状态</param>
        /// <returns>更新结果</returns>
        public async Task<bool> UpdatePositionStatusAsync( string positionId , DockingPositionStatus status )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( positionId ))
            {
                throw new ArgumentException( "接驳位ID不能为空" , nameof( positionId ) );
            }

            // 获取接驳位
            var position = await _dockingPositionRepository.GetByIdAsync( positionId );
            if (position == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{positionId}的接驳位" );
            }

            // 如果状态相同，不需要更新
            if (position.Status == status)
            {
                return true;
            }

            // 如果设置为空闲，需要清除当前托盘ID
            if (status == DockingPositionStatus.空闲)
            {
                position.CurrentPalletId = null;
            }
            else if (status == DockingPositionStatus.有料 && string.IsNullOrEmpty( position.CurrentPalletId ))
            {
                // 如果设置为占用但没有托盘ID，抛出异常
                throw new InvalidOperationException( "设置接驳位为占用状态时，必须分配托盘" );
            }

            // 更新状态
            position.Status = status;
            position.LastUpdate = DateTime.Now;

            // 保存到数据库
            return await _dockingPositionRepository.UpdateAsync( position );
        }

        /// <summary>
        /// 分配托盘到接驳位
        /// </summary>
        /// <param name="positionId">接驳位ID</param>
        /// <param name="palletId">托盘ID</param>
        /// <returns>分配结果</returns>
        public async Task<bool> AssignPalletToPositionAsync( string positionId , string palletId )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( positionId ))
            {
                throw new ArgumentException( "接驳位ID不能为空" , nameof( positionId ) );
            }

            if (string.IsNullOrWhiteSpace( palletId ))
            {
                throw new ArgumentException( "托盘ID不能为空" , nameof( palletId ) );
            }

            // 获取接驳位
            var position = await _dockingPositionRepository.GetByIdAsync( positionId );
            if (position == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{positionId}的接驳位" );
            }

            // 检查接驳位是否已被占用
            if (position.Status == DockingPositionStatus.有料 && position.CurrentPalletId != palletId)
            {
                throw new InvalidOperationException( $"接驳位{positionId}已被托盘{position.CurrentPalletId}占用" );
            }

            // 获取托盘
            var pallet = await _palletRepository.GetByIdAsync( palletId );
            if (pallet == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{palletId}的托盘" );
            }

            // 更新接驳位状态
            position.Status = DockingPositionStatus.有料;
            position.CurrentPalletId = palletId;
            position.LastUpdate = DateTime.Now;

            // 更新托盘位置信息（如果托盘有LocationId属性来标识位置）
            // 注意：这里假设托盘使用LocationId来标识当前位置，包括接驳位
            // 如果你的设计不同，请根据实际情况调整
            if (pallet.LocationId != positionId)
            {
                pallet.LocationId = positionId; // 将接驳位ID作为托盘的位置
                pallet.LastUpdate = DateTime.Now;
                await _palletRepository.UpdateAsync( pallet );
            }

            // 保存到数据库
            return await _dockingPositionRepository.UpdateAsync( position );
        }

        /// <summary>
        /// 从接驳位移除托盘
        /// </summary>
        /// <param name="positionId">接驳位ID</param>
        /// <returns>移除结果</returns>
        public async Task<bool> RemovePalletFromPositionAsync( string positionId )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( positionId ))
            {
                throw new ArgumentException( "接驳位ID不能为空" , nameof( positionId ) );
            }

            // 获取接驳位
            var position = await _dockingPositionRepository.GetByIdAsync( positionId );
            if (position == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{positionId}的接驳位" );
            }

            // 如果接驳位没有托盘，不需要处理
            if (position.Status != DockingPositionStatus.有料 || string.IsNullOrEmpty( position.CurrentPalletId ))
            {
                return true;
            }

            // 获取托盘
            var palletId = position.CurrentPalletId;
            var pallet = await _palletRepository.GetByIdAsync( palletId );

            // 更新接驳位状态
            position.Status = DockingPositionStatus.空闲;
            position.CurrentPalletId = null;
            position.LastUpdate = DateTime.Now;

            // 如果找到托盘，清除托盘的位置信息
            if (pallet != null && pallet.LocationId == positionId)
            {
                pallet.LocationId = null;
                pallet.LastUpdate = DateTime.Now;
                await _palletRepository.UpdateAsync( pallet );
            }

            // 保存到数据库
            return await _dockingPositionRepository.UpdateAsync( position );
        }

        /// <summary>
        /// 查找可用的接驳位
        /// </summary>
        /// <param name="positionType">接驳位类型</param>
        /// <returns>可用接驳位列表</returns>
        public async Task<IEnumerable<DockingPosition>> FindAvailablePositionsAsync( string positionType )
        {
            // 获取所有指定类型的接驳位
            var positions = await _dockingPositionRepository.GetByTypeAsync( positionType );

            // 过滤出空闲的接驳位
            return positions.Where( p => p.Status == DockingPositionStatus.空闲 );
        }

        /// <summary>
        /// 获取接驳位详情
        /// </summary>
        /// <param name="positionId">接驳位ID</param>
        /// <returns>接驳位详情</returns>
        public async Task<DockingPosition> GetPositionDetailsAsync( string positionId )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( positionId ))
            {
                throw new ArgumentException( "接驳位ID不能为空" , nameof( positionId ) );
            }

            // 获取接驳位
            var position = await _dockingPositionRepository.GetByIdAsync( positionId );
            if (position == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{positionId}的接驳位" );
            }

            return position;
        }

        /// <summary>
        /// 生成接驳位ID
        /// </summary>
        /// <param name="positionType">接驳位类型</param>
        /// <returns>接驳位ID</returns>
        private string GeneratePositionId( string positionType )
        {
            // 生成规则：类型前缀 + 年月日 + 4位随机数
            string prefix;

            // 根据接驳位类型生成不同的前缀
            switch (positionType?.ToLower())
            {
                case "试验区":
                case "试验区接驳位":
                    prefix = "TAD";
                    break;
                case "产线":
                case "产线接驳位":
                    prefix = "PLD";
                    break;
                case "成品检测":
                case "成品检测接驳位":
                    prefix = "FPI";
                    break;
                default:
                    prefix = "DOP";
                    break;
            }

            string dateStr = DateTime.Now.ToString( "yyMMdd" );
            string randomStr = new Random().Next( 1000 , 9999 ).ToString();

            return $"{prefix}{dateStr}{randomStr}";
        }
    }
}