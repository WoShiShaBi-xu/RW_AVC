using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.Products
{
    public interface IProductService
    {
        /// <summary>
        /// 获取所有产品
        /// </summary>
        /// <returns>产品列表</returns>
        Task<IEnumerable<Product>> GetAllProductsAsync( );

        /// <summary>
        /// 创建新产品
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <param name="productName">产品名称</param>
        /// <param name="productSpecs">产品规格</param>
        /// <returns>创建的产品</returns>
        Task<Product> CreateProductAsync( ProductType productType , string productName , string productSpecs );

        /// <summary>
        /// 更新产品状态
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="status">新状态</param>
        /// <returns>更新结果</returns>
        Task<bool> UpdateProductStatusAsync( string productId , string status );

        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>产品详情</returns>
        Task<Product> GetProductDetailsAsync( string productId );

        /// <summary>
        /// 根据产品类型获取产品列表
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>指定类型的产品列表</returns>
        Task<IEnumerable<Product>> GetProductsByTypeAsync( ProductType productType );

        /// <summary>
        /// 根据产品状态获取产品列表
        /// </summary>
        /// <param name="status">产品状态</param>
        /// <returns>指定状态的产品列表</returns>
        Task<IEnumerable<Product>> GetProductsByStatusAsync( string status );

        /// <summary>
        /// 获取可用的产品（未分配位置的产品）
        /// </summary>
        /// <returns>可用产品列表</returns>
        Task<IEnumerable<Product>> GetAvailableProductsAsync( );

        /// <summary>
        /// 更新产品信息
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="productName">产品名称</param>
        /// <param name="productSpecs">产品规格</param>
        /// <returns>更新结果</returns>
        Task<bool> UpdateProductAsync( string productId , string productName , string productSpecs );

        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>删除结果</returns>
        Task<bool> DeleteProductAsync( string productId );

        /// <summary>
        /// 检查产品是否可以删除
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>是否可以删除</returns>
        Task<bool> CanDeleteProductAsync( string productId );

        /// <summary>
        /// 根据产品ID查找产品位置
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>产品位置信息</returns>
        Task<string> GetProductLocationAsync( string productId );
    }
}
