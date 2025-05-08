using System.Linq.Expressions;
using RW.Framework.Application.Services;
using RW.VAC.Application.Contracts.Records;
using RW.VAC.Domain.Records;

namespace RW.VAC.Application.Services.Records;

public class ProductionDataService(IProductionDataRepository repository) :
	CrudAppService<ProductionData, Guid, ProductionDataDto, ProdDataListRequestDto, ProductionData, ProductionData>(repository),
	IProductionDataService
{
	protected override Expression<Func<ProductionData, bool>>? GreateFilter(ProdDataListRequestDto input)
	{
		Expression<Func<ProductionData, bool>>? where = null;
		where = where.And(input.DetailId.HasValue, t => t.DetailId == input.DetailId!.Value);
		return where;
	}
}