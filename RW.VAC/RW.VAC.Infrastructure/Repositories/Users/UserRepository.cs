using FreeSql;
using RW.Framework.Auditing;
using RW.Framework.Domain;
using RW.VAC.Domain.Users;

namespace RW.VAC.Infrastructure.Repositories.Users;

public class UserRepository(IFreeSql freeSql, UnitOfWorkManager uowManger, IAuditPropertySetter auditPropertySetter)
	: ExtendRepository<User, Guid>(freeSql, uowManger, auditPropertySetter), IUserRepository
{
	public Task<bool> IsExistAccountAsync(string account)
	{
		return Select.AnyAsync(t => t.Account == account);
	}
}