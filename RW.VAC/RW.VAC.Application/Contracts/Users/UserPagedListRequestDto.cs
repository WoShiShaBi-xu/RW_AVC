using RW.Framework.Application.Dtos;

namespace RW.VAC.Application.Contracts.Users;

public class UserPagedListRequestDto : PagedAndSortedResultRequestDto
{
	public string? UserName { get; set; }

	public string? Account { get; set; }
}