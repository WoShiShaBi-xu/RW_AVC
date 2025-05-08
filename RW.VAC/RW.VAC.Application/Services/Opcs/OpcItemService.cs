using System.Linq.Expressions;
using RW.Framework.Application.Services;
using RW.VAC.Application.Contracts.Opcs;
using RW.VAC.Domain.Opcs;

namespace RW.VAC.Application.Services.Opcs;

public class OpcItemService(
	IOpcItemRepository repository,
	OpcItemManager opcItemManager)
	: CrudAppService<OpcItem, Guid, OpcItemDto, OpcItemListRequestDto,
		OpcItemCreateUpdateDto, OpcItemCreateUpdateDto>(repository), IOpcItemService
{
	public override Task<OpcItem> CreateAsync(OpcItemCreateUpdateDto input)
	{
		return opcItemManager.CreateAsync(input.GroupId, input.Name!, input.Code!, input.Description,input.IsProcess);
	}

	public override Task<OpcItem> UpdateAsync(Guid id, OpcItemCreateUpdateDto input)
	{
		return opcItemManager.UpdateAsync(id, input.Name!, input.Code!, input.Description, input.IsProcess);
	}

	protected override Expression<Func<OpcItem, bool>>? GreateFilter(OpcItemListRequestDto input)
	{
		Expression<Func<OpcItem, bool>>? where = null;
		where = where.And(input.GroupId.HasValue, t => t.GroupId == input.GroupId);
		return where;
	}
}