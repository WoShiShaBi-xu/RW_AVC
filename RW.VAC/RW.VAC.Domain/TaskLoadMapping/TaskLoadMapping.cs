using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.TaskLoadMapping
{
    [Table( Name = "TaskLoadMapping" )]
    public class TaskLoadMapping
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [Column( IsPrimary = true , IsIdentity = true )]
        public int Id { get; set; }

        /// <summary>
        /// 任务ID
        /// </summary>
        [Column( StringLength = 36 )]
        public string TaskId { get; set; }

        /// <summary>
        /// 载具编码
        /// </summary>
        [Column( StringLength = 50 )]
        public string LoadCode { get; set; }

        /// <summary>
        /// 载具类型
        /// </summary>
        [Column( StringLength = 50 )]
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
