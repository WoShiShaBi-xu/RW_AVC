using FreeSql;

namespace RW.VAC.Domain.Records;

public interface IProductionRecordRepository : IBaseRepository<ProductionRecord, Guid>
{
	Task<ProductionRecord?> GetByAsync(string serialNumber);
}