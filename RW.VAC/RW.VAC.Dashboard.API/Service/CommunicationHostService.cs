using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RW.VAC.Application.Contracts.Opcs;
using RW.VAC.Infrastructure.Opc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RW.VAC.Dashboard.settings;
using RW.VAC.Dashboard.subscribe;

namespace RW.VAC.Dashboard.service;

// CommunicationHostService 类实现了 IHostedService 接口，
// 负责在应用程序启动时初始化 OPC 通信，并在停止时关闭 OPC 连接。
public class CommunicationHostService( IServiceProvider serviceProvider , 
    IUaClient uaClient , DeviceSubscribe deviceSubscribe ) : IHostedService
{
    // 应用程序启动时调用此方法
    // 该方法会异步调用 InitOpc 方法初始化 OPC 连接。
    public async Task StartAsync( CancellationToken cancellationToken )
    {
        // 异步初始化 OPC 连接
        await InitOpc();
    }

    // 应用程序停止时调用此方法
    // 该方法负责关闭 OPC 客户端连接。
    public Task StopAsync( CancellationToken cancellationToken )
    {
        // 关闭 OPC 客户端连接
        uaClient.Close();

        // 返回已完成任务，表示没有其他停止操作
        return Task.CompletedTask;
    }

    // 初始化 OPC 连接
    // 该方法使用 SettingProvider 中的配置参数，建立与 OPC 服务器的连接。
    private async Task InitOpc( )
    {
        // 从服务提供者中获取 SettingProvider 实例（尽管在构造函数中已传入，但此处又获取了一次）
        var settingProvider = serviceProvider.GetRequiredService<SettingProvider>();

        // 使用 SettingProvider 中的 OpcUrl 参数与 OPC 服务器建立连接
        await uaClient.Connect( settingProvider.Parameter [ "OpcUrl" ] );
    }
}
