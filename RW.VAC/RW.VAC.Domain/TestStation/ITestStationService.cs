using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.TestStation
{
    public interface ITestStationService
    {
        /// <summary>
        /// 创建新试验台
        /// </summary>
        /// <param name="stationType">试验台类型</param>
        /// <returns>创建的试验台</returns>
        Task<TestStation> CreateTestStationAsync( StationType stationType );

        /// <summary>
        /// 更新试验台状态
        /// </summary>
        /// <param name="stationId">试验台ID</param>
        /// <param name="status">新状态</param>
        /// <returns>更新结果</returns>
        Task<bool> UpdateStationStatusAsync( string stationId , StationStatus status );

        /// <summary>
        /// 分配产品到试验台
        /// </summary>
        /// <param name="stationId">试验台ID</param>
        /// <param name="productId">产品ID</param>
        /// <returns>分配结果</returns>
        Task<bool> AssignProductToStationAsync( string stationId , string productId );

        /// <summary>
        /// 从试验台移除产品
        /// </summary>
        /// <param name="stationId">试验台ID</param>
        /// <returns>移除结果</returns>
        Task<bool> RemoveProductFromStationAsync( string stationId );

        /// <summary>
        /// 查找可用的试验台
        /// </summary>
        /// <param name="stationType">试验台类型</param>
        /// <returns>可用试验台列表</returns>
        Task<IEnumerable<TestStation>> FindAvailableStationsAsync( StationType stationType );

        /// <summary>
        /// 获取试验台详情
        /// </summary>
        /// <param name="stationId">试验台ID</param>
        /// <returns>试验台详情</returns>
        Task<TestStation> GetStationDetailsAsync( string stationId );
    }
}
