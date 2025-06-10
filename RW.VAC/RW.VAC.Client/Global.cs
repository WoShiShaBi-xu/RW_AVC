using RW.VAC.Domain.CodeReaders;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;

namespace RW.VAC.Client;

public class Global( IServiceProvider serviceProvider )
{

    /// <summary>
    ///		系统参数
    /// </summary>
    public ConcurrentDictionary<string , string> Parameter { get; } = new();

    public ConcurrentDictionary<string , (string process, ProcessType type)> CodeReader { get; } = new();
}