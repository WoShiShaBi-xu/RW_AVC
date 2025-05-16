
using RW.Framework.EventBus;
using RW.Framework.EventBus.Local;
using RW.VAC.Application.Contracts.Opcs;
using RW.VAC.Application.Services.Opcs;
using RW.VAC.Dashboard.API.WebSocketManagers;
using RW.VAC.Dashboard.subscribe;
using RW.VAC.Domain.Opcs;
using RW.VAC.Infrastructure.Opc;
using RW.VAC.Infrastructure.Repositories.Opcs;
using System.Net.WebSockets;
using System.Configuration;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RW.Framework.Autofac.Modules;
using RW.Framework.Config;
using RW.Framework.Security.Auth;
using RW.VAC.Application.Events.EventData;
using RW.VAC.Dashboard.service;
using RW.VAC.Dashboard.settings;
using RW.VAC.Dashboard.subscribe;
using RW.VAC.Domain.Opcs;
using RW.VAC.Infrastructure.Opc;


var builder = WebApplication.CreateBuilder( args );

// Add services to the container.
//var configuration = new ConfigurationBuilder()
//    .SetBasePath( AppContext.BaseDirectory )
//    .AddJsonFile( "appsettings.json" , optional: false , reloadOnChange: true )
//    .Build();

var configuration = builder.Configuration;
configuration.AddJsonFile( "appsettings.json" , optional: false , reloadOnChange: true );


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<WebSocketManagers>();

var applicationLayer = configuration [ "LayerConfig:ApplicationLayer" ];
if ( applicationLayer == null ) throw new ConfigurationErrorsException( "应用服务层配置错误" );
builder.Services.AddAutoMapper( Assembly.Load( applicationLayer ) );

builder.Services.AddHostedService<CommunicationHostService>();


// 使用 Autofac 作为 DI 容器

builder.Host.UseServiceProviderFactory( new AutofacServiceProviderFactory() );

builder.Host.ConfigureContainer<ContainerBuilder>( container =>
{
    // 注册模块
    container.RegisterModule( new FrameworkModule() );
    container.RegisterModule( new FreeSqlModule( configuration ) );
    container.RegisterModule( new RepositoryModule( configuration ) );
    container.RegisterModule( new ServiceModule( configuration ) );
    // 注册服务

    container.RegisterType<ExternalAuthService>().SingleInstance();
    container.RegisterType<ExternalAuthenticationStateProvider>().As<AuthenticationStateProvider>();
    container.RegisterType<SettingProvider>().SingleInstance();
    container.RegisterType<TagStorage>().SingleInstance();
    container.RegisterType<UaClient>().As<IUaClient>().SingleInstance();
    container.RegisterType<DeviceSubscribe>().SingleInstance();
    container.RegisterType<OpcItemService>().As<IOpcItemService>().InstancePerLifetimeScope();
    // 领域服务注册
    container.RegisterType<OpcNodeManager>().InstancePerLifetimeScope();
    container.RegisterType<OpcGroupManager>().InstancePerLifetimeScope();
    container.RegisterType<OpcItemManager>().InstancePerLifetimeScope();
    container.RegisterType<WebSocketManagers>().SingleInstance();

} );
builder.Services.AddCors( options =>
{
    options.AddDefaultPolicy( policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    } );
} );
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.Map( "/ws" , async context =>
{
    var webSocketManager = context.RequestServices.GetRequiredService<WebSocketManagers>();
    if ( context.WebSockets.IsWebSocketRequest )//等待客户端连接
    {
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        webSocketManager.AddSocket( webSocket );
        using var scope = app.Services.CreateScope();//连接后连接opc触发第一次事件初始化
        var opcNodeManager = scope.ServiceProvider.GetRequiredService<OpcNodeManager>();
        await opcNodeManager.LoadOpcNodeAsync();
        await EchoWebSocket( webSocket );
        
    }
    else
    {
        context.Response.StatusCode = 400;
    }
} );

app.UseCors();
app.UseHttpsRedirection();
app.UseWebSockets();
app.UseAuthorization();
app.MapControllers();
app.Run();

static async Task EchoWebSocket( WebSocket webSocket )
{
    var buffer = new byte [ 1024 ];
    WebSocketReceiveResult result;
    do
    {
        result = await webSocket.ReceiveAsync( new ArraySegment<byte>( buffer ) , CancellationToken.None );
        await webSocket.SendAsync( new ArraySegment<byte>( buffer , 0 , result.Count ) , result.MessageType , result.EndOfMessage , CancellationToken.None );
    } while ( !result.CloseStatus.HasValue );
}




