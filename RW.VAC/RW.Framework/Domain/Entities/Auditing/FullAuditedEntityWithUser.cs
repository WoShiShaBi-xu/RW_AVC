using RW.Framework.Auditing;

namespace RW.Framework.Domain.Entities.Auditing;

[Serializable]
public abstract class FullAuditedEntityWithUser<TUser> : FullAuditedEntity, IFullAuditedObject<TUser>
    where TUser : IEntity<Guid>
{
    public virtual TUser? Deleter { get; set; }

    public virtual TUser? Creator { get; protected set; }

    public virtual TUser? LastModifier { get; set; }
}

[Serializable]
public abstract class FullAuditedEntityWithUser<TKey, TUser> : FullAuditedEntity<TKey>, IFullAuditedObject<TUser>
    where TUser : IEntity<Guid>
{
    public virtual TUser? Deleter { get; set; }

    public virtual TUser? Creator { get; protected set; }

    public virtual TUser? LastModifier { get; set; }

    protected FullAuditedEntityWithUser()
    {
    }

    protected FullAuditedEntityWithUser(TKey id)
        : base(id)
    {
    }
}