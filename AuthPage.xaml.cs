
using CommunityToolkit.Maui.Alerts;

namespace MauiApp1;

public partial class AuthPage : ContentPage
{
	public AuthPage()
	{
		InitializeComponent();
		string cacheName = DataSaver.ReadTextFile("cache.txt");
		string cacheUrl = DataSaver.ReadTextFile("url.txt");
		string cacheLiveUrl = DataSaver.ReadTextFile("live.txt");
		UsernameEntry.Text = cacheName;
		URLEntry.Text = cacheUrl;
		if(cacheLiveUrl == null || cacheLiveUrl.Trim().Length == 0)
		{
			cacheLiveUrl = WebSocketMgr.LiveUrl;
		}
		LiveURLEntry.Text = cacheLiveUrl;
	}
	public async void OnLoginClicked(object sender, EventArgs e)
	{
		var s = UsernameEntry.Text;
		if(s == null)
		{
			return;
		}
		if(s.Trim().Length == 0 )
		{
			return;
		}
		DataSaver.WriteTextFile("cache.txt", s);
		if(URLEntry.Text != null && URLEntry.Text.Trim().Length > 0) 
		{
			WebSocketMgr.Url = URLEntry.Text;
			DataSaver.WriteTextFile("url.txt",URLEntry.Text);
		}
		if(LiveURLEntry.Text != null && LiveURLEntry.Text.Trim().Length > 0)
		{
			WebSocketMgr.LiveUrl = LiveURLEntry.Text;
			DataSaver.WriteTextFile("live.txt", LiveURLEntry.Text);
		}
		if (!MsgSender.SendAuth(s))
		{
            if (Microsoft.Maui.Devices.DeviceInfo.Platform == Microsoft.Maui.Devices.DevicePlatform.WinUI)
            {
                await App.Current.MainPage.DisplayAlert("����", "����ʧ�ܡ�", "OK");
			}
			else
			{
                Toast.Make("����ʧ�ܡ�", CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
            }            
			return;
		}
		App.ChangePage(new NavigationPage(new ListPage(s)));
    }
}