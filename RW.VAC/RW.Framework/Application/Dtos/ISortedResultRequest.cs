namespace RW.Framework.Application.Dtos;

public interface ISortedResultRequest
{
	List<(string, bool)> Sorting { get; set; }
}