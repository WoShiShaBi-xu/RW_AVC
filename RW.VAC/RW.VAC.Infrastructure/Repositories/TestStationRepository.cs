using FreeSql;
using RW.VAC.Domain.TestStation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Infrastructure.Repositories
{
    public class TestStationRepository : BaseRepository<TestStation , string>, ITestStationRepository
    {
        public TestStationRepository( IFreeSql freeSql ) : base( freeSql , null )
        {
        }

        public async Task<IEnumerable<TestStation>> GetAllAsync( )
        {
            return await Select.ToListAsync();
        }

        public async Task<TestStation> GetByIdAsync( string stationId )
        {
            return await Select.Where( x => x.StationId == stationId ).FirstAsync();
        }

        public async Task<IEnumerable<TestStation>> GetByTypeAsync( StationType stationType )
        {
            return await Select.Where( x => x.StationType == stationType ).ToListAsync();
        }

        public async Task<IEnumerable<TestStation>> GetByStatusAsync( StationStatus status )
        {
            return await Select.Where( x => x.Status == status ).ToListAsync();
        }

        public async Task<TestStation> GetByProductIdAsync( string productId )
        {
            return await Select.Where( x => x.CurrentProductId == productId ).FirstAsync();
        }

        public async Task<bool> AddAsync( TestStation station )
        {
            var result = await InsertAsync( station );
            return result != null;
        }

        public async Task<bool> UpdateAsync( TestStation station )
        {
            var result = await base.UpdateAsync( station );
            return result > 0;
        }

        public async Task<bool> DeleteAsync( string stationId )
        {
            var result = await base.DeleteAsync( x => x.StationId == stationId );
            return result > 0;
        }
    }
}
