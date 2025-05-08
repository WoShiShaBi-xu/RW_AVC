using RW.Framework.Application.Dtos;

namespace RW.VAC.Application.Contracts.Opcs;

public class OpcItemListRequestDto : PagedAndSortedResultRequestDto
{
    public Guid? GroupId { get; set; }
}