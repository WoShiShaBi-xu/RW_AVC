using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.DockingPosition
{
    public class DockingPosition
    {
        /// <summary>
        /// 接驳位ID
        /// </summary>
        public string PositionId { get; set; }

        /// <summary>
        /// 接驳位类型(试验区/产线/成品检测区)
        /// </summary>
        public string PositionType { get; set; }

        /// <summary>
        /// 关联的试验台ID
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 接驳位状态(空闲/有料)
        /// </summary>
        public PositionStatus Status { get; set; }

        /// <summary>
        /// 当前托盘ID
        /// </summary>
        public string CurrentPalletId { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastUpdate { get; set; }
    }

    /// <summary>
    /// 接驳位状态枚举
    /// </summary>
    public enum PositionStatus
    {
        /// <summary>
        /// 空闲
        /// </summary>
        空闲,

        /// <summary>
        /// 有料
        /// </summary>
        有料
    }
}
