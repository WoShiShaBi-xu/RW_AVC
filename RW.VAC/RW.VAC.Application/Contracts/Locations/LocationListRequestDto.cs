using RW.Framework.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Application.Contracts.Locations
{
    public class LocationListRequestDto : PagedAndSortedResultRequestDto
    {
        public string LocationType { get; set; }
        public bool? IsOccupied { get; set; }
        public string SearchText { get; set; }
    }
}
