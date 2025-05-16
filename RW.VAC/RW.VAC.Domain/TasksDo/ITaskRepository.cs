using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.TasksDo
{
    public interface ITaskRepository
    {
        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <returns>任务列表</returns>
        Task<IEnumerable<Task>> GetAllAsync( );

        /// <summary>
        /// 根据ID获取任务
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <returns>任务实体</returns>
        Task<Task> GetByIdAsync( string taskId );

        /// <summary>
        /// 根据自定义任务编码获取任务
        /// </summary>
        /// <param name="customTaskCode">自定义任务编码</param>
        /// <returns>任务实体</returns>
        Task<Task> GetByCustomCodeAsync( string customTaskCode );

        /// <summary>
        /// 根据类型获取任务
        /// </summary>
        /// <param name="taskType">任务类型</param>
        /// <returns>任务列表</returns>
        Task<IEnumerable<Task>> GetByTypeAsync( TaskType taskType );

        /// <summary>
        /// 根据状态获取任务
        /// </summary>
        /// <param name="status">任务状态</param>
        /// <returns>任务列表</returns>
        Task<IEnumerable<Task>> GetByStatusAsync( TaskStatus status );

        /// <summary>
        /// 根据批次号获取任务
        /// </summary>
        /// <param name="batchNumber">批次号</param>
        /// <returns>任务列表</returns>
        Task<IEnumerable<Task>> GetByBatchNumberAsync( int batchNumber );

        /// <summary>
        /// 根据起始位置获取任务
        /// </summary>
        /// <param name="sourceLocation">起始位置</param>
        /// <returns>任务列表</returns>
        Task<IEnumerable<Task>> GetBySourceLocationAsync( string sourceLocation );

        /// <summary>
        /// 根据目标位置获取任务
        /// </summary>
        /// <param name="targetLocation">目标位置</param>
        /// <returns>任务列表</returns>
        Task<IEnumerable<Task>> GetByTargetLocationAsync( string targetLocation );

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="task">任务实体</param>
        /// <returns>添加结果</returns>
        Task<bool> AddAsync( Task task );

        /// <summary>
        /// 更新任务
        /// </summary>
        /// <param name="task">任务实体</param>
        /// <returns>更新结果</returns>
        Task<bool> UpdateAsync( Task task );

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <returns>删除结果</returns>
        Task<bool> DeleteAsync( string taskId );
    }
}
