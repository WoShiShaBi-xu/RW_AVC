using FreeSql;
using RW.VAC.Domain.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Infrastructure.Repositories
{
    public class LocationRepository : BaseRepository<Location , string>, ILocationRepository
    {
        public LocationRepository( IFreeSql freeSql ) : base( freeSql , null )
        {
        }

        public async Task<IEnumerable<Location>> GetAllAsync( )
        {
            return await Select.ToListAsync();
        }

        public async Task<Location> GetByIdAsync( string locationId )
        {
            return await Select.Where( x => x.LocationId == locationId ).FirstAsync();
        }

        public async Task<IEnumerable<Location>> GetByTypeAsync( LocationType locationType )
        {
            return await Select.Where( x => x.LocationType == locationType ).ToListAsync();
        }

        public async Task<IEnumerable<Location>> GetAvailableAsync( )
        {
            return await Select.Where( x => x.IsOccupied != true ).ToListAsync();
        }

        public async Task<Location> GetByPalletIdAsync( string palletId )
        {
            return await Select.Where( x => x.CurrentPalletId == palletId ).FirstAsync();
        }

        public async Task<bool> AddAsync( Location location )
        {
            var result = await InsertAsync( location );
            return result != null;
        }

        public async Task<bool> UpdateAsync( Location location )
        {
            var result = await base.UpdateAsync( location );
            return result > 0;
        }

        public async Task<bool> DeleteAsync( string locationId )
        {
            var result = await base.DeleteAsync( x => x.LocationId == locationId );
            return result > 0;
        }
    }
}
