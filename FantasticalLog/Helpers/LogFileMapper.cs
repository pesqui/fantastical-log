using FantasticalLog.Core.Models;
using FantasticalLog.Models;

namespace FantasticalLog.Helpers;

public static class LogFileMapper
{
    public static List<TreeviewItem> MapToTreeviewItems(List<LogFile> logFiles)
    {
        var items = logFiles.Select(logFile =>
        {
            TreeviewItem item = new TreeviewItem { Name = logFile.Name };
            item.Children = logFile.Accounts.Select(account =>
            {
                TreeviewItem item = new TreeviewItem { Name = account.Name };
                item.Children = account.Calendars.Select(calendar =>
                {
                    TreeviewItem item = new TreeviewItem { Name = calendar.Name };
                    item.Glyph = "\xE787";
                    item.Reference = calendar;

                    return item;
                }).ToList();
                item.Glyph = "\xE77B";
                item.Reference = account;
                item.ExistError = account.SyncInfo != null ? account.SyncInfo.ExistError : false;

                return item;
            }).ToList();
            item.Glyph = "\xF000";
            item.Reference = logFile;
            item.ExistError = logFile.ExistSyncErrors;

            return item;
        }).ToList();

        return items;
    }
}
