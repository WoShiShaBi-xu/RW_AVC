using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RW.VAC.Application.Contracts.Parameters;
using RW.VAC.Dashboard.settings;

namespace RW.VAC.Dashboard.service;

// ApplicationHostService 类实现了 IHostedService 接口，
// 用于在应用程序启动和停止时执行初始化和清理任务。
public class ApplicationHostService( IServiceProvider serviceProvider ) : IHostedService
{
    // 当应用程序启动时调用此方法
    // 该方法会异步调用 LoadParameter 来加载参数配置。
    public async Task StartAsync( CancellationToken cancellationToken )
    {
        // 异步加载参数
        await LoadParameter();
    }

    // 当应用程序停止时调用此方法
    // 此实现中不需要任何停止逻辑，因此直接返回一个已完成的任务。
    public Task StopAsync( CancellationToken cancellationToken )
    {
        // 返回已完成任务，表示没有特殊的停止操作
        return Task.CompletedTask;
    }

    // 加载参数配置
    // 该方法从 IParameterService 中获取参数列表，并将这些参数加载到 SettingProvider 中。
    private async Task LoadParameter( )
    {
        // 从服务提供者中获取 IParameterService 实例
        var parameterService = serviceProvider.GetRequiredService<IParameterService>();

        // 从服务提供者中获取 SettingProvider 实例
        var settingProvider = serviceProvider.GetRequiredService<SettingProvider>();

        // 异步获取参数列表
        var parameterList = await parameterService.GetListAsync();

        // 遍历参数列表，并将每个参数添加到 SettingProvider 的 Parameter 字典中
        foreach ( var param in parameterList )
        {
            settingProvider.Parameter.Add( param.Code , param.Value );
        }
    }
}
