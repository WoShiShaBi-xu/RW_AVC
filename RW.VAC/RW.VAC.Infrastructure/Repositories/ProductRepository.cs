using FreeSql;
using RW.VAC.Domain.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product , string>, IProductRepository
    {
        public ProductRepository( IFreeSql freeSql ) : base( freeSql , null )
        {
        }

        public async Task<IEnumerable<Product>> GetAllAsync( )
        {
            return await Select.ToListAsync();
        }

        public async Task<Product> GetByIdAsync( string productId )
        {
            return await Select.Where( x => x.ProductId == productId ).FirstAsync();
        }

        public async Task<IEnumerable<Product>> GetByTypeAsync( ProductType productType )
        {
            return await Select.Where( x => x.ProductType == productType ).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByStatusAsync( string status )
        {
            return await Select.Where( x => x.Status == status ).ToListAsync();
        }

        public async Task<bool> AddAsync( Product product )
        {
            var result = await InsertAsync( product );
            return result != null;
        }

        public async Task<bool> UpdateAsync( Product product )
        {
            var result = await base.UpdateAsync( product );
            return result > 0;
        }

        public async Task<bool> DeleteAsync( string productId )
        {
            var result = await base.DeleteAsync( x => x.ProductId == productId );
            return result > 0;
        }
    }
}
