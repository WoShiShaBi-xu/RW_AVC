using Microsoft.AspNetCore.Http;
using System.Net.WebSockets;
using System.Text;

namespace RW.VAC.Dashboard.API.WebSocketManagers
{
    public class WebSocketManagers
    {
        private readonly List<WebSocket> _sockets = new();
        private readonly object _lock = new();

        public void AddSocket( WebSocket socket )
        {
            lock ( _lock )
            {
                _sockets.Add( socket );
            }
        }

        public async Task BroadcastMessageAsync( string message )
        {
            var buffer = Encoding.UTF8.GetBytes( message );
            var closedSockets = new List<WebSocket>();

            // 使用快照避免在遍历时修改原列表
            WebSocket [ ] snapshot;
            lock ( _sockets )
            {
                snapshot = _sockets.ToArray();
            }

            foreach ( var socket in snapshot )
            {
                // 检查连接状态是否仍然是Open
                if ( socket.State == WebSocketState.Open )
                {
                    try
                    {
                        await socket.SendAsync(
                            new ArraySegment<byte>( buffer ) ,
                            WebSocketMessageType.Text ,
                            true ,
                            CancellationToken.None );
                    }
                    catch ( WebSocketException )
                    {
                        // 发送失败，表明对端可能已断开，标记移除
                        closedSockets.Add( socket );
                    }
                    catch ( ObjectDisposedException )
                    {
                        // 对象已释放，同样标记移除
                        closedSockets.Add( socket );
                    }
                }
                else
                {
                    // 状态不是Open，说明已关闭或不再可用
                    closedSockets.Add( socket );
                }
            }

            // 移除所有已关闭的socket
            if ( closedSockets.Count > 0 )
            {
                lock ( _sockets )
                {
                    foreach ( var s in closedSockets )
                    {
                        _sockets.Remove( s );
                    }
                }
            }
        }


        // Optional: A method to gracefully close all sockets.
        public async Task CloseAllAsync( WebSocketCloseStatus status = WebSocketCloseStatus.NormalClosure , string description = "Closing" )
        {
            WebSocket [ ] snapshot;
            lock ( _lock )
            {
                snapshot = _sockets.ToArray();
            }

            foreach ( var socket in snapshot )
            {
                if ( socket.State == WebSocketState.Open || socket.State == WebSocketState.CloseReceived )
                {
                    try
                    {
                        await socket.CloseAsync( status , description , CancellationToken.None );
                    }
                    catch
                    {
                        // Ignored - socket may already be closed or faulted
                    }
                }
            }

            lock ( _lock )
            {
                _sockets.Clear();
            }
        }
    }
}
