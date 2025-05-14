using RW.Framework.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Application.Contracts.Locations
{
    public interface ILocationService
    {
        Task<PagedResultDto<LocationDto>> GetPagedListAsync( LocationListRequestDto input );
        Task<LocationDto> GetAsync( string id );
        Task<List<LocationDto>> GetAllLocationsAsync( );
        Task<List<LocationDto>> GetLocationsByTypeAsync( string locationType );
        Task<bool> ClearLocationAsync( string locationId );
    }
}
