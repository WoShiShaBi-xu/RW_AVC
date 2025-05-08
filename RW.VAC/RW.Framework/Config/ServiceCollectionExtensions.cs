using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RW.Framework.Guids;

namespace RW.Framework.Config;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration configuration)
	{
		services.Configure<SequentialGuidGeneratorOptions>(configuration.GetSection("GuidGenerator"));
		return services;
	}
}