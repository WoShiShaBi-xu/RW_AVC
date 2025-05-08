using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.API
{
    public class OilPressureData
    {
        /// <summary>
        /// SN码
        /// </summary>
        public string SN { get; set; }
        /// <summary>
        /// 设定压力
        /// </summary>
        public string SetPressure { get; set; }
        /// <summary>
        /// 实际压力
        /// </summary>
        public string RealTimePressure { get; set; }
        /// <summary>
        /// 泄露设定值
        /// </summary>
        public string StdLeakage { get; set; }
        /// <summary>
        /// 泄露实际值
        /// </summary>
        public string LeakageFlow { get; set; }
        /// <summary>
        /// 保压时长(秒)
        /// </summary>
        public string HoldingTime { get; set; }
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
