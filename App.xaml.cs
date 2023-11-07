using Microsoft.UI.Xaml.Controls;

namespace MauiApp1;

public partial class App : Application
{
	private static Application ins;
	public App()
	{
		InitializeComponent();
		ins = this;
		MainPage = new ListPage();
	}
	public static void QuitApp()
	{
		ins.Quit();
	}
}
