namespace RW.Framework.Application.Dtos;

public interface IPagedResultRequest : ILimitedResultRequest
{
	int PageIndex { get; set; }
}