using FreeSql;
using RW.VAC.Domain.Pallet;
using RW.VAC.Domain.ProductPalletBinding;
using RW.VAC.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Infrastructure.Repositories
{
    public class ProductPalletBindingRepository : BaseRepository<ProductPalletBinding , int>, IProductPalletBindingRepository
    {
        public ProductPalletBindingRepository( IFreeSql freeSql ) : base( freeSql , null )
        {
        }

        /// <summary>
        /// 添加绑定记录
        /// </summary>
        /// <param name="binding">绑定实体</param>
        /// <returns>添加结果</returns>
        public async Task<ProductPalletBinding> AddAsync( ProductPalletBinding binding )
        {
            var result = await InsertAsync( binding );
            return result;
        }

        /// <summary>
        /// 更新绑定记录
        /// </summary>
        /// <param name="binding">绑定实体</param>
        /// <returns>更新结果</returns>
        public async Task<bool> UpdateAsync( ProductPalletBinding binding )
        {
            var result = await base.UpdateAsync( binding );
            return result > 0;
        }

        /// <summary>
        /// 根据ID获取绑定记录
        /// </summary>
        /// <param name="bindingId">绑定ID</param>
        /// <returns>绑定实体</returns>
        public async Task<ProductPalletBinding> GetByIdAsync( int bindingId )
        {
            return await Select.Where( x => x.BindingId == bindingId ).FirstAsync();
        }

        /// <summary>
        /// 根据产品ID获取活跃的绑定关系
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>绑定实体</returns>
        public async Task<ProductPalletBinding> GetActiveBindingByProductIdAsync( string productId )
        {
            return await Select
                .Where( x => x.ProductId == productId && x.BindingStatus == BindingStatus.绑定中 )
                .FirstAsync();
        }

        /// <summary>
        /// 根据托盘ID获取活跃的绑定关系
        /// </summary>
        /// <param name="palletId">托盘ID</param>
        /// <returns>绑定实体</returns>
        public async Task<ProductPalletBinding> GetActiveBindingByPalletIdAsync( string palletId )
        {
            return await Select
                .Where( x => x.PalletId == palletId && x.BindingStatus == BindingStatus.绑定中 )
                .FirstAsync();
        }

        /// <summary>
        /// 获取所有活跃的绑定关系
        /// </summary>
        /// <returns>绑定关系列表</returns>
        public async Task<IEnumerable<ProductPalletBinding>> GetActiveBindingsAsync( )
        {
            return await Select
                .Where( x => x.BindingStatus == BindingStatus.绑定中 )
                .OrderBy( x => x.BindTime )
                .ToListAsync();
        }

        /// <summary>
        /// 根据产品类型获取绑定关系
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>绑定关系列表</returns>
        public async Task<IEnumerable<ProductPalletBinding>> GetBindingsByProductTypeAsync( ProductType productType )
        {
            // 方案1：使用子查询
            var productIds = await Orm.Select<RW.VAC.Domain.Products.Product>()
                .Where( p => p.ProductType == productType )
                .ToListAsync( p => p.ProductId );

            return await Select
                .Where( b => productIds.Contains( b.ProductId ) && b.BindingStatus == BindingStatus.绑定中 )
                .ToListAsync();

           
        }

        /// <summary>
        /// 根据托盘类型获取绑定关系
        /// </summary>
        /// <param name="palletType">托盘类型</param>
        /// <returns>绑定关系列表</returns>
        public async Task<IEnumerable<ProductPalletBinding>> GetBindingsByPalletTypeAsync( PalletType palletType )
        {
            var palletIds = await Orm.Select<RW.VAC.Domain.Pallet.Pallet>()
                 .Where( p => p.PalletType == palletType )
                 .ToListAsync( p => p.PalletId );

            return await Select
                .Where( b => palletIds.Contains( b.PalletId ) && b.BindingStatus == BindingStatus.绑定中 )
                .ToListAsync();
        }

        /// <summary>
        /// 获取产品的绑定历史记录
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>绑定历史记录</returns>
        public async Task<IEnumerable<ProductPalletBinding>> GetBindingHistoryByProductIdAsync( string productId )
        {
            return await Select
                .Where( x => x.ProductId == productId )
                .OrderByDescending( x => x.BindTime )
                .ToListAsync();
        }

        /// <summary>
        /// 获取托盘的绑定历史记录
        /// </summary>
        /// <param name="palletId">托盘ID</param>
        /// <returns>绑定历史记录</returns>
        public async Task<IEnumerable<ProductPalletBinding>> GetBindingHistoryByPalletIdAsync( string palletId )
        {
            return await Select
                .Where( x => x.PalletId == palletId )
                .OrderByDescending( x => x.BindTime )
                .ToListAsync();
        }

        /// <summary>
        /// 删除绑定记录
        /// </summary>
        /// <param name="bindingId">绑定ID</param>
        /// <returns>删除结果</returns>
        public async Task<bool> DeleteAsync( int bindingId )
        {
            var result = await base.DeleteAsync( x => x.BindingId == bindingId );
            return result > 0;
        }

        /// <summary>
        /// 获取所有绑定记录
        /// </summary>
        /// <returns>绑定记录列表</returns>
        public async Task<IEnumerable<ProductPalletBinding>> GetAllAsync( )
        {
            return await Select.ToListAsync();
        }

        /// <summary>
        /// 根据时间范围获取绑定记录
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>绑定记录列表</returns>
        public async Task<IEnumerable<ProductPalletBinding>> GetBindingsByTimeRangeAsync( DateTime startTime , DateTime endTime )
        {
            return await Select
                .Where( x => x.BindTime >= startTime && x.BindTime <= endTime )
                .OrderBy( x => x.BindTime )
                .ToListAsync();
        }

        /// <summary>
        /// 根据绑定状态获取绑定记录
        /// </summary>
        /// <param name="bindingStatus">绑定状态</param>
        /// <returns>绑定记录列表</returns>
        public async Task<IEnumerable<ProductPalletBinding>> GetBindingsByStatusAsync( BindingStatus bindingStatus )
        {
            return await Select
                .Where( x => x.BindingStatus == bindingStatus )
                .OrderBy( x => x.BindTime )
                .ToListAsync();
        }

        /// <summary>
        /// 获取指定产品的当前绑定数量
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>绑定数量</returns>
        public async Task<long> GetActiveBindingCountByProductIdAsync( string productId )
        {
            return await Select
                .Where( x => x.ProductId == productId && x.BindingStatus == BindingStatus.绑定中 )
                .CountAsync();
        }

        /// <summary>
        /// 获取指定托盘的当前绑定数量
        /// </summary>
        /// <param name="palletId">托盘ID</param>
        /// <returns>绑定数量</returns>
        public async Task<long> GetActiveBindingCountByPalletIdAsync( string palletId )
        {
            return await Select
                .Where( x => x.PalletId == palletId && x.BindingStatus == BindingStatus.绑定中 )
                .CountAsync();
        }

        /// <summary>
        /// 获取绑定详情（包含关联信息）
        /// </summary>
        /// <param name="bindingId">绑定ID</param>
        /// <returns>绑定详情</returns>
        public async Task<ProductPalletBinding> GetBindingDetailsAsync( int bindingId )
        {
            return await Select
                .Where( x => x.BindingId == bindingId )
                .Include( x => x.Product )
                .Include( x => x.Pallet )
                .FirstAsync();
        }

        /// <summary>
        /// 批量更新绑定状态
        /// </summary>
        /// <param name="bindingIds">绑定ID列表</param>
        /// <param name="bindingStatus">新的绑定状态</param>
        /// <returns>更新结果</returns>
        public async Task<bool> BatchUpdateBindingStatusAsync( IEnumerable<int> bindingIds , BindingStatus bindingStatus )
        {
            var ids = bindingIds.ToList();
            if (!ids.Any())
            {
                return true;
            }

            var result = await UpdateDiy
                .Set<BindingStatus>( x => x.BindingStatus , bindingStatus )
                .Set<DateTime?>( x => x.UnbindTime , bindingStatus == BindingStatus.已解绑 ? DateTime.Now : null )
                .Where( x => ids.Contains( x.BindingId ) )
                .ExecuteAffrowsAsync();

            return result > 0;
        }

        /// <summary>
        /// 获取最近的绑定记录
        /// </summary>
        /// <param name="count">记录数量</param>
        /// <returns>最近的绑定记录</returns>
        public async Task<IEnumerable<ProductPalletBinding>> GetRecentBindingsAsync( int count = 10 )
        {
            return await Select
                .OrderByDescending( x => x.BindTime )
                .Take( count )
                .ToListAsync();
        }

        /// <summary>
        /// 检查产品和托盘是否曾经绑定过
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="palletId">托盘ID</param>
        /// <returns>是否曾经绑定过</returns>
        public async Task<bool> HasEverBeenBoundAsync( string productId , string palletId )
        {
            var count = await Select
                .Where( x => x.ProductId == productId && x.PalletId == palletId )
                .CountAsync();

            return count > 0;
        }

       
    }
}
