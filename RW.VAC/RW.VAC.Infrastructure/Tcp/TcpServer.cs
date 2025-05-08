using Microsoft.Extensions.Logging;
using TouchSocket.Core;
using TouchSocket.Sockets;

namespace RW.VAC.Infrastructure.Tcp;

public class TcpServer(ILogger<TcpServer> logger) : TcpClientBase, ITcpServer
{
	public event Action<ClientConnectedEventArgs>? ClientConnected;

	public event Action<ClientDisconnectedEventArgs>? ClientDisconnected;

	public event Action<DataReceivedEventArgs>? DataReceived;

	private TcpService? _service;

	public async Task StartAsync(string port)
	{
		_service = new TcpService();
		_service.Connected += (s, _) =>
		{
			logger.LogInformation($"客户端连接：Id：{s.Id} IPAddress:{s.IP}:{s.Port}");
			ClientConnected?.Invoke(new ClientConnectedEventArgs(s));
			return Task.CompletedTask;
		};
		_service.Disconnected += (s, e) =>
		{
			logger.LogWarning($"客户端断开：Id：{s.Id} IPAddress:{s.IP}:{s.Port} Message:{e.Message}");
			ClientDisconnected?.Invoke(new ClientDisconnectedEventArgs(s));
			return Task.CompletedTask;
		};
		_service.Received += (s, e) =>
		{
			DataReceived?.Invoke(new DataReceivedEventArgs(s, e.ByteBlock));
			return Task.CompletedTask;
		};
		await _service.SetupAsync(new TouchSocketConfig().SetListenIPHosts(port));
		await _service.StartAsync();
	}

	public async Task StopAsync()
	{
		if(_service == null) return;
		await _service.StopAsync();
		_service.SafeDispose();
	}
}