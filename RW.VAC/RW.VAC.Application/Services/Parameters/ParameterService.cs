using System.Linq.Expressions;
using RW.Framework.Application.Services;
using RW.Framework.Extensions;
using RW.VAC.Application.Contracts.Parameters;
using RW.VAC.Domain.Parameters;

namespace RW.VAC.Application.Services.Parameters;

public class ParameterService(
	IParameterRepository repository,
	ParameterManager parameterManager)
	: CrudAppService<Parameter, Guid, ParameterDto, ParameterPagedListRequestDto,
		ParameterCreateUpdateDto, ParameterCreateUpdateDto>(repository), IParameterService
{
	public override Task<Parameter> CreateAsync(ParameterCreateUpdateDto input)
	{
		return parameterManager.CreateAsync(input.Code!, input.Value!, input.Description);
	}

    public async Task SetParameterAsync( string code , string value )
    {
        Guid id = Guid.Parse( "5ebcd3ea-4577-11ef-a226-cc827f3eebf7" );
        await parameterManager.UpdateAsync( id , code , value , "产品型号" );
    }
    public async Task SetParameterAsyncStatisticType( string code , string value )
    {
        Guid id = Guid.Parse( "3e53cc03-8f78-11ef-8fa9-00fffeb23495" );
        await parameterManager.UpdateAsync( id , code , value , "下料统计工位" );
    }
    public override Task<Parameter> UpdateAsync(Guid id, ParameterCreateUpdateDto input)
	{
		return parameterManager.UpdateAsync(id, input.Code!, input.Value!, input.Description);
	}

	protected override Expression<Func<Parameter, bool>>? GreateFilter(ParameterPagedListRequestDto input)
	{
		Expression<Func<Parameter, bool>>? where = null;
		where = where.And(input.Code.NotNullOrWhiteSpace(), t => t.Code.Contains(input.Code!));
		return where;
	}

}