using RW.VAC.Domain.DockingPosition;
using RW.VAC.Domain.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Client
{
    public static class BindingExtensions
    {
        public static bool IsOccupied( this Location location )
        {
            return location?.CurrentBinding != null;
        }

        public static bool IsOccupied( this DockingPosition position )
        {
            return position?.CurrentBinding != null;
        }

        public static string GetPalletId( this Location location )
        {
            return location?.CurrentBinding?.PalletId;
        }
        public static string GetProductld( this Location location )
        {
            return location?.CurrentBinding?.ProductId;
        }
        public static string GetPalletId( this DockingPosition position )
        {
            return position?.CurrentBinding?.PalletId;
        }

        public static bool IsBindingActive( this DockingPosition position )
        {
            return position?.CurrentBinding?.BindingStatus ==  Domain.ProductPalletBinding.BindingStatus.绑定中;
        }
    }
}
