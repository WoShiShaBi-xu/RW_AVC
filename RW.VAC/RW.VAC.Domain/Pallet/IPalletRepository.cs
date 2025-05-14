using FreeSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Domain.Pallet
{
    public interface IPalletRepository : IBaseRepository<Pallet , string>
    {
        Task<Pallet> GetByIdAsync( string palletId );
        Task<List<Pallet>> GetPalletsByLocationAsync( string locationId );
    }
}
