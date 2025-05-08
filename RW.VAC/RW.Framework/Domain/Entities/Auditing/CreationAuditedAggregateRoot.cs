using RW.Framework.Auditing;

namespace RW.Framework.Domain.Entities.Auditing;

[Serializable]
public class CreationAuditedAggregateRoot : AggregateRoot, ICreationAuditedObject
{
	public virtual DateTime CreationTime { get; protected set; }

	public virtual Guid? CreatorId { get; protected set; }
}

public abstract class CreationAuditedAggregateRoot<TKey> : AggregateRoot<TKey>, ICreationAuditedObject
{
	public virtual DateTime CreationTime { get; set; }

	public virtual Guid? CreatorId { get; set; }

	protected CreationAuditedAggregateRoot()
	{

	}

	protected CreationAuditedAggregateRoot(TKey id) : base(id)
	{
	}
}