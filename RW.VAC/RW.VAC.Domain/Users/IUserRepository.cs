using FreeSql;

namespace RW.VAC.Domain.Users;

public interface IUserRepository : IBaseRepository<User, Guid>
{
	Task<bool> IsExistAccountAsync(string account);
}