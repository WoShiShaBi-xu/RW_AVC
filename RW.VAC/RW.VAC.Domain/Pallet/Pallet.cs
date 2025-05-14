using RW.Framework.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql.DataAnnotations;

namespace RW.VAC.Domain.Pallet
{
    [Table( Name = "pallets" )]
    public class Pallet : AggregateRoot<string>
    {
        public Pallet( ) { }

        public Pallet( string palletId , string palletType , string status = "空闲" , string locationId = null )
        {
            Id = palletId;
            PalletType = palletType;
            Status = status;
            LocationId = locationId;
            CreateTime = DateTime.Now;
            LastUpdate = DateTime.Now;
        }

        [Column( Name = "pallet_id" , StringLength = 20 )]
        public string Id { get; set; }

        [Column( Name = "pallet_type" , StringLength = 50 )]
        public string PalletType { get; set; }

        [Column( Name = "status" , StringLength = 20 )]
        public string Status { get; set; }

        [Column( Name = "location_id" , StringLength = 20 )]
        public string LocationId { get; set; }

        [Column( Name = "create_time" )]
        public DateTime? CreateTime { get; set; }

        [Column( Name = "last_update" )]
        public DateTime? LastUpdate { get; set; }

        // 关联的产品
        public string ProductId { get; set; }
    }
}
