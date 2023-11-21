using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MauiApp1
{
    internal class EventBus
    {
        public delegate void RuntimeError(Exception e);
        public static RuntimeError codeError;
        public delegate void UpdateList(List<LiveInfo> list);
        public static UpdateList updateList;
        public delegate void EnterRoom(string username);
        public static EnterRoom enterRoom;
        public delegate void QuitRoom(string username);
        public static QuitRoom quitRoom;
        public delegate void UpdateNums(int nums);
        public static UpdateNums updateNums;
        public delegate void UpdateComments(List<string> comments);
        public static UpdateComments updateComments;
        public delegate void Disconnect();
        public static Disconnect disconnect;
        public delegate void Reconnect();
        public static Reconnect reconnect;
       
        static EventBus()
        {
            codeError += defaultError;
            updateList += defaultUpdateList;
            enterRoom += defaultEnterRoom;
            quitRoom += defaultQuitRoom;
            updateNums += defaultUpdateNums;
            updateComments += defaultUpdateComments;
            disconnect += defaultDisconnect;
            reconnect += defaultReconnect;
        }
        public static void GlobalEventHandler(string json)
        {
            try
            {
                JObject jobj = JObject.Parse(json);
                string cmd = jobj["msg"].ToString();
                Debug.WriteLine("receive msg:"+cmd);
                switch (cmd)
                {
                    case "updateList":
                        updateList(JsonConvert.DeserializeObject<List<LiveInfo>>(jobj["data"].ToString()));
                        break;
                    case "comments":
                        updateComments(JsonConvert.DeserializeObject<List<string>>(jobj["comments"].ToString()));
                        break;
                    case "enter":
                        enterRoom(jobj["username"].ToString());
                        break;
                    case "quit":
                        quitRoom(jobj["username"].ToString());
                        break;
                    case "updateNums":
                        updateNums(int.Parse(jobj["nums"].ToString()));
                        break;
                }
            }
            catch(Exception e) 
            {
                codeError(e);
            }
        }
        private static void defaultError(Exception e)
        {
            Debug.WriteLine("EventBusError:"+e.Message);
        }
        private static void defaultUpdateList(List<LiveInfo> list)
        {
            Debug.WriteLine("EventBusUpdateList:" + list.Count);
        }
        private static void defaultEnterRoom(string username)
        {
            Debug.WriteLine("EventBusEnter:" + username);
        }
        private static void defaultQuitRoom(string username)
        {
            Debug.WriteLine("EventBusQuit:" + username);
        }
        private static void defaultUpdateNums(int nums)
        {
            Debug.WriteLine("EventBusUpdateNums:" + nums);
        }
        private static void defaultUpdateComments(List<string> comments)
        {
            Debug.WriteLine("EventBusUpdateComments:" + comments);
        }
        private static void defaultDisconnect()
        {
            Debug.WriteLine("EventBusDisconnected!!");
        }
        private static void defaultReconnect()
        {
            Debug.WriteLine("EventBusReconnected!!");
        }

    }
}
