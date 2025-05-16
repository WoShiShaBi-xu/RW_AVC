using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.Pallet
{
    /// <summary>
    /// 托盘实体类
    /// </summary>
    public class Pallet
    {
        /// <summary>
        /// 托盘ID
        /// </summary>
        public string PalletId { get; set; }

        /// <summary>
        /// 托盘类型（制动装置托盘/辅助装置托盘）
        /// </summary>
        public PalletType PalletType { get; set; }

        /// <summary>
        /// 托盘状态(空闲/使用中)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 当前位置
        /// </summary>
        public string LocationId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime? LastUpdate { get; set; }
    }

    /// <summary>
    /// 托盘类型枚举
    /// </summary>
    public enum PalletType
    {
        /// <summary>
        /// 制动装置托盘
        /// </summary>
        BrakeDevicePallet,

        /// <summary>
        /// 辅助装置托盘
        /// </summary>
        AuxiliaryDevicePallet
    }
}
