using FreeSql;

namespace RW.VAC.Domain.Opcs;

public interface IOpcItemRepository : IBaseRepository<OpcItem, Guid>
{
	Task<bool> IsExistNameAsync(Guid groupId, string name);

	Task<bool> IsExistCodeAsync(Guid groupId, string code);
}