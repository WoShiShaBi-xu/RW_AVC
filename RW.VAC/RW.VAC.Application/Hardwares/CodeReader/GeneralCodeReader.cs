using Microsoft.Extensions.DependencyInjection;
using RW.Framework.Guids;
using RW.VAC.Application.Contracts.Parameters;
using RW.VAC.Application.Contracts.Records;
using RW.VAC.Domain.API;
using RW.VAC.Domain.Products;
using RW.VAC.Domain.Records;
using RW.VAC.Infrastructure.Opc;
using System;
using System.Data;
using Ubiety.Dns.Core.Common;

namespace RW.VAC.Application.Hardwares.CodeReader;

// 通用条码读取器类，继承了 ICodeReader 接口
public class GeneralCodeReader(
    IProductionRecordService productionRecordService , // 生产记录服务接口
    IProductionDetailService productionDetailService , // 生产详情服务接口
    IAutoAssemblyWorkClient autoAssemblyWorkClient ,   // 自动组装工作客户端
    IServiceProvider serviceProvider )                 // 服务提供器
    : ICodeReader
{
    public static string? username; // 用户名（静态变量）
    protected readonly IProductionDetailService ProductionDetailService = productionDetailService;
    protected readonly IProductionRecordService ProductionRecordService = productionRecordService;
    protected readonly IServiceProvider ServiceProvider = serviceProvider;
    public required IGuidGenerator GuidGenerator { protected get; init; }
    public required TagStorage Tags { protected get; init; }

    // 创建生产记录方法
    public async Task MakeRecord( string serialNumber , string processName , Product product )
    {
        // 通过序列号获取生产记录
        var record = await ProductionRecordService.GetByAsync( serialNumber );
        if ( record == null )
        {
            // 如果记录不存在，创建新生产记录
            var recordDto = new ProductionRecordCreateDto(
                serialNumber ,
                product.Name ,
                product.Code ,
                product.Routing ,
                new List<string> { processName }
            );
            await ProductionRecordService.CreateWithDetailAsync( recordDto );

            // 获取新创建的记录并设置标签
            var records = await ProductionRecordService.GetByAsync( serialNumber );
            SetTagValuesForProcess( serialNumber , records.StartTime , processName );
        }
        else
        {
            // 获取生产详情
            var detail = await ProductionDetailService.GetByAsync( record.Id , processName );
            string number = "0";
            if ( detail == null )
            {
                // 如果详情不存在，创建新的生产详情
                await ProductionDetailService.CreateAsync( new ProductionDetail( GuidGenerator.Greate() , record.Id ,
                    processName , DateTime.Now ) );

                detail = await ProductionDetailService.GetByAsync( record.Id , processName );// 获取生产详情
                if (detail!=null) {
                    SetTagValuesForProcess(serialNumber, detail.StartTime, processName);
                }
            }
            else
            {
                // 如果详情已存在，计算开始时间差
                var timeDifference = DateTime.Now - detail.StartTime;
                // 从服务提供器获取参数服务，并检索参数列表
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
                // 如果时间差小于10秒或状态为已完成，则返回
                if ( timeDifference.TotalSeconds < 5 || detail.Status == ProcessStatus.Completed )
                {
                    return;
                }
                if ( processName == "开关测试" && number == "3" )
                {

                    var reportTypes = GetReportTypeByProcessName( processName );
                    await HandleLastStationAsync( serialNumber , detail , "KG" , "M10_1" );//设置完成并上报
                    await ReportWorkAndHandleResult( serialNumber , reportTypes , detail );
                    SetTagValuesForProces(serialNumber, DateTime.Now, processName);
                    return;
                }
                else if ( processName == "阀盖组装" && number == "2" )
                {
                    await HandleLastStationAsync( serialNumber , detail , "FG" , "M15_1" );//设置完成并上报
                    SetTagValuesForProces(serialNumber, DateTime.Now, processName);
                    return;

                }

                var reportType = GetReportTypeByProcessName( processName );
                SetTagValuesForProces( serialNumber , DateTime.Now , processName );
                await autoAssemblyWorkClient.ReportWorkAsync( "80" , serialNumber , reportType , DateTime.Now.ToString() , username , true );
                await Finish( detail );
            }
        }
    }
    private static Dictionary<(string SerialNumber, string ProcessName), DateTime?> _inboundTimeStore
        = new();
    // 设置工艺流程的标签值
    private void SetTagValuesForProcess( string serialNumber , DateTime? time , string processName )
    {
        var tagMapping = new Dictionary<string , string>
        {
            { "推杆组装", "M04_1" },
            { "推杆紧固", "M06_1" },
            { "压圈螺母组装", "M08_1" },
            { "开关测试", "M10_1" },
            { "指示牌组装", "M11_1" },
            { "贴标", "M14_1" },
            { "阀盖组装", "M15_1" }
        };

        // 根据工艺流程名称查找对应的标签，并设置标签的序列号和入站时间
        if ( tagMapping.TryGetValue( processName , out var tag ) )
        {
            if (time != null)
            {
                _inboundTimeStore.Add((serialNumber, processName), time);
            }

            // 存储入站时间到字典（键由序列号和流程名称组成）
           
            //Tags [ $"{tag}" , TagTypeConsts.SNTag ]!.Value = serialNumber;
            //Tags [ $"{tag}" , TagTypeConsts.InboundTimeTag ]!.Value = time.ToString();
        }
    }

    private void SetTagValuesForProces( string serialNumber , DateTime? time , string processName )
    {
        var tagMapping = new Dictionary<string , string>
        {
            { "推杆组装", "M04_1" },
            { "推杆紧固", "M06_1" },
            { "压圈螺母组装", "M08_1" },
            { "开关测试", "M10_1" },
            { "指示牌组装", "M11_1" },
            { "贴标", "M14_1" },
            { "阀盖组装", "M15_1" }
        };

        // 根据工艺流程名称查找对应的标签，并设置标签的序列号和入站时间
        if ( tagMapping.TryGetValue( processName , out var tag ) )
        {
            //Tags [ $"{tag}" , TagTypeConsts.SNTag ]!.Value = serialNumber;
            
            if (_inboundTimeStore.TryGetValue((serialNumber, processName), out var inboundTime))
            {
                if (time!=null)
                {
                    // 计算处理时长（示例）
                    if (time.HasValue && inboundTime.HasValue)
                    {
                        Tags[$"{tag}", TagTypeConsts.SNTag]!.Value = serialNumber;
                        Tags[$"{tag}", TagTypeConsts.InboundTimeTag]!.Value = inboundTime.Value.ToString();
                        Tags[$"{tag}", TagTypeConsts.OutboundTimeTag]!.Value = time.ToString();
                    }
                }
            }
        }
    }

    // 处理最后一个工位的方法
    private async Task HandleLastStationAsync( string serialNumber , ProductionDetail detail , string reportType , string tag )
    {
        // 通过序列号将生产记录状态设置为完成
        await productionRecordService.SetCompletedAsync( serialNumber );

        // 如果 detail.EndTime 为 null，则使用当前时间
        var nonNullableEndTime = detail.EndTime ?? DateTime.Now;

        // 将标签的出站时间设置为 nonNullableEndTime 的字符串表示
        Tags [ $"{tag}" , TagTypeConsts.OutboundTimeTag ]!.Value = nonNullableEndTime.ToString();

        await Finish( detail );
        // 调用 autoAssemblyWorkClient 上报工作状态

        var result = await autoAssemblyWorkClient.ReportWorkAsync( "80" , serialNumber , reportType , nonNullableEndTime.ToString() , username , true );
        // 如果上报没有错误，调用 Finish 方法完成详细记录


    }

    // 根据工艺流程名称获取报告类型
    private string GetReportTypeByProcessName( string processName )
    {
        var reportTypeMapping = new Dictionary<string , string>
        {
            { "推杆组装", "YPZZ" },
            { "推杆紧固", "YPJG" },
            { "压圈螺母组装", "YQLM" },
            { "开关测试", "KG" },
            { "指示牌组装", "ZSP" },
            { "贴标", "TB" }
        };

        return reportTypeMapping.ContainsKey( processName ) ? reportTypeMapping [ processName ] : string.Empty;
    }

    // 报告工作并处理结果
    private async Task ReportWorkAndHandleResult( string serialNumber , string reportType , ProductionDetail detail )
    {
        if ( !string.IsNullOrEmpty( reportType ) && detail != null )
        {
            await Finish( detail );
            TorqueData torque=new TorqueData();
            torque.SN = serialNumber;
            torque.TorqueSet = Tags [ "M10" , "SettingTorsion" ]?.Value?.ToString() ?? "N/A";
            torque.TorqueValue= Tags [ "M10" , "Torsion" ]?.Value?.ToString() ?? "N/A";
            torque.Displacement= Tags [ "M10" , "Displacement" ]?.Value?.ToString() ?? "N/A";
            torque.CheckDate= DateTime.Now.ToString();
            torque.Station ="1";
            await autoAssemblyWorkClient.TorqueDataAPI( torque );
            
        }
        else
        {
            
        }
    }


    // 完成生产详情记录的方法
    protected virtual Task Finish( ProductionDetail detail )
    {
        return ProductionDetailService.SetCompletedAsync( detail );
    }

}
