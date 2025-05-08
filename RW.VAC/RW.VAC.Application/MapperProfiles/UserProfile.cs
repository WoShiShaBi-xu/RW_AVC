using AutoMapper;
using RW.VAC.Application.Contracts.Users;
using RW.VAC.Domain.Users;

namespace RW.VAC.Application.MapperProfiles;

public class UserProfile : Profile
{
	public UserProfile()
	{
		CreateMap<User, UserDto>();

		CreateMap<UserDto, UserCreateUpdateDto>();
	}
}