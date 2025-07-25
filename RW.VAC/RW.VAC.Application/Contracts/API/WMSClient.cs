using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Polly;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RW.VAC.Domain.API;
using Org.BouncyCastle.Asn1.Ocsp;
using MySql.Data.MySqlClient.Memcached;
using RestSharp;

namespace RW.VAC.Application.Contracts.API
{
    public class WMSClient : IWMSClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WMSClient> _logger;
        private readonly AsyncPolicy<HttpResponseMessage> _retryPolicy;
        private readonly string _baseUrl;

        public WMSClient( HttpClient httpClient , ILogger<WMSClient> logger , string baseUrl = "http://10.19.1.34:8000" )
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
        public async Task<SendOrderTasksResponse> SendOrderTasksAsync(
string customCode ,
Dictionary<string , int [ ]> parameters ,
string sectionId = "2" ,
string authToken = "eyJraWQiOiJhcHBUb2tlbiIsInR5cCI6IkpXVCIsImFsZyI6IkhTMjU2In0.eyJhdWQiOlsiNjZhOWEzZGExNzkyMDIwNzU0NDFkOTJjIiwiV0VCIl19.GsSlxCBOEdPlALyzHgRnXW0ToEHLVCUbZYoFKYdE_zc" )
        {
            try
            {
                // 构建请求URL
                string url = $"{_baseUrl}/platform/interface/V2/runCustomTask";

                var requestBody = new
                {
                    customCode = customCode ,
                    @params = parameters  // 使用 @params 因为 params 是 C# 关键字
                };

                // 将参数序列化为 JSON 字符串
                string jsonContent = JsonConvert.SerializeObject( requestBody , Formatting.None , new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                } );
                _logger.LogInformation( $"请求体: {jsonContent}" );

                // 发送 POST 请求 - 修改此处以确保每次重试创建新的请求
                HttpResponseMessage response = await _retryPolicy.ExecuteAsync( async ( ) =>
                {
                    // 每次重试都创建新的 HttpRequestMessage
                    var request = new HttpRequestMessage( HttpMethod.Post , url )
                    {
                        Content = new StringContent( jsonContent , Encoding.UTF8 , "application/json" )
                    };

                    // 设置请求头
                    request.Headers.Add( "Authorization" , "Bearer eyJraWQiOiJhcHBUb2tlbiIsInR5cCI6IkpXVCIsImFsZyI6IkhTMjU2In0.eyJhdWQiOlsiNjZhOWEzZGExNzkyMDIwNzU0NDFkOTJjIiwiV0VCIl19.GsSlxCBOEdPlALyzHgRnXW0ToEHLVCUbZYoFKYdE_zc" );
                    request.Headers.Add( "sectionId" , "2" );

                    return await _httpClient.SendAsync( request );
                } );

                // 确保请求成功
                response.EnsureSuccessStatusCode();

                // 读取响应内容
                string responseContent = await response.Content.ReadAsStringAsync();

                // 解析响应内容
                var result = JsonConvert.DeserializeObject<SendOrderTasksResponse>( responseContent );

                _logger.LogInformation( $"发送订单任务成功，customCode: {customCode}" );
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError( ex , $"发送订单任务失败，customCode: {customCode}" );
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