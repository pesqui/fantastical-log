using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;

namespace FantasticalLog.Helpers;

public static class Utils
{
    public static void MaximizeWindow(Window window)
    {
        IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
        WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
        AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
        if (appWindow.Presenter is OverlappedPresenter p)
        {
            p.Maximize();
        }
    }
}
