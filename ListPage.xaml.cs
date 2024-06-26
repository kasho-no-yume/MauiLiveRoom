using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;
using static MauiApp1.EventBus;

namespace MauiApp1;

public partial class ListPage : ContentPage
{
	private ObservableCollection<LiveInfo> liveInfos = new ObservableCollection<LiveInfo>();
	public ObservableCollection<LiveInfo> Lives 
	{ 
		get => liveInfos; 
		set 
		{
            if (liveInfos != value)
            {
                liveInfos = value;
                OnPropertyChanged();
            }
        } 
	}
	public ListPage(string auth)
	{
        InitializeComponent();
        liveList.ItemsSource = Lives;
        EventBus.updateList += UpdateList;
        EventBus.disconnect += Disconnect;
        MsgSender.SendRefresh();
        UserData.username = auth;
        page.Title = auth+"大佬好，看看今天有什么直播吧";
    }
    ~ListPage()
    {
        EventBus.updateList -= UpdateList;
        EventBus.disconnect -= Disconnect;
    }
    private void UpdateList(List<LiveInfo> lst)
    {
        liveInfos.Clear();
        foreach (LiveInfo info in lst)
        {
            if(info.title.Length == 0)
            {
                info.title = null;
            }
            if(info.desc.Length == 0)
            {
                info.desc = null;
            }
            liveInfos.Add(info);
        }
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {     
        var s = (e.Item as LiveInfo);
        Debug.WriteLine(s.path);
        UserData.currentWatch = s.path;
        //App.ChangePage(new MainPage(s));
        await Navigation.PushAsync(new MainPage(s));
    }
    private void Refresh_list(object sender, EventArgs e) 
    {
        MsgSender.SendRefresh();
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
}