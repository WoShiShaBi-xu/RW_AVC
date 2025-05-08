using AutoMapper;
using RW.VAC.Application.Contracts.Products;
using RW.VAC.Domain.Products;

namespace RW.VAC.Application.MapperProfiles;

public class ProductProfile : Profile
{
	public ProductProfile()
	{
		CreateMap<Product, ProductDto>();

		CreateMap<ProductDto, ProductCreateUpdateDto>();
	}
}