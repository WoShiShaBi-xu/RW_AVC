using RW.Framework.Domain.Services;
using RW.Framework.Exceptions;

namespace RW.VAC.Domain.Parameters;

public class ParameterManager(IParameterRepository parameterRepository) : DomainService
{
	public async Task<Parameter> CreateAsync(string code, string value, string? description)
	{
		await CheckCode(code);
		var parameter = new Parameter(GuidGenerator.Greate(), code, value, description);
		await parameterRepository.InsertAsync(parameter);
		return parameter;
	}
		
	public async Task<Parameter> UpdateAsync(Guid id, string code, string value, string? description)
	{
		var parameter = await parameterRepository.GetAsync(id);
		await CheckCode(code, parameter);
		parameter.Update(code, value, description);
		await parameterRepository.UpdateAsync(parameter);
		return parameter;
	}

	private async Task CheckCode(string code, Parameter? parameter = null)
	{
		if (parameter != null && string.Equals(parameter.Code, code, StringComparison.OrdinalIgnoreCase)) return;
		if (await parameterRepository.IsExistCodeAsync(code)) throw new BusinessException("存在相同的参数编码").WithData("Code", code);
	}
}