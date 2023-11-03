namespace MauiApp1;

public partial class ListPage : ContentPage
{
	public ListPage()
	{
		InitializeComponent();
		string res = WebSocketMgr.SendAsync("").Result;
	}
}