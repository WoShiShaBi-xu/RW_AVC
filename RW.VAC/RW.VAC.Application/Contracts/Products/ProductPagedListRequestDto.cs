using RW.Framework.Application.Dtos;

namespace RW.VAC.Application.Contracts.Products;

public class ProductPagedListRequestDto : PagedAndSortedResultRequestDto
{
	public string? Name { get; set; }

	public string? Code { get; set; }
}