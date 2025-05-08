using RW.Framework.Auditing;

namespace RW.Framework.Domain.Entities.Auditing;

[Serializable]
public abstract class AuditedEntityWithUser<TUser> : AuditedEntity, IAuditedObject<TUser> where TUser : IEntity<Guid>
{
    public virtual TUser? Creator { get; protected set; }

    public virtual TUser? LastModifier { get; set; }
}

[Serializable]
public abstract class AuditedEntityWithUser<TKey, TUser> : AuditedEntity<TKey>, IAuditedObject<TUser> where TUser : IEntity<Guid>
{
    public virtual TUser? Creator { get; protected set; }

    public virtual TUser? LastModifier { get; set; }

    protected AuditedEntityWithUser()
    {
    }

    protected AuditedEntityWithUser(TKey id) : base(id)
    {
    }
}