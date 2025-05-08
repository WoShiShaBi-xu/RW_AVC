using TouchSocket.Sockets;

namespace RW.VAC.Infrastructure.Tcp;

public class ClientDisconnectedEventArgs(SocketClient client) : EventArgs
{
    public string IP { get; } = client.IP;

    public SocketClient Client { get; } = client;
}