using FreeSql;
using RW.Framework.Auditing;
using RW.Framework.Domain;
using RW.VAC.Domain.Opcs;

namespace RW.VAC.Infrastructure.Repositories.Opcs;

public class OpcItemRepository(IFreeSql freeSql, UnitOfWorkManager uowManger, IAuditPropertySetter auditPropertySetter)
	: ExtendRepository<OpcItem, Guid>(freeSql, uowManger, auditPropertySetter), IOpcItemRepository
{
	public Task<bool> IsExistNameAsync(Guid groupId, string name)
	{
		return Select.AnyAsync(t => t.GroupId == groupId && t.Name == name);
	}

	public Task<bool> IsExistCodeAsync(Guid groupId, string code)
	{
		return Select.AnyAsync(t => t.GroupId == groupId && t.Code == code);
	}
}