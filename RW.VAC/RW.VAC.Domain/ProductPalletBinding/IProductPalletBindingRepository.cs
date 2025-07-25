using RW.VAC.Domain.Pallet;
using RW.VAC.Domain.Products;
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
        /// 添加绑定记录
        /// </summary>
        /// <param name="binding">绑定实体</param>
        /// <returns>添加结果</returns>
        Task<ProductPalletBinding> AddAsync( ProductPalletBinding binding );

        /// <summary>
        /// 更新绑定记录
        /// </summary>
        /// <param name="binding">绑定实体</param>
        /// <returns>更新结果</returns>
        Task<bool> UpdateAsync( ProductPalletBinding binding );

        /// <summary>
        /// 根据ID获取绑定记录
        /// </summary>
        /// <param name="bindingId">绑定ID</param>
        /// <returns>绑定实体</returns>
        Task<ProductPalletBinding> GetByIdAsync( int bindingId );

        /// <summary>
        /// 根据产品ID获取活跃的绑定关系
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>绑定实体</returns>
        Task<ProductPalletBinding> GetActiveBindingByProductIdAsync( string productId );

        /// <summary>
        /// 根据托盘ID获取活跃的绑定关系
        /// </summary>
        /// <param name="palletId">托盘ID</param>
        /// <returns>绑定实体</returns>
        Task<ProductPalletBinding> GetActiveBindingByPalletIdAsync( string palletId );

        /// <summary>
        /// 获取所有活跃的绑定关系
        /// </summary>
        /// <returns>绑定关系列表</returns>
        Task<IEnumerable<ProductPalletBinding>> GetActiveBindingsAsync( );

        /// <summary>
        /// 根据产品类型获取绑定关系
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>绑定关系列表</returns>
        Task<IEnumerable<ProductPalletBinding>> GetBindingsByProductTypeAsync( ProductType productType );

        /// <summary>
        /// 根据托盘类型获取绑定关系
        /// </summary>
        /// <param name="palletType">托盘类型</param>
        /// <returns>绑定关系列表</returns>
        Task<IEnumerable<ProductPalletBinding>> GetBindingsByPalletTypeAsync( PalletType  palletType );

        /// <summary>
        /// 获取产品的绑定历史记录
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>绑定历史记录</returns>
        Task<IEnumerable<ProductPalletBinding>> GetBindingHistoryByProductIdAsync( string productId );

        /// <summary>
        /// 获取托盘的绑定历史记录
        /// </summary>
        /// <param name="palletId">托盘ID</param>
        /// <returns>绑定历史记录</returns>
        Task<IEnumerable<ProductPalletBinding>> GetBindingHistoryByPalletIdAsync( string palletId );

        /// <summary>
        /// 删除绑定记录
        /// </summary>
        /// <param name="bindingId">绑定ID</param>
        /// <returns>删除结果</returns>
        Task<bool> DeleteAsync( int bindingId );

        /// <summary>
        /// 获取所有绑定记录
        /// </summary>
        /// <returns>绑定记录列表</returns>
        Task<IEnumerable<ProductPalletBinding>> GetAllAsync( );

        /// <summary>
        /// 根据时间范围获取绑定记录
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>绑定记录列表</returns>
        Task<IEnumerable<ProductPalletBinding>> GetBindingsByTimeRangeAsync( DateTime startTime , DateTime endTime );
    }
}
