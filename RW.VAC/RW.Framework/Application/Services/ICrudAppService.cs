namespace RW.Framework.Application.Services;

public interface ICrudAppService<TEntity, in TKey, TGetListOutputDto, in TGetListInput, in TCreateInput,
	in TUpdateInput> : IReadOnlyAppService<TEntity, TKey, TGetListOutputDto, TGetListInput>
{
	Task<TEntity> CreateAsync(TCreateInput input);

	Task<TEntity> UpdateAsync(TKey id, TUpdateInput input);

	Task<int> DeleteAsync(TKey key);
}