using RW.VAC.Domain.Users;
using System.ComponentModel.DataAnnotations;

namespace RW.VAC.Application.Contracts.Users;

public class UserCreateUpdateDto
{
	[Required(ErrorMessage = "请输入用户名")]
	[MaxLength(20, ErrorMessage = "输入字符超过限制，最大长度20")]
	public string? UserName { get; set; }

	[Required(ErrorMessage = "请输入账号")]
	[MaxLength(20, ErrorMessage = "输入字符超过限制，最大长度20")]
	public string? Account { get; set; }

	public string? Password { get; set; }

	public RoleTypes Role { get; set; }
}