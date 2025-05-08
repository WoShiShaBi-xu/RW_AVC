namespace RW.Framework.Domain.Entities;

[Serializable]
public abstract class AggregateRoot : Entity, IAggregateRoot
{
}

[Serializable]
public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot
{
	protected AggregateRoot()
	{
	}

	protected AggregateRoot(TKey id) : base(id)
	{
	}
}