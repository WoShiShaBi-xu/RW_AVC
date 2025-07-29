using RW.VAC.Domain.API;
using RW.VAC.Infrastructure.Opc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Application.Contracts.AGV
{
    public interface IAgvService
    {
        Task<bool> SendLoadCommandAsync(string Code, string para1, string para2, int chehao, int target_point);
       
        Task<TaskDetailResponse> GetTaskStatusAsync( string taskId );
        Task<bool> DeleteLoadAsync( int loadCode , string loadType );

        Task<bool> AddLoadAsync( AddLoadRequest request );
        Task<GetVehicleResponse> GetVehicleResponse();
         Task<GetLoadsResponse> GetLoadsAsync();
    }
    public class AgvService : IAgvService
    {
        private readonly IWMSClient _wmsClient;
        private readonly IUaClient _uaClient;
        private readonly TagStorage _tagStorage;

        public AgvService( IWMSClient wmsClient , IUaClient uaClient , TagStorage tagStorage )
        {
            _wmsClient = wmsClient;
            _uaClient = uaClient;
            _tagStorage = tagStorage;
        }
        
        public async Task<bool> SendLoadCommandAsync(string Code,string para1,string para2 ,int chehao,int target_point )
        {

            var parameters = new Dictionary<string , int [ ]>
{
    { para1, new int[] { chehao } },
    { para2, new int[] { target_point } }
};

            var response = await _wmsClient.SendOrderTasksAsync( Code , parameters );
            return response.Code == "0";
        }
        public async Task<GetVehicleResponse> GetVehicleResponse() 
        {
          return await _wmsClient.GetVehicleByCodeAsync("1567", "latent");
        }
        /// <summary>
        /// 新增载具
        /// </summary>
        /// <param name="request">新增载具请求参数</param>
        /// <returns>新增载具响应</returns>
        public async Task<bool> AddLoadAsync( AddLoadRequest request )
        {
            try
            {
               var parameters = new Dictionary<string , object>
                {
                    { "loadCode", request.LoadCode  },
                    { "loadAngle",  request.LoadAngle  },
                    { "loadSpecificationCode", request.LoadSpecificationCode  },
                    { "storageCode",  request.StorageCode  }
                };
                var response = await _wmsClient.AddLoadAsync( parameters );
                return response.Code == "0";
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 删除载具
        /// </summary>
        /// <param name="loadCode">载具编码</param>
        /// <param name="loadType">载具类型</param>
        /// <returns>删除是否成功</returns>
        public async Task<bool> DeleteLoadAsync( int loadCode , string loadType )
        {
            try
            {
                // 修正：使用 object[] 或者分别定义不同类型
                var parameters = new Dictionary<string , object>
        {
            { "loadCode", loadCode  },
            { "loadType",  loadType }
        };

                var response = await _wmsClient.DeleteLoadAsync( parameters , loadType );
                return response.Code == "0";
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        

        public async Task<TaskDetailResponse> GetTaskStatusAsync( string taskId )
        {
            return await _wmsClient.GetTaskDetailByTaskIdAsync( taskId );
        }

        public async Task<GetLoadsResponse> GetLoadsAsync( )
        {
            return await _wmsClient.GetLoadsAsync();
        }
    }
    public class AddLoadRequest
    {
        /// <summary>
        /// 载具编码
        /// </summary>
        public string LoadCode { get; set; }

        /// <summary>
        /// 载具角度
        /// </summary>
        public string LoadAngle { get; set; }

        /// <summary>
        /// 载具规格编码
        /// </summary>
        public string LoadSpecificationCode { get; set; }

        /// <summary>
        /// 储位编码（载具放置的位置）
        /// </summary>
        public string StorageCode { get; set; }
    }
}
