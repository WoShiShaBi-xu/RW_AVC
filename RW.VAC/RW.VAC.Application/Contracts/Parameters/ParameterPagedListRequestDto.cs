using RW.Framework.Application.Dtos;

namespace RW.VAC.Application.Contracts.Parameters;

public class ParameterPagedListRequestDto : PagedAndSortedResultRequestDto
{
	public string? Code { get; set; }
}