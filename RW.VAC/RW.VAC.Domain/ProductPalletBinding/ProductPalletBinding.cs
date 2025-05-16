using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.ProductPalletBinding
{
    /// <summary>
    /// 产品托盘绑定实体类
    /// </summary>
    public class ProductPalletBinding
    {
        /// <summary>
        /// 绑定ID
        /// </summary>
        public int BindingId { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 托盘ID
        /// </summary>
        public string PalletId { get; set; }

        /// <summary>
        /// 绑定时间
        /// </summary>
        public DateTime? BindTime { get; set; }

        /// <summary>
        /// 解绑时间
        /// </summary>
        public DateTime? UnbindTime { get; set; }

        /// <summary>
        /// 绑定状态(绑定中/已解绑)
        /// </summary>
        public BindingStatus BindingStatus { get; set; }
    }

    /// <summary>
    /// 绑定状态枚举
    /// </summary>
    public enum BindingStatus
    {
        /// <summary>
        /// 绑定中
        /// </summary>
        Bound,

        /// <summary>
        /// 已解绑
        /// </summary>
        Unbound
    }
}
