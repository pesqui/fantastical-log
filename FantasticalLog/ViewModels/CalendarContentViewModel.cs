using CommunityToolkit.Mvvm.ComponentModel;
using FantasticalLog.Core.Contracts;
using FantasticalLog.Core.Models;

namespace FantasticalLog.ViewModels;

public partial class CalendarContentViewModel : ObservableObject
{
    private readonly ILogFileService logFileService;

    private Calendar? calendar;
    public Calendar? Calendar
    {
        get => calendar;
        set
        {
            calendar = value;
            if (calendar == null)
            {
                return;
            }

            Id = calendar.Id;
            Name = calendar.Name;
        }
    }

    [ObservableProperty]
    private string id = "";

    [ObservableProperty]
    private string name = "";

    public CalendarContentViewModel(ILogFileService logFileService)
    {
        this.logFileService = logFileService;
    }
}
