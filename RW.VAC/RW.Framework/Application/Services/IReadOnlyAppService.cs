using RW.Framework.Application.Dtos;

namespace RW.Framework.Application.Services;

public interface IReadOnlyAppService<TEntity, in TKey, TGetListOutputDto, in TGetListInput> : IApplicationService
{
	Task<TEntity> GetAsync(TKey key);

	Task<IList<TGetListOutputDto>> GetListAsync();

	Task<IList<TGetListOutputDto>> GetListAsync(TGetListInput input);

	Task<PagedResultDto<TGetListOutputDto>> GetPagedListAsync(TGetListInput input);
}