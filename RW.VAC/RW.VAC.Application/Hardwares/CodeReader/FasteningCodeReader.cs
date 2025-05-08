using RW.VAC.Application.Contracts.Products;
using RW.VAC.Application.Contracts.Records;
using RW.VAC.Domain.API;
using RW.VAC.Domain.Records;
using RW.VAC.Infrastructure.Opc;

namespace RW.VAC.Application.Hardwares.CodeReader;

public class FasteningCodeReader(
	IProductionRecordService productionRecordService,
	IProductionDetailService productionDetailService,
    IAutoAssemblyWorkClient autoAssemblyWorkClient ,
    IServiceProvider serviceProvider )
	: GeneralCodeReader(productionRecordService, productionDetailService, autoAssemblyWorkClient, serviceProvider )
{
	//public required TagStorage Tags { protected get; init; }

	protected override Task Finish(ProductionDetail detail)
	{
		var data = new List<ProductionDataCreateDto>
		{
			new("磨合叶片正向最大扭矩", Tags["M06", "Torsion1"]?.Value?.ToString() ?? "N/A", "Nm"),
			new("关闭叶片最大扭矩", Tags["M06", "Torsion2"]?.Value?.ToString() ?? "N/A", "Nm"),
            new("磨合叶片反向最大扭矩", Tags["M06", "Torsion3"]?.Value?.ToString() ?? "N/A", "Nm")
        };
		return ProductionDetailService.SetCompletedWithDataAsync(detail, data);
	}
}