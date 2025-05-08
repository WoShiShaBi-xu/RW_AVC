using RW.Framework.Application.Dtos;

namespace RW.VAC.Application.Contracts.Records;

public class ProdDetailListRequestDto : ISortedResultRequest
{
	public Guid? RecordId { get; set; }

	public List<(string, bool)> Sorting { get; set; }
}