using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MauiApp1;

public partial class MainPage : ContentPage
{
	private string path;
	private ObservableCollection<string> commentsList = new ObservableCollection<string>();
	public ObservableCollection<string> comments 
	{ 
		get => commentsList; 
		set 
		{
			if (commentsList != value)
			{
				commentsList = value;
                OnPropertyChanged();
            }			
		} 
	}
	public MainPage(LiveInfo info)
	{
		InitializeComponent();
		this.path = info.path;
		//commentList.ItemsSource = comments;
		EventBus.updateComments += UpdateComments;
		EventBus.quitRoom += SomebodyQuit;
		EventBus.enterRoom += SomebodyEnter;
		EventBus.updateNums += UpdateNums;
		EventBus.disconnect += Disconnect;
		media.Source = WebSocketMgr.LiveUrl +"/"+ path + "/index.m3u8";
		//web.Source = "http://mc.jsm.asia:8899/" + path;
        MsgSender.SendEnter(path);
		page.Title = ("当前正在观看的直播是：" + (info.title == null ? path:info.title));

    }
	~MainPage()
	{
		media.Stop();
        EventBus.updateComments -= UpdateComments;
        EventBus.quitRoom -= SomebodyQuit;
        EventBus.enterRoom -= SomebodyEnter;
		EventBus.updateNums -= UpdateNums;
		EventBus.disconnect -= Disconnect;
        MsgSender.SendQuit(this.path);
    }
    protected override bool OnBackButtonPressed()
    {
		media.Stop();
        EventBus.updateComments -= UpdateComments;
        EventBus.quitRoom -= SomebodyQuit;
        EventBus.enterRoom -= SomebodyEnter;
		EventBus.updateNums -= UpdateNums;
		EventBus.disconnect -= Disconnect;
        MsgSender.SendQuit(this.path);
		UserData.currentWatch = null;
        return base.OnBackButtonPressed();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        DeviceDisplay.Current.KeepScreenOn = true;
    }

    private void UpdateNums(int nums)
	{
		watching.Text = "当前观看人数：" + nums;
	}
    private void SomebodyEnter(string user)
	{
		enterquit.Text = user + "进入了直播间";
	}
    private void SomebodyQuit(string user)
    {
        enterquit.Text = user + "退出了直播间";
    }
    private async void UpdateComments(List<string> comments)
	{
		var size = 0;
        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            size = 18;
        }
        else
        {
            size = 12;
        }
        lst.Children.Clear();
		foreach (string comment in comments)
		{
			var label = new Label();
			label.Text = comment;
			label.Padding = 3;
			label.FontSize = size;
			lst.Children.Add(label);
		}
		await Task.Delay(100);
		await scroller.ScrollToAsync(0, lst.Height, true);
    }

	public void SendMessage_Clicked(object sender,EventArgs args)
	{
		var s=messageEntry.Text;
		if(s==null)
		{
			s = "";
			return;
		}
        if (s.Trim().Length==0)
        {
            messageEntry.Text = "";
            return;
        }     
		messageEntry.Text = "";
		if(s=="command disconnect")
		{
			WebSocketMgr.getIns().Abort();
			return;
		}
        MsgSender.SendSay(s);
    }
	private void Disconnect()
	{
        try
        {
            var pop = new ReconnectingPage();
            this.ShowPopup(pop);
        }
        catch (Exception ex)
        {
            EventBus.codeError(ex);
        }
    }

    private void RefreshSource(object sender, EventArgs e)
    {
        media.Source = "http://mc.jsm.asia:8899/" + this.path + "/index.m3u8";
		media.Play();
    }
}

