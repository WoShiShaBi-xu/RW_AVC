using Microsoft.Extensions.Logging;
using RW.Framework.Guids;
using RW.VAC.Application.Contracts.Records;
using RW.VAC.Application.Services.Records;
using RW.VAC.Domain.API;
using RW.VAC.Domain.Products;
using RW.VAC.Domain.Records;
using RW.VAC.Infrastructure.Devices;
using RW.VAC.Infrastructure.Opc;

// 命名空间 RW.VAC.Application.Hardwares.CodeReader 定义了硬件设备 CodeReader 的相关逻辑
namespace RW.VAC.Application.Hardwares.CodeReader
{
    // FeedCodeReader 类实现了 ICodeReader 接口，负责处理上料工序中条码读取的逻辑
    public class FeedCodeReader(IProductionRecordService productionRecordService, ILogger<FeedCodeReader> logger, IAutoAssemblyWorkClient autoAssemblyWorkClient, IProductionDetailService productionDetailService) : ICodeReader
    {
        // 服务注入：生产明细服务、生产记录服务、自动装配工作客户端
        protected readonly IProductionDetailService ProductionDetailService = productionDetailService;
        protected readonly IProductionRecordService ProductionRecordService = productionRecordService;
        protected readonly IAutoAssemblyWorkClient AutoAssemblyWorkClient = autoAssemblyWorkClient;
        public required IGuidGenerator GuidGenerator { protected get; init; }

        // 必须初始化的属性：条码队列和点位存储
        public required CodeQueue CodeQueue { private get; init; }
        public required TagStorage Tags { protected get; init; }

        private static  Dictionary<(string SerialNumber, string ProcessName), DateTime?> _inboundTimeStore
            = new Dictionary<(string, string), DateTime?>();

        // 主要方法：根据传入的产品序列号、工序名称和产品信息生成记录
        public async Task MakeRecord(string serialNumber, string processName, Product product)
        {

            // 如果当前工序是 "上料"
            if (processName == "上料")
            {
                // 创建生产记录 DTO 包含序列号、产品名称、产品代码、产品路由及工序名称

                // 获取当前序列号的生产记录
                var records = await ProductionRecordService.GetByAsync(serialNumber);
                if (records == null)
                {
                    var recordDto = new ProductionRecordCreateDto(serialNumber, product.Name, product.Code, product.Routing, [processName]);
                    // 通过生产记录服务创建记录并包含详细信息
                    await productionRecordService.CreateWithDetailAsync(recordDto, true);
                    records = await ProductionRecordService.GetByAsync(serialNumber);
                    // 通过自动装配客户端查询序列号的相关信息（从 API 获取数据）
                    var result = await AutoAssemblyWorkClient.QuerySNInfo(serialNumber, records.ProductName);

                    // 获取当前工序对应的生产明细
                    var detail = await ProductionDetailService.GetByAsync(records.Id, processName);

                    // 向装配客户端报告上料状态，包括时间、操作员信息

                    await autoAssemblyWorkClient.ReportWorkAsync("80", serialNumber, "SL", DateTime.Now.ToString(), GeneralCodeReader.username, true);

                    // 将序列号写入指定点位，记录时间写入点位
                    Tags["M01_1", TagTypeConsts.SNTag]!.Value = serialNumber;
                    Tags["M01_1", TagTypeConsts.InboundTimeTag]!.Value = records.StartTime.ToString();
                    if (product.Routing == RoutingTypes.Vacuum)
                    {
                        //Tags["M02_1", TagTypeConsts.SNTag]!.Value = serialNumber;

                        _inboundTimeStore.Add((serialNumber, processName), DateTime.Now);
                        //Tags [ "M02_1" , TagTypeConsts.InboundTimeTag ]!.Value =DateTime.Now.ToString();
                    }

                    // 调用完成方法，设置明细状态为完成
                    await Finish(detail);

                }
                else if (product.Routing == RoutingTypes.Vacuum) // 如果产品的路由类型为真空
                {//创建钻孔生产记录
                 // 创建或获取生产详情
                    var record = await ProductionRecordService.GetByAsync(serialNumber);
                    if (record == null)
                    {
                        logger.LogWarning($"没有找到连续生产记录 number: {serialNumber}");
                        return;
                    }

                    var detail = await ProductionDetailService.GetByAsync(record.Id, "钻孔");
                    if (detail == null)
                    {
                        detail = new ProductionDetail(GuidGenerator.Greate(), record.Id, "钻孔", DateTime.Now);
                        await ProductionDetailService.CreateAsync(detail);
                    }

                    // 报告状态
                    await autoAssemblyWorkClient.ReportWorkAsync("80", serialNumber, "ZK", DateTime.Now.ToString(), GeneralCodeReader.username, true);
                   
                    if (_inboundTimeStore.TryGetValue((serialNumber, processName), out var inboundTime))
                    {
                        // 计算处理时长（示例）
                        Tags[$"M02_1", TagTypeConsts.SNTag]!.Value = serialNumber;
                        Tags[$"M02_1", TagTypeConsts.InboundTimeTag]!.Value = inboundTime.Value.ToString();
                        Tags[$"M02_1", TagTypeConsts.OutboundTimeTag]!.Value = DateTime.Now.ToString();

                    }
                    // 标记完成
                    await Finish(detail);

                }
            }
        }

        // 设置生产明细完成状态
        protected virtual Task Finish(ProductionDetail detail)
        {
            return ProductionDetailService.SetCompletedAsync(detail);
        }
    }
}
