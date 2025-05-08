using RW.Framework.Application.Services;
using RW.VAC.Domain.Records;

namespace RW.VAC.Application.Contracts.Records;

public interface IProductionDetailService : ICrudAppService<ProductionDetail, Guid, ProductionDetailDto,
	ProdDetailListRequestDto, ProductionDetail, ProductionDetail>
{
	Task<ProductionDetail?> GetByAsync(Guid recordId, string processName);

    Task SetAbnormalOffline(ProductionDetail detail);

    Task SetCompletedAsync(ProductionDetail detail);

	Task SetCompletedWithDataAsync(ProductionDetail detail, List<ProductionDataCreateDto> data);
}