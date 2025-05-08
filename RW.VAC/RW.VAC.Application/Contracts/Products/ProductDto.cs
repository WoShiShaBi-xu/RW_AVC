using RW.Framework.Application.Dtos;
using RW.VAC.Domain.Products;

namespace RW.VAC.Application.Contracts.Products;

public class ProductDto : EntityDto<Guid>
{
	public string Name { get; set; }

	public string Code { get; set; }

	public RoutingTypes Routing { get; set; }

	public int? Recipe { get; set; }

	public DateTime CreationTime { get; set; }
}