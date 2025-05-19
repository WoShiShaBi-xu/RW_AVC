using RW.VAC.Domain.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Client.Models
{
    public class LocationEditModel
    {
        public string LocationName { get; set; } = string.Empty;
        public LocationType LocationType { get; set; } = LocationType.BufferWaitingTest;
    }

    public class DockingPositionEditModel
    {
        public string PositionType { get; set; } = string.Empty;
        public string StationId { get; set; } = string.Empty;
    }

    public class AssignPalletModalModel
    {
        public string LocationId { get; set; } = string.Empty;
        public string PalletId { get; set; } = string.Empty;
        public Domain.Pallet.PalletType PalletType { get; set; } = Domain.Pallet.PalletType.BrakeDevicePallet;
    }

    public class AssignDockingPalletModalModel
    {
        public string PositionId { get; set; } = string.Empty;
        public string PalletId { get; set; } = string.Empty;
        public Domain.Pallet.PalletType PalletType { get; set; } = Domain.Pallet.PalletType.BrakeDevicePallet;
    }
}
