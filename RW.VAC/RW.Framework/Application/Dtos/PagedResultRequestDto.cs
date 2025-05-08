namespace RW.Framework.Application.Dtos;

public class PagedResultRequestDto : LimitedResultRequestDto, IPagedResultRequest
{
	public int PageIndex { get; set; } = 1;
}