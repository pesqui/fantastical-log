// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using FantasticalLog.Core.Models;
using FantasticalLog.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FantasticalLog.Views;
public sealed partial class CalendarControl : UserControl
{
    public static readonly DependencyProperty CurrentItemProperty =
    DependencyProperty.Register(nameof(CurrentItem), typeof(object), typeof(CalendarControl), new PropertyMetadata(null));

    public CalendarContentViewModel ViewModel { get; }

    public object CurrentItem
    {
        get => GetValue(CurrentItemProperty);
        set
        {
            SetValue(CurrentItemProperty, value);
            ViewModel.Calendar = value as Calendar;
        }
    }

    public CalendarControl()
    {
        ViewModel = App.GetService<CalendarContentViewModel>();
        this.InitializeComponent();
    }
}
