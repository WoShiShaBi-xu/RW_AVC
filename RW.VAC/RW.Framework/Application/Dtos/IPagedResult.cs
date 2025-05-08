namespace RW.Framework.Application.Dtos;

public interface IPagedResult<T> : IListResult<T>, IHasTotalCount
{
}