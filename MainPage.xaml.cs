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
	public MainPage(string path)
	{
		InitializeComponent();
		this.path = path;
		commentList.ItemsSource = comments;
		EventBus.updateComments += UpdateComments;
		EventBus.quitRoom += SomebodyQuit;
		EventBus.enterRoom += SomebodyEnter;
		EventBus.updateNums += UpdateNums;
		media.Source = "http://mc.jsm.asia:8899/" + path + "/index.m3u8";
		MsgSender.SendEnter(path);
		page.Title = "当前正在观看的直播是：" + path;

    }
	~MainPage()
	{
        EventBus.updateComments -= UpdateComments;
        EventBus.quitRoom -= SomebodyQuit;
        EventBus.enterRoom -= SomebodyEnter;
        MsgSender.SendQuit(this.path);
    }
    protected override bool OnBackButtonPressed()
    {
        EventBus.updateComments -= UpdateComments;
        EventBus.quitRoom -= SomebodyQuit;
        EventBus.enterRoom -= SomebodyEnter;
        MsgSender.SendQuit(this.path);
        return base.OnBackButtonPressed();
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
    private void UpdateComments(List<string> comments)
	{
		commentsList.Clear();
		foreach (var comment in comments)
		{
			commentsList.Add(comment);
		}
	}

	public async void SendMessage_Clicked(object sender,EventArgs args)
	{
		var s=messageEntry.Text;
		MsgSender.SendSay(s);
		messageEntry.Text = "";
	}
}

