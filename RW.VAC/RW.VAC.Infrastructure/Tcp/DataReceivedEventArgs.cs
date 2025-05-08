using TouchSocket.Core;
using TouchSocket.Sockets;

namespace RW.VAC.Infrastructure.Tcp;

public class DataReceivedEventArgs(SocketClient client, ByteBlock byteBlock) : EventArgs
{
    public string IP { get; } = client.IP;

    public SocketClient Client { get; } = client;

    public ByteBlock ByteBlock { get; } = byteBlock;
}