using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FantasticalLog.Core.Contracts;
using FantasticalLog.Core.Models;
using FantasticalLog.Helpers;
using FantasticalLog.Models;

namespace FantasticalLog.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ILogFileService logFileService;

    public ObservableCollection<TreeviewItem> Files { get; private set; } = new();

    [ObservableProperty]
    private bool isLogFileContentVisible;

    [ObservableProperty]
    private bool isAccountContentVisible;

    [ObservableProperty]
    private bool isCalendarContentVisible;

    [ObservableProperty]
    private IItem? currentItem;

    [ObservableProperty]
    private bool isNoFilesLoadedMsgVisible = true;

    [ObservableProperty]
    private bool isNoItemSelectedMsgVisible = false;

    [ObservableProperty]
    private bool isErrorMsgVisible;

    [ObservableProperty]
    private string errorMsg = "";

    public MainViewModel(ILogFileService logFileService)
    {
        this.logFileService = logFileService;
    }

    public async Task onOpenFile(string filePath)
    {
        try
        {
            List<LogFile> logFiles = await logFileService.GetLogFiles(filePath);
            List<TreeviewItem> items = LogFileMapper.MapToTreeviewItems(logFiles);
            foreach (var item in items)
            {
                Files.Add(item);
            }
            IsNoFilesLoadedMsgVisible = false;
            IsNoItemSelectedMsgVisible = true;
        }
        catch (Exception e)
        {
            ErrorMsg = e.Message;
            IsErrorMsgVisible = true;
        }
    }

    public void onItemSelected(TreeviewItem item)
    {
        CurrentItem = item.Reference;
        IsLogFileContentVisible = item.Reference is LogFile;
        IsAccountContentVisible = item.Reference is Account;
        IsCalendarContentVisible = item.Reference is Calendar;
        IsNoItemSelectedMsgVisible = false;
    }

    public void onCloseFiles()
    {
        Files.Clear();
        CurrentItem = null;
        IsLogFileContentVisible = false;
        IsAccountContentVisible = false;
        IsCalendarContentVisible = false;
        IsNoFilesLoadedMsgVisible = true;
        IsNoItemSelectedMsgVisible = false;
    }
}
