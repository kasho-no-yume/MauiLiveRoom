namespace MauiApp1;

public partial class AuthPage : ContentPage
{
	public AuthPage()
	{
		InitializeComponent();
	}
	public void OnLoginClicked(object sender, EventArgs e)
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
		App.ChangePage(new NavigationPage(new ListPage(s)));
    }
}