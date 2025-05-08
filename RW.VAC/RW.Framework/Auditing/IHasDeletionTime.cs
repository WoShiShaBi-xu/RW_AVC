using RW.Framework.Core;

namespace RW.Framework.Auditing;

public interface IHasDeletionTime : ISoftDelete
{
    DateTime? DeletionTime { get; }
}