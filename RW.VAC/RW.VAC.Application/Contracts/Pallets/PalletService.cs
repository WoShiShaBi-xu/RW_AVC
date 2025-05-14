using AutoMapper.Internal.Mappers;
using RW.Framework.Application.Dtos;
using RW.Framework.Application.Services;
using RW.VAC.Domain.Pallet;
using RW.VAC.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Application.Contracts.Pallets
{
    public class PalletService(
    IPalletRepository repository ,
    IProductRepository productRepository )
    : ReadOnlyAppService<Pallet , string , PalletDto , PagedAndSortedResultRequestDto>( repository ), IPalletService
    {
        public async Task<PalletDto> GetByIdWithProductAsync( string palletId )
        {
            var pallet = await repository.GetByIdAsync( palletId );
            if (pallet == null)
                return null;

            var dto = ObjectMapper.Map<Pallet , PalletDto>( pallet );

            // 获取托盘绑定的产品信息
            var productBinding = await productRepository.GetProductByPalletIdAsync( palletId );
            if (productBinding != null)
            {
                dto.ProductId = productBinding.ProductId;
                dto.ProductName = productBinding.ProductName;
                dto.ProductType = productBinding.ProductType;
            }

            return dto;
        }
    }
}
