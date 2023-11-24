namespace MauiApp1;

public partial class AuthPage : ContentPage
{
	public AuthPage()
	{
		InitializeComponent();
		string cacheName = DataSaver.ReadTextFile("cache.txt");
		UsernameEntry.Text = cacheName;
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
		App.ChangePage(new NavigationPage(new ListPage(s)));
    }
}