using Microsoft.Extensions.Logging;
using RW.VAC.Domain.Pallet;
using RW.VAC.Domain.ProductPalletBinding;
using RW.VAC.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Application.Services.ProductPalletBindings
{
    public class ProductPalletBindingService : IProductPalletBindingService
    {
        private readonly IProductPalletBindingRepository _bindingRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPalletRepository _palletRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bindingRepository">绑定关系仓储</param>
        /// <param name="productRepository">产品仓储</param>
        /// <param name="palletRepository">托盘仓储</param>
        public ProductPalletBindingService(
            IProductPalletBindingRepository bindingRepository ,
            IProductRepository productRepository ,
            IPalletRepository palletRepository )
        {
            _bindingRepository = bindingRepository ?? throw new ArgumentNullException( nameof( bindingRepository ) );
            _productRepository = productRepository ?? throw new ArgumentNullException( nameof( productRepository ) );
            _palletRepository = palletRepository ?? throw new ArgumentNullException( nameof( palletRepository ) );
        }

        /// <summary>
        /// 绑定产品与托盘
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="palletId">托盘ID</param>
        /// <returns>绑定结果</returns>
        public async Task<RW.VAC.Domain.ProductPalletBinding.ProductPalletBinding> BindProductToPalletAsync( string productId , string palletId )
        {
            // 验证参数
            

            if (string.IsNullOrWhiteSpace( palletId ))
            {
                throw new ArgumentException( "托盘ID不能为空" , nameof( palletId ) );
            }

            

            // 检查托盘是否存在
            var pallet = await _palletRepository.GetByIdAsync( palletId );
            if (pallet == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{palletId}的托盘" );
            }

            // 检查产品是否已经绑定其他托盘
            var existingProductBinding = await _bindingRepository.GetActiveBindingByProductIdAsync( productId );
            if (existingProductBinding != null)
            {
                throw new InvalidOperationException( $"产品{productId}已经绑定托盘{existingProductBinding.PalletId}" );
            }

            //// 检查托盘是否已经绑定其他产品
            //var existingPalletBinding = await _bindingRepository.GetActiveBindingByPalletIdAsync( palletId );
            //if (existingPalletBinding != null)
            //{
            //    throw new InvalidOperationException( $"托盘{palletId}已经绑定产品{existingPalletBinding.ProductId}" );
            //}

            // 检查托盘状态是否允许绑定
            if (pallet.Status != "空闲")
            {
                throw new InvalidOperationException( $"托盘{palletId}当前状态为{pallet.Status}，无法进行绑定" );
            }

            // 创建绑定记录
            var binding = new RW.VAC.Domain.ProductPalletBinding.ProductPalletBinding
            {
                ProductId = productId ,
                PalletId = palletId ,
                BindTime = DateTime.Now ,
                BindingStatus = BindingStatus.绑定中
            };

            // 保存绑定记录
            await _bindingRepository.AddAsync( binding );

            // 更新托盘状态
            pallet.Status = "使用中";
            pallet.LastUpdate = DateTime.Now;
            await _palletRepository.UpdateAsync( pallet );

            return binding;
        }

        /// <summary>
        /// 解绑产品与托盘
        /// </summary>
        /// <param name="bindingId">绑定ID</param>
        /// <returns>解绑结果</returns>
        public async Task<bool> UnbindProductFromPalletAsync( int bindingId )
        {
            // 验证参数
            if (bindingId <= 0)
            {
                throw new ArgumentException( "绑定ID必须大于0" , nameof( bindingId ) );
            }

            // 获取绑定记录
            var binding = await _bindingRepository.GetByIdAsync( bindingId );
            if (binding == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{bindingId}的绑定记录" );
            }

            // 检查绑定状态
            if (binding.BindingStatus != BindingStatus.绑定中)
            {
                throw new InvalidOperationException( $"绑定记录{bindingId}当前状态为{binding.BindingStatus}，无法解绑" );
            }

            // 获取托盘信息
            var pallet = await _palletRepository.GetByIdAsync( binding.PalletId );

            // 更新绑定状态
            binding.BindingStatus = BindingStatus.已解绑;
            binding.UnbindTime = DateTime.Now;

            // 保存绑定记录
            var bindingResult = await _bindingRepository.UpdateAsync( binding );

            // 如果托盘存在，更新托盘状态
            if (pallet != null)
            {
                pallet.Status = "空闲";
                pallet.LocationId = null; // 清空位置信息
                pallet.LastUpdate = DateTime.Now;
                await _palletRepository.UpdateAsync( pallet );
            }

            return bindingResult;
        }

        /// <summary>
        /// 获取产品当前绑定的托盘
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>托盘实体</returns>
        public async Task<RW.VAC.Domain.Pallet.Pallet> GetPalletByProductIdAsync( string productId )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( productId ))
            {
                throw new ArgumentException( "产品ID不能为空" , nameof( productId ) );
            }

            // 获取活跃的绑定关系
            var binding = await _bindingRepository.GetActiveBindingByProductIdAsync( productId );
            if (binding == null)
            {
                return null;
            }

            // 获取托盘信息
            return await _palletRepository.GetByIdAsync( binding.PalletId );
        }

        /// <summary>
        /// 获取托盘当前绑定的产品
        /// </summary>
        /// <param name="palletId">托盘ID</param>
        /// <returns>产品实体</returns>
        public async Task<RW.VAC.Domain.Products.Product> GetProductByPalletIdAsync( string palletId )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( palletId ))
            {
                throw new ArgumentException( "托盘ID不能为空" , nameof( palletId ) );
            }

            // 获取活跃的绑定关系
            var binding = await _bindingRepository.GetActiveBindingByPalletIdAsync( palletId );
            if (binding == null)
            {
                return null;
            }

            // 获取产品信息
            return await _productRepository.GetByIdAsync( binding.ProductId );
        }

        /// <summary>
        /// 获取绑定详情
        /// </summary>
        /// <param name="bindingId">绑定ID</param>
        /// <returns>绑定详情</returns>
        public async Task<RW.VAC.Domain.ProductPalletBinding.ProductPalletBinding> GetBindingDetailsAsync( int bindingId )
        {
            // 验证参数
            if (bindingId <= 0)
            {
                throw new ArgumentException( "绑定ID必须大于0" , nameof( bindingId ) );
            }

            // 获取绑定详情
            var binding = await _bindingRepository.GetByIdAsync( bindingId );
            if (binding == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{bindingId}的绑定记录" );
            }

            return binding;
        }

        /// <summary>
        /// 获取所有活跃的绑定关系
        /// </summary>
        /// <returns>绑定关系列表</returns>
        public async Task<IEnumerable<RW.VAC.Domain.ProductPalletBinding.ProductPalletBinding>> GetAllActiveBindingsAsync( )
        {
            return await _bindingRepository.GetActiveBindingsAsync();
        }

        /// <summary>
        /// 根据产品类型获取绑定关系
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>绑定关系列表</returns>
        public async Task<IEnumerable<RW.VAC.Domain.ProductPalletBinding.ProductPalletBinding>> GetBindingsByProductTypeAsync( ProductType productType )
        {
            return await _bindingRepository.GetBindingsByProductTypeAsync( productType );
        }

        /// <summary>
        /// 根据托盘类型获取绑定关系
        /// </summary>
        /// <param name="palletType">托盘类型</param>
        /// <returns>绑定关系列表</returns>
        public async Task<IEnumerable<RW.VAC.Domain.ProductPalletBinding.ProductPalletBinding>> GetBindingsByPalletTypeAsync( PalletType palletType )
        {
            return await _bindingRepository.GetBindingsByPalletTypeAsync( palletType );
        }

        /// <summary>
        /// 检查产品是否已绑定托盘
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>是否已绑定</returns>
        public async Task<bool> IsProductBoundAsync( string productId )
        {
            if (string.IsNullOrWhiteSpace( productId ))
            {
                return false;
            }

            var binding = await _bindingRepository.GetActiveBindingByProductIdAsync( productId );
            return binding != null;
        }

        /// <summary>
        /// 检查托盘是否已绑定产品
        /// </summary>
        /// <param name="palletId">托盘ID</param>
        /// <returns>是否已绑定</returns>
        public async Task<bool> IsPalletBoundAsync( string palletId )
        {
            if (string.IsNullOrWhiteSpace( palletId ))
            {
                return false;
            }

            var binding = await _bindingRepository.GetActiveBindingByPalletIdAsync( palletId );
            return binding != null;
        }

        /// <summary>
        /// 批量解绑产品与托盘
        /// </summary>
        /// <param name="bindingIds">绑定ID列表</param>
        /// <returns>解绑结果</returns>
        public async Task<bool> BatchUnbindProductFromPalletAsync( IEnumerable<int> bindingIds )
        {
            if (bindingIds == null || !bindingIds.Any())
            {
                return true;
            }

            var results = new List<bool>();
            foreach (var bindingId in bindingIds)
            {
                try
                {
                    var result = await UnbindProductFromPalletAsync( bindingId );
                    results.Add( result );
                }
                catch
                {
                    results.Add( false );
                }
            }

            return results.All( r => r );
        }

        /// <summary>
        /// 强制解绑产品（用于异常情况）
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>解绑结果</returns>
        public async Task<bool> ForceUnbindProductAsync( string productId )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( productId ))
            {
                throw new ArgumentException( "产品ID不能为空" , nameof( productId ) );
            }

            // 获取活跃的绑定关系
            var binding = await _bindingRepository.GetActiveBindingByProductIdAsync( productId );
            if (binding == null)
            {
                return true; // 没有绑定关系，返回成功
            }

            // 强制解绑
            return await UnbindProductFromPalletAsync( binding.BindingId );
        }

        /// <summary>
        /// 强制解绑托盘（用于异常情况）
        /// </summary>
        /// <param name="palletId">托盘ID</param>
        /// <returns>解绑结果</returns>
        public async Task<bool> ForceUnbindPalletAsync( string palletId )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( palletId ))
            {
                throw new ArgumentException( "托盘ID不能为空" , nameof( palletId ) );
            }

            // 获取活跃的绑定关系
            var binding = await _bindingRepository.GetActiveBindingByPalletIdAsync( palletId );
            if (binding == null)
            {
                return true; // 没有绑定关系，返回成功
            }

            // 强制解绑
            return await UnbindProductFromPalletAsync( binding.BindingId );
        }

        /// <summary>
        /// 获取绑定历史记录
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="palletId">托盘ID</param>
        /// <returns>绑定历史记录</returns>
        public async Task<IEnumerable<RW.VAC.Domain.ProductPalletBinding.ProductPalletBinding>> GetBindingHistoryAsync( string productId = null , string palletId = null )
        {
            if (!string.IsNullOrWhiteSpace( productId ))
            {
                return await _bindingRepository.GetBindingHistoryByProductIdAsync( productId );
            }

            if (!string.IsNullOrWhiteSpace( palletId ))
            {
                return await _bindingRepository.GetBindingHistoryByPalletIdAsync( palletId );
            }

            throw new ArgumentException( "产品ID和托盘ID不能同时为空" );
        }

        /// <summary>
        /// 检查绑定是否可以解除
        /// </summary>
        /// <param name="bindingId">绑定ID</param>
        /// <returns>是否可以解除绑定</returns>
        public async Task<bool> CanUnbindAsync( int bindingId )
        {
            if (bindingId <= 0)
            {
                return false;
            }

            var binding = await _bindingRepository.GetByIdAsync( bindingId );
            if (binding == null || binding.BindingStatus != BindingStatus.绑定中)
            {
                return false;
            }

            // 检查托盘是否在执行任务
            var pallet = await _palletRepository.GetByIdAsync( binding.PalletId );
            if (pallet != null && pallet.Status == "运输中")
            {
                return false; // 托盘正在运输中，不能解绑
            }

            return true;
        }
    }


}
