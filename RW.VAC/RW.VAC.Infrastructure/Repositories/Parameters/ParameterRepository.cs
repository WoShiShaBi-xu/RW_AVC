using FreeSql;
using RW.Framework.Auditing;
using RW.Framework.Domain;
using RW.VAC.Domain.Parameters;

namespace RW.VAC.Infrastructure.Repositories.Parameters;

public class ParameterRepository(
	IFreeSql freeSql,
	UnitOfWorkManager uowManger,
	IAuditPropertySetter auditPropertySetter)
	: ExtendRepository<Parameter, Guid>(freeSql, uowManger, auditPropertySetter), IParameterRepository
{
	public Task<bool> IsExistCodeAsync(string code)
	{
		return Select.AnyAsync(t => t.Code == code);
	}
}