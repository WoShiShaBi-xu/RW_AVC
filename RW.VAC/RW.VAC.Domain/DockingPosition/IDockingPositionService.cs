using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.DockingPosition
{
    public interface IDockingPositionService
    {
        /// <summary>
        /// 获取所有接驳位
        /// </summary>
        /// <returns>接驳位列表</returns>
        Task<IEnumerable<DockingPosition>> GetAllDockingPositionsAsync( );
        /// <summary>
        /// 创建新接驳位
        /// </summary>
        /// <param name="positionType">接驳位类型</param>
        /// <param name="stationId">关联的试验台ID</param>
        /// <returns>创建的接驳位</returns>
        Task<DockingPosition> CreateDockingPositionAsync( string positionType , string stationId );

        /// <summary>
        /// 更新接驳位状态
        /// </summary>
        /// <param name="positionId">接驳位ID</param>
        /// <param name="status">新状态</param>
        /// <returns>更新结果</returns>
        Task<bool> UpdatePositionStatusAsync( string positionId , PositionStatus status );

        /// <summary>
        /// 分配托盘到接驳位
        /// </summary>
        /// <param name="positionId">接驳位ID</param>
        /// <param name="palletId">托盘ID</param>
        /// <returns>分配结果</returns>
        Task<bool> AssignPalletToPositionAsync( string positionId , string palletId );

        /// <summary>
        /// 从接驳位移除托盘
        /// </summary>
        /// <param name="positionId">接驳位ID</param>
        /// <returns>移除结果</returns>
        Task<bool> RemovePalletFromPositionAsync( string positionId );

        /// <summary>
        /// 查找可用的接驳位
        /// </summary>
        /// <param name="positionType">接驳位类型</param>
        /// <returns>可用接驳位列表</returns>
        Task<IEnumerable<DockingPosition>> FindAvailablePositionsAsync( string positionType );

        /// <summary>
        /// 获取接驳位详情
        /// </summary>
        /// <param name="positionId">接驳位ID</param>
        /// <returns>接驳位详情</returns>
        Task<DockingPosition> GetPositionDetailsAsync( string positionId );
    }
}
