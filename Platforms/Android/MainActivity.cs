using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using System.Runtime.CompilerServices;

namespace MauiApp1;

[Activity(Theme = "@style/Maui.SplashTheme", ScreenOrientation = ScreenOrientation.Landscape, MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public static MainActivity Instance { get; private set; }
    public MainActivity()
    {
        Instance = this;
    }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        Platform.Init(this, savedInstanceState);

        this.Window.AddFlags(WindowManagerFlags.Fullscreen);
    }
}
