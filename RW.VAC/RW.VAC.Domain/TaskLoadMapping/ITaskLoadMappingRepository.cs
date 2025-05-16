using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.TaskLoadMapping
{
    public interface ITaskLoadMappingRepository
    {
        /// <summary>
        /// 获取所有任务载具关系
        /// </summary>
        /// <returns>任务载具关系列表</returns>
        Task<IEnumerable<TaskLoadMapping>> GetAllAsync( );

        /// <summary>
        /// 根据ID获取任务载具关系
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>任务载具关系实体</returns>
        Task<TaskLoadMapping> GetByIdAsync( int id );

        /// <summary>
        /// 根据任务ID获取任务载具关系
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <returns>任务载具关系列表</returns>
        Task<IEnumerable<TaskLoadMapping>> GetByTaskIdAsync( string taskId );

        /// <summary>
        /// 根据载具编码获取任务载具关系
        /// </summary>
        /// <param name="loadCode">载具编码</param>
        /// <returns>任务载具关系列表</returns>
        Task<IEnumerable<TaskLoadMapping>> GetByLoadCodeAsync( string loadCode );

        /// <summary>
        /// 根据载具类型获取任务载具关系
        /// </summary>
        /// <param name="loadType">载具类型</param>
        /// <returns>任务载具关系列表</returns>
        Task<IEnumerable<TaskLoadMapping>> GetByLoadTypeAsync( string loadType );

        /// <summary>
        /// 添加任务载具关系
        /// </summary>
        /// <param name="mapping">任务载具关系实体</param>
        /// <returns>添加结果</returns>
        Task<bool> AddAsync( TaskLoadMapping mapping );

        /// <summary>
        /// 更新任务载具关系
        /// </summary>
        /// <param name="mapping">任务载具关系实体</param>
        /// <returns>更新结果</returns>
        Task<bool> UpdateAsync( TaskLoadMapping mapping );

        /// <summary>
        /// 删除任务载具关系
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>删除结果</returns>
        Task<bool> DeleteAsync( int id );
    }
}
