using FreeSql;
using RW.Framework.Core;
using RW.Framework.Domain.Entities;

namespace RW.Framework.Application.Services;

public class CrudAppService<TEntity, TKey, TGetListOutputDto, TGetListInput, TCreateInput, TUpdateInput>(
	IBaseRepository<TEntity, TKey> repository) :
	ReadOnlyAppService<TEntity, TKey, TGetListOutputDto, TGetListInput>(repository),
	ICrudAppService<TEntity, TKey, TGetListOutputDto, TGetListInput, TCreateInput, TUpdateInput>
	where TEntity : class, IEntity<TKey>
{
	public virtual Task<TEntity> CreateAsync(TCreateInput input)
	{
		var entity = MapToEntity(input);
		return Repository.InsertAsync(entity);
	}

	public virtual async Task<TEntity> UpdateAsync(TKey id, TUpdateInput input)
	{
		var entity = await Repository.GetAsync(id);
		Mapper.Map(input, entity);
		await Repository.UpdateAsync(entity);
		return entity;
	}

	public virtual Task<int> DeleteAsync(TKey key)
	{
		return Repository.DeleteAsync(key);
	}

	protected virtual TEntity MapToEntity(TCreateInput input)
	{
		var entity = Mapper.Map<TEntity>(input);
		SetIdForGuids(entity);
		return entity;
	}

	protected virtual void SetIdForGuids(TEntity entity)
	{
		if (entity is IEntity<Guid> entityWithGuid && entityWithGuid.Id == Guid.Empty)
			ObjectHelper.TrySetProperty(entityWithGuid, t => t.Id, () => GuidGenerator.Greate());
	}
}