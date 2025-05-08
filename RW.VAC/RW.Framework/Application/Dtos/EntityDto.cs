namespace RW.Framework.Application.Dtos;

[Serializable]
public class EntityDto : IEntityDto
{
}

[Serializable]
public abstract class EntityDto<TKey> : EntityDto, IEntityDto<TKey>
{
	public TKey Id { get; set; } = default!;
}