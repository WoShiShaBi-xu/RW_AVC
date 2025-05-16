using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.Location
{
    public class Location
    {
        /// <summary>
        /// 库位ID
        /// </summary>
        public string LocationId { get; set; }

        /// <summary>
        /// 库位类型(缓存区-待试验/缓存区-已试验/试验区接驳位/成品检测接驳位/护箱备料区/产线接驳位)
        /// </summary>
        public LocationType LocationType { get; set; }

        /// <summary>
        /// 库位名称
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// 是否被占用
        /// </summary>
        public bool? IsOccupied { get; set; }

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
    /// 库位类型枚举
    /// </summary>
    public enum LocationType
    {
        /// <summary>
        /// 缓存区-待试验
        /// </summary>
        BufferWaitingTest,

        /// <summary>
        /// 缓存区-已试验
        /// </summary>
        BufferTested,

        /// <summary>
        /// 试验区接驳位
        /// </summary>
        TestAreaDock,

        /// <summary>
        /// 成品检测接驳位
        /// </summary>
        FinishedProductInspectionDock,

        /// <summary>
        /// 护箱备料区
        /// </summary>
        ProtectiveBoxPreparationArea,

        /// <summary>
        /// 产线接驳位
        /// </summary>
        ProductionLineDock
    }
}
