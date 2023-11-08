

namespace MauiApp1;

public partial class App : Application
{
	private static Application ins;
	public static void ChangePage(Page page)
	{
		ins.MainPage = page;
	}
	public App()
	{
		InitializeComponent();
		ins = this;
		MainPage = new AuthPage();
	}
	public static void QuitApp()
	{
		ins.Quit();
	}
}
