using System.ComponentModel.DataAnnotations;

namespace RW.VAC.Application.Contracts.Parameters;

public class ParameterCreateUpdateDto
{

	[Required(ErrorMessage = "请输入参数编码")]
	[MaxLength(50, ErrorMessage = "输入字符超过限制，最大长度50")]
	public string? Code { get; set; }

	[Required(ErrorMessage = "请输入参数值")]
	[MaxLength(500, ErrorMessage = "输入字符超过限制，最大长度200")]
	public string? Value { get; set; }

	[MaxLength(100, ErrorMessage = "输入字符超过限制，最大长度100")]
	public string? Description { get; set; }
}