namespace RW.Framework.Application.Dtos;

public interface IListResult<T>
{
	IReadOnlyList<T> Items { get; set; }
}