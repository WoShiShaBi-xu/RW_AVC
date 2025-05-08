using System.ComponentModel.DataAnnotations;

namespace RW.VAC.Application.Contracts.Opcs;

public class OpcGroupCreateUpdateDto
{
	[Required(ErrorMessage = "请输入分组名称")]
	[MaxLength(50, ErrorMessage = "输入字符超过限制，最大长度50")]
	public string? Name { get; set; }

	[Required(ErrorMessage = "请输入分组编码")]
	[MaxLength(50, ErrorMessage = "输入字符超过限制，最大长度50")]
	public string? Code { get; set; }

	[Required(ErrorMessage = "请输入设备编码")]
	[MaxLength(100, ErrorMessage = "输入字符超过限制，最大长度100")]
	public string? Device { get; set; }

	[MaxLength(50, ErrorMessage = "输入字符超过限制，最大长度50")]
	public string? Group { get; set; }
}