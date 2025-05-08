using RW.VAC.Application.Contracts.Records;
using RW.VAC.Application.Contracts.Users;
using RW.VAC.Application.Services.Users;
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
	public class ArtificialBitCodeReader(
	IProductionRecordService productionRecordService ,
	IProductionDetailService productionDetailService ,
    IAutoAssemblyWorkClient autoAssemblyWorkClient,

        IServiceProvider serviceProvider
    )
	: GeneralCodeReader( productionRecordService , productionDetailService, autoAssemblyWorkClient , serviceProvider )
	{
		//protected override Task Finish( ProductionDetail detail )
		//{
		////	var data = new List<ProductionDataCreateDto>
		////{
				
		////	new("人员:",username, "")
			
		////};
		////	return ProductionDetailService.SetCompletedWithDataAsync( detail , data );
		//}
	}
}
