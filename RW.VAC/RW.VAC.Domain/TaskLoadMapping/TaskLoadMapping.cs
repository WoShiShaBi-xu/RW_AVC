using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.TaskLoadMapping
{
    public class TaskLoadMapping
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 任务ID
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// 载具编码
        /// </summary>
        public string LoadCode { get; set; }

        /// <summary>
        /// 载具类型
        /// </summary>
        public string LoadType { get; set; }

        /// <summary>
        /// 载具角度
        /// </summary>
        public int? LoadAngle { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
    }
}
