using RW.Framework.Application.Dtos;

namespace RW.VAC.Application.Contracts.Opcs
{
    public class OpcGroupListRequestDto : ISortedResultRequest
    {
        public List<(string, bool)> Sorting { get; set; }
    }
}
