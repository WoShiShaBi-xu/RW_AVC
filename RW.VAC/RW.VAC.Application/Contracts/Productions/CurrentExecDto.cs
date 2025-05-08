using System.ComponentModel.DataAnnotations;

namespace RW.VAC.Application.Contracts.Productions;

public class CurrentExecDto
{
	[Required(ErrorMessage = "请选择产品信息")]
	public Guid? ProductId { get; set; }

	public int? Recipe { get; set; }
    [Required( ErrorMessage = "请选择统计方式" )]
    public string? StatisticType { get; set; }
}