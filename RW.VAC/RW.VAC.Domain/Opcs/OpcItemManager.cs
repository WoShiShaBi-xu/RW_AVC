using RW.Framework.Domain.Services;
using RW.Framework.Exceptions;

namespace RW.VAC.Domain.Opcs;

public class OpcItemManager(IOpcItemRepository opcItemRepository) : DomainService
{
	public async Task<OpcItem> CreateAsync(Guid groupId, string name, string code, string? description,bool isProcess)
	{
		await CheckName(groupId, name);
		await CheckCode(groupId, code);
		var opcItem = new OpcItem(GuidGenerator.Greate(), groupId, code, name, description, isProcess);
		await opcItemRepository.InsertAsync(opcItem);
		return opcItem;
	}

	public async Task<OpcItem> UpdateAsync(Guid id, string name, string code, string? description, bool isProcess)
	{
		var opcItem = await opcItemRepository.GetAsync(id);
		if (opcItem == null) throw new BusinessException("点位信息不存在").WithData("Id", id);
		await CheckName(opcItem.GroupId, name, opcItem);
		await CheckCode(opcItem.GroupId, code, opcItem);
		opcItem.Update(code, name, description, isProcess);
		await opcItemRepository.UpdateAsync(opcItem);
		return opcItem;
	}

	private async Task CheckName(Guid groupId, string name, OpcItem? opcItem = null)
	{
		if (opcItem != null && string.Equals(opcItem.Name, name, StringComparison.OrdinalIgnoreCase)) return;
		if (await opcItemRepository.IsExistNameAsync(groupId, name))
			throw new BusinessException("该分组下存在相同的标记名称").WithData("Name", name);
	}

	private async Task CheckCode(Guid groupId, string code, OpcItem? opcItem = null)
	{
		if (opcItem != null && string.Equals(opcItem.Code, code, StringComparison.OrdinalIgnoreCase)) return;
		if (await opcItemRepository.IsExistCodeAsync(groupId, code))
			throw new BusinessException("该分组下存在相同的标记编码").WithData("Code", code);
	}
}