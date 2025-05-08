using FreeSql;
using RW.Framework.Auditing;
using RW.Framework.Domain;
using RW.VAC.Domain.Records;

namespace RW.VAC.Infrastructure.Repositories.Records;

public class ProductionRecordRepository(IFreeSql freeSql, UnitOfWorkManager uowManger, IAuditPropertySetter auditPropertySetter)
	: ExtendRepository<ProductionRecord, Guid>(freeSql, uowManger, auditPropertySetter), IProductionRecordRepository
{
	public async Task<ProductionRecord?> GetByAsync(string serialNumber)
	{
		return await Select.Where(t => t.SerialNumber == serialNumber).ToOneAsync();
	}
}