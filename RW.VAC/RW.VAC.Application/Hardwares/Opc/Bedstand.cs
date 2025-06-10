using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RW.Framework.Guids;
using RW.VAC.Application.Contracts.Parameters;

using RW.VAC.Domain.API;
using RW.VAC.Infrastructure.Devices;
using RW.VAC.Infrastructure.Opc;
using TouchSocket.Core;
using Ubiety.Dns.Core.Common;
using static System.Collections.Specialized.BitVector32;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RW.VAC.Application.Hardwares.Opc
{
    public class Bedstand(
   IServiceProvider serviceProvider, IAutoAssemblyWorkClient autoAssemblyWorkClient ) 
    {
       
        public required TagStorage Tags { protected get; init; }
        public async Task OiltightEndofprocess( string num )
        {
           
        }
        public async void OiltightBlanking(TagChangedEventArgs e)
        {
          
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

        }


        private async Task HandleLastStationAsync( string serialNumber)
        {
          
        }
    }
}
