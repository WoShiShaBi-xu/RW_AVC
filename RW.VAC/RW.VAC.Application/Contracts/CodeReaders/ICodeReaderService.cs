using RW.Framework.Application.Dtos;
using RW.Framework.Application.Services;
using RW.VAC.Domain.CodeReaders;

namespace RW.VAC.Application.Contracts.CodeReaders;

public interface ICodeReaderService: IReadOnlyAppService<CodeReader, Guid, CodeReader, PagedAndSortedResultRequestDto>
{
}