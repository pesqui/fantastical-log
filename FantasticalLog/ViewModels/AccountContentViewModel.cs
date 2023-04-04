using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using FantasticalLog.Core.Contracts;
using FantasticalLog.Core.Models;

namespace FantasticalLog.ViewModels;

public partial class AccountContentViewModel : ObservableObject
{
    private readonly ILogFileService logFileService;

    private Account? account;
    public Account? Account
    {
        get => account;
        set
        {
            account = value;
            if (account == null)
            {
                return;
            }

            Id = account.Id;
            Name = account.Name;
            Enabled = account.Enabled;
            CalendarCount = account.Calendars.Count;
            if (account.SyncInfo != null)
            {
                LastSyncDate = account.SyncInfo.LastSync.ToString();
                ExistError = account.SyncInfo.ExistError;
                ErrorDesc = account.SyncInfo.ErrorDesc;
            }
            else
            {
                LastSyncDate = "(Not Synced)";
                ExistError = false;
                ErrorDesc = "";
            }
        }
    }

    [ObservableProperty]
    private string id = "";

    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private bool enabled = false;

    [ObservableProperty]
    private int calendarCount = 0;

    [ObservableProperty]
    private string lastSyncDate = "(Not Synced)";

    [ObservableProperty]
    private bool existError = false;

    [ObservableProperty]
    private string errorDesc = "";

    public AccountContentViewModel(ILogFileService logFileService)
    {
        this.logFileService = logFileService;
    }
}
