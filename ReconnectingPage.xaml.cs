using CommunityToolkit.Maui.Views;

namespace MauiApp1;

public partial class ReconnectingPage : Popup
{
	public ReconnectingPage()
	{
		InitializeComponent();
		EventBus.reconnect += ReconnectSuccess;
	}
	private void ReconnectSuccess()
	{
		EventBus.reconnect -= ReconnectSuccess;
		this.CloseAsync().Wait();
	}
}