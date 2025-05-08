namespace RW.Framework.Domain.Entities;

[Serializable]
public abstract class Entity : IEntity
{
}

[Serializable]
public abstract class Entity<TKey> : Entity, IEntity<TKey>
{
	public TKey Id { get; protected set; } = default!;

	protected Entity()
	{
	}

	protected Entity(TKey id)
	{
		Id = id;
	}
}