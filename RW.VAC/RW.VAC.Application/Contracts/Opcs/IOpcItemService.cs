using RW.Framework.Application.Services;
using RW.VAC.Domain.Opcs;

namespace RW.VAC.Application.Contracts.Opcs;

public interface IOpcItemService : ICrudAppService<OpcItem, Guid, OpcItemDto, OpcItemListRequestDto,
    OpcItemCreateUpdateDto, OpcItemCreateUpdateDto>
{
}