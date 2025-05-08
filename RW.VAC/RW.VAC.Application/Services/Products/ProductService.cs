using System.Linq.Expressions;
using RW.Framework.Application.Services;
using RW.Framework.Extensions;
using RW.VAC.Application.Contracts.Products;
using RW.VAC.Domain.Products;

namespace RW.VAC.Application.Services.Products;

public class ProductService(
	IProductRepository repository,
	ProductManager productManager) : CrudAppService<Product, Guid, ProductDto, ProductPagedListRequestDto,
	ProductCreateUpdateDto, ProductCreateUpdateDto>(repository), IProductService
{
	public override Task<Product> CreateAsync(ProductCreateUpdateDto input)
	{
		return productManager.CreateAsync(input.Name!, input.Code!, input.Routing, input.Recipe,input.StatisticType);
	}

	public override Task<Product> UpdateAsync(Guid id, ProductCreateUpdateDto input)
	{
		return productManager.UpdateAsync(id, input.Name!, input.Code!, input.Routing, input.Recipe);
	}

    public Task<List<Product>> GetOriginListAsync()
    {
        return repository.Select.ToListAsync();
    }

    protected override Expression<Func<Product, bool>>? GreateFilter(ProductPagedListRequestDto input)
	{
		Expression<Func<Product, bool>>? where = null;
		where = where.And(input.Code.NotNullOrWhiteSpace(), t => t.Code.Contains(input.Code!));
		where = where.And(input.Name.NotNullOrWhiteSpace(), t => t.Name.Contains(input.Name!));
		return where;
	}
}