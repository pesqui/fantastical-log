using FantasticalLog.Core.Contracts;
using FantasticalLog.Core.Models;

namespace FantasticalLog.Core.Tests.MSTest.IntegrationTests.Stubs;
public static class StubFactory
{
    public static Account CreateAccount()
    {
        var account = new Account
        {
            Id = "1",
            Name = "Account1",
            Enabled = true,
            SyncInfo = new SyncInfo
            {
                LastSync = DateTime.Now,
                ExistError = true,
                ErrorDesc = "Sync error"
            }
        };

        return account;
    }

    public static Calendar CreateCalendar()
    {
        var account = new Calendar
        {
            Id = "1",
            Name = "Calendar1",
        };

        return account;
    }

    public static LogFile CreateLogFile()
    {
        var logFile = new LogFile { Name = "File1.log" };
        var account = CreateAccount();
        var calendar = CreateCalendar();
        var calendars = new List<Calendar>() { calendar};
        account.Calendars.AddRange(calendars);
        var accounts = new List<Account>() { account };
        logFile.Accounts.AddRange(accounts);

        return logFile;
    }
}
