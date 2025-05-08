using RW.Framework.Application.Services;
using RW.VAC.Domain.Products;

namespace RW.VAC.Application.Contracts.Products;

public interface IProductService : ICrudAppService<Product, Guid, ProductDto, ProductPagedListRequestDto,
	ProductCreateUpdateDto, ProductCreateUpdateDto>
{
    Task<List<Product>> GetOriginListAsync();
}