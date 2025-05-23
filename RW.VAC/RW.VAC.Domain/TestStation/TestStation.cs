using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.TestStation
{
    [Table( Name = "TestStations" )]
    public class TestStation
    {
        /// <summary>
        /// 试验台ID
        /// </summary>
        [Column( IsPrimary = true , StringLength = 20 )]
        public string StationId { get; set; }

        /// <summary>
        /// 试验台类型(制动装置/辅助装置)
        /// </summary>
        [Column( MapType = typeof( string ) )]
        public StationType StationType { get; set; }

        /// <summary>
        /// 试验台状态(空闲/试验中/故障)
        /// </summary>
        [Column( MapType = typeof( string ) )]
        public StationStatus Status { get; set; }

        /// <summary>
        /// 当前产品ID
        /// </summary>
        [Column( StringLength = 20 )]
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
        制动装置,

        /// <summary>
        /// 辅助装置
        /// </summary>
        辅助装置
    }


    /// <summary>
    /// 试验台状态枚举
    /// </summary>
    public enum StationStatus
    {
        /// <summary>
        /// 空闲
        /// </summary>
        空闲,

        /// <summary>
        /// 试验中
        /// </summary>
        试验中,

        /// <summary>
        /// 故障
        /// </summary>
        故障
    }
}
