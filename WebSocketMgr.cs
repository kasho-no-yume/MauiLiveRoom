using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MauiApp1
{
    static class WebSocketMgr
    {
        private static ClientWebSocket _webSocket;
        private static CancellationTokenSource _cancellationTokenSource;
        private static int TimeOut = 1;

        static WebSocketMgr()
        {
        }

        public static bool Connect()
        {
            _webSocket = new ClientWebSocket();
            _cancellationTokenSource = new CancellationTokenSource();
            var responseTask = _webSocket.ConnectAsync(new Uri("ws://mc.jsm.asia:8889"), CancellationToken.None);
            Task timeoutTask = Task.Delay(TimeSpan.FromSeconds(TimeOut));

            Task completedTask = Task.WhenAny(responseTask, timeoutTask).Result;

            if (completedTask == responseTask)
            {
                WebSocketMgr.ReceiveLoop();
                Debug.WriteLine("连接成功！");
                return true;
            }
            else
            {
                Debug.WriteLine("连接失败。");
                return false;
            }
        }

        public static async Task<string> SendAsync(string message)
        {
            Debug.WriteLine("发送信息："+message);
            try
            {
                if (_webSocket.State != WebSocketState.Open)
                {
                    Connect();
                }

                var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                var responseTask = _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                Task timeoutTask = Task.Delay(TimeSpan.FromSeconds(TimeOut));

                Task completedTask = await Task.WhenAny(responseTask, timeoutTask);

                if (completedTask == responseTask)
                {
                    // 异步操作完成
                    Debug.WriteLine("异步操作完成");
                    return await ReceiveAsync();
                }
                else
                {
                    // 超时处理逻辑
                    Debug.WriteLine("异步操作超时");
                    // 执行超时后的操作
                    return "发送超时";
                }

                
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
                var buffer = new ArraySegment<byte>(new byte[40960]);
                var responseTask = _webSocket.ReceiveAsync(buffer, CancellationToken.None);
                Task timeoutTask = Task.Delay(TimeSpan.FromSeconds(TimeOut));

                Task completedTask = Task.WhenAny(responseTask, timeoutTask).Result;
                if (completedTask == responseTask) 
                {
                    var result = await responseTask;
                    var messageBytes = buffer.Array[..result.Count];
                    var message = Encoding.UTF8.GetString(messageBytes);
                    Debug.WriteLine("收到信息：" +message);
                    return message;
                }
                else
                {
                    // 超时处理逻辑
                    Debug.WriteLine("异步操作超时");
                    return "接收超时";
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return ex.Message;
            }
        }

        private static async Task ReceiveLoop()
        {
            var buffer = new byte[40960];
            while (_webSocket.State == WebSocketState.Open)
            {
                var receiveResult = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);
                if (receiveResult.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                    // 处理收到的消息，可以通过事件或回调函数通知其他部分
                    Debug.WriteLine("监听收到信息：" + message);
                }
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
