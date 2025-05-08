using FreeSql;

namespace RW.VAC.Domain.Opcs;

public interface IOpcGroupRepository : IBaseRepository<OpcGroup, Guid>
{
	Task<bool> IsExistNameAsync(string name);

	Task<bool> IsExistCodeAsync(string code);
}