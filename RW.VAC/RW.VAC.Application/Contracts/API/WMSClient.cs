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
using RW.VAC.Application.Contracts.AGV;

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
        public async Task<GetVehicleResponse> GetVehicleByCodeAsync(string vehicleCode, string vehicleType,
    string authToken = "eyJraWQiOiJhcHBUb2tlbiIsInR5cCI6IkpXVCIsImFsZyI6IkhTMjU2In0.eyJhdWQiOlsiNjZhOWEzZGExNzkyMDIwNzU0NDFkOTJjIiwiV0VCIl09.GsSlxCBOEdPlALyzHgRnXW0ToEHLVCUbZYoFKYdE_zc",string i="")
        {
            // 参数验证
            if (string.IsNullOrEmpty(vehicleCode))
                throw new ArgumentException("vehicleCode不能为空", nameof(vehicleCode));
            if (string.IsNullOrEmpty(vehicleType))
                throw new ArgumentException("vehicleType不能为空", nameof(vehicleType));

            try
            {
                // 构建带查询参数的URL，使用URL编码确保安全
                var baseUrl = $"{_baseUrl}/platform/interface/V2/getVehicleByCode";
                var queryParams = $"vehicleCode={Uri.EscapeDataString(vehicleCode)}&vehicleType={Uri.EscapeDataString(vehicleType)}";
                string url = $"{baseUrl}?{queryParams}";

                _logger.LogInformation($"请求URL: {url}");

                HttpResponseMessage response = await _retryPolicy.ExecuteAsync(async () =>
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, url);

                    request.Headers.Add("sectionId", "2");
                    request.Headers.Add("Authorization", "eyJraWQiOiJhcHBUb2tlbiIsInR5cCI6IkpXVCIsImFsZyI6IkhTMjU2In0.eyJhdWQiOlsiNjZhOWEzZGExNzkyMDIwNzU0NDFkOTJjIiwiV0VCIl19.GsSlxCBOEdPlALyzHgRnXW0ToEHLVCUbZYoFKYdE_zc");

                    return await _httpClient.SendAsync(request);
                });

                _logger.LogInformation($"响应状态: {response.StatusCode}");

                response.EnsureSuccessStatusCode();

                string responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"响应内容: {responseContent}");

                var result = JsonConvert.DeserializeObject<GetVehicleResponse>(responseContent);

                // 检查业务层面的响应
                if (result?.Code != "0")
                {
                    throw new InvalidOperationException($"API返回错误: {result?.Message ?? "未知错误"}");
                }

                return result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"HTTP请求异常: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"获取车辆信息异常: {ex.Message}");
                throw;
            }
        }
        /// <summary>
        /// 查询所有载具信息
        /// </summary>
        /// <param name="sectionId">区域Id，默认为2</param>
        /// <param name="authToken">认证Token</param>
        /// <returns>所有载具信息查询结果</returns>
        public async Task<GetLoadsResponse> GetLoadsAsync()
        {
            try
            {
                // 构建请求URL
                string url = $"{_baseUrl}/platform/interface/V2/getLoads";

                _logger.LogInformation( $"请求URL: {url}" );

                // 发送 GET 请求
                HttpResponseMessage response = await _retryPolicy.ExecuteAsync( async ( ) =>
                {
                    var request = new HttpRequestMessage( HttpMethod.Get , url );

                    request.Headers.Add( "sectionId" , "2" );
                    request.Headers.Add( "Authorization" , "eyJraWQiOiJhcHBUb2tlbiIsInR5cCI6IkpXVCIsImFsZyI6IkhTMjU2In0.eyJhdWQiOlsiNjZhOWEzZGExNzkyMDIwNzU0NDFkOTJjIiwiV0VCIl19.GsSlxCBOEdPlALyzHgRnXW0ToEHLVCUbZYoFKYdE_zc" );



                    return await _httpClient.SendAsync( request );
                } );

                _logger.LogInformation( $"响应状态码: {response.StatusCode}" );

                // 确保请求成功
                response.EnsureSuccessStatusCode();

                // 读取响应内容
                string responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation( $"响应内容: {responseContent}" );

                // 解析响应内容
                var result = JsonConvert.DeserializeObject<GetLoadsResponse>( responseContent );

                // 检查业务层面的响应
                if (result?.Code != "0")
                {
                    throw new InvalidOperationException( $"API返回错误: {result?.Message ?? "未知错误"}" );
                }

                _logger.LogInformation( $"查询所有载具信息成功，共返回 {result.Data?.Length ?? 0} 条记录" );
                return result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError( ex , "HTTP请求异常" );

                // 返回错误响应
                return new GetLoadsResponse
                {
                    Code = "-1" ,
                    Message = $"HTTP请求异常: {ex.Message}" ,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError( ex , "查询所有载具信息失败" );

                // 返回错误响应
                return new GetLoadsResponse
                {
                    Code = "-1" ,
                    Message = $"查询所有载具信息失败: {ex.Message}" ,
                    Data = null
                };
            }
        }
        public async Task<SendOrderTasksResponse1> AddLoadAsync(Dictionary<string, object> parameters)
        {
            try
            {
                string url = $"{_baseUrl}/platform/interface/V2/addLoad";

                var jsonContent = $@"{{
    ""loadCode"":""{parameters["loadCode"]}"",
    ""loadAngle"":""{parameters["loadAngle"]}"",
    ""loadSpecificationCode"":""{parameters["loadSpecificationCode"]}"",
    ""storageCode"":""{parameters["storageCode"]}""
}}";

                _logger.LogInformation($"请求URL: {url}");
                _logger.LogInformation($"请求体: {jsonContent}");

                HttpResponseMessage response = await _retryPolicy.ExecuteAsync(async () =>
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                    };

                    request.Headers.Add("sectionId", "2");
                    request.Headers.Add("Authorization", "eyJraWQiOiJhcHBUb2tlbiIsInR5cCI6IkpXVCIsImFsZyI6IkhTMjU2In0.eyJhdWQiOlsiNjZhOWEzZGExNzkyMDIwNzU0NDFkOTJjIiwiV0VCIl19.GsSlxCBOEdPlALyzHgRnXW0ToEHLVCUbZYoFKYdE_zc");

                    // 调试：打印所有headers
                    _logger.LogInformation("请求Headers:");
                    foreach (var header in request.Headers)
                    {
                        _logger.LogInformation($"{header.Key}: {string.Join(", ", header.Value)}");
                    }
                    foreach (var header in request.Content.Headers)
                    {
                        _logger.LogInformation($"{header.Key}: {string.Join(", ", header.Value)}");
                    }

                    var result = await _httpClient.SendAsync(request);

                    // 调试：打印响应状态
                    _logger.LogInformation($"响应状态码: {result.StatusCode}");
                    _logger.LogInformation($"响应Headers: {string.Join(", ", result.Headers.Select(h => $"{h.Key}={string.Join(",", h.Value)}"))}");

                    return result;
                });

                string responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"响应内容: {responseContent}");

                response.EnsureSuccessStatusCode();
                var result = JsonConvert.DeserializeObject<SendOrderTasksResponse1>(responseContent);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"请求异常: {ex.Message}");
                throw;
            }
        }
        public async Task<SendOrderTasksResponse> DeleteLoadAsync(Dictionary<string, object> parameters,
        string authToken = "eyJraWQiOiJhcHBUb2tlbiIsInR5cCI6IkpXVCIsImFsZyI6IkhTMjU2In0.eyJhdWQiOlsiNjZhOWEzZGExNzkyMDIwNzU0NDFkOTJjIiwiV0VCIl19.GsSlxCBOEdPlALyzHgRnXW0ToEHLVCUbZYoFKYdE_zc")
        {
            try
            {
                string url = $"{_baseUrl}/platform/interface/V2/deleteLoad";

                // 修复：loadCode为int类型，不加引号
                var jsonContent = $@"{{
    ""loadCode"":{parameters["loadCode"]},
    ""loadType"":""{(parameters.ContainsKey("loadType") ? parameters["loadType"] : "")}"" 
}}";

                _logger.LogInformation($"请求体: {jsonContent}");

                // 发送 POST 请求 - 修改此处以确保每次重试创建新的请求
                HttpResponseMessage response = await _retryPolicy.ExecuteAsync(async () =>
                {
                    // 每次重试都创建新的 HttpRequestMessage
                    var request = new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                    };
                    // 设置请求头
                    request.Headers.Add("sectionId", "2");
                    request.Headers.Add("Authorization", "eyJraWQiOiJhcHBUb2tlbiIsInR5cCI6IkpXVCIsImFsZyI6IkhTMjU2In0.eyJhdWQiOlsiNjZhOWEzZGExNzkyMDIwNzU0NDFkOTJjIiwiV0VCIl19.GsSlxCBOEdPlALyzHgRnXW0ToEHLVCUbZYoFKYdE_zc");

                    return await _httpClient.SendAsync(request);
                });

                // 确保请求成功
                response.EnsureSuccessStatusCode();
                // 读取响应内容
                string responseContent = await response.Content.ReadAsStringAsync();
                // 解析响应内容
                var result = JsonConvert.DeserializeObject<SendOrderTasksResponse>(responseContent);
                return result;
            }
            catch (Exception)
            {
                throw;
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