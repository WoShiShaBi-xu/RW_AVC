using RW.VAC.Application.Contracts.Records;
using RW.VAC.Domain.API;
using RW.VAC.Domain.Records;
using RW.VAC.Infrastructure.Opc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Application.Hardwares.CodeReader
{
    public class SwitchingCodeReader(
    IProductionRecordService productionRecordService,
    IProductionDetailService productionDetailService,
    IAutoAssemblyWorkClient autoAssemblyWorkClient,
    IServiceProvider serviceProvider )
    : GeneralCodeReader(productionRecordService, productionDetailService, autoAssemblyWorkClient , serviceProvider )
    {
        //public required TagStorage Tags { protected get; init; }

        protected override Task Finish(ProductionDetail detail)
        {
            var data = new List<ProductionDataCreateDto>
        {
            new("扭矩", Tags["M10", "Torsion"]?.Value?.ToString() ?? "N/A", "Nm"),
            new("角度", Tags["M10", "Displacement"]?.Value?.ToString() ?? "N/A", "°"),
        };
            

            return ProductionDetailService.SetCompletedWithDataAsync(detail, data);
        }
    }
}
