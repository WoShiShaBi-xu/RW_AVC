using AutoMapper.Internal.Mappers;
using RW.Framework.Application.Services;
using RW.Framework.Extensions;
using RW.VAC.Domain.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Application.Contracts.Locations
{
    public class LocationService( ILocationRepository repository )
    : ReadOnlyAppService<Location , string , LocationDto , LocationListRequestDto>( repository ), ILocationService
    {
        public async Task<List<LocationDto>> GetAllLocationsAsync( )
        {
            var locations = await repository.GetAllLocationsAsync();
            return ObjectMapper.Map<List<Location> , List<LocationDto>>( locations );
        }

        public async Task<List<LocationDto>> GetLocationsByTypeAsync( string locationType )
        {
            var locations = await repository.GetLocationsByTypeAsync( locationType );
            return ObjectMapper.Map<List<Location> , List<LocationDto>>( locations );
        }

        public Task<bool> ClearLocationAsync( string locationId )
        {
            return repository.ClearLocationAsync( locationId );
        }

        protected override Expression<Func<Location , bool>>? GreateFilter( LocationListRequestDto input )
        {
            Expression<Func<Location , bool>>? where = null;

            where = where.And( input.LocationType.NotNullOrWhiteSpace() ,
                t => t.LocationType == input.LocationType );

            where = where.And( input.IsOccupied.HasValue ,
                t => t.IsOccupied == input.IsOccupied!.Value );

            where = where.And( input.SearchText.NotNullOrWhiteSpace() ,
                t => t.Id.Contains( input.SearchText! ) ||
                     t.LocationName.Contains( input.SearchText! ) );

            return where;
        }

        Task<LocationDto> ILocationService.GetAsync( string id )
        {
            throw new NotImplementedException();
        }
    }
}
