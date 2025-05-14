using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.Locations
{
    public interface ILocationRepository : IBaseRepository<Location , string>
    {
        Task<List<Location>> GetAllLocationsAsync( );
        Task<List<Location>> GetLocationsByTypeAsync( string locationType );
        Task<bool> ClearLocationAsync( string locationId );
    }
}
