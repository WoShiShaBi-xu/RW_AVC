using RW.Framework.Auditing;

namespace RW.Framework.Domain.Entities.Auditing;

[Serializable]
public abstract class FullAuditedAggregateRootWithUser<TUser> : FullAuditedAggregateRoot, IFullAuditedObject<TUser>
	where TUser : IEntity<Guid>
{
	public virtual TUser? Deleter { get; set; }

	public virtual TUser? Creator { get; protected set; }

	public virtual TUser? LastModifier { get; set; }
}

[Serializable]
public abstract class FullAuditedAggregateRootWithUser<TKey, TUser> : FullAuditedAggregateRoot<TKey>, IFullAuditedObject<TUser>
	where TUser : IEntity<Guid>
{
	public virtual TUser? Deleter { get; set; }

	public virtual TUser? Creator { get; protected set; }

	public virtual TUser? LastModifier { get; set; }

	protected FullAuditedAggregateRootWithUser()
	{
	}

	protected FullAuditedAggregateRootWithUser(TKey id)
		: base(id)
	{
	}
}