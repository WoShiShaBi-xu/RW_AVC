using RW.Framework.Domain.Services;
using RW.Framework.Exceptions;

namespace RW.VAC.Domain.Products;

public class ProductManager(IProductRepository productRepository) : DomainService
{
	public async Task<Product> CreateAsync(string name, string code, RoutingTypes routing, int? recipe,string statisticType )
	{
		await CheckName(name);
		await CheckCode(code);
		var product = new Product(GuidGenerator.Greate(), name, code, routing, recipe, statisticType );
		await productRepository.InsertAsync(product);
		return product;
	}

	public async Task<Product> UpdateAsync(Guid id, string name, string code, RoutingTypes routing, int? recipe)
	{
		var product = await productRepository.GetAsync(id);
		await CheckName(name, product);
		await CheckCode(code, product);
		product.Update(name, code, routing, recipe);
		await productRepository.UpdateAsync(product);
		return product;
	}

	private async Task CheckCode(string code, Product? product = null)
	{
		if (product != null && string.Equals(product.Code, code, StringComparison.OrdinalIgnoreCase)) return;
		if (await productRepository.IsExistCodeAsync(code)) throw new BusinessException("存在相同的产品编码").WithData("Code", code);
	}
	private async Task CheckName(string name, Product? product = null)
	{
		if (product != null && string.Equals(product.Name, name, StringComparison.OrdinalIgnoreCase)) return;
		if (await productRepository.IsExistNameAsync(name)) throw new BusinessException("存在相同的产品名称").WithData("Name", name);
	}
}