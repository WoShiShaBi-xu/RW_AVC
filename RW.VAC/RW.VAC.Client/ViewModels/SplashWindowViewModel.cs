using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ControlzEx.Standard;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RW.Framework.Extensions;
using RW.VAC.Application.Contracts.CodeReaders;
using RW.VAC.Application.Contracts.Opcs;
using RW.VAC.Application.Contracts.Parameters;
using RW.VAC.Application.Hardwares.CodeReader;
using RW.VAC.Application.Hardwares.Opc;
using RW.VAC.Client.Storage;
using RW.VAC.Infrastructure.Devices.State;
using RW.VAC.Infrastructure.Opc;
using RW.VAC.Infrastructure.Tcp;
using System.Linq;
using AppClient = System.Windows.Application;

namespace RW.VAC.Client.ViewModels;

public partial class SplashWindowViewModel(IServiceProvider serviceProvider, Global global, ILogger<SplashWindowViewModel> logger) : ObservableObject
{
	[ObservableProperty] private double _progress;

	[ObservableProperty] private string _title = "蝶阀自动装配线";

	[ObservableProperty] private string _tooltip = string.Empty;

	[RelayCommand]
	private async Task OnLoaded()
	{
		await Task.Run(async () =>
		{
			try
			{
				Tooltip = "加载参数";
				await LoadParameter();
				Progress = 10;

				Tooltip = "加载日志缓冲区";
				LoadLogBuffer();
				Progress = 20;

				Tooltip = "加载配置";
				await LoadCodeReader();
				await LoadOpcTag();
				Progress = 30;

				Tooltip = "初始化OPC";
				await InitOpc();
				Progress = 60;

				Tooltip = "初始化TCP服务";
				await InitTcpServer();
				Progress = 95;

				Tooltip = "启动主窗体";
			}
			catch(Exception e)
			{
				Tooltip = "初始化失败，3秒后退出程序";
				await Task.Delay(3000);
				logger.LogError(e, e.Message);
				AppClient.Current.Dispatcher.BeginInvoke( ( ) =>
				{
					AppClient.Current.Shutdown();
				} );
			}
		});
		var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
		mainWindow.Width = 0;
		mainWindow.Height = 0;
		mainWindow.ShowInTaskbar = false;
		AppClient.Current.MainWindow = mainWindow;
		AppClient.Current.MainWindow.Show();
	}

	private async Task LoadParameter()
	{
        var parameterService = serviceProvider.GetRequiredService<IParameterService>();
        var parameterList = await parameterService.GetListAsync();
        await parameterService.SetParameterAsync( "CurrentProduct" , "0" );
        await parameterService.SetParameterAsyncStatisticType( "StatisticType" , "0" );
        foreach ( var param in parameterList ) global.Parameter.TryAdd( param.Code , param.Value );
    }

	private void LoadLogBuffer()
	{
		var logBuffer = serviceProvider.GetRequiredService<LogBuffer>();
		int? capacity = null;
		global.Parameter.TryGetValue("LogMaxLength", out var len);
		if (len.NotNullOrWhiteSpace() && int.TryParse(len, out var result)) capacity = result;
		logBuffer.Initialize(capacity);
	}

	private async Task LoadCodeReader()
	{
		var codeReaderService = serviceProvider.GetRequiredService<ICodeReaderService>();
		var codeReaderList = await codeReaderService.GetListAsync();
		foreach (var codeReader in codeReaderList)
			global.CodeReader.TryAdd(codeReader.IP, (codeReader.ProcessName, codeReader.ProcessType));
	}

	private async Task LoadOpcTag()
	{
		var tag = serviceProvider.GetRequiredService<TagStorage>();
		var uaClient = serviceProvider.GetRequiredService<IUaClient>();
		var plcState = serviceProvider.GetRequiredService<PLCState>();

		var opcGroupService = serviceProvider.GetRequiredService<IOpcGroupService>();
		var opcItemService = serviceProvider.GetRequiredService<IOpcItemService>();
		var groupList = await opcGroupService.GetListAsync();
		var itemList = await opcItemService.GetListAsync();
		foreach (var group in groupList)
		{
			var prefix = group.Group.NotNullOrWhiteSpace()
				? string.Concat(group.Device, ".", group.Group)
				: group.Device;
			var items = itemList.Where(t => t.GroupId == group.Id).ToList();
			foreach (var i in items)
			{
				var nodeName = string.Concat(prefix, ".", i.Name);
				tag.AddTag(group.Code, i.Code, nodeName, uaClient);
			}

			plcState[string.Concat(group.Device, "._System._NoError")] = false;
		}
		tag.Initialize();
	}

	private async Task InitOpc()
	{
		var uaClient = serviceProvider.GetRequiredService<IUaClient>();
		await uaClient.Connect(global.Parameter["OpcUrl"]);

		var tags = serviceProvider.GetRequiredService<TagStorage>();
        var plcState = serviceProvider.GetRequiredService<PLCState>();
        var generalControl = serviceProvider.GetRequiredService<GeneralControl>();
        var trussControl = serviceProvider.GetRequiredService<TrussControl>();
		var bedstand = serviceProvider.GetRequiredService<Bedstand>();


        #region 总控相关订阅事件

        uaClient.Subscribe(tags["Control", TagTypeConsts.RemoteModeTag]!.TagName, TagTypeConsts.RemoteModeTag, true,
            generalControl.RemoteMode);
        uaClient.Subscribe(tags["Control", TagTypeConsts.StartTag]!.TagName, TagTypeConsts.StartTag, true,
            generalControl.Start);
        uaClient.Subscribe(tags["Control", TagTypeConsts.PauseTag]!.TagName, TagTypeConsts.PauseTag, true,
            generalControl.Pause);
        uaClient.Subscribe(tags["Control", TagTypeConsts.ResetTag]!.TagName, TagTypeConsts.ResetTag, true,
            generalControl.Reset);
        uaClient.Subscribe(tags["Control", TagTypeConsts.EmergencyStopTag]!.TagName, TagTypeConsts.EmergencyStopTag, true,
            generalControl.EmergencyStop);

        #endregion

        #region 上料桁架相关

        uaClient.Subscribe(tags["M01", "Feed"]!.TagName, "TrussFeed", true, trussControl.Feed);
        uaClient.Subscribe(tags["M01", "Lower"]!.TagName, "TrussLower", true, trussControl.Lower);

        #endregion

        #region 试验台数据导出
        //uaClient.Subscribe(tags["M09", "Endofprocess"]!.TagName, "TrussEndofprocess", true, bedstand.AirtightEndofprocess);//工序结束
        uaClient.Subscribe(tags["M09", "Blanking"]!.TagName, "TrussBlanking", true, bedstand.AirtightBlanking);//获取下料工位号
        //uaClient.Subscribe(tags["M12", "Endofprocess"]!.TagName, "12TrussEndofprocess", true, bedstand.OiltightEndofprocess);//工序结束
        uaClient.Subscribe(tags["M12", "Blanking"]!.TagName, "12TrussBlanking", true, bedstand.OiltightBlanking);//获取下料工位号
        #endregion

        uaClient.Subscribe(plcState.Tags, "PLCState", false, args =>
		{
			plcState[args.MonitoredItem.StartNodeId.ToString()] = (bool)args.Value;
		});
	}

	private async Task InitTcpServer()
	{
		var tcpServer = serviceProvider.GetRequiredService<ITcpServer>();
		var adaptor = serviceProvider.GetRequiredService<CodeReaderAdaptor>();
		var codeReaderState = serviceProvider.GetRequiredService<CodeReaderState>();

		tcpServer.ClientConnected += args => codeReaderState[args.IP] = true;
		tcpServer.ClientDisconnected += args => codeReaderState[args.IP] = false;
		tcpServer.DataReceived += async args =>
        { // 将接收到的字节块转换为字符串，即序列号
            var serialNumber = args.ByteBlock.ToString();
			logger.LogDebug($"读码器：{args.IP} 读码内容：{serialNumber}");
            // 检查序列号是否为空或是否与"NoRead读码失败"参数相同，若是则不处理
            if (serialNumber.IsNullOrWhiteSpace() || string.Equals(global.Parameter["NoRead"], serialNumber,
					StringComparison.OrdinalIgnoreCase)) return;
			if ( serialNumber.Contains( global.Parameter [ "NoRead" ] ) )
				return;

            // 调用 CodeReaderAdaptor 的 Do 方法，传入 IP 地址、序列号和当前产品
            await adaptor.Do(args.IP, serialNumber, global.CurrentProduct);
		};
		await tcpServer.StartAsync(global.Parameter["TcpServerPort"]);
	}
}