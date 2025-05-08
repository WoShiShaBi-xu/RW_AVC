using FreeSql;
using Microsoft.Extensions.Logging;
using RW.Framework.Domain.Services;
using RW.Framework.Application.Services; // 自定义框架中的应用服务基类
using RW.VAC.Domain.Records; // 记录模块的领域层模型

namespace RW.VAC.Domain.Records;

// ProductionDetailManager 继承自 DomainService，是一个领域服务类
public class ProductionDetailManager(
    IProductionDetailRepository productionDetailRepository ,  // 用于访问生产详情数据的仓储接口
    IProductionDataRepository productionDataRepository ) : DomainService  // 用于访问生产数据的仓储接口
{
    // SetCompletedWithDataAsync 方法用于设置生产详情为完成状态并插入相关的生产数据
    public async Task SetCompletedWithDataAsync( ProductionDetail detail , List<(string name, string value, string? unit)> data )
    {
        try
        {
            detail.SetCompleted();
            await productionDetailRepository.UpdateAsync( detail ); // 更新生产详情

            if ( data.Count > 0 )
            {
                var dataList = data.Select( item =>
                    new ProductionData( GuidGenerator.Greate() , detail.Id , item.name , item.value , item.unit ) ).ToList();

                await productionDataRepository.InsertAsync( dataList ); // 插入生产数据
            }
            
        }
        catch ( Exception ex )
        {
            throw;
        }

       
    }
}
