using Autofac;
using Autofac.Extensions.DependencyInjection;
using RW.VAC.Application.Contracts.AGV;
using RW.VAC.Application.Contracts.API;
using RW.VAC.Application.Services.Locations;
using RW.VAC.Application.Services.Pallets;
using RW.VAC.Application.Services.ProductPalletBindings;
using RW.VAC.Application.Services.Products;
using RW.VAC.Domain.API;
using RW.VAC.Domain.Location;
using RW.VAC.Domain.Pallet;
using RW.VAC.Domain.ProductPalletBinding;
using RW.VAC.Domain.Products;
using RW.VAC.Infrastructure.Opc;
using RW.VAC.Infrastructure.Repositories;
using FreeSql;

var builder = WebApplication.CreateBuilder( args );

builder.Services.AddHttpClient();
builder.Host.UseServiceProviderFactory( new AutofacServiceProviderFactory() );
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // 1. 配置FreeSql
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    var freeSqlBuilder = new FreeSql.FreeSqlBuilder()
        .UseConnectionString(FreeSql.DataType.MySql, connectionString) // 根据实际数据库类型调整
        .UseAutoSyncStructure(true) // 自动同步结构
        .Build();

    // 注册IFreeSql
    containerBuilder.RegisterInstance(freeSqlBuilder).As<IFreeSql>().SingleInstance();

    // 2. 注册Repository层
    containerBuilder.RegisterType<ProductRepository>().As<IProductRepository>();
    containerBuilder.RegisterType<ProductPalletBindingRepository>().As<IProductPalletBindingRepository>();
    containerBuilder.RegisterType<LocationRepository>().As<ILocationRepository>();
    containerBuilder.RegisterType<PalletRepository>().As<IPalletRepository>();

    // 3. 注册Service层
    containerBuilder.RegisterType<ProductService>().As<IProductService>();
    containerBuilder.RegisterType<ProductPalletBindingService>().As<IProductPalletBindingService>();
    containerBuilder.RegisterType<LocationService>().As<ILocationService>();
    containerBuilder.RegisterType<PalletService>().As<IPalletService>();
});
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
