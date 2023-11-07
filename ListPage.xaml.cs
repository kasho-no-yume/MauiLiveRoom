using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace MauiApp1;

public partial class ListPage : ContentPage
{
	private List<LiveInfo> liveInfos = new List<LiveInfo>();
	public List<LiveInfo> Lives 
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
	public ListPage()
	{
        EventBus.updateList += UpdateList;
        InitializeComponent();
        liveList.ItemsSource = Lives;       
        MsgSender.SendAuth("xydltql");
    }
    private void UpdateList(List<LiveInfo> lst)
    {
        liveInfos = lst;
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {     
        var s = (e.Item as LiveInfo).path;
        Debug.WriteLine(s);
        MsgSender.SendEnter(s);
    }
}