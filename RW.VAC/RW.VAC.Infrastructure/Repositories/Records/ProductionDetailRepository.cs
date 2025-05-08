using FreeSql;
using RW.Framework.Auditing;
using RW.Framework.Domain;
using RW.VAC.Domain.Records;

namespace RW.VAC.Infrastructure.Repositories.Records;

public class ProductionDetailRepository(
	IFreeSql freeSql,
	UnitOfWorkManager uowManger,
	IAuditPropertySetter auditPropertySetter)
	: ExtendRepository<ProductionDetail, Guid>(freeSql, uowManger, auditPropertySetter), IProductionDetailRepository
{
	public async Task<ProductionDetail> GetByAsync(Guid recordId, string processName)
	{
		return await Select.Where(t => t.RecordId == recordId && t.ProcessName == processName).ToOneAsync();
	}
}