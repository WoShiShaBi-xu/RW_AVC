using RW.Framework.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql.DataAnnotations;
using RW.Framework.Domain.Entities;

namespace RW.VAC.Domain.Locations
{
    [Table( Name = "locations" )]
  public  class Location : AggregateRoot<string>
    {
        public Location( ) { }

        public Location( string locationId , string locationType , string locationName , bool isOccupied = false ,
            string currentPalletId = null )
        {
            Id = locationId;
            LocationType = locationType;
            LocationName = locationName;
            IsOccupied = isOccupied;
            CurrentPalletId = currentPalletId;
            LastUpdate = DateTime.Now;
        }

        [Column( Name = "location_id" , StringLength = 20 )]
        public  string Id { get; set; }

        [Column( Name = "location_type" , StringLength = 50 )]
        public string LocationType { get; set; }

        [Column( Name = "location_name" , StringLength = 100 )]
        public string LocationName { get; set; }

        [Column( Name = "is_occupied" )]
        public bool IsOccupied { get; set; }

        [Column( Name = "current_pallet_id" , StringLength = 20 )]
        public string CurrentPalletId { get; set; }

        [Column( Name = "last_update" )]
        public DateTime? LastUpdate { get; set; }

        // 关联的托盘
        public RW.VAC.Domain.Pallet.Pallet CurrentPallet { get; set; }

        // 更新库位状态
        public void SetOccupied( string palletId )
        {
            IsOccupied = true;
            CurrentPalletId = palletId;
            LastUpdate = DateTime.Now;
        }

        // 清空库位
        public void ClearLocation( )
        {
            IsOccupied = false;
            CurrentPalletId = null;
            LastUpdate = DateTime.Now;
        }
    }
}
