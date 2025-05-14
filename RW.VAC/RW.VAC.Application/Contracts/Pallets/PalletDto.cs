using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Application.Contracts.Pallets
{
    public class PalletDto
    {
        public string Id { get; set; }
        public string PalletType { get; set; }
        public string Status { get; set; }
        public string LocationId { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? LastUpdate { get; set; }

        // 关联的产品信息
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
    }
}
