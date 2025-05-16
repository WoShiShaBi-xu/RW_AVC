using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.Products
{
    public interface IProductRepository
    {
        /// <summary>
        /// 获取所有产品
        /// </summary>
        /// <returns>产品列表</returns>
        Task<IEnumerable<Product>> GetAllAsync( );

        /// <summary>
        /// 根据ID获取产品
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>产品实体</returns>
        Task<Product> GetByIdAsync( string productId );

        /// <summary>
        /// 根据类型获取产品
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>产品列表</returns>
        Task<IEnumerable<Product>> GetByTypeAsync( ProductType productType );

        /// <summary>
        /// 根据状态获取产品
        /// </summary>
        /// <param name="status">产品状态</param>
        /// <returns>产品列表</returns>
        Task<IEnumerable<Product>> GetByStatusAsync( string status );

        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="product">产品实体</param>
        /// <returns>添加结果</returns>
        Task<bool> AddAsync( Product product );

        /// <summary>
        /// 更新产品
        /// </summary>
        /// <param name="product">产品实体</param>
        /// <returns>更新结果</returns>
        Task<bool> UpdateAsync( Product product );

        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>删除结果</returns>
        Task<bool> DeleteAsync( string productId );
    }
}
