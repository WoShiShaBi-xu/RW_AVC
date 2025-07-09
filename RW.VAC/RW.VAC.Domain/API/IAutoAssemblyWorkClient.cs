
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RW.VAC.Domain.API
{
    public interface IWMSClient
    {
        /// <summary>
        /// 根据任务Id获取任务详情
        /// </summary>
        /// <param name="taskId">任务Id</param>
        /// <param name="sectionId">区域Id，默认为1</param>
        /// <param name="authToken">认证Token</param>
        /// <returns>任务详情查询结果</returns>
        Task<TaskDetailResponse> GetTaskDetailByTaskIdAsync( string taskId , string sectionId = "1" , string authToken = "" );

        /// <summary>
        /// 发送并执行自定义任务
        /// </summary>
        /// <param name="customCode">自定义任务编码</param>
        /// <param name="customTaskCode">客户运单号（可选）</param>
        /// <param name="taskParams">任务参数</param>
        /// <param name="sectionId">区域Id，默认为1</param>
        /// <param name="authToken">认证Token</param>
        /// <returns>自定义任务执行结果</returns>
        Task<CustomTaskResponse> RunCustomTaskAsync( string customCode , string customTaskCode = null , object taskParams = null , string sectionId = "1" , string authToken = "" );
    }
}
