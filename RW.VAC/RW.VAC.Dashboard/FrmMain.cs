using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.WebView2.Core;
using RW.Framework.EventBus;
using RW.Framework.Extensions;
using RW.VAC.Application.Contracts.Opcs;
using RW.VAC.Application.Events.EventData;
using RW.VAC.Dashboard.settings;
using RW.VAC.Infrastructure.Opc;
using System;
using RW.VAC.Dashboard.subscribe;

namespace RW.VAC.Dashboard;

public partial class FrmMain : Form
{
    private readonly IComponentContext _componentContext;
	private readonly IEventBus _eventBus;
	
	private readonly IUaClient _uaClient;
	private readonly TagStorage _tag;
	private readonly DeviceSubscribe _deviceSubscribe;
	private readonly IOpcItemService _opcItemService;
    private readonly IOpcGroupService _opcGroupService;

    public FrmMain(IComponentContext componentContext, DeviceSubscribe deviceSubscribe,
	    IUaClient uaClient, TagStorage tag, IEventBus eventBus, IOpcGroupService opcGroupService,IOpcItemService opcItemService)
	{
		InitializeComponent();
        _componentContext = componentContext;
		_uaClient=uaClient;
		_tag = tag;
		_deviceSubscribe =deviceSubscribe;
		_eventBus = eventBus;
		_opcItemService=opcItemService;
		_opcGroupService=opcGroupService;
        InitWebView();
	}

	public async void InitWebView()
	{
		await WebView.EnsureCoreWebView2Async();
		var path = Path.Combine( Environment.CurrentDirectory , "site" );
		WebView.CoreWebView2.SetVirtualHostNameToFolderMapping( "app.local" , path , CoreWebView2HostResourceAccessKind.DenyCors );
		//WebView.CoreWebView2.AddHostObjectToScript( "capacity" , _componentContext.Resolve<CapacityBridge>() );
		//WebView.CoreWebView2.Navigate("http://localhost:5173/");
		WebView.CoreWebView2.Navigate( "https://app.local/index.html" );
		WebView.NavigationCompleted += WebView_NavigationCompleted;
	}
	//状态点位初始化订阅
	private async Task LoadOpcNode()
	{       
		var groupList = await _opcGroupService.GetListAsync();
		var itemList = await _opcItemService.GetListAsync();
        var statusList = new List<string>();
		foreach (var group in groupList)
		{     
			var statusNode = itemList.Where(t =>
				t.GroupId == group.Id &&
				t.Code is TagTypeConsts.StatusTag
					or TagTypeConsts.LackTag
					or TagTypeConsts.OverLimitTag
					or TagTypeConsts.BeatTag).ToList();

            var prefix = group.Group.NotNullOrWhiteSpace()
				? string.Concat(group.Device, ".", group.Group)
				: group.Device;
			
			statusNode.ForEach(t => {
				var nodeName = string.Concat(prefix, ".", t.Name);
				_tag.AddTag(group.Code, t.Code, nodeName, _uaClient);
				statusList.Add(nodeName);
			});
		}//点位订阅
		_uaClient.Subscribe(statusList,"statusNode", false, _deviceSubscribe.StatusChanged);
    }
	private async void WebView_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
	{
		WindowState = FormWindowState.Maximized;

		_eventBus.Register<DeviceStatusEventData>(data =>
		{
			WriteWeb(data.ToJson());
			return Task.CompletedTask;
		});
		await LoadOpcNode();
	}
	public void WriteWeb(string content)
	{
		BeginInvoke(() => { WebView.ExecuteScriptAsync($"StatusNode.write({content})").Wait(0); });
	}



}