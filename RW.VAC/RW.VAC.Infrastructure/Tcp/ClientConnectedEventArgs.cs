using TouchSocket.Sockets;

namespace RW.VAC.Infrastructure.Tcp;

public class ClientConnectedEventArgs(SocketClient client) : EventArgs
{
    public string IP { get; } = client.IP;

    public SocketClient Client { get; } = client;
}