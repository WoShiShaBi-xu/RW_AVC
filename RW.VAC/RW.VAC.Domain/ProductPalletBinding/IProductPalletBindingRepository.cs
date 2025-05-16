using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.ProductPalletBinding
{
    public interface IProductPalletBindingRepository
    {
        /// <summary>
        /// 获取所有绑定关系
        /// </summary>
        /// <returns>绑定关系列表</returns>
        Task<IEnumerable<ProductPalletBinding>> GetAllAsync( );

        /// <summary>
        /// 根据ID获取绑定关系
        /// </summary>
        /// <param name="bindingId">绑定ID</param>
        /// <returns>绑定关系实体</returns>
        Task<ProductPalletBinding> GetByIdAsync( int bindingId );

        /// <summary>
        /// 根据产品ID获取绑定关系
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>绑定关系列表</returns>
        Task<IEnumerable<ProductPalletBinding>> GetByProductIdAsync( string productId );

        /// <summary>
        /// 根据托盘ID获取绑定关系
        /// </summary>
        /// <param name="palletId">托盘ID</param>
        /// <returns>绑定关系列表</returns>
        Task<IEnumerable<ProductPalletBinding>> GetByPalletIdAsync( string palletId );

        /// <summary>
        /// 根据绑定状态获取绑定关系
        /// </summary>
        /// <param name="bindingStatus">绑定状态</param>
        /// <returns>绑定关系列表</returns>
        Task<IEnumerable<ProductPalletBinding>> GetByStatusAsync( BindingStatus bindingStatus );

        /// <summary>
        /// 添加绑定关系
        /// </summary>
        /// <param name="binding">绑定关系实体</param>
        /// <returns>添加结果</returns>
        Task<bool> AddAsync( ProductPalletBinding binding );

        /// <summary>
        /// 更新绑定关系
        /// </summary>
        /// <param name="binding">绑定关系实体</param>
        /// <returns>更新结果</returns>
        Task<bool> UpdateAsync( ProductPalletBinding binding );

        /// <summary>
        /// 删除绑定关系
        /// </summary>
        /// <param name="bindingId">绑定关系ID</param>
        /// <returns>删除结果</returns>
        Task<bool> DeleteAsync( int bindingId );
    }
}
