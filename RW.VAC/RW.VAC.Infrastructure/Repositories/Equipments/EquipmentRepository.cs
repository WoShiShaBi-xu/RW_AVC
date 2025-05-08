using FreeSql;
using RW.Framework.Auditing;
using RW.Framework.Domain;
using RW.VAC.Domain.Equipments;

namespace RW.VAC.Infrastructure.Repositories.Equipments;

public class EquipmentRepository(
	IFreeSql freeSql,
	UnitOfWorkManager uowManger,
	IAuditPropertySetter auditPropertySetter)
	: ExtendRepository<Equipment, Guid>(freeSql, uowManger, auditPropertySetter), IEquipmentRepository
{
}