using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RW.Framework.Guids;
using RW.VAC.Application.Contracts.Parameters;
using RW.VAC.Application.Contracts.Records;
using RW.VAC.Application.Services.Parameters;
using RW.VAC.Application.Services.Records;
using RW.VAC.Domain.API;
using RW.VAC.Domain.Records;
using RW.VAC.Infrastructure.Devices;
using RW.VAC.Infrastructure.Opc;
using TouchSocket.Core;
using Ubiety.Dns.Core.Common;
using static System.Collections.Specialized.BitVector32;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RW.VAC.Application.Hardwares.Opc
{
    public class Bedstand(IProductionRecordService productionRecordService,
    IProductionDetailService productionDetailService, IServiceProvider serviceProvider, IAutoAssemblyWorkClient autoAssemblyWorkClient ) 
    {
        Dictionary<string, ProductionBedstandDataresult> keyValues = new Dictionary<string, ProductionBedstandDataresult>();
        public required TagStorage Tags { protected get; init; }
        public async Task OiltightEndofprocess( string num )
        {
           
        }
        public async void OiltightBlanking(TagChangedEventArgs e)
        {
            if (e.Value!=null)
            {
                ProductionDataBedstand._Oiltightblanking=e.Value.ToString();
                if ( e.Value.ToString() == "0" )
                {
                    return;
                }
              await  OiltightEndofprocess( ProductionDataBedstand._Oiltightblanking );
            }
        }
        private bool isFirstAssignment = false;

        /// <summary>
        /// 工序结束事件
        /// </summary>
        /// <param name="e"></param>
        public async Task AirtightEndofprocess( string num )
        {
            
        }

        /// <summary>
        /// 获取下料工位号
        /// </summary>
        public async void AirtightBlanking( TagChangedEventArgs e )
        {
            if ( e.Value != null )
            {

                ProductionDataBedstand._airtightblanking = e.Value.ToString();
                if ( e.Value.ToString() == "0" )
                {
                    return;
                }
               await AirtightEndofprocess( ProductionDataBedstand._airtightblanking );
            }


        }


        private async Task HandleLastStationAsync( string serialNumber , ProductionDetail detail  )
        {
            // 通过序列号将生产记录状态设置为完成
            await productionRecordService.SetCompletedAsync( serialNumber );

            // 如果 detail.EndTime 为 null，则使用当前时间
            var nonNullableEndTime = detail.EndTime ?? DateTime.Now;
        }
    }
}
