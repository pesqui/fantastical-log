// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using FantasticalLog.Core.Models;
using FantasticalLog.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FantasticalLog.Views;
public sealed partial class LogFileControl : UserControl
{
    public static readonly DependencyProperty CurrentItemProperty =
        DependencyProperty.Register(nameof(CurrentItem), typeof(object), typeof(LogFileControl), new PropertyMetadata(null));

    public LogFileContentViewModel ViewModel { get; }

    public object CurrentItem
    {
        get => GetValue(CurrentItemProperty);
        set
        {
            SetValue(CurrentItemProperty, value);
            ViewModel.LogFile = value as LogFile;
        }
    }

    public LogFileControl()
    {
        ViewModel = App.GetService<LogFileContentViewModel>();
        this.InitializeComponent();
        errorDialog.RegisterPropertyChangedCallback(UIElement.VisibilityProperty, ErrorDialogVisibilityChanged);
    }

    private async void exportBtn_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var savePicker = new FileSavePicker();
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hWnd);
        savePicker.FileTypeChoices.Add("JSON File", new List<string> { ".json" });
        savePicker.DefaultFileExtension = ".json";

        var file = await savePicker.PickSaveFileAsync();
        if (file != null)
        {
            await ViewModel.onExportToJsonFile(file.Path);
        }
    }

    private async void ErrorDialogVisibilityChanged(DependencyObject sender, DependencyProperty dp)
    {
        if (!ViewModel.IsErrorMsgVisible) { return; }

        await errorDialog.ShowAsync();
        ViewModel.IsErrorMsgVisible = false;
    }
}
