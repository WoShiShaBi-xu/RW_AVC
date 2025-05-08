using RW.VAC.Domain.CodeReaders;
using System.Collections.Concurrent;
using RW.VAC.Domain.Products;
using Microsoft.Extensions.DependencyInjection;
using RW.VAC.Application.Contracts.Parameters;
using RW.VAC.Application.Services.Parameters;
using System;
using System.Security.Claims;

namespace RW.VAC.Client;

public class Global( IServiceProvider serviceProvider )
{
	private readonly ConcurrentDictionary<string, object> _locks = new();

	/// <summary>
	///		系统参数
	/// </summary>
	public ConcurrentDictionary<string, string> Parameter { get;} = new();

	/// <summary>
	///     读码器
	/// </summary>
	public ConcurrentDictionary<string, (string process, ProcessType type)> CodeReader { get; } = new();

    public static ClaimsPrincipal CurrentUser;


    private Product? _currentProduct;
    private string? _statisticType;

    public string? StatisticType
    {
        get
        {
            return _statisticType;
        }
        set
        {
            _statisticType = value;
            var parameterService = serviceProvider.GetRequiredService<IParameterService>();
            parameterService.SetParameterAsyncStatisticType( "StatisticType" , _statisticType ?? "0" );
        }
    }
    /// <summary>
    ///		当前产品Id
    /// </summary>
    public Product? CurrentProduct
	{
		get
		{
			var locker = _locks.GetOrAdd(nameof(Product), _ => new object());
			lock (locker)
			{
				return _currentProduct;
			}
		}
		set
		{
			var locker = _locks.GetOrAdd(nameof(Product), _ => new object());
			lock (locker)
			{
                _currentProduct = value;
			}
            var parameterService = serviceProvider.GetRequiredService<IParameterService>();
            parameterService.SetParameterAsync( "CurrentProduct" , _currentProduct?.Recipe?.ToString() ?? "0" );
        }
    }
}