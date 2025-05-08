using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RW.VAC.Application.Hardwares.Opc;
using RW.VAC.Domain.API;
using RW.VAC.Domain.Entities;
using System;
using System.Collections.Generic;
using Polly;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.Web;
using static System.Collections.Specialized.BitVector32;

namespace RW.VAC.Application.Contracts.API
{

    public class WMSClient : IAutoAssemblyWorkClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WMSClient> _logger;
        private readonly AsyncPolicy<HttpResponseMessage> _retryPolicy;

        public WMSClient( HttpClient httpClient , ILogger<WMSClient> logger )
        {
            _httpClient = httpClient;
            _logger = logger;

            // 使用 Polly 设置重试策略
            _retryPolicy = Policy.Handle<HttpRequestException>()
                                 .OrResult<HttpResponseMessage>( r => !r.IsSuccessStatusCode )
                                 .WaitAndRetryAsync( 3 , retryAttempt => TimeSpan.FromSeconds( Math.Pow( 2 , retryAttempt ) ) ,
                                 onRetry: ( outcome , timespan , retryAttempt , context ) =>
                                 {
                                     _logger.LogWarning( $"重试第 {retryAttempt} 次: {outcome.Exception?.Message ?? outcome.Result.ReasonPhrase}" );
                                 } );
        }

        public async Task<(bool HasError, string Message, string Value1)> ReportWorkAsync( string lineCode , string lotSN , string processCode , string workDate , string userCode , bool isProduction = true )
        {
            try
            {
                // 设置请求地址，选择正式还是测试环境
                string url = "http://10.132.128.22:10020/WMSCommon/WMS/Auto_Assembly_AutoWork";

                // 构建请求参数
                var requestData = new
                {
                    LineCode = lineCode ,
                    LotSN = lotSN ,
                    ProcessCode = processCode ,
                    WorkDate = workDate ,
                    UserCode = userCode
                };
                //abbb
                // 将参数序列化为 JSON 字符串
                string jsonContent = JsonConvert.SerializeObject( requestData );

                // 创建 HttpContent 对象
                var content = new StringContent( jsonContent , Encoding.UTF8 , "application/json" );

                // 发送 POST 请求
                HttpResponseMessage response = await _httpClient.PostAsync( url , content );

                // 确保请求成功
                response.EnsureSuccessStatusCode();

                // 读取响应内容
                string responseContent = await response.Content.ReadAsStringAsync();

                // 解析响应内容
                var result = JsonConvert.DeserializeObject<ResponseData>( responseContent );

                // 返回处理后的结果
                return (result.HasError, result.Message, result.Value1);
            }
            catch ( Exception )
            {

                return (false,"","");
            }
            

        }
        public async Task TorqueDataAPI( TorqueData torque)
        {
            try
            {
                string url = "http://10.132.128.22:10020/WMSCommon/WMS/TorqueTestData";
                var torque_1 = new
                {
                    SN = torque.SN ,
                    TorqueSet = torque.TorqueSet ,
                    TorqueValue = torque.TorqueValue ,
                    Displacement = torque.Displacement ,
                    CheckDate = torque.CheckDate ,
                    Station = torque.Station ,
                };
                string jsonContent = JsonConvert.SerializeObject( torque_1 );

                // 创建 HttpContent 对象
                var content = new StringContent( jsonContent , Encoding.UTF8 , "application/json" );

                // 发送 POST 请求
                HttpResponseMessage response = await _httpClient.PostAsync( url , content );

                // 确保请求成功
                response.EnsureSuccessStatusCode();

                // 读取响应内容
                string responseContent = await response.Content.ReadAsStringAsync();

                // 解析响应内容
                var result = JsonConvert.DeserializeObject<ResponseData>( responseContent );

                _logger.LogWarning( result.ToString() ?? "" );
            }
            catch ( Exception ex)
            {
                _logger.LogWarning(ex.ToString());


            }
        }


        public async Task OilPressureDataAPI( OilPressureData  oilPressureData )
        {
            try
            { // 设置请求地址，选择正式还是测试环境
                string url = "http://10.132.128.22:10020/WMSCommon/WMS/OilPressureTestData";
                // 构建请求参数
                var oilPressureData_1 = new
                {
                    SN = oilPressureData.SN ,
                    SetPressure = oilPressureData.SetPressure ,
                    RealTimePressure = oilPressureData.RealTimePressure ,
                    StdLeakage = oilPressureData.StdLeakage ,
                    LeakageFlow = oilPressureData.LeakageFlow ,
                    HoldingTime = oilPressureData.HoldingTime ,
                    CheckDate = oilPressureData.CheckDate ,
                    Station = oilPressureData.Station ,
                };
                string jsonContent = JsonConvert.SerializeObject( oilPressureData_1 );

                // 创建 HttpContent 对象
                var content = new StringContent( jsonContent , Encoding.UTF8 , "application/json" );

                // 发送 POST 请求
                HttpResponseMessage response = await _httpClient.PostAsync( url , content );

                // 确保请求成功
                response.EnsureSuccessStatusCode();

                // 读取响应内容
                string responseContent = await response.Content.ReadAsStringAsync();

                // 解析响应内容
                var result = JsonConvert.DeserializeObject<ResponseData>( responseContent );
                _logger.LogWarning( result.ToString()??"" );
            }
            catch ( Exception ex )
            {

                _logger.LogWarning( ex.ToString() );

            }
        }
        public async Task AirtightDataAPI( AirtightData airtightData)
        {
            try
            { // 设置请求地址，选择正式还是测试环境
                string url = "http://10.132.128.22:10020/WMSCommon/WMS/AirtightTestData";
                // 构建请求参数
                var airtightData_1 = new
                {
                    SN = airtightData.SN,
                    Result_A= airtightData.Result_A,
                    Divulge_A= airtightData.Divulge_A ,
                    Result_B = airtightData.Result_B ,
                    Divulge_B = airtightData.Divulge_B ,
                    Pressure= airtightData.Pressure ,
                    CheckDate = airtightData.CheckDate ,
                    Station = airtightData.Station ,
                };
                string jsonContent = JsonConvert.SerializeObject( airtightData_1 );

                // 创建 HttpContent 对象
                var content = new StringContent( jsonContent , Encoding.UTF8 , "application/json" );

                // 发送 POST 请求
                HttpResponseMessage response = await _httpClient.PostAsync( url , content );

                // 确保请求成功
                response.EnsureSuccessStatusCode();

                // 读取响应内容
                string responseContent = await response.Content.ReadAsStringAsync();

                // 解析响应内容
                var result = JsonConvert.DeserializeObject<ResponseData>( responseContent );
                _logger.LogWarning( result.ToString() ?? "" );
            }
            catch ( Exception ex)
            {
                _logger.LogWarning( ex.ToString() );


            }
        }

        public async Task<bool> QuerySNInfo( string sN ,string ProductName)
        {
            try
            {
                string url = "http://10.132.128.22:10020/WMSCommon/WMS/FMQuerySNInfo_RUNWEI";
                var requestData = new
                {
                    SN = sN
                };

                // 将参数序列化为 JSON 字符串
                string jsonContent = JsonConvert.SerializeObject( requestData );

                // 创建 HttpContent 对象
                var content = new StringContent( jsonContent , Encoding.UTF8 , "application/json" );

                // 发送 POST 请求
                HttpResponseMessage response = await _httpClient.PostAsync( url , content );

                // 确保请求成功
                response.EnsureSuccessStatusCode();

                // 读取响应内容
                string responseContent = await response.Content.ReadAsStringAsync();

                // 解析响应内容
                var result = JsonConvert.DeserializeObject<SNInfoResponse>( responseContent );
                if ( result.Message == "" )
                {
                    if ( result.ProductName == ProductName )
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogWarning( ex.ToString() );
                return false;
            }
           
        }
        public class SNInfoResponse
        {
            public int Code { get; set; }
            public string Message { get; set; }
            public string SN { get; set; }
            public string MoCode { get; set; }
            public string ProductRootCode { get; set; }
            public string ProductName { get; set; }
        }

        public class ResponseData
        {
            public bool HasError { get; set; }
            public string Message { get; set; }
            public string Value1 { get; set; }
        }

        /// <summary>
        /// 气密测试数据
        /// </summary>
        
        /// <summary>
		/// 油压测试数据 
		/// </summary>
		
       

    }

   

}
