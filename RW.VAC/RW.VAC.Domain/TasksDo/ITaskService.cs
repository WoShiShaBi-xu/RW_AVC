using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.TasksDo
{
    public interface ITaskService
    {
        /// <summary>
        /// 创建新任务
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <param name="sourceLocation">起始位置</param>
        /// <param name="targetLocation">目标位置</param>
        /// <param name="priority">优先级</param>
        /// <returns>创建的任务</returns>
        Task<Task> CreateTaskAsync( TaskType taskType , string sourceLocation , string targetLocation , int priority );

        /// <summary>
        /// 更新任务状态
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <param name="status">新状态</param>
        /// <returns>更新结果</returns>
        Task<bool> UpdateTaskStatusAsync( string taskId , TaskStatus status );

        /// <summary>
        /// 分配任务到批次
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <param name="batchNumber">批次号</param>
        /// <param name="sortNumber">批次内顺序</param>
        /// <returns>分配结果</returns>
        Task<bool> AssignTaskToBatchAsync( string taskId , int batchNumber , int sortNumber );

        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <returns>取消结果</returns>
        Task<bool> CancelTaskAsync( string taskId );

        /// <summary>
        /// 根据条件查询任务
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <param name="status">任务状态</param>
        /// <param name="batchNumber">批次号</param>
        /// <returns>任务列表</returns>
        Task<IEnumerable<Task>> QueryTasksAsync( TaskType? taskType , TaskStatus? status , int? batchNumber );

        /// <summary>
        /// 获取任务详情
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <returns>任务详情</returns>
        Task<Task> GetTaskDetailsAsync( string taskId );
    }
}
