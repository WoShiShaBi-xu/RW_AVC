using Autofac.Extensions.DependencyInjection;
using Autofac;
using RW.VAC.Application.Contracts.AGV;
using RW.VAC.Application.Contracts.API;
using RW.VAC.Domain.API;
using RW.VAC.Infrastructure.Opc;

var builder = WebApplication.CreateBuilder( args );

builder.Services.AddHttpClient();
builder.Host.UseServiceProviderFactory( new AutofacServiceProviderFactory() );
builder.Host.ConfigureContainer<ContainerBuilder>( containerBuilder =>
{
    // 注册你的服务
    containerBuilder.RegisterType<AgvService>().As<IAgvService>().SingleInstance();
    containerBuilder.RegisterType<WMSClient>().As<IWMSClient>().SingleInstance();
    containerBuilder.RegisterType<UaClient>().As<IUaClient>().SingleInstance();
    containerBuilder.RegisterType<TagStorage>().SingleInstance();
    // 添加其他需要的服务注册...
} );
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
