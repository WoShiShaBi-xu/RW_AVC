using FreeSql; // 使用 FreeSql 作为 ORM 框架，用于数据库操作
using RW.Framework.Application.Dtos; // 应用层通用 DTO 定义
using RW.Framework.Domain.Entities; // 领域层实体接口定义
using System.Linq.Expressions; // 用于构造动态查询条件的表达式树
using RW.Framework.Extensions; // 框架扩展方法
using AutoMapper; // 用于对象之间的自动映射

namespace RW.Framework.Application.Services;

// 只读应用服务基类，封装对实体的查询功能
public class ReadOnlyAppService<TEntity, TKey, TGetListOutputDto, TGetListInput>(
    IBaseRepository<TEntity , TKey> repository // FreeSql 仓储接口，用于操作实体
) : ApplicationService, // 继承基础应用服务类
    IReadOnlyAppService<TEntity , TKey , TGetListOutputDto , TGetListInput> // 实现只读服务接口
    where TEntity : class, IEntity<TKey> // 泛型约束：实体必须是实现了 `IEntity<TKey>` 的类
{
    protected readonly IBaseRepository<TEntity , TKey> Repository = repository; // 仓储属性，用于数据库操作

    // 根据主键异步获取单个实体
    public Task<TEntity> GetAsync( TKey key )
    {
        return Repository.GetAsync( key ); // 通过仓储方法获取实体
    }

    // 获取所有实体的列表，并转换为 DTO 列表
    public virtual async Task<IList<TGetListOutputDto>> GetListAsync( )
    {
        var list = await Repository.Select.ToListAsync(); // 从数据库查询所有数据
        return Mapper.Map<List<TGetListOutputDto>>( list ); // 使用 AutoMapper 转换为输出 DTO 列表
    }

    // 根据输入参数获取实体列表，并转换为 DTO 列表
    public virtual async Task<IList<TGetListOutputDto>> GetListAsync( TGetListInput input )
    {
        var filter = GreateFilter( input ); // 根据输入参数生成过滤条件
        List<(string, bool)>? order = null;
        if ( input is ISortedResultRequest sortedInput ) // 如果输入支持排序
            order = sortedInput.Sorting; // 获取排序规则

        // 根据过滤条件和排序规则查询数据
        var query = Repository.Where( filter ).OrderByPropertyList( order );
        if ( input is ILimitedResultRequest limitedInput ) // 如果输入支持结果数量限制
            query.Take( limitedInput.Count ); // 限制返回的数据数量

        var list = await query.ToListAsync(); // 执行查询
        return Mapper.Map<List<TGetListOutputDto>>( list ); // 转换为输出 DTO 列表
    }

    // 获取分页的实体列表，并转换为分页结果 DTO
    public virtual async Task<PagedResultDto<TGetListOutputDto>> GetPagedListAsync( TGetListInput input )
    {
        if ( input is not IPagedResultRequest pagedInput ) // 检查是否支持分页输入
            throw new NotImplementedException( "分页参数错误" ); // 如果不支持，抛出异常

        var filter = GreateFilter( input ); // 根据输入生成过滤条件
        List<(string, bool)>? order = null;
        if ( input is ISortedResultRequest sortedInput ) // 如果支持排序
            order = sortedInput.Sorting; // 获取排序规则

        // 执行分页查询，并获取总记录数
        var list = await Repository.Select
            .Where( filter ) // 添加过滤条件
            .OrderByPropertyList( order ) // 应用排序
            .Count( out var total ) // 计算总记录数
            .Page( pagedInput.PageIndex , pagedInput.Count ) // 分页
            .ToListAsync(); // 执行查询

        // 构造分页结果 DTO
        return new PagedResultDto<TGetListOutputDto>(
            total , // 总记录数
            Mapper.Map<IReadOnlyList<TGetListOutputDto>>( list ) // 转换为输出 DTO 列表
        );
    }

    // 根据输入生成过滤条件（可被子类重写）
    protected virtual Expression<Func<TEntity , bool>>? GreateFilter( TGetListInput input )
    {
        // 默认返回 null（无过滤条件）
        return default;
    }
}
