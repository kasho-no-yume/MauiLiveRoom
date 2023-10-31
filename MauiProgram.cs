using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace MauiApp1;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseMauiApp<App>().UseMauiCommunityToolkitMediaElement();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
