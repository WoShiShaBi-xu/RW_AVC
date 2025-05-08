using RW.Framework.Application.Services;
using RW.VAC.Domain.Records;

namespace RW.VAC.Application.Contracts.Records;

public interface IProductionDataService : ICrudAppService<ProductionData, Guid, ProductionDataDto, ProdDataListRequestDto, ProductionData, ProductionData>
{
}