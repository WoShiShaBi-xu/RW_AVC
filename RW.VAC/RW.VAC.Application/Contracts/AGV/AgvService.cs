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
        Task<bool> SendLoadCommandAsync( string customCode );
       
        Task<TaskDetailResponse> GetTaskStatusAsync( string taskId );
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

        public async Task<bool> SendLoadCommandAsync( string customCode )
        {

            var parameters = new Dictionary<string , int [ ]>
{
    { "chehao", new int[] { 52 } },
    { "target_point", new int[] { 204 } }
};

            var response = await _wmsClient.SendOrderTasksAsync( customCode, parameters );
            return response.Code == "0";
        }

       
        
        public async Task<TaskDetailResponse> GetTaskStatusAsync( string taskId )
        {
            return await _wmsClient.GetTaskDetailByTaskIdAsync( taskId );
        }
    }
}
