using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.ProductPalletBinding
{
    public interface IProductPalletBindingService
    {
        /// <summary>
        /// 绑定产品与托盘
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="palletId">托盘ID</param>
        /// <returns>绑定结果</returns>
        Task<ProductPalletBinding> BindProductToPalletAsync( string productId , string palletId );

        /// <summary>
        /// 解绑产品与托盘
        /// </summary>
        /// <param name="bindingId">绑定ID</param>
        /// <returns>解绑结果</returns>
        Task<bool> UnbindProductFromPalletAsync( int bindingId );

        /// <summary>
        /// 获取产品当前绑定的托盘
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>托盘实体</returns>
        Task<RW.VAC.Domain.Pallet.Pallet> GetPalletByProductIdAsync( string productId );

        /// <summary>
        /// 获取托盘当前绑定的产品
        /// </summary>
        /// <param name="palletId">托盘ID</param>
        /// <returns>产品实体</returns>
        Task<RW.VAC.Domain.Products.Product> GetProductByPalletIdAsync( string palletId );

        /// <summary>
        /// 获取绑定详情
        /// </summary>
        /// <param name="bindingId">绑定ID</param>
        /// <returns>绑定详情</returns>
        Task<ProductPalletBinding> GetBindingDetailsAsync( int bindingId );
    }
}
