using RW.VAC.Domain.Pallet;
using RW.VAC.Domain.Products;
using RW.VAC.Domain.TestStation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Application.Services.TasksDo
{
    public class TestStationService : ITestStationService
    {
        private readonly ITestStationRepository _stationRepository;
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stationRepository">试验台仓储</param>
        /// <param name="productRepository">产品仓储</param>
        public TestStationService( ITestStationRepository stationRepository , IProductRepository productRepository )
        {
            _stationRepository = stationRepository ?? throw new ArgumentNullException( nameof( stationRepository ) );
            _productRepository = productRepository ?? throw new ArgumentNullException( nameof( productRepository ) );
        }

        /// <summary>
        /// 创建新试验台
        /// </summary>
        /// <param name="stationType">试验台类型</param>
        /// <returns>创建的试验台</returns>
        public async Task<TestStation> CreateTestStationAsync( StationType stationType )
        {
            // 创建试验台实体
            var station = new TestStation
            {
                StationId = GenerateStationId( stationType ) ,
                StationType = stationType ,
                Status = StationStatus.Idle ,
                CurrentProductId = null ,
                LastUpdate = DateTime.Now
            };

            // 保存到数据库
            await _stationRepository.AddAsync( station );

            return station;
        }

        /// <summary>
        /// 更新试验台状态
        /// </summary>
        /// <param name="stationId">试验台ID</param>
        /// <param name="status">新状态</param>
        /// <returns>更新结果</returns>
        public async Task<bool> UpdateStationStatusAsync( string stationId , StationStatus status )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( stationId ))
            {
                throw new ArgumentException( "试验台ID不能为空" , nameof( stationId ) );
            }

            // 获取试验台
            var station = await _stationRepository.GetByIdAsync( stationId );
            if (station == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{stationId}的试验台" );
            }

            // 如果状态相同，不需要更新
            if (station.Status == status)
            {
                return true;
            }

            // 如果设置为空闲，需要清除当前产品ID
            if (status == StationStatus.Idle)
            {
                station.CurrentProductId = null;
            }
            else if (status == StationStatus.Testing && string.IsNullOrEmpty( station.CurrentProductId ))
            {
                // 如果设置为试验中但没有产品ID，抛出异常
                throw new InvalidOperationException( "设置试验台为试验中状态时，必须分配产品" );
            }

            // 更新状态
            station.Status = status;
            station.LastUpdate = DateTime.Now;

            // 保存到数据库
            return await _stationRepository.UpdateAsync( station );
        }

        /// <summary>
        /// 分配产品到试验台
        /// </summary>
        /// <param name="stationId">试验台ID</param>
        /// <param name="productId">产品ID</param>
        /// <returns>分配结果</returns>
        public async Task<bool> AssignProductToStationAsync( string stationId , string productId )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( stationId ))
            {
                throw new ArgumentException( "试验台ID不能为空" , nameof( stationId ) );
            }

            if (string.IsNullOrWhiteSpace( productId ))
            {
                throw new ArgumentException( "产品ID不能为空" , nameof( productId ) );
            }

            // 获取试验台
            var station = await _stationRepository.GetByIdAsync( stationId );
            if (station == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{stationId}的试验台" );
            }

            // 检查试验台是否已被占用
            if (station.Status != StationStatus.Idle)
            {
                throw new InvalidOperationException( $"试验台{stationId}不是空闲状态，无法分配产品" );
            }

            // 获取产品
            var product = await _productRepository.GetByIdAsync( productId );
            if (product == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{productId}的产品" );
            }

            // 检查产品和试验台类型是否匹配
            bool isTypeMatched = (product.ProductType == ProductType.BrakeDevice && station.StationType == StationType.BrakeDevice) ||
                                (product.ProductType == ProductType.AuxiliaryDevice && station.StationType == StationType.AuxiliaryDevice);
            if (!isTypeMatched)
            {
                throw new InvalidOperationException( "产品类型与试验台类型不匹配" );
            }

            // 检查产品是否已在其他试验台
            var otherStations = await _stationRepository.GetByProductIdAsync( productId );
            if (otherStations != null)
            {
                throw new InvalidOperationException( $"产品{productId}已经分配到试验台{otherStations.StationId}" );
            }

            // 更新试验台状态
            station.Status = StationStatus.Testing;
            station.CurrentProductId = productId;
            station.LastUpdate = DateTime.Now;

            // 更新产品状态
            product.Status = "待试验";
            product.UpdateTime = DateTime.Now;

            // 保存到数据库
            var stationResult = await _stationRepository.UpdateAsync( station );
            var productResult = await _productRepository.UpdateAsync( product );

            return stationResult && productResult;
        }

        /// <summary>
        /// 从试验台移除产品
        /// </summary>
        /// <param name="stationId">试验台ID</param>
        /// <returns>移除结果</returns>
        public async Task<bool> RemoveProductFromStationAsync( string stationId )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( stationId ))
            {
                throw new ArgumentException( "试验台ID不能为空" , nameof( stationId ) );
            }

            // 获取试验台
            var station = await _stationRepository.GetByIdAsync( stationId );
            if (station == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{stationId}的试验台" );
            }

            // 如果试验台没有产品，不需要处理
            if (string.IsNullOrEmpty( station.CurrentProductId ))
            {
                return true;
            }

            // 获取产品
            var productId = station.CurrentProductId;
            var product = await _productRepository.GetByIdAsync( productId );

            // 更新试验台状态
            station.Status = StationStatus.Idle;
            station.CurrentProductId = null;
            station.LastUpdate = DateTime.Now;

            // 如果找到产品，更新产品状态
            if (product != null)
            {
                product.Status = "已试验";
                product.UpdateTime = DateTime.Now;
                await _productRepository.UpdateAsync( product );
            }

            // 保存到数据库
            return await _stationRepository.UpdateAsync( station );
        }

        /// <summary>
        /// 查找可用的试验台
        /// </summary>
        /// <param name="stationType">试验台类型</param>
        /// <returns>可用试验台列表</returns>
        public async Task<IEnumerable<TestStation>> FindAvailableStationsAsync( StationType stationType )
        {
            // 获取所有指定类型的试验台
            var stations = await _stationRepository.GetByTypeAsync( stationType );

            // 过滤出空闲的试验台
            return stations.Where( s => s.Status == StationStatus.Idle );
        }

        /// <summary>
        /// 获取试验台详情
        /// </summary>
        /// <param name="stationId">试验台ID</param>
        /// <returns>试验台详情</returns>
        public async Task<TestStation> GetStationDetailsAsync( string stationId )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( stationId ))
            {
                throw new ArgumentException( "试验台ID不能为空" , nameof( stationId ) );
            }

            // 获取试验台
            var station = await _stationRepository.GetByIdAsync( stationId );
            if (station == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{stationId}的试验台" );
            }

            return station;
        }

        /// <summary>
        /// 生成试验台ID
        /// </summary>
        /// <param name="stationType">试验台类型</param>
        /// <returns>试验台ID</returns>
        private string GenerateStationId( StationType stationType )
        {
            // 生成规则：类型前缀 + 年月日 + 3位随机数
            string prefix = stationType == StationType.BrakeDevice ? "BDS" : "ADS";
            string dateStr = DateTime.Now.ToString( "yyMMdd" );
            string randomStr = new Random().Next( 100 , 999 ).ToString();

            return $"{prefix}{dateStr}{randomStr}";
        }

        public Task<IEnumerable<TestStation>> GetAllAsync( )
        {
            return _stationRepository.GetAllAsync();
        }
    }
}
