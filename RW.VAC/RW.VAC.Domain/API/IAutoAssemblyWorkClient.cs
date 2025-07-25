
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
        /// 发送订单任务到iRMS系统
        /// </summary>
        /// <param name="orderTasks">订单任务列表</param>
        /// <param name="sectionId">区域Id，默认为1</param>
        /// <param name="authToken">认证Token</param>
        /// <returns>发送订单任务结果</returns>
        Task<SendOrderTasksResponse> SendOrderTasksAsync( string customCode ,
    Dictionary<string , int [ ]> parameters ,
    string sectionId = "1" ,
    string authToken = "eyJraWQiOiJhcHBUb2tlbiIsInR5cCI6IkpXVCIsImFsZyI6IkhTMjU2In0.eyJhdWQiOlsiNjZhOWEzZGExNzkyMDIwNzU0NDFkOTJjIiwiV0VCIl19.GsSlxCBOEdPlALyzHgRnXW0ToEHLVCUbZYoFKYdE_zc" );
    }
}
