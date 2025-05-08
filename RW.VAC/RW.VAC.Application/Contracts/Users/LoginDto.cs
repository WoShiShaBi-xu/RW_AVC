using System.ComponentModel.DataAnnotations;

namespace RW.VAC.Application.Contracts.Users;

public class LoginDto
{
	[Required(ErrorMessage = "请输入账号")]
	[MaxLength(20, ErrorMessage = "输入字符超过限制，最大长度20")]
	public string Account { get; set; }

	[Required(ErrorMessage = "请输入密码")]
	public string Password { get; set; }
}