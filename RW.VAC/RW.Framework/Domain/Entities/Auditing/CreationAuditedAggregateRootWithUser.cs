using RW.Framework.Auditing;

namespace RW.Framework.Domain.Entities.Auditing;

[Serializable]
public abstract class CreationAuditedAggregateRootWithUser<TUser> : CreationAuditedAggregateRoot, ICreationAuditedObject<TUser>
{
	public virtual TUser? Creator { get; protected set; }
}

[Serializable]
public abstract class CreationAuditedAggregateRootWithUser<TKey, TUser> : CreationAuditedAggregateRoot<TKey>, ICreationAuditedObject<TUser>
{
	public virtual TUser? Creator { get; protected set; }

	protected CreationAuditedAggregateRootWithUser()
	{
	}

	protected CreationAuditedAggregateRootWithUser(TKey id) : base(id)
	{
	}
}