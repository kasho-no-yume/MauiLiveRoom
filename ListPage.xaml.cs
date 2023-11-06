using Newtonsoft.Json;
using System.Diagnostics;

namespace MauiApp1;

public partial class ListPage : ContentPage
{
	public List<LiveInfo> lives = new List<LiveInfo>();
	public ListPage()
	{
		InitializeComponent();
		if (WebSocketMgr.Connect())
		{
			var res = WebSocketMgr.SendAsync("auth xydltql").Result;
			LiveInfo[] lists;
            if (res == null)
			{
				
				return;
			}
			else if(res == "接收超时" || res == "")
			{
				lists = new LiveInfo[0];
			}
			else
			{
                lists = Newtonsoft.Json.JsonConvert.DeserializeObject<LiveInfo[]>(res);
            }          
			lives = lists.ToList();
        }
		else
		{
			Debug.WriteLine("不能连接");
        }
	}

    private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
		
    }
}