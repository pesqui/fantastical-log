using FantasticalLog.Helpers;

namespace FantasticalLog;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/Fantastical.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();
    }
}
