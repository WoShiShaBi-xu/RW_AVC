using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using RW.Framework.Application.Services;
using RW.Framework.Exceptions;
using RW.Framework.Extensions;
using RW.VAC.Application.Contracts.Users;
using RW.VAC.Domain.Users;

namespace RW.VAC.Application.Services.Users;

public class UserService(IUserRepository repository, UserManager userManager)
	: CrudAppService<User, Guid, UserDto, UserPagedListRequestDto, UserCreateUpdateDto,
		UserCreateUpdateDto>(repository), IUserService
{
	public override Task<User> CreateAsync(UserCreateUpdateDto input)
	{
		input.Password = Encoding.Default.GetBytes($"rw.vac{input.Password!}^%").Md5();
		return userManager.CreateAsync(input.UserName!, input.Account!, input.Password,input.Role);
	}

	public override Task<User> UpdateAsync(Guid id, UserCreateUpdateDto input)
	{
		if(input.Password.NotNullOrWhiteSpace()) input.Password = Encoding.Default.GetBytes($"rw.vac{input.Password!}^%").Md5();
		return userManager.UpdateAsync(id, input.UserName!, input.Account!, input.Password,input.Role);
	}

    public async Task<ClaimsIdentity> GetIdentityAsync( LoginDto dto )
    {
        var user = await repository.Where( t => t.Account == dto.Account ).ToOneAsync();
        if ( user == null )
            throw new BusinessException( "用户名或密码错误" ).WithData( nameof( dto.Account ) , dto.Account );

        if ( user.IsLoggedIn )
            throw new BusinessException( "账户已在其他地方登录" ).WithData( nameof( dto.Account ) , dto.Account );

        var password = Encoding.Default.GetBytes( $"rw.vac{dto.Password!}^%" ).Md5();
        if ( !string.Equals( password , user.Password , StringComparison.OrdinalIgnoreCase ) )
            throw new BusinessException( "用户名或密码错误" ).WithData( nameof( dto.Password ) , dto.Password );

        // 通过验证后，将用户状态置为已登录
        user.IsLoggedIn = true;
        await repository.UpdateAsync( user );

        // 生成ClaimsIdentity
        var claims = new List<Claim>
    {
        new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new(ClaimTypes.Name, user.UserName),
        new(ClaimTypes.Role, user.Role.ToString())
    };

        return new ClaimsIdentity( claims , "UserAuth" );
    }


    protected override Expression<Func<User, bool>>? GreateFilter(UserPagedListRequestDto input)
	{
		Expression<Func<User, bool>>? where = null;
		where = where.And(input.UserName.NotNullOrWhiteSpace(), t => t.UserName.Contains(input.UserName!));
		where = where.And(input.Account.NotNullOrWhiteSpace(), t => t.Account.Contains(input.Account!));
		return where;
	}
}