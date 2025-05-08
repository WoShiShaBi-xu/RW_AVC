namespace RW.Framework.Auditing;

public interface IHasModificationTime
{
    DateTime? LastModificationTime { get; }
}