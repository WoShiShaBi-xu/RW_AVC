using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.Products
{
    public class Product
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 产品类型（制动装置/辅助装置）
        /// </summary>
        public ProductType ProductType { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产品规格
        /// </summary>
        public string ProductSpecs { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 产品状态(待组装/已组装/待试验/已试验/已检测)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }

    /// <summary>
    /// 产品类型枚举
    /// </summary>
    public enum ProductType
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
}
