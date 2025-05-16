using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.DockingPosition
{
    public interface IDockingPositionRepository
    {
        /// <summary>
        /// 获取所有接驳位
        /// </summary>
        /// <returns>接驳位列表</returns>
        Task<IEnumerable<DockingPosition>> GetAllAsync( );

        /// <summary>
        /// 根据ID获取接驳位
        /// </summary>
        /// <param name="positionId">接驳位ID</param>
        /// <returns>接驳位实体</returns>
        Task<DockingPosition> GetByIdAsync( string positionId );

        /// <summary>
        /// 根据类型获取接驳位
        /// </summary>
        /// <param name="positionType">接驳位类型</param>
        /// <returns>接驳位列表</returns>
        Task<IEnumerable<DockingPosition>> GetByTypeAsync( string positionType );

        /// <summary>
        /// 根据状态获取接驳位
        /// </summary>
        /// <param name="status">接驳位状态</param>
        /// <returns>接驳位列表</returns>
        Task<IEnumerable<DockingPosition>> GetByStatusAsync( PositionStatus status );

        /// <summary>
        /// 根据试验台ID获取接驳位
        /// </summary>
        /// <param name="stationId">试验台ID</param>
        /// <returns>接驳位列表</returns>
        Task<IEnumerable<DockingPosition>> GetByStationIdAsync( string stationId );

        /// <summary>
        /// 根据托盘ID获取接驳位
        /// </summary>
        /// <param name="palletId">托盘ID</param>
        /// <returns>接驳位实体</returns>
        Task<DockingPosition> GetByPalletIdAsync( string palletId );

        /// <summary>
        /// 添加接驳位
        /// </summary>
        /// <param name="position">接驳位实体</param>
        /// <returns>添加结果</returns>
        Task<bool> AddAsync( DockingPosition position );

        /// <summary>
        /// 更新接驳位
        /// </summary>
        /// <param name="position">接驳位实体</param>
        /// <returns>更新结果</returns>
        Task<bool> UpdateAsync( DockingPosition position );

        /// <summary>
        /// 删除接驳位
        /// </summary>
        /// <param name="positionId">接驳位ID</param>
        /// <returns>删除结果</returns>
        Task<bool> DeleteAsync( string positionId );
    }
}
