// 引入必要的命名空间
using System.Linq.Expressions; // 用于构建表达式树，常用于动态生成查询条件。
using RW.Framework.Application.Services; // 应用服务基类的命名空间。
using RW.Framework.Extensions; // 包含扩展方法，比如动态条件拼接。
using RW.VAC.Application.Contracts.Records; // 生产记录模块的契约定义。
using RW.VAC.Domain.Records; // 生产记录模块的领域模型。

namespace RW.VAC.Application.Services.Records;

// `ProductionRecordService` 类继承了 `CrudAppService` 并实现了 `IProductionRecordService` 接口。
// 提供对生产记录的增删改查功能以及自定义业务逻辑。
public class ProductionRecordService(
    IProductionRecordRepository repository , // 注入的生产记录仓储接口。
    ProductionRecordManager productionRecordManager // 注入的生产记录管理器，负责复杂领域逻辑。
) : CrudAppService<ProductionRecord , Guid , ProductionRecordDto ,
    ProdRecordPagedListRequestDto , ProductionRecordCreateDto , ProductionRecord>( repository ),
    IProductionRecordService
{
    // 根据序列号获取生产记录，返回 ProductionRecord 类型的对象。
    public async Task<ProductionRecord?> GetByAsync( string serialNumber )
    {
        return await repository.GetByAsync( serialNumber ); // 调用仓储层方法查询记录。
    }

    // 创建生产记录并详细初始化，支持设置完成状态。
    public async Task CreateWithDetailAsync( ProductionRecordCreateDto dto , bool setCompleted = false )
    {
        // 开始工作单元，进行事务管理
        using var uow = UnitOfWorkManager.Begin();

        try
        {
            // 调用领域管理器创建记录
            await productionRecordManager.CreateAsync(
                dto.SerialNumber ,
                dto.ProductName ,
                dto.ProductCode ,
                dto.Routing ,
                dto.Process ,
                setCompleted
            );

            // 如果没有异常，提交事务
            uow.Commit();
        }
        catch ( Exception ex )
        {
            // 发生异常时回滚事务
            uow.Rollback();

            // 记录异常日志或抛出
            // 你可以选择记录异常或重新抛出异常
            throw new InvalidOperationException( "创建生产记录时发生错误" , ex );
        }
    }


    // 设置生产记录为异常下线状态。
    public async Task SetAbnormalOfflineAsync( string serialNumber )
    {
        var record = await repository.GetByAsync( serialNumber ); // 查询记录。
        if ( record != null ) // 如果记录存在。
        {
            record.SetAbnormalOffline(); // 调用领域方法设置状态。
            await repository.UpdateAsync( record ); // 更新到数据库。
        }
    }

    // 恢复生产记录为在线状态。
    public async Task SetBackOnlineAsync( string serialNumber )
    {
        var record = await repository.GetByAsync( serialNumber );
        if ( record != null )
        {
            record.SetBackOnline();
            await repository.UpdateAsync( record );
        }
    }

    // 设置生产记录为完成状态。
    public async Task SetCompletedAsync( string serialNumber )
    {
        var record = await repository.GetByAsync( serialNumber );
        if ( record != null )
        {
            record.SetCompleted();
            await repository.UpdateAsync( record );
        }
    }

    // 获取过去一周的生产记录统计数据。
    public IList<ProdRecordCountDto> GetWeekCapacity( )
    {
        var query = new ProdRecordQueryDto
        {
            StartTime = DateTime.Today.AddDays( -7 ) , // 开始时间为7天前。
            EndTime = DateTime.Now , // 结束时间为当前时间。
            Status = ProdStatus.Completed // 仅统计已完成状态的记录。
        };
        return GetProdCondition( query ); // 调用条件查询方法。
    }

    // 根据类型获取当天的生产容量。
    public long GetDateCapacity( int type )
    {
        var query = new ProdRecordQueryDto { Status = ProdStatus.Completed };
        var now = DateTime.Now;
        switch ( type )
        {
            case 1: // 按天统计。
                query.StartTime = DateTime.Today;
                query.EndTime = now;
                break;
            case 2: // 按小时统计。
                query.StartTime =
                    new DateTime( now.Year , now.Month , now.Day , now.Hour , 0 , 0 );
                query.EndTime = now;
                break;
        }

        // 查询满足条件的记录数量。
        return Repository.Select.Where( t =>
            t.EndTime >= query.StartTime &&
            t.EndTime <= query.EndTime
            && t.Status == query.Status ).Count();
    }

    // 获取当天的异常下线数量。
    public long GetDateOffLine( int type )
    {
        var query = new ProdRecordQueryDto { Status = ProdStatus.Completed };
        var now = DateTime.Now;
        switch ( type )
        {
            case 1:
                query.StartTime = DateTime.Today;
                query.EndTime = now;
                break;
            case 2:
                query.StartTime =
                    new DateTime( now.Year , now.Month , now.Day , now.Hour , 0 , 0 );
                query.EndTime = now;
                break;
        }

        // 查询满足条件的异常下线数量。
        return ( long ) Repository.Select.Where( t =>
            t.EndTime >= query.StartTime &&
            t.EndTime <= query.EndTime
            && t.Status == query.Status ).Sum( t => t.OfflineCount );
    }

    // 根据条件获取生产记录的统计信息。
    public List<ProdRecordCountDto> GetProdCondition( ProdRecordQueryDto query )
    {
        return Repository.Select.Where( t =>
                t.EndTime >= query.StartTime &&
                t.EndTime <= query.EndTime
                && t.Status == query.Status )
            .GroupBy( t => new { t.EndTime.Value.Date , t.ProductName } // 按日期和产品名分组。
            )
            .Select( g => new
            {
                g.Key.Date , // 日期。
                Count = g.Count() , // 记录数量。
                Type = g.Key.ProductName // 产品名称。
            } )
            .GroupBy( t => t.Date ) // 按日期再分组。
            .Select( t => new ProdRecordCountDto
            {
                Date = t.Key.ToString( "MM-dd" ) , // 格式化日期。
                Count = t.Select( x => x.Count ).Sum() , // 统计总数量。
                Type = t.Select( x => new ProdTypeCount // 按产品分类统计。
                {
                    Type = x.Type ,
                    Count = x.Count
                } ).ToDictionary( x => x.Type , x => x )
            } )
            .ToList();
    }
    public async Task<List<string>> GetTodayCompletedSerialNumbersAsync( )
    {
        var startTime = DateTime.Today;
        var endTime = DateTime.Today.AddDays( 1 ).AddTicks( -1 );

        // 获取所有满足条件的记录
        var records = await Repository.Select
            .Where( t => t.EndTime >= startTime && t.EndTime <= endTime && t.Status == ProdStatus.Completed )
            .ToListAsync();

        // 提取 SerialNumber
        var serialNumbers = records.Select( t => t.SerialNumber ).ToList();

        return serialNumbers;
    }



    // 创建分页查询的过滤器。
    protected override Expression<Func<ProductionRecord , bool>>? GreateFilter( ProdRecordPagedListRequestDto input )
    {
        Expression<Func<ProductionRecord , bool>>? where = null;
        // 添加序列号过滤条件。
        where = where.And( input.SerialNumber.NotNullOrWhiteSpace() , t => t.SerialNumber == input.SerialNumber );
        if ( input.DateRange.Length == 2 ) // 添加日期范围过滤条件。
        {
            where = where.And( t =>
                ( t.StartTime >= input.DateRange [ 0 ] && t.StartTime <= input.DateRange [ 1 ] ) ||
                ( t.EndTime >= input.DateRange [ 0 ] && t.EndTime <= input.DateRange [ 1 ] ) );
        }
        return where;
    }
}
