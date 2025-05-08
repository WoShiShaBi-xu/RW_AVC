using RW.Framework.Domain.Services;
using RW.Framework.Exceptions;

namespace RW.VAC.Domain.Opcs;

public class OpcGroupManager(
	IOpcGroupRepository opcGroupRepository,
	IOpcItemRepository opcItemRepository) : DomainService
{
    public async Task<OpcGroup> CreateAsync( string name , string code , string device , string? group )
    {
        try
        {
            // 校验名称和代码
            await CheckName( name );
            await CheckCode( code );

            // 创建 OpcGroup 实例
            var opcGroup = new OpcGroup( GuidGenerator.Greate() , name , code , device , group );

            // 使用数据库连接池或上下文，确保每次操作独立
            await opcGroupRepository.InsertAsync( opcGroup );

            // 返回插入的 opcGroup
            return opcGroup;
        }
        catch ( Exception ex )
        {
            // 捕获异常并处理
            // 记录日志或重新抛出异常
            throw new InvalidOperationException( "创建 OPC 组时发生错误" , ex );
        }
    }


    public async Task<OpcGroup> UpdateAsync(Guid id, string name, string code, string device, string? group)
	{
		var opcGroup = await opcGroupRepository.GetAsync(id);
		if (opcGroup == null) throw new BusinessException("点位分组不存在").WithData("Id", id);
		await CheckName(name, opcGroup);
		await CheckCode(code, opcGroup);
		opcGroup.Update(name, code, device, group);
		await opcGroupRepository.UpdateAsync(opcGroup);
		return opcGroup;
	}

	public async Task DeleteAsync(Guid id)
	{
		var opcGroup = await opcGroupRepository.GetAsync(id);
		if (opcGroup == null) throw new BusinessException("点位分组不存在").WithData("Id", id);
		await opcGroupRepository.DeleteAsync(opcGroup);
		await opcItemRepository.DeleteAsync(t => t.GroupId == id);
	}

	private async Task CheckName(string name, OpcGroup? orGroup = null)
	{
		if (orGroup != null && string.Equals(orGroup.Name, name, StringComparison.OrdinalIgnoreCase)) return;
		if (await opcGroupRepository.IsExistNameAsync(name)) throw new BusinessException("存在相同的分组名称").WithData("Name", name);
	}

	private async Task CheckCode(string code, OpcGroup? orGroup = null)
	{
		if (orGroup != null && string.Equals(orGroup.Code, code, StringComparison.OrdinalIgnoreCase)) return;
		if (await opcGroupRepository.IsExistCodeAsync(code)) throw new BusinessException("存在相同的分组编码").WithData("Code", code);
	}
}