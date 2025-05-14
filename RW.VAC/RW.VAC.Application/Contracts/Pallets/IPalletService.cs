using RW.Framework.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RW.VAC.Application.Contracts.Pallets
{
    public interface IPalletService
    {
        Task<PagedResultDto<PalletDto>> GetPagedListAsync( PagedAndSortedResultRequestDto input );
        Task<PalletDto> GetAsync( string id );
        Task<PalletDto> GetByIdWithProductAsync( string palletId );
    }
}
