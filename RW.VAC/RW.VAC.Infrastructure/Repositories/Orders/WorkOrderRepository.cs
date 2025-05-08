using FreeSql;
using RW.Framework.Auditing;
using RW.Framework.Domain;
using RW.VAC.Domain.Orders;

namespace RW.VAC.Infrastructure.Repositories.Orders;

public class WorkOrderRepository(
	IFreeSql freeSql,
	UnitOfWorkManager uowManger,
	IAuditPropertySetter auditPropertySetter)
	: ExtendRepository<WorkOrder, Guid>(freeSql, uowManger, auditPropertySetter), IWorkOrderRepository
{
}