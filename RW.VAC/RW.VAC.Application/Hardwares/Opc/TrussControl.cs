using Microsoft.Extensions.Logging;
using RW.Framework.Guids;
using RW.VAC.Application.Contracts.Records;
using RW.VAC.Domain.Records;
using RW.VAC.Infrastructure.Devices;
using RW.VAC.Infrastructure.Opc;
using TouchSocket.Core;

namespace RW.VAC.Application.Hardwares.Opc;

public class TrussControl(
    CodeQueue codeQueue,
    IProductionRecordService productionRecordService,
    IProductionDetailService productionDetailService,
    IGuidGenerator guidGenerator,
    ILogger<TrussControl> logger)
{
    public required TagStorage Tags { protected get; init; }
    /// <summary>
    ///     上料
    /// </summary>
    /// <param name="e"></param>
    public async void Feed(TagChangedEventArgs e)
    {
		if (!Equals(e.Value, (ushort)1)) return;
        var code = codeQueue.Move();
        if (code.IsNullOrWhiteSpace()) return;
        logger.LogDebug($"真空阀进入钻孔队列，产品序列号：{code}");
		var record = await productionRecordService.GetByAsync(code!);
        if (record == null) return;
        await productionDetailService.CreateAsync(new ProductionDetail(guidGenerator.Greate(), record.Id, "钻孔",
            DateTime.Now));
        record = await productionRecordService.GetByAsync( code! );
        Tags [ "M02_1" , TagTypeConsts.SNTag ]!.Value = code;//值写入到点位，气密写入到对应工位点位
        Tags [ "M02_1" , TagTypeConsts.InboundTimeTag ]!.Value = record?.StartTime;//值写入到点位，气密写入到对应工位点位 
    }

    /// <summary>
    ///     下料
    /// </summary>
    /// <param name="e"></param>
    public async void Lower(TagChangedEventArgs e)
    {
        var value = (ushort)e.Value;
        if (value != 1 && value != 2) return;
        var code = codeQueue.Leave();
        if (code.IsNullOrWhiteSpace()) return;
        logger.LogDebug($"真空阀移除钻孔队列，产品序列号：{code}");
		var record = await productionRecordService.GetByAsync(code!);
        if (record == null) return;
        var detail = await productionDetailService.GetByAsync(record.Id, "钻孔");
        if (detail == null) return;
        if (value == 1)
        {
            await productionDetailService.SetCompletedAsync(detail);
        }
        else
        {
            //await productionDetailService.SetAbnormalOffline(detail);
        }
        Tags [ "M02_1" , TagTypeConsts.OutboundTimeTag ]!.Value = record?.EndTime;//值写入到点位，气密写入到对应工位点位 
    }
}