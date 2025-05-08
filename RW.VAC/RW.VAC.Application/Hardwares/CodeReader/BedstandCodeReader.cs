using Microsoft.Extensions.Logging;
using RW.VAC.Application.Contracts.Records;
using RW.VAC.Domain.API;
using RW.VAC.Domain.Products;
using RW.VAC.Domain.Records;
using RW.VAC.Infrastructure.Devices;
using RW.VAC.Infrastructure.Opc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Application.Hardwares.CodeReader
{
    public class BedstandCodeReader(
    IProductionRecordService productionRecordService ,
    IProductionDetailService productionDetailService ,
    IAutoAssemblyWorkClient autoAssemblyWorkClient ,
    IServiceProvider serviceProvider )
    : GeneralCodeReader( productionRecordService , productionDetailService , autoAssemblyWorkClient , serviceProvider ), ICodeReader
    {
        public required CodeQueue CodeQueue { private get; init; }
        //public required TagStorage Tags { protected get; init; }
        public async Task MakeRecord( string serialNumber , string processName , Product product )
        {
            // 尝试通过序列号获取生产记录
            var record = await ProductionRecordService.GetByAsync( serialNumber );
            if ( record == null )
            {
                // 如果记录不存在，创建一个新的生产记录 DTO，并调用服务创建记录及其详细信息
                var recordDto = new ProductionRecordCreateDto( serialNumber , product.Name , product.Code , product.Routing ,
                    [ processName ] );
                await ProductionRecordService.CreateWithDetailAsync( recordDto );
                record = await ProductionRecordService.GetByAsync( serialNumber );
                var detail = await ProductionDetailService.GetByAsync( record.Id , processName );
                if ( detail == null )
                {
                    // 如果详情不存在，创建新的生产详情
                    await ProductionDetailService.CreateAsync( new ProductionDetail( GuidGenerator.Greate() , record.Id ,
                        processName , DateTime.Now ) );
                }
            }
            else
            {
                // 获取生产详情
                var detail = await ProductionDetailService.GetByAsync( record.Id , processName );
                if ( detail == null )
                {
                    // 如果详情不存在，创建新的生产详情
                    await ProductionDetailService.CreateAsync( new ProductionDetail( GuidGenerator.Greate() , record.Id ,
                        processName , DateTime.Now ) );
                }
            }
            if ( processName == "气密性试验" )
            {
                Tags [ "M09" , TagTypeConsts.ScanningDataTag ]!.Value = serialNumber;//值写入到点位，气密写入到对应工位点位
                CodeQueue.Entry( serialNumber );
            }
            if ( product.Routing == RoutingTypes.Seal )
            {
                if ( processName == "液压试验" )
                {
                    Tags [ "M12" , TagTypeConsts.ScanningDataTag ]!.Value = serialNumber;//值写入到点位，气密写入到对应工位点位
                    CodeQueue.Entry( serialNumber );
                }
            }
        }

    }
}
