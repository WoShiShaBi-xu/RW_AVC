using FreeSql;
using RW.VAC.Domain.Pallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Infrastructure.Repositories
{
    public class PalletRepository : BaseRepository<Pallet , string>, IPalletRepository
    {
        public PalletRepository( IFreeSql freeSql ) : base( freeSql , null )
        {
        }

        public async Task<IEnumerable<Pallet>> GetAllAsync( )
        {
            return await Select.ToListAsync();
        }

        public async Task<Pallet> GetByIdAsync( string palletId )
        {
            return await Select.Where( x => x.PalletId == palletId ).FirstAsync();
        }

        public async Task<IEnumerable<Pallet>> GetByTypeAsync( PalletType palletType )
        {
            return await Select.Where( x => x.PalletType == palletType ).ToListAsync();
        }

        public async Task<IEnumerable<Pallet>> GetByStatusAsync( string status )
        {
            return await Select.Where( x => x.Status == status ).ToListAsync();
        }

        public async Task<IEnumerable<Pallet>> GetByLocationAsync( string locationId )
        {
            return await Select.Where( x => x.LocationId == locationId ).ToListAsync();
        }

        public async Task<bool> AddAsync( Pallet pallet )
        {
            var result = await InsertAsync( pallet );
            return result != null;
        }

        public async Task<bool> UpdateAsync( Pallet pallet )
        {
            var result = await base.UpdateAsync( pallet );
            return result > 0;
        }

        public async Task<bool> DeleteAsync( string palletId )
        {
            var result = await base.DeleteAsync( x => x.PalletId == palletId );
            return result > 0;
        }
    }
}
