using RW.Framework.Guids;
// ReSharper disable InconsistentNaming

namespace RW.Framework.Domain.Services;

public class DomainService : IDomainService
{
	public IGuidGenerator GuidGenerator => _guidGenerator.Value;

	public required Lazy<IGuidGenerator> _guidGenerator { private get; init; }
}