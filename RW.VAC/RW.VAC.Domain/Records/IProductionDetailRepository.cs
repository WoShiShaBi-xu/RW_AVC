using FreeSql;

namespace RW.VAC.Domain.Records;

public interface IProductionDetailRepository : IBaseRepository<ProductionDetail, Guid>
{
	Task<ProductionDetail> GetByAsync(Guid recordId, string processName);
}