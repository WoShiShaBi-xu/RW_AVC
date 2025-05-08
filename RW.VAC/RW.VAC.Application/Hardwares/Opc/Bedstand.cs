using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RW.Framework.Guids;
using RW.VAC.Application.Contracts.Parameters;
using RW.VAC.Application.Contracts.Records;
using RW.VAC.Application.Hardwares.CodeReader;
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
            //获取工位号，然后从工位号获取到对应参数

            var code = Tags [ "M12" , num + "_itemNumber" ]?.Value;
            // 获取相应的生产记录，如果不存在则返回
            if ( code == null )
                return;
            var record = await productionRecordService.GetByAsync( code.ToString() );
            if ( record == null )
                return;
            // 获取相应的生产详细信息，如果不存在则返回
            var detail = await productionDetailService.GetByAsync( record.Id , "液压试验" );
            if ( detail == null )
                return;
            // 根据事件值设置生产详细信息的状态

            var data = new List<ProductionDataCreateDto>
                {
                    new("测试结果", Tags["M12", num+"_Result"]?.Value?.ToString() ?? "N/A", ""),
                    new("泄露量", Tags["M12", num+"_Flowrate"]?.Value?.ToString() ?? "N/A", ""),
                 };

            string number = "0";
            var 设定压力 = Tags [ "M12" , num + "_PressureValue" ]?.Value?.ToString() ?? "N/A";
            var 实际压力 = Tags [ "M12" , num + "_RealTimePressure" ]?.Value?.ToString() ?? "N/A";
            var 设定泄露= Tags [ "M12" , num + "_LeakageValue" ]?.Value?.ToString() ?? "N/A";
            var 实际泄露= Tags [ "M12" , num + "_Flowrate" ]?.Value?.ToString() ?? "N/A";
            var 保压时间= Tags [ "M12" , num + "_HoldingTime" ]?.Value?.ToString() ?? "N/A";
            var 测试时间= DateTime.Now.ToString(  );
            var 测试工位 = num;

            await productionDetailService.SetCompletedWithDataAsync( detail , data );//油密设置完成

            var parameterService = serviceProvider.GetRequiredService<IParameterService>();
            var parameterList = await parameterService.GetListAsync();
            foreach ( var param in parameterList )
            {
                if ( param.Code == "StatisticType" )
                {
                    number = param.Value;
                    break;
                }
            }
            if ( number =="4")
            {
              await  HandleLastStationAsync( code.ToString() , detail);
            }
            //上传数据

            OilPressureData oilPressureData = new OilPressureData();
            oilPressureData.SN = code.ToString();
            oilPressureData.SetPressure = 设定压力;
            oilPressureData.RealTimePressure = 实际压力;
            oilPressureData.StdLeakage = 设定泄露;
            oilPressureData.LeakageFlow = 实际泄露;
            oilPressureData.HoldingTime = 保压时间;
            oilPressureData.CheckDate = 测试时间;
            oilPressureData.Station = 测试工位;

            await autoAssemblyWorkClient.OilPressureDataAPI( oilPressureData );
            await autoAssemblyWorkClient.ReportWorkAsync( "80" , code.ToString() , "YY" , 测试时间.ToString() , GeneralCodeReader.username , true );
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
            //获取工位号，然后从工位号获取到对应参数
            var code = Tags [ "M09" , num + "_itemNumber" ]?.Value;
            // 获取相应的生产记录，如果不存在则返回
            if ( code == null )
                return;
            var record = await productionRecordService.GetByAsync( code.ToString() );
            if ( record == null )
                return;

            // 获取相应的生产详细信息，如果不存在则返回
            var detail = await productionDetailService.GetByAsync( record.Id , "气密性试验" );
            if ( detail == null )
                return;

            // 根据事件值设置生产详细信息的状态

            var data = new List<ProductionDataCreateDto>
                {
                    new("A测结果", Tags["M09", num+"_Aresult"]?.Value?.ToString() ?? "N/A", ""),
                    new("A测压力值", Tags["M09", num+"_AleakageRate"]?.Value?.ToString() ?? "N/A", "Pa"),
                    new("B测结果", Tags["M09", num+"_Bresult"]?.Value?.ToString() ?? "N/A", ""),
                    new("B测压力值", Tags["M09",num+"_BleakageRate"]?.Value?.ToString() ?? "N/A", "Pa"),
                     new("测试气压", Tags["M09", num+"_Testpressure"]?.Value?.ToString() ?? "N/A", "Mpa")

                 };
            await productionDetailService.SetCompletedWithDataAsync( detail , data );
            //上传数据
            AirtightData airtightData = new AirtightData();
            airtightData.SN = code.ToString();
            foreach ( var item in data )
            {
                if(item.Name== "A测结果" )
                {
                    airtightData.Result_A = item.Value;
                }
                if ( item.Name == "B测结果" )
                {
                    airtightData.Result_B = item.Value;
                }
                if ( item.Name == "A测压力值" )
                {
                    airtightData.Divulge_A = item.Value;
                }
                if ( item.Name == "B测压力值" )
                {
                    airtightData.Divulge_B = item.Value;
                }
                if ( item.Name == "测试气压" )
                {
                    airtightData.Pressure = item.Value;
                }
            }

            airtightData.Station = num;
            airtightData.CheckDate = DateTime.Now.ToString();
            await autoAssemblyWorkClient.AirtightDataAPI( airtightData );
            await autoAssemblyWorkClient.ReportWorkAsync( "80" , code.ToString() , "QMSY" , airtightData.CheckDate , GeneralCodeReader.username , true );
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
