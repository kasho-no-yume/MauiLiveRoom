﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    static class MsgSender
    {
        static MsgSender()
        {

        }
        public static bool Send(string content)
        {
            if (WebSocketMgr.Connect())
            {
                var res = WebSocketMgr.SendAsync(content).Result;
                if (res == false)
                {
                    App.ChangePage(new AuthPage());
                    EventBus.disconnect();
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        public static bool SendAuth(string username)
        {
            return Send("auth " + username);
        }
        public static void SendRefresh()
        {
            Send("refresh");
        }
        public static void SendEnter(string path)
        {
            Send("enter " + path);
        }
        public static void SendQuit(string path)
        {
            Send("quit " + path);
        }
        public static void SendSay(string msg)
        {
            Send("say:" + msg);
        }
    }
}
