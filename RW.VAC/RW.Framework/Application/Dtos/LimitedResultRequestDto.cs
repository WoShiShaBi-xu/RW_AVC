namespace RW.Framework.Application.Dtos;

public class LimitedResultRequestDto : ILimitedResultRequest
{
	public int Count { get; set; } = 20;
}