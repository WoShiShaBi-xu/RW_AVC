using FreeSql;
using RW.Framework.Auditing;
using RW.Framework.Domain;
using RW.VAC.Domain.Products;

namespace RW.VAC.Infrastructure.Repositories.Products;

public class ProductRepository(IFreeSql freeSql, UnitOfWorkManager uowManger, IAuditPropertySetter auditPropertySetter)
	: ExtendRepository<Product, Guid>(freeSql, uowManger, auditPropertySetter), IProductRepository
{
	public Task<bool> IsExistNameAsync(string name)
	{
		return Select.AnyAsync(t => t.Name == name);
	}

	public Task<bool> IsExistCodeAsync(string code)
	{
		return Select.AnyAsync(t => t.Code == code);
	}
}