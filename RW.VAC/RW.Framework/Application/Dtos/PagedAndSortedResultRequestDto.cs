namespace RW.Framework.Application.Dtos;

public class PagedAndSortedResultRequestDto :PagedResultRequestDto, IPagedAndSortedResultRequest
{
	public List<(string, bool)>? Sorting { get; set; }
}