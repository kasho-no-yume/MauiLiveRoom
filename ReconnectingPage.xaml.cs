using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;

namespace MauiApp1;

public partial class ReconnectingPage : Popup
{
	private bool needClose = false;
    private bool opened = false;
	public ReconnectingPage()
	{
		InitializeComponent();
		EventBus.reconnect += ReconnectSuccess;
		this.Closed += CancelDelegate;
		this.Opened += PopupOpened;
	}

    private void PopupOpened(object sender, PopupOpenedEventArgs e)
    {
        opened = true;
        if(needClose)
        {
            ReconnectCallBack();
        }
    }

    private void CancelDelegate(object sender, PopupClosedEventArgs e)
    {
        EventBus.reconnect -= ReconnectSuccess;
    }

    private void ReconnectSuccess()
	{        
		needClose = true;
        if(opened)
        {
            ReconnectCallBack();
        }
	}
	private async void ReconnectCallBack()
	{
        await CloseAsync();
        await Toast.Make("重连成功！", ToastDuration.Short, 14).Show();
    }
}