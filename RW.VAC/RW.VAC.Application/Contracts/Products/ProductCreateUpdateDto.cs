using System.ComponentModel.DataAnnotations;
using RW.VAC.Domain.Products;

namespace RW.VAC.Application.Contracts.Products;

public class ProductCreateUpdateDto
{
	[Required(ErrorMessage = "请输入产品名称")]
	[MaxLength(50, ErrorMessage = "输入字符超过限制，最大长度50")]
	public string? Name { get; set; }

	[Required(ErrorMessage = "请输入产品编码")]
	[MaxLength(50, ErrorMessage = "输入字符超过限制，最大长度50")]
	public string? Code { get; set; }

	public RoutingTypes Routing { get; set; }

	public int? Recipe { get; set; }

	public string? StatisticType { get; set;}
}