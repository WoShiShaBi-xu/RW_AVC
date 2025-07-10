using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.DockingPosition
{
    [Table( Name = "DockingPositions" )]
    public class DockingPosition
    {
        /// <summary>
        /// 接驳位ID
        /// </summary>
        [Column( IsPrimary = true , StringLength = 20 )]
        public string PositionId { get; set; }

        /// <summary>
        /// 接驳位类型(试验区/产线/成品检测区)
        /// </summary>
        [Column( StringLength = 50 )]
        public string PositionType { get; set; }

        /// <summary>
        /// 关联的试验台ID
        /// </summary>
        [Column( StringLength = 20 )]
        public string StationId { get; set; }

        /// <summary>
        /// 接驳位状态(空闲/有料)
        /// </summary>
        [Column( MapType = typeof( string ) )]
        public DockingPositionStatus Status { get; set; }

        /// <summary>
        /// 当前托盘ID
        /// </summary>
        [Column( StringLength = 20 )]
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
    /// <summary>
    /// 接驳位状态枚举
    /// </summary>
    public enum DockingPositionStatus
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
