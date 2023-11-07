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
        private static int TimeOut = 2;
        public static WebSocketState connState { get { return _webSocket.State; } }

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
                ReceiveLoop();
                ListenConnectionStatus();
                Debug.WriteLine("连接成功！");
                return true;
            }
            else
            {
                Debug.WriteLine("连接失败。");
                return false;
            }
        }

        public static async Task<bool> SendAsync(string message)
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
                    Debug.WriteLine("发送异步操作完成");
                    return true;
                }
                else
                {
                    // 超时处理逻辑
                    Debug.WriteLine("发送异步操作超时");
                    // 执行超时后的操作
                    return false;
                }

                
            }
            catch (Exception ex)
            {
                // Handle exception
                Debug.WriteLine(ex.Message);
                EventBus.networkError(ex);
                return false;
            }
        }

        private static async Task ListenConnectionStatus()
        {
            Debug.WriteLine("心跳检测开始");
            while (_webSocket.State == WebSocketState.Open)
            {
                Debug.WriteLine("心跳成功，连接正常。");
                // 每5秒检查一次连接状态
                await Task.Delay(TimeSpan.FromSeconds(5));
               
            }
            Debug.WriteLine("服务器断开");
            App.QuitApp();
        }

            private static async Task<string> ReceiveAsync()
        {           
            try
            {
                var buffer = new ArraySegment<byte>(new byte[40960]);
                var responseTask = _webSocket.ReceiveAsync(buffer, CancellationToken.None);
                Task timeoutTask = Task.Delay(TimeSpan.FromSeconds(TimeOut));

                Task completedTask = await Task.WhenAny(responseTask, timeoutTask);
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
                    Debug.WriteLine("接收异步操作超时");
                    return "接收超时";
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                EventBus.networkError(ex);
                return ex.Message;
            }
        }

        private static async Task ReceiveLoop()
        {
            Debug.WriteLine("循环接收消息开启。");
            var buffer = new byte[40960];
            while (_webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Debug.WriteLine("Received message: " + message);
                    EventBus.GlobalEventHandler(message);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", _cancellationTokenSource.Token);
                }
            }
            Debug.WriteLine("循环接收消息结束。");
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
                EventBus.networkError(ex);
                // Handle exception
            }
        }
    }
}
