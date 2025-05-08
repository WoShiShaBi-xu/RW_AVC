using RW.Framework.Application.Dtos;
using RW.Framework.Application.Services;
using RW.VAC.Application.Contracts.CodeReaders;
using RW.VAC.Domain.CodeReaders;

namespace RW.VAC.Application.Services.CodeReaders;

public class CodeReaderService(ICodeReaderRepository repository)
	: ReadOnlyAppService<CodeReader, Guid, CodeReader, PagedAndSortedResultRequestDto>(
		repository), ICodeReaderService
{
}