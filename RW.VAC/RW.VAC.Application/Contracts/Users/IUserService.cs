using RW.Framework.Application.Services;
using RW.VAC.Domain.Users;
using System.Security.Claims;

namespace RW.VAC.Application.Contracts.Users;

public interface IUserService : ICrudAppService<User, Guid, UserDto, UserPagedListRequestDto, UserCreateUpdateDto,
	UserCreateUpdateDto>
{
	Task<ClaimsIdentity> GetIdentityAsync(LoginDto dto);
}