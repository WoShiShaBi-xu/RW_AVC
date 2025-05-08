using RW.Framework.Domain.Services;
using RW.Framework.Exceptions;

namespace RW.VAC.Domain.Users;

public class UserManager(IUserRepository userRepository) : DomainService
{
	public async Task<User> CreateAsync(string userName, string account, string password,RoleTypes role)
	{
		await CheckAccount(account);
		var user = new User(GuidGenerator.Greate(), userName, account, password, role , false);
		await userRepository.InsertAsync(user);
		return user;
	}

	public async Task<User> UpdateAsync(Guid id, string userName, string account, string? password,RoleTypes role)
	{
		var user = await userRepository.GetAsync(id);
		await CheckAccount(account, user);
		user.Update(userName, account, password, role );
		await userRepository.UpdateAsync(user);
		return user;
	}

	private async Task CheckAccount(string account, User? user = null)
	{
		if(user!=null && string.Equals(user.Account, account, StringComparison.OrdinalIgnoreCase)) return;
		if(await userRepository.IsExistAccountAsync(account)) throw new BusinessException("存在相同的参数编码").WithData("Account", account);
	}
}