using System.Diagnostics;
using Autofac;
using FreeSql;
using Microsoft.Extensions.Configuration;

namespace RW.Framework.Autofac.Modules;

public class FreeSqlModule(IConfiguration configuration) : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		var fsql = new FreeSqlBuilder()
			.UseConnectionString(DataType.MySql, configuration.GetConnectionString("DefaultConnection"))
			#if DEBUG
			.UseMonitorCommand(cmd => Debug.WriteLine($"Sql：{cmd.CommandText}")) //监听SQL语句
			.UseAutoSyncStructure(true) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。	
			#endif
			.Build();

		builder.RegisterInstance(fsql).SingleInstance();
		builder.RegisterType<UnitOfWorkManager>().InstancePerLifetimeScope();
	}
}