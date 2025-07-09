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

        // <summary>
        /// 发送并执行自定义任务
        /// </summary>
        /// <param name="customCode">自定义任务编码</param>
        /// <param name="customTaskCode">客户运单号（可选）</param>
        /// <param name="taskParams">任务参数</param>
        /// <param name="sectionId">区域Id，默认为1</param>
        /// <param name="authToken">认证Token</param>
        /// <returns>自定义任务执行结果</returns>
        public async Task<CustomTaskResponse> RunCustomTaskAsync( string customCode , string customTaskCode = null , object taskParams = null , string sectionId = "1" , string authToken = "" )
        {
            try
            {
                // 构建请求URL
                string url = $"{_baseUrl}/platform/interface/V2/runCustomTask";

                // 构建请求参数
                var requestData = new
                {
                    customCode = customCode ,
                    customTaskCode = customTaskCode ,
                    @params = taskParams
                };

                // 将参数序列化为 JSON 字符串
                string jsonContent = JsonConvert.SerializeObject( requestData , Formatting.None , new JsonSerializerSettings
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
                var result = JsonConvert.DeserializeObject<CustomTaskResponse>( responseContent );

                _logger.LogInformation( $"自定义任务执行成功，任务编码: {customCode}" );
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError( ex , $"自定义任务执行失败，任务编码: {customCode}" );

                // 返回错误响应
                return new CustomTaskResponse
                {
                    Code = "-1" ,
                    Message = $"自定义任务执行失败: {ex.Message}" ,
                    Data = null
                };
            }
        }
      
    } }