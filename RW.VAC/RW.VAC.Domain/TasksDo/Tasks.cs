using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.TasksDo
{
    [Table( Name = "Tasks" )]
    public class Tasks
    {
        /// <summary>
        /// 任务ID（UUID格式）
        /// </summary>
        [Column( IsPrimary = true , StringLength = 36 )]
        public string TaskId { get; set; }

        /// <summary>
        /// 自定义任务编码
        /// </summary>
        [Column( StringLength = 50 )]
        public string CustomTaskCode { get; set; }

        /// <summary>
        /// 任务类型(运输/取料/上料/送回)
        /// </summary>
        [Column( MapType = typeof( string ) )]
        public TaskType TaskType { get; set; }

        /// <summary>
        /// 任务状态（与API对应的状态集合）
        /// </summary>
        [Column( MapType = typeof( string ) )]
        public TaskStatus TaskStatus { get; set; }

        /// <summary>
        /// 任务优先级(1-10)
        /// </summary>
        public int? Priority { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public int? BatchNumber { get; set; }

        /// <summary>
        /// 批次内顺序
        /// </summary>
        public int? BatchSortNumber { get; set; }

        /// <summary>
        /// 依赖批次号
        /// </summary>
        public int? DepBatchNumber { get; set; }

        /// <summary>
        /// 截止时间戳
        /// </summary>
        public long? CutOffTime { get; set; }

        /// <summary>
        /// 起始位置
        /// </summary>
        [Column( StringLength = 20 )]
        public string SourceLocation { get; set; }

        /// <summary>
        /// 目标位置
        /// </summary>
        [Column( StringLength = 20 )]
        public string TargetLocation { get; set; }

        /// <summary>
        /// 到达顺序
        /// </summary>
        public int? TargetArrivalSequence { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

    }
    /// <summary>
    /// 任务类型枚举
    /// </summary>
    public enum TaskType
    {
        /// <summary>
        /// 运输
        /// </summary>
        运输,

        /// <summary>
        /// 取料
        /// </summary>
        取料,

        /// <summary>
        /// 上料
        /// </summary>
        上料,

        /// <summary>
        /// 送回
        /// </summary>
        送回
    }


    /// <summary>
    /// 任务状态枚举
    /// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// 新建
        /// </summary>
        New,

        /// <summary>
        /// 等待中
        /// </summary>
        Waiting,

        /// <summary>
        /// 处理中
        /// </summary>
        Processing,

        /// <summary>
        /// 已完成
        /// </summary>
        Completed,

        /// <summary>
        /// 失败
        /// </summary>
        Failed
    }
}
