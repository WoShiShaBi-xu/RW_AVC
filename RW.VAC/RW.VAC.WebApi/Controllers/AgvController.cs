using Microsoft.AspNetCore.Mvc;
using RW.VAC.Application.Contracts.AGV;
using RW.VAC.Application.Services.AGV;
using RW.VAC.Domain.Location;
using RW.VAC.Domain.Pallet;
using RW.VAC.Domain.ProductPalletBinding;
using RW.VAC.Domain.Products;

namespace RW.VAC.WebApi.Controllers
{
    [ApiController]
    [Route( "api/[controller]" )]
    public class AgvController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductPalletBindingService _productPalletBindingService;
        private readonly ILocationService _locationService;
        private readonly IPalletService _palletService;

        public AgvController(
            IProductService productService ,
            IProductPalletBindingService productPalletBindingService ,
            ILocationService locationService ,
            IPalletService palletService )
        {
            _productService = productService;
            _productPalletBindingService = productPalletBindingService;
            _locationService = locationService;
            _palletService = palletService;
        }

        [HttpPost( "load" )]
        public async Task<IActionResult> SendLoadCommand( LoadCommandRequest request )
        {
            try
            {
                // 验证请求参数
                if (string.IsNullOrWhiteSpace( request.StationId ))
                {
                    return BadRequest( new { Success = false , Message = "工位ID不能为空" } );
                }

                if (string.IsNullOrWhiteSpace( request.PalletId ))
                {
                    return BadRequest( new { Success = false , Message = "托盘ID不能为空" } );
                }

                // 检查托盘是否存在，如果不存在则创建
                RW.VAC.Domain.Pallet.Pallet pallet;
                try
                {
                    pallet = await _palletService.GetPalletDetailsAsync( request.PalletId );
                }
                catch (KeyNotFoundException)
                {
                    // 托盘不存在，创建新托盘 (统一使用制动装置托盘)
                    pallet = await _palletService.CreatePalletAsync( PalletType.制动装置托盘 );
                    // 更新托盘ID为请求中的ID
                    await _palletService.UpdatePalletStatusAsync( pallet.PalletId , "空闲" );
                }

                // 1. 创建产品
                var productType = DetermineProductType( request.StationId );
                var productName = $"产品-{request.StationId}-{DateTime.Now:yyyyMMddHHmmss}";
                var productSpecs = $"工位{request.StationId}生产的产品";

                var product = await _productService.CreateProductAsync(
                    productType ,
                    productName ,
                    productSpecs
                );

                // 2. 创建产品托盘绑定
                var binding = await _productPalletBindingService.BindProductToPalletAsync(
                   null,
                    pallet.PalletId
                );

                // 3. 统一分配到护箱备料区并更新库位的CurrentBindingId
                var locationId = await GetLocationIdFromStationId( request.StationId, request.StationNumber);
                if (locationId!="")
                {
                    await _locationService.AssignPalletToLocationAsync( locationId , binding );
                }

                return Ok( new
                {
                    Success = true ,
                    Message = "上料指令处理成功" ,
                    Data = new
                    {
                        ProductId = product.ProductId ,
                        ProductName = product.ProductName ,
                        ProductType = product.ProductType.ToString() ,
                        PalletId = request.PalletId ,
                        BindingId = binding.BindingId ,
                        StationId = request.StationId ,
                        LocationId = locationId ,
                        CreateTime = DateTime.Now
                    }
                } );
            }
            catch (Exception ex)
            {
                return BadRequest( new { Success = false , Message = ex.Message } );
            }
        }
        /// <summary>
        /// 根据工位ID确定产品类型
        /// </summary>
        /// <param name="stationId">工位ID</param>
        /// <returns>产品类型</returns>
        private ProductType DetermineProductType( string stationId )
        {
            // 统一使用制动装置类型
            return ProductType.制动装置;
        }
        /// <summary>
        /// 根据工位ID获取对应的库位ID
        /// </summary>
        /// <param name="stationId">工位ID</param>
        /// <returns>库位ID</returns>
        private async Task<string> GetLocationIdFromStationId( string stationId ,string StationNumber)
        {
            try
            {
                if (StationNumber == "1")
                {
                    return "LOC014";
                }
                else if (StationNumber == "2")
                {
                    return "LOC015";
                }
                if (StationNumber == "3")
                {
                    return "LOC016";
                }
                if (StationNumber == "4")
                {
                    return "PBP2507159937";
                }
                return "";
            }
            catch (Exception ex)
            {
                // 记录日志并返回默认值或抛出异常
                throw new InvalidOperationException( $"无法为工位{stationId}确定库位ID: {ex.Message}" );
            }
        }
    }
       
    }

