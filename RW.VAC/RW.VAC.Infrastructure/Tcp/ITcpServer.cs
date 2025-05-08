namespace RW.VAC.Infrastructure.Tcp;

public interface ITcpServer
{
	event Action<ClientConnectedEventArgs>? ClientConnected;

	event Action<ClientDisconnectedEventArgs>? ClientDisconnected;

	event Action<DataReceivedEventArgs>? DataReceived;

	Task StartAsync(string port);

	Task StopAsync();
}