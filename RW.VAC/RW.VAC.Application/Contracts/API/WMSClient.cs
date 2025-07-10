using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Polly;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RW.VAC.Domain.API;

namespace RW.VAC.Application.Contracts.API
{
    public class WMSClient : IWMSClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WMSClient> _logger;
        private readonly AsyncPolicy<HttpResponseMessage> _retryPolicy;
        private readonly string _baseUrl;

        public WMSClient( HttpClient httpClient , ILogger<WMSClient> logger , string baseUrl = "http://10.132.128.22:10020" )
        {
            _httpClient = httpClient;
            _logger = logger;
            _baseUrl = baseUrl;

            // 使用 Polly 设置重试策略
            _retryPolicy = Policy.Handle<HttpRequestException>()
                                 .OrResult<HttpResponseMessage>( r => !r.IsSuccessStatusCode )
                                 .WaitAndRetryAsync( 3 , retryAttempt => TimeSpan.FromSeconds( Math.Pow( 2 , retryAttempt ) ) ,
                                 onRetry: ( outcome , timespan , retryAttempt , context ) =>
                                 {
                                     _logger.LogWarning( $"重试第 {retryAttempt} 次: {outcome.Exception?.Message ?? outcome.Result.ReasonPhrase}" );
                                 } );
        }

        /// <summary>
        /// 根据任务Id获取任务详情
        /// </summary>
        /// <param name="taskId">任务Id</param>
        /// <param name="sectionId">区域Id，默认为1</param>
        /// <param name="authToken">认证Token</param>
        /// <returns>任务详情查询结果</returns>
        public async Task<TaskDetailResponse> GetTaskDetailByTaskIdAsync( string taskId , string sectionId = "1" , string authToken = "" )
        {
            try
            {
                // 构建请求URL
                string url = $"{_baseUrl}/platform/interface/V2/getTaskDetailByTaskId?taskId={Uri.EscapeDataString( taskId )}";

                // 设置请求头
                var request = new HttpRequestMessage( HttpMethod.Get , url );
                request.Headers.Add( "sectionId" , sectionId );

                if (!string.IsNullOrEmpty( authToken ))
                {
                    request.Headers.Add( "Authorization" , $"Bearer {authToken}" );
                }

                // 发送 GET 请求
                HttpResponseMessage response = await _retryPolicy.ExecuteAsync( async ( ) =>
                {
                    return await _httpClient.SendAsync( request );
                } );

                // 确保请求成功
                response.EnsureSuccessStatusCode();

                // 读取响应内容
                string responseContent = await response.Content.ReadAsStringAsync();

                // 解析响应内容
                var result = JsonConvert.DeserializeObject<TaskDetailResponse>( responseContent );

                _logger.LogInformation( $"任务查询成功，任务ID: {taskId}" );
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError( ex , $"查询任务详情失败，任务ID: {taskId}" );

                // 返回错误响应
                return new TaskDetailResponse
                {
                    Code = "-1" ,
                    Message = $"查询任务详情失败: {ex.Message}" ,
                    Data = null
                };
            }
        }

        /// <summary>
        /// 发送订单任务到iRMS系统
        /// </summary>
        /// <param name="orderTasks">订单任务列表</param>
        /// <param name="sectionId">区域Id，默认为1</param>
        /// <param name="authToken">认证Token</param>
        /// <returns>发送订单任务结果</returns>
        public async Task<SendOrderTasksResponse> SendOrderTasksAsync( List<OrderTask> orderTasks , string sectionId = "1" , string authToken = "" )
        {
            try
            {
                // 构建请求URL
                string url = $"{_baseUrl}/platform/interface/V2/sendOrderTasks";

                // 将参数序列化为 JSON 字符串
                string jsonContent = JsonConvert.SerializeObject( orderTasks , Formatting.None , new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                } );

                // 创建 HttpContent 对象
                var content = new StringContent( jsonContent , Encoding.UTF8 , "application/json" );

                // 设置请求头
                var request = new HttpRequestMessage( HttpMethod.Post , url )
                {
                    Content = content
                };
                request.Headers.Add( "sectionId" , sectionId );

                if (!string.IsNullOrEmpty( authToken ))
                {
                    request.Headers.Add( "Authorization" , $"Bearer {authToken}" );
                }

                // 发送 POST 请求
                HttpResponseMessage response = await _retryPolicy.ExecuteAsync( async ( ) =>
                {
                    return await _httpClient.SendAsync( request );
                } );

                // 确保请求成功
                response.EnsureSuccessStatusCode();

                // 读取响应内容
                string responseContent = await response.Content.ReadAsStringAsync();

                // 解析响应内容
                var result = JsonConvert.DeserializeObject<SendOrderTasksResponse>( responseContent );

                _logger.LogInformation( $"发送订单任务成功，共 {orderTasks.Count} 个任务" );
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError( ex , $"发送订单任务失败，任务数量: {orderTasks.Count}" );

                // 返回错误响应
                return new SendOrderTasksResponse
                {
                    Code = "-1" ,
                    Message = $"发送订单任务失败: {ex.Message}" ,
                    Data = null
                };
            }
        }
    }
}