using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.TestStation
{
    public class TestStation
    {
        /// <summary>
        /// 试验台ID
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 试验台类型(制动装置/辅助装置)
        /// </summary>
        public StationType StationType { get; set; }

        /// <summary>
        /// 试验台状态(空闲/试验中/故障)
        /// </summary>
        public StationStatus Status { get; set; }

        /// <summary>
        /// 当前产品ID
        /// </summary>
        public string CurrentProductId { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastUpdate { get; set; }
    }

    /// <summary>
    /// 试验台类型枚举
    /// </summary>
    public enum StationType
    {
        /// <summary>
        /// 制动装置
        /// </summary>
        BrakeDevice,

        /// <summary>
        /// 辅助装置
        /// </summary>
        AuxiliaryDevice
    }

    /// <summary>
    /// 试验台状态枚举
    /// </summary>
    public enum StationStatus
    {
        /// <summary>
        /// 空闲
        /// </summary>
        Idle,

        /// <summary>
        /// 试验中
        /// </summary>
        Testing,

        /// <summary>
        /// 故障
        /// </summary>
        Fault
    }
}
