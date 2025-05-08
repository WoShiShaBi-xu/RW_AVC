using FreeSql;
using RW.Framework.Auditing;
using RW.Framework.Domain;
using RW.VAC.Domain.Records;

namespace RW.VAC.Infrastructure.Repositories.Records;

public class ProductionDataRepository(
	IFreeSql freeSql,
	UnitOfWorkManager uowManger,
	IAuditPropertySetter auditPropertySetter)
	: ExtendRepository<ProductionData, Guid>(freeSql, uowManger, auditPropertySetter), IProductionDataRepository
{
}