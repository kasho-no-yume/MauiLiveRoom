using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    static class WebSocketMgr
    {
        private static ClientWebSocket _webSocket;

        static WebSocketMgr()
        {
            ConnectWebSocket().Wait();
        }

        private static async Task ConnectWebSocket()
        {
            _webSocket = new ClientWebSocket();
            await _webSocket.ConnectAsync(new Uri("ws://mc.jsm.asia:8899?id=bstql"), CancellationToken.None);
        }

        public static async Task<string> SendAsync(string message)
        {
            try
            {
                if (_webSocket.State != WebSocketState.Open)
                {
                    await ConnectWebSocket();
                }

                var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

                return await ReceiveAsync();
            }
            catch (Exception ex)
            {
                // Handle exception
                return ex.Message;
            }
        }

        private static async Task<string> ReceiveAsync()
        {
            try
            {
                var buffer = new ArraySegment<byte>(new byte[4096]);
                var result = await _webSocket.ReceiveAsync(buffer, CancellationToken.None);
                var messageBytes = buffer.Array[..result.Count];
                var message = Encoding.UTF8.GetString(messageBytes);

                return message;
            }
            catch (Exception ex)
            {
                // Handle exception
                return ex.Message;
            }
        }

        public static async Task DisconnectWebSocket()
        {
            try
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing connection", CancellationToken.None);
                _webSocket.Dispose();
            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }
    }
}
