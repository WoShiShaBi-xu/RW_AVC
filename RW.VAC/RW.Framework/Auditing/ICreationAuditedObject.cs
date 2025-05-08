namespace RW.Framework.Auditing;

public interface ICreationAuditedObject : IHasCreationTime
{
    Guid? CreatorId { get; }
}

public interface ICreationAuditedObject<TUser> : ICreationAuditedObject
{
    TUser? Creator { get; }
}