using RW.Framework.Application.Dtos;
using RW.VAC.Domain.Users;

namespace RW.VAC.Application.Contracts.Users;

public class UserDto : EntityDto<Guid>
{
	public string UserName { get; set; }

	public string Account { get; set; }

	public RoleTypes Role { get; set; }

	public bool IsSystemUser { get; set; }
}