using RW.Framework.Application.Services;
using RW.VAC.Domain.Parameters;

namespace RW.VAC.Application.Contracts.Parameters;

public interface IParameterService : ICrudAppService<Parameter, Guid, ParameterDto, ParameterPagedListRequestDto,
	ParameterCreateUpdateDto, ParameterCreateUpdateDto>
{
    Task SetParameterAsync( string code , string value );

    Task SetParameterAsyncStatisticType( string code , string value );
}
