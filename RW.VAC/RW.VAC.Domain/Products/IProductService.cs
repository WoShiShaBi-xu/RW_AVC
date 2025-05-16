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
    }
}
