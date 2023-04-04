using CommunityToolkit.Mvvm.ComponentModel;
using FantasticalLog.Core.Contracts;
using FantasticalLog.Core.Models;
using Windows.Storage;

namespace FantasticalLog.ViewModels;

public partial class LogFileContentViewModel : ObservableObject
{
    private readonly ILogFileService logFileService;

    private LogFile? logFile;
    public LogFile? LogFile
    {
        get => logFile;
        set
        {
            logFile = value;
            if (logFile == null)
            {
                return;
            }

            Name = logFile.Name;
            AccountCount = logFile.Accounts.Count;
        }
    }

    [ObservableProperty]
    private string name = "";

    [ObservableProperty]
    private int accountCount = 0;

    [ObservableProperty]
    private bool isErrorMsgVisible;

    [ObservableProperty]
    private string errorMsg = "";

    public LogFileContentViewModel(ILogFileService logFileService)
    {
        this.logFileService = logFileService;
    }

    public async Task onExportToJsonFile(string filePath)
    {
        try
        {
            await logFileService.ExportToJsonFile(filePath, LogFile);
        }
        catch (Exception e)
        {
            ErrorMsg = e.Message;
            IsErrorMsgVisible = true;
        }
    }
}
