using RW.Framework.Auditing;

namespace RW.Framework.Domain.Entities.Auditing;

[Serializable]
public abstract class AuditedAggregateRootWithUser<TUser> : AuditedAggregateRoot, IAuditedObject<TUser>
	where TUser : IEntity<Guid>
{
	public virtual TUser? Creator { get; protected set; }

	public virtual TUser? LastModifier { get; set; }
}

[Serializable]
public abstract class AuditedAggregateRootWithUser<TKey, TUser> : AuditedAggregateRoot<TKey>, IAuditedObject<TUser>
	where TUser : IEntity<Guid>
{
	public virtual TUser? Creator { get; protected set; }

	public virtual TUser? LastModifier { get; set; }

	protected AuditedAggregateRootWithUser()
	{
	}

	protected AuditedAggregateRootWithUser(TKey id) : base(id)
	{
	}
}