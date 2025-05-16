using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.TestStation
{
    public interface ITestStationRepository
    {
        /// <summary>
        /// 获取所有试验台
        /// </summary>
        /// <returns>试验台列表</returns>
        Task<IEnumerable<TestStation>> GetAllAsync( );

        /// <summary>
        /// 根据ID获取试验台
        /// </summary>
        /// <param name="stationId">试验台ID</param>
        /// <returns>试验台实体</returns>
        Task<TestStation> GetByIdAsync( string stationId );

        /// <summary>
        /// 根据类型获取试验台
        /// </summary>
        /// <param name="stationType">试验台类型</param>
        /// <returns>试验台列表</returns>
        Task<IEnumerable<TestStation>> GetByTypeAsync( StationType stationType );

        /// <summary>
        /// 根据状态获取试验台
        /// </summary>
        /// <param name="status">试验台状态</param>
        /// <returns>试验台列表</returns>
        Task<IEnumerable<TestStation>> GetByStatusAsync( StationStatus status );

        /// <summary>
        /// 根据产品ID获取试验台
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>试验台实体</returns>
        Task<TestStation> GetByProductIdAsync( string productId );

        /// <summary>
        /// 添加试验台
        /// </summary>
        /// <param name="station">试验台实体</param>
        /// <returns>添加结果</returns>
        Task<bool> AddAsync( TestStation station );

        /// <summary>
        /// 更新试验台
        /// </summary>
        /// <param name="station">试验台实体</param>
        /// <returns>更新结果</returns>
        Task<bool> UpdateAsync( TestStation station );

        /// <summary>
        /// 删除试验台
        /// </summary>
        /// <param name="stationId">试验台ID</param>
        /// <returns>删除结果</returns>
        Task<bool> DeleteAsync( string stationId );
    }
}
