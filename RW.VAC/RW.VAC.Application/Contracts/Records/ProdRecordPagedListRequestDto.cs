using RW.Framework.Application.Dtos;

namespace RW.VAC.Application.Contracts.Records;

public class ProdRecordPagedListRequestDto : PagedAndSortedResultRequestDto
{
	public string? SerialNumber { get; set; }

	public DateTime?[] DateRange { get; set; }
}