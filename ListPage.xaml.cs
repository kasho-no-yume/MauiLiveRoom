using System.Collections.ObjectModel;
using System.Diagnostics;

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
        MsgSender.SendAuth(auth);
        page.Title = auth+"大佬好，看看今天有什么直播吧";
    }
    ~ListPage()
    {
        EventBus.updateList -= UpdateList;
    }
    private void UpdateList(List<LiveInfo> lst)
    {
        liveInfos.Clear();
        foreach (LiveInfo info in lst)
        {
            liveInfos.Add(info);
        }
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {     
        var s = (e.Item as LiveInfo).path;
        Debug.WriteLine(s);
        //App.ChangePage(new MainPage(s));
        await Navigation.PushAsync(new MainPage(s));
    }
    private void Refresh_list(object sender, EventArgs e) 
    {
        MsgSender.SendRefresh();
    }
}