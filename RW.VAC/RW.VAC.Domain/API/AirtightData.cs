using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.API
{
    public class AirtightData
    {
        /// <summary>
        /// SN码
        /// </summary>
        public string SN { get; set; }
        /// <summary>
        /// 测试结果A
        /// </summary>
        public string Result_A { get; set; }
        /// <summary>
        /// 泄漏量A
        /// </summary>
        public string Divulge_A { get; set; }
        /// <summary>
        /// 测试结果B
        /// </summary>
        public string Result_B { get; set; }
        /// <summary>
        /// 泄漏量B
        /// </summary>
        public string Divulge_B { get; set; }
        /// <summary>
        /// 气压
        /// </summary>
        public string Pressure { get; set; }
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
