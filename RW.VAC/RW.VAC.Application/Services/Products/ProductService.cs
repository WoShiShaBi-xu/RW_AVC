using RW.VAC.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Application.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="productRepository">产品仓储</param>
        public ProductService( IProductRepository productRepository )
        {
            _productRepository = productRepository ?? throw new ArgumentNullException( nameof( productRepository ) );
        }

        /// <summary>
        /// 创建新产品
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <param name="productName">产品名称</param>
        /// <param name="productSpecs">产品规格</param>
        /// <returns>创建的产品</returns>
        public async Task<Product> CreateProductAsync( ProductType productType , string productName , string productSpecs )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( productName ))
            {
                throw new ArgumentException( "产品名称不能为空" , nameof( productName ) );
            }

            // 创建产品实体
            var product = new Product
            {
                ProductId = GenerateProductId( productType ) ,
                ProductType = productType ,
                ProductName = productName ,
                ProductSpecs = productSpecs ,
                Status = "待组装" ,
                CreateTime = DateTime.Now
            };

            // 保存到数据库
            await _productRepository.AddAsync( product );

            return product;
        }

        /// <summary>
        /// 更新产品状态
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="status">新状态</param>
        /// <returns>更新结果</returns>
        public async Task<bool> UpdateProductStatusAsync( string productId , string status )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( productId ))
            {
                throw new ArgumentException( "产品ID不能为空" , nameof( productId ) );
            }

            if (string.IsNullOrWhiteSpace( status ))
            {
                throw new ArgumentException( "状态不能为空" , nameof( status ) );
            }

            // 获取产品
            var product = await _productRepository.GetByIdAsync( productId );
            if (product == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{productId}的产品" );
            }

            // 更新状态
            product.Status = status;
            product.UpdateTime = DateTime.Now;

            // 保存到数据库
            return await _productRepository.UpdateAsync( product );
        }

        /// <summary>
        /// 获取产品详情
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>产品详情</returns>
        public async Task<Product> GetProductDetailsAsync( string productId )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( productId ))
            {
                throw new ArgumentException( "产品ID不能为空" , nameof( productId ) );
            }

            // 获取产品
            var product = await _productRepository.GetByIdAsync( productId );
            if (product == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{productId}的产品" );
            }

            return product;
        }

        /// <summary>
        /// 生成产品ID
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>产品ID</returns>
        private string GenerateProductId( ProductType productType )
        {
            // 生成规则：类型前缀 + 年月日 + 6位随机数
            string prefix = productType == ProductType.制动装置 ? "BD" : "AD";
            string dateStr = DateTime.Now.ToString( "yyyyMMdd" );
            string randomStr = new Random().Next( 100000 , 999999 ).ToString();

            return $"{prefix}{dateStr}{randomStr}";
        }
    }
}
