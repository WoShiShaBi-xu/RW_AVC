using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Application.Contracts.Locations
{
    public class LocationDto
    {
        public string Id { get; set; }
        public string LocationType { get; set; }
        public string LocationName { get; set; }
        public bool IsOccupied { get; set; }
        public string CurrentPalletId { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
