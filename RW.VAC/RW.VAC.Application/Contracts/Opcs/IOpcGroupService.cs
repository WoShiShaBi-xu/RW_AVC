using RW.Framework.Application.Dtos;
using RW.Framework.Application.Services;
using RW.VAC.Domain.Opcs;

namespace RW.VAC.Application.Contracts.Opcs;

public interface IOpcGroupService : ICrudAppService<OpcGroup, Guid, OpcGroupDto, OpcGroupListRequestDto,
	OpcGroupCreateUpdateDto, OpcGroupCreateUpdateDto>
{
	Task DeleteWithItemAsync(Guid id);

	Task<List<ProcessDto>> GetOPCProcessAsync();
}