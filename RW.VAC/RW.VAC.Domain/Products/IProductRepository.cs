using FreeSql;

namespace RW.VAC.Domain.Products;

public interface IProductRepository : IBaseRepository<Product, Guid>
{
	Task<bool> IsExistNameAsync(string name);

	Task<bool> IsExistCodeAsync(string code);
}