// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using FantasticalLog.Core.Models;
using FantasticalLog.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FantasticalLog.Views;
public sealed partial class AccountControl : UserControl
{
    public static readonly DependencyProperty CurrentItemProperty =
        DependencyProperty.Register(nameof(CurrentItem), typeof(object), typeof(AccountControl), new PropertyMetadata(null));

    public AccountContentViewModel ViewModel { get; }

    public object CurrentItem
    {
        get => GetValue(CurrentItemProperty);
        set
        {
            SetValue(CurrentItemProperty, value);
            ViewModel.Account = value as Account;
        }
    }

    public AccountControl()
    {
        ViewModel = App.GetService<AccountContentViewModel>();
        this.InitializeComponent();
    }
}
