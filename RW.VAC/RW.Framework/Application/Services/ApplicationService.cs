using AutoMapper;
using FreeSql;
using RW.Framework.Guids;
// ReSharper disable InconsistentNaming

namespace RW.Framework.Application.Services;

public class ApplicationService : IApplicationService
{

	protected IMapper Mapper => _mapper.Value;

	public required Lazy<IMapper> _mapper { private get; init; }

	protected IGuidGenerator GuidGenerator => _guidGenerator.Value;

	public required Lazy<IGuidGenerator> _guidGenerator { private get; init; }

	protected UnitOfWorkManager UnitOfWorkManager => _unitOfWorkManager.Value;

	public required Lazy<UnitOfWorkManager> _unitOfWorkManager { private get; init; }
}