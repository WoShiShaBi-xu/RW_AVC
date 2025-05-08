using FreeSql;
using RW.Framework.Auditing;

namespace RW.Framework.Domain;

public class ExtendRepository<TEntity, TKey>(
	IFreeSql freeSql,
	UnitOfWorkManager uowManger,
	IAuditPropertySetter auditPropertySetter)
	: DefaultRepository<TEntity, TKey>(freeSql, uowManger)
	where TEntity : class
{
	protected readonly IAuditPropertySetter AuditPropertySetter = auditPropertySetter;

	public override Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		AuditPropertySetter.SetCreationProperties(entity);
		return base.InsertAsync(entity, cancellationToken);
	}

	public override Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
	{
		AuditPropertySetter.SetModificationProperties(entity);
		return base.UpdateAsync(entity, cancellationToken);
	}
}