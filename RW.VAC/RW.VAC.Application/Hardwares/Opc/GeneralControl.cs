using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RW.VAC.Infrastructure.Opc;


namespace RW.VAC.Application.Hardwares.Opc;

public class GeneralControl
{
	private readonly ConcurrentDictionary<string, List<string>> _cacheTag = new();
	private readonly IUaClient _client;
	private readonly TagStorage _tags;
    private readonly ILogger<GeneralControl> _logger;
	private readonly IServiceProvider _serviceProvider;
    public GeneralControl(IUaClient client, TagStorage tags, ILogger<GeneralControl> logger , IServiceProvider serviceProvider )
	{
		_client = client;
		_tags = tags;
        _logger = logger;
		_serviceProvider = serviceProvider;
        Init();
	}
	/// <summary>
	/// 写入配方号
	/// </summary>
	public void Writeformula( UInt16? recipe )
	{
		if ( recipe == null )
		{
			return;
		}
        var tags = _cacheTag.GetValueOrDefault( TagTypeConsts.WriteRecipeTag );
        BatchWrite( tags , recipe );
    }

    /// <summary>
    ///	心跳信号
    /// </summary>
    public void Heartbeat()
	{
		var tags = _cacheTag.GetValueOrDefault(TagTypeConsts.HeartbeatTag);
		BatchWrite(tags, true);
	}

	/// <summary>
	/// 启动
	/// </summary>
	/// <param name="e"></param>
	public void Start(TagChangedEventArgs e)
	{
		var tags = _cacheTag.GetValueOrDefault(TagTypeConsts.StartTag);
		BatchWrite(tags, e.Value);
	}

	/// <summary>
	///     切换模式（远程\本地）
	/// </summary>
	/// <param name="e"></param>
	public void RemoteMode(TagChangedEventArgs e)
	{
		//var tags = _cacheTag.GetValueOrDefault(TagTypeConsts.RemoteModeTag);
		//BatchWrite(tags, e.Value);

	}

	/// <summary>
	///     暂停
	/// </summary>
	/// <param name="e"></param>
	public void Pause(TagChangedEventArgs e)
	{
		//if (e.Value!=null)
		//{
  //          var tags = _cacheTag.GetValueOrDefault(TagTypeConsts.PauseTag);
  //          BatchWrite(tags, e.Value);
  //          _logger.LogError("暂停触发");
  //      }
		
	}

	/// <summary>
	///     复位
	/// </summary>
	/// <param name="e"></param>
	public void Reset(TagChangedEventArgs e)
	{
		var tags = _cacheTag.GetValueOrDefault(TagTypeConsts.ResetTag);
		BatchWrite(tags, e.Value);
	}

	/// <summary>
	///     急停
	/// </summary>
	/// <param name="e"></param>
	public void EmergencyStop(TagChangedEventArgs e)
	{
		//if (e.Value!=null)
		//{
  //          var tags = _cacheTag.GetValueOrDefault(TagTypeConsts.EmergencyStopTag);
  //          BatchWrite(tags, e.Value);
  //          _logger.LogError("急停触发");
  //      }
            
        
           
    }
	
    /// <summary>
    ///     批量写入
    /// </summary>
    /// <param name="tags"></param>
    /// <param name="value"></param>
    private void BatchWrite(IEnumerable<string>? tags, object value)
    {
		if (tags == null) return;
       
       
        if (_client.Write( tags , value ))
		{
			

        }
	}

	private void Init()
	{
		var usedList = _tags.TagList.Where(t => !string.Equals("Control", t.GroupCode)).ToList();
		_cacheTag.TryAdd(TagTypeConsts.HeartbeatTag,
			usedList.Where(t => t.ItemCode == TagTypeConsts.HeartbeatTag).Select(t => t.TagName).ToList());
		_cacheTag.TryAdd(TagTypeConsts.RemoteModeTag, 
			usedList.Where(t => t.ItemCode == TagTypeConsts.RemoteModeTag).Select(t => t.TagName).ToList());
		_cacheTag.TryAdd(TagTypeConsts.StartTag,
			usedList.Where(t => t.ItemCode == TagTypeConsts.StartTag).Select(t => t.TagName).ToList());
		_cacheTag.TryAdd(TagTypeConsts.PauseTag,
			usedList.Where(t => t.ItemCode == TagTypeConsts.PauseTag).Select(t => t.TagName).ToList());
		_cacheTag.TryAdd(TagTypeConsts.ResetTag,
			usedList.Where(t => t.ItemCode == TagTypeConsts.ResetTag).Select(t => t.TagName).ToList());
		_cacheTag.TryAdd(TagTypeConsts.EmergencyStopTag,
			usedList.Where(t => t.ItemCode == TagTypeConsts.EmergencyStopTag).Select(t => t.TagName).ToList());
        _cacheTag.TryAdd( TagTypeConsts.WriteRecipeTag ,
            usedList.Where( t => t.ItemCode == TagTypeConsts.WriteRecipeTag ).Select( t => t.TagName ).ToList() );

    }
}