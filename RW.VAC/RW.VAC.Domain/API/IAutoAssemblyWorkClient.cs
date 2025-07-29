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
        Task<TaskDetailResponse> GetTaskDetailByTaskIdAsync(string taskId, string sectionId = "1", string authToken = "");

        /// <summary>
        /// 发送订单任务到iRMS系统
        /// </summary>
        /// <param name="orderTasks">订单任务列表</param>
        /// <param name="sectionId">区域Id，默认为1</param>
        /// <param name="authToken">认证Token</param>
        /// <returns>发送订单任务结果</returns>
        Task<SendOrderTasksResponse> SendOrderTasksAsync(string customCode,
            Dictionary<string, int[]> parameters,
            string sectionId = "2",
            string authToken = "eyJraWQiOiJhcHBUb2tlbiIsInR5cCI6IkpXVCIsImFsZyI6IkhTMjU2In0.eyJhdWQiOlsiNjZhOWEzZGExNzkyMDIwNzU0NDFkOTJjIiwiV0VCIl19.GsSlxCBOEdPlALyzHgRnXW0ToEHLVCUbZYoFKYdE_zc");

        /// <summary>
        /// 删除负载
        /// </summary>
        /// <param name="parameters">删除参数</param>
        /// <param name="authToken">认证Token</param>
        /// <returns>删除结果</returns>
        Task<SendOrderTasksResponse> DeleteLoadAsync(Dictionary<string, object> parameters, string authToken);

        /// <summary>
        /// 添加负载
        /// </summary>
        /// <param name="parameters">添加参数</param>
        /// <returns>添加结果</returns>
        Task<SendOrderTasksResponse1> AddLoadAsync(Dictionary<string, object> parameters);

        /// <summary>
        /// 根据车辆代码获取车辆信息
        /// </summary>
        /// <param name="vehicleCode">车辆代码</param>
        /// <param name="vehicleType">车辆类型</param>
        /// <param name="sectionId">区域Id，默认为2</param>
        /// <param name="authToken">认证Token</param>
        /// <returns>车辆信息查询结果</returns>
        Task<GetVehicleResponse> GetVehicleByCodeAsync(string vehicleCode, string vehicleType,
            string sectionId = "2",
            string authToken = "eyJraWQiOiJhcHBUb2tlbiIsInR5cCI6IkpXVCIsImFsZyI6IkhTMjU2In0.eyJhdWQiOlsiNjZhOWEzZGExNzkyMDIwNzU0NDFkOTJjIiwiV0VCIl09.GsSlxCBOEdPlALyzHgRnXW0ToEHLVCUbZYoFKYdE_zc");

         Task<GetLoadsResponse> GetLoadsAsync();
    }
}