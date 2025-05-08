using FreeSql;

namespace RW.VAC.Domain.Parameters;

public interface IParameterRepository: IBaseRepository<Parameter, Guid>
{
	Task<bool> IsExistCodeAsync(string code);
}