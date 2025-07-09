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
        /// 获取所有产品
        /// </summary>
        /// <returns>产品列表</returns>
        public async Task<IEnumerable<Product>> GetAllProductsAsync( )
        {
            return await _productRepository.GetAllAsync();
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
        /// 根据产品类型获取产品列表
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>指定类型的产品列表</returns>
        public async Task<IEnumerable<Product>> GetProductsByTypeAsync( ProductType productType )
        {
            return await _productRepository.GetByTypeAsync( productType );
        }
        /// <summary>
        /// 根据产品状态获取产品列表
        /// </summary>
        /// <param name="status">产品状态</param>
        /// <returns>指定状态的产品列表</returns>
        public async Task<IEnumerable<Product>> GetProductsByStatusAsync( string status )
        {
            if (string.IsNullOrWhiteSpace( status ))
            {
                throw new ArgumentException( "状态不能为空" , nameof( status ) );
            }

            return await _productRepository.GetByStatusAsync( status );
        }
        /// <summary>
        /// 获取可用的产品（未分配位置的产品）
        /// </summary>
        /// <returns>可用产品列表</returns>
        public async Task<IEnumerable<Product>> GetAvailableProductsAsync( )
        {
            var allProducts = await _productRepository.GetAllAsync();

            // 过滤出没有分配位置的产品
            return allProducts.Where( p => string.IsNullOrEmpty( p.LocationId ) );
        }
        /// <summary>
        /// 更新产品信息
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <param name="productName">产品名称</param>
        /// <param name="productSpecs">产品规格</param>
        /// <returns>更新结果</returns>
        public async Task<bool> UpdateProductAsync( string productId , string productName , string productSpecs )
        {
            // 验证参数
            if (string.IsNullOrWhiteSpace( productId ))
            {
                throw new ArgumentException( "产品ID不能为空" , nameof( productId ) );
            }

            if (string.IsNullOrWhiteSpace( productName ))
            {
                throw new ArgumentException( "产品名称不能为空" , nameof( productName ) );
            }

            // 获取产品
            var product = await _productRepository.GetByIdAsync( productId );
            if (product == null)
            {
                throw new KeyNotFoundException( $"找不到ID为{productId}的产品" );
            }

            // 更新产品信息
            product.ProductName = productName;
            product.ProductSpecs = productSpecs;
            product.UpdateTime = DateTime.Now;

            // 保存到数据库
            return await _productRepository.UpdateAsync( product );
        }
        /// <summary>
        /// 删除产品
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>删除结果</returns>
        public async Task<bool> DeleteProductAsync( string productId )
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

            // 检查是否可以删除
            if (!await CanDeleteProductAsync( productId ))
            {
                throw new InvalidOperationException( "产品当前被使用，无法删除" );
            }

            // 删除产品
            return await _productRepository.DeleteAsync( productId );
        }
        /// <summary>
        /// 检查产品是否可以删除
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>是否可以删除</returns>
        public async Task<bool> CanDeleteProductAsync( string productId )
        {
            if (string.IsNullOrWhiteSpace( productId ))
            {
                return false;
            }

            var product = await _productRepository.GetByIdAsync( productId );
            if (product == null)
            {
                return false;
            }

            // 检查产品是否在某个位置
            if (!string.IsNullOrEmpty( product.LocationId ))
            {
                return false;
            }

            // 检查产品是否在任何任务中
            // 这里可以添加更多的业务逻辑检查

            return true;
        }
        /// <summary>
        /// 根据产品ID查找产品位置
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>产品位置信息</returns>
        public async Task<string> GetProductLocationAsync( string productId )
        {
            if (string.IsNullOrWhiteSpace( productId ))
            {
                return null;
            }

            var product = await _productRepository.GetByIdAsync( productId );
            return product?.LocationId;
        }

        /// <summary>
        /// 生成产品ID
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>产品ID</returns>
        private string GenerateProductId( ProductType productType )
        {
            // 生成规则：P + 类型前缀 + 年月日 + 3位随机数
            string prefix = productType switch
            {
                ProductType.制动装置 => "PB",
                ProductType.辅助装置 => "PA",
                _ => "P"
            };

            string dateStr = DateTime.Now.ToString( "yyMMdd" );
            string randomStr = new Random().Next( 100 , 999 ).ToString();

            return $"{prefix}{dateStr}{randomStr}";
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
