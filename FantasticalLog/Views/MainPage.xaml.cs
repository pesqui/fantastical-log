using FantasticalLog.Models;
using FantasticalLog.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.Pickers;

namespace FantasticalLog.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
        errorDialog.RegisterPropertyChangedCallback(UIElement.VisibilityProperty, ErrorDialogVisibilityChanged);
    }

    private void TreeView_ItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
    {
        TreeviewItem item = ((TreeviewItem) args.InvokedItem);
        ViewModel.onItemSelected(item);
    }

    private async void openFileBtn_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var openPicker = new FileOpenPicker();
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);
        openPicker.ViewMode = PickerViewMode.Thumbnail;
        openPicker.FileTypeFilter.Add(".log");
        openPicker.FileTypeFilter.Add(".zip");

        var file = await openPicker.PickSingleFileAsync();
        if (file != null)
        {
            await ViewModel.onOpenFile(file.Path);
        }
    }

    private void closeFilesBtn_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.onCloseFiles();
    }

    private async void ErrorDialogVisibilityChanged(DependencyObject sender, DependencyProperty dp)
    {
        if (!ViewModel.IsErrorMsgVisible) { return; }

        await errorDialog.ShowAsync();
        ViewModel.IsErrorMsgVisible = false;
    }
}
