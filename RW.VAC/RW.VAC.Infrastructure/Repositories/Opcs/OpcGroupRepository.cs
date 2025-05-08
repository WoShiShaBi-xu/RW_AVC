using FreeSql;
using RW.Framework.Auditing;
using RW.Framework.Domain;
using RW.VAC.Domain.Opcs;

namespace RW.VAC.Infrastructure.Repositories.Opcs;

public class OpcGroupRepository(IFreeSql freeSql, UnitOfWorkManager uowManger, IAuditPropertySetter auditPropertySetter)
	: ExtendRepository<OpcGroup, Guid>(freeSql, uowManger, auditPropertySetter), IOpcGroupRepository
{
	public Task<bool> IsExistNameAsync(string name)
	{
		return Select.AnyAsync(t => t.Name == name);
	}

	public Task<bool> IsExistCodeAsync(string code)
	{
		return Select.AnyAsync(t => t.Code == code);
	}
}