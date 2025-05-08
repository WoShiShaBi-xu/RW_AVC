namespace RW.Framework.Application.Dtos;

[Serializable]
public class ListResultDto<T> : IListResult<T>
{
	private IReadOnlyList<T>? _items;

	public IReadOnlyList<T> Items
	{
		get { return _items ??= new List<T>(); }
		set => _items = value;
	}

	public ListResultDto()
	{
	}

	public ListResultDto(IReadOnlyList<T> items)
	{
		Items = items;
	}
}