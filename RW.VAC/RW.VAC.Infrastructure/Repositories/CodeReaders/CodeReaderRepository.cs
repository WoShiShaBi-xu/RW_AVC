using FreeSql;
using RW.Framework.Auditing;
using RW.Framework.Domain;
using RW.VAC.Domain.CodeReaders;

namespace RW.VAC.Infrastructure.Repositories.CodeReaders;

public class CodeReaderRepository(
	IFreeSql freeSql,
	UnitOfWorkManager uowManger,
	IAuditPropertySetter auditPropertySetter)
	: ExtendRepository<CodeReader, Guid>(freeSql, uowManger, auditPropertySetter), ICodeReaderRepository
{
}