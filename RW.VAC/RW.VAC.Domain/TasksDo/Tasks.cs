using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.TasksDo
{
    public class Tasks
    {
        /// <summary>
        /// 任务ID（UUID格式）
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// 自定义任务编码
        /// </summary>
        public string CustomTaskCode { get; set; }

        /// <summary>
        /// 任务类型(运输/取料/上料/送回)
        /// </summary>
        public TaskType TaskType { get; set; }

        /// <summary>
        /// 任务状态（与API对应的状态集合）
        /// </summary>
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
        public string SourceLocation { get; set; }

        /// <summary>
        /// 目标位置
        /// </summary>
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

        /// <summary>
        /// 外部系统任务ID
        /// </summary>
        public string ExternalTaskId { get; set; }
    }
    /// <summary>
    /// 任务类型枚举
    /// </summary>
    public enum TaskType
    {
        /// <summary>
        /// 运输
        /// </summary>
        Transport,

        /// <summary>
        /// 取料
        /// </summary>
        Pickup,

        /// <summary>
        /// 上料
        /// </summary>
        Loading,

        /// <summary>
        /// 送回
        /// </summary>
        Return
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
        /// 已分配
        /// </summary>
        Assigned,

        /// <summary>
        /// 进行中
        /// </summary>
        InProgress,

        /// <summary>
        /// 已完成
        /// </summary>
        Completed,

        /// <summary>
        /// 已取消
        /// </summary>
        Canceled,

        /// <summary>
        /// 失败
        /// </summary>
        Failed
    }
}
