using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.API
{
    /// <summary>
    /// 扭矩测试数据 
    /// </summary>
    public class TorqueData
    {
        /// <summary>
        /// SN码
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 扭矩设定值
        /// </summary>
        public string? TorqueSet { get; set; }

        /// <summary>
        /// 扭矩实际值
        /// </summary>
        public string? TorqueValue { get; set; }

        /// <summary>
        /// 位移
        /// </summary>
        public string? Displacement { get; set; }
        /// <summary>
        /// 测试时间
        /// </summary>
        public string CheckDate { get; set; }

        /// <summary>
        /// 测试工位
        /// </summary>
        public string Station { get; set; }

    }
}
