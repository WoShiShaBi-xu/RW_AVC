using Microsoft.AspNetCore.Components;
using RW.Framework.Http;
using RW.VAC.Application.Contracts.Opcs;
using RW.VAC.Application.Contracts.Products;
using RW.VAC.Application.Contracts.Records;
using RW.VAC.Application.Services.Opcs;

namespace RW.VAC.Dashboard.Bridge;

// CapacityBridge 类，用于在前端和后台服务之间进行数据桥接。
// 通过注入的服务接口 IProductionRecordService 和 IProductService 进行数据获取和处理。

public class CapacityBridge( IProductionRecordService productionRecordService , IProductService productService, IOpcGroupService opcGroupService)
{
    // 获取一周产能数据
    // 该方法从生产记录服务中获取过去一周的产能数据，并将其封装在通用响应对象中返回。
    // 如果在获取数据时发生异常，将返回错误信息。
    public string GetWeekCapacity( )
    {
        try
        {
            // 从生产记录服务获取一周的产能数据
            var data = productionRecordService.GetWeekCapacity();

            // 返回封装在 GeneralResponse 中的成功响应
            return GeneralResponse<IList<ProdRecordCountDto>>.Success( data );
        }
        catch ( Exception )
        {
            // 如果发生异常，返回错误信息
            return GeneralResponse.Error( -1 , "系统错误，请联系管理人员" );
        }
    }

    // 获取工艺参数点位数据
    public async Task<string> getProcessDate()
    {
        try
        {
            // 获取所有的工艺参数点信息
            List<ProcessDto> opcProcessList = await opcGroupService.GetOPCProcessAsync();

            // 返回封装在 GeneralResponse 中的成功响应
            return GeneralResponse<List<ProcessDto>>.Success(opcProcessList);
        }
        catch (Exception)
        {
            // 如果发生异常，返回错误信息
            return GeneralResponse.Error(-1, "系统错误，请联系管理人员");
        }
    }


    // 获取指定日期的产能数据
    // 该方法根据传入的类型参数，获取对应日期的产能数据并返回。
    // 如果发生异常，将返回错误信息。
    public string GetDateCapacity( int type )
    {
        try
        {
            // 根据类型参数获取指定日期的产能数据
            var data = productionRecordService.GetDateCapacity( type );

            // 返回封装在 GeneralResponse 中的成功响应
            return GeneralResponse<long>.Success( data );
        }
        catch ( Exception )
        {
            // 如果发生异常，返回错误信息
            return GeneralResponse.Error( -1 , "系统错误，请联系管理人员" );
        }
    }

    // 获取指定日期的下线数量数据
    // 该方法根据传入的类型参数，获取对应日期的下线数量数据并返回。
    // 如果发生异常，将返回错误信息。
    public string GetDateOffLine( int type )
    {
        try
        {
            // 根据类型参数获取指定日期的下线数量数据
            var data = productionRecordService.GetDateOffLine( type );

            // 返回封装在 GeneralResponse 中的成功响应
            return GeneralResponse<long>.Success( data );
        }
        catch ( Exception )
        {
            // 如果发生异常，返回错误信息
            return GeneralResponse.Error( -1 , "系统错误，请联系管理人员" );
        }
    }

    // 异步获取产品名称列表
    // 该方法异步地从产品服务中获取产品列表，并提取产品名称返回。
    // 如果发生异常，将返回错误信息。
    public async Task<string> GetProductName( )
    {
        try
        {
            // 异步获取产品列表
            var list = await productService.GetListAsync();

            // 提取产品名称并返回封装在 GeneralResponse 中的成功响应
            return GeneralResponse<IList<string>>.Success( list.Select( x => x.Name ).ToList() );
        }
        catch ( Exception )
        {
            // 如果发生异常，返回错误信息
            return GeneralResponse.Error( -1 , "系统错误，请联系管理人员" );
        }
    }
}
