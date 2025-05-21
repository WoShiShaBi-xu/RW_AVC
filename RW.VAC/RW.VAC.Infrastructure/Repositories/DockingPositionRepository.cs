using FreeSql;
using RW.VAC.Domain.DockingPosition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Infrastructure.Repositories
{
    public class DockingPositionRepository : BaseRepository<DockingPosition , string>, IDockingPositionRepository
    {
        public DockingPositionRepository( IFreeSql freeSql ) : base( freeSql , null )
        {
        }

        public async Task<IEnumerable<DockingPosition>> GetAllAsync( )
        {
            return await Select.ToListAsync();
        }

        public async Task<DockingPosition> GetByIdAsync( string positionId )
        {
            return await Select.Where( x => x.PositionId == positionId ).FirstAsync();
        }

        public async Task<IEnumerable<DockingPosition>> GetByTypeAsync( string positionType )
        {
            return await Select.Where( x => x.PositionType == positionType ).ToListAsync();
        }

        public async Task<IEnumerable<DockingPosition>> GetByStatusAsync( PositionStatus status )
        {
            return await Select.Where( x => x.Status == status ).ToListAsync();
        }

        public async Task<IEnumerable<DockingPosition>> GetByStationIdAsync( string stationId )
        {
            return await Select.Where( x => x.StationId == stationId ).ToListAsync();
        }

        public async Task<DockingPosition> GetByPalletIdAsync( string palletId )
        {
            return await Select.Where( x => x.CurrentPalletId == palletId ).FirstAsync();
        }

        public async Task<bool> AddAsync( DockingPosition position )
        {
            var result = await InsertAsync( position );
            return result != null;
        }

        public async Task<bool> UpdateAsync( DockingPosition position )
        {
            var result = await base.UpdateAsync( position );
            return result > 0;
        }

        public async Task<bool> DeleteAsync( string positionId )
        {
            var result = await base.DeleteAsync( x => x.PositionId == positionId );
            return result > 0;
        }
    }
}
