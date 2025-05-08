using System.Linq.Expressions; // 提供表达式树的功能，用于动态构造查询条件
using RW.Framework.Application.Services; // 自定义框架中的应用服务基类
using RW.VAC.Application.Contracts.Records; // 记录模块的应用层契约
using RW.VAC.Domain.Records; // 记录模块的领域层模型

namespace RW.VAC.Application.Services.Records;

// 服务类 `ProductionDetailService`，用于处理生产详细记录的业务逻辑
public class ProductionDetailService(
    IProductionDetailRepository repository , // 数据库仓储接口，用于操作生产详细记录数据
    ProductionDetailManager productionDetailManager // 管理器类，封装复杂业务逻辑
) : CrudAppService<ProductionDetail , Guid , ProductionDetailDto ,
    ProdDetailListRequestDto , ProductionDetail , ProductionDetail>( repository ), // 基类，实现增删改查功能
    IProductionDetailService // 定义接口，确保实现统一的业务逻辑约定
{
    // 根据记录ID和工序名称异步查询生产详细记录
    public async Task<ProductionDetail?> GetByAsync( Guid recordId , string processName )
    {
        return await repository.GetByAsync( recordId , processName ); // 调用仓储方法获取数据
    }

    // 设置生产详细记录为异常下线状态，并更新数据库
    public Task SetAbnormalOffline( ProductionDetail detail )
    {
        if( detail !=null)
        {
            detail.SetAbnormalOffline(); // 调用领域模型中的方法更改状态
            return repository.UpdateAsync( detail ); // 异步保存更改
        }
        return Task.CompletedTask;
    }

    // 设置生产详细记录为完成状态，并更新数据库
    public Task SetCompletedAsync( ProductionDetail detail )
    {
        if( detail !=null ) {
            detail.SetCompleted(); // 调用领域模型中的方法更改状态
            return Repository.UpdateAsync( detail ); // 异步保存更改
        }
       return Task.CompletedTask;
    }
    // 设置生产详细记录为完成状态，并保存附加数据
    public object _lock=new object();
    public async Task SetCompletedWithDataAsync( ProductionDetail detail , List<ProductionDataCreateDto> data )
    {

        var dataList = GetDataListTuple( data );
        await productionDetailManager.SetCompletedWithDataAsync( detail , dataList );
        
    }
    // 根据请求DTO创建动态过滤条件
    protected override Expression<Func<ProductionDetail , bool>>? GreateFilter( ProdDetailListRequestDto input )
    {
        Expression<Func<ProductionDetail , bool>>? where = null; // 初始化空的条件表达式
                                                                 // 动态构造条件：如果RecordId有值，则增加对应过滤条件
        where = where.And( input.RecordId.HasValue , t => t.RecordId == input.RecordId!.Value );
        return where; // 返回生成的条件表达式
    }

    // 将输入的生产数据DTO列表转换为元组列表
    private List<(string name, string value, string? unit)> GetDataListTuple( List<ProductionDataCreateDto> data )
    {
        // 使用 LINQ 将 DTO 转换为元组
        return data.Select( item => (item.Name, item.Value, item.Unit) ).ToList();
    }
}
