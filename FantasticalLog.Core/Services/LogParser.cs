using System.Text.RegularExpressions;
using FantasticalLog.Core.Models;

namespace FantasticalLog.Core.Services;
public class LogParser: IDisposable
{
    private StreamReader reader;
    private string currentLine;
    private readonly string filePath;
    private Dictionary<string, Account> accountDic;

    public LogFile LogFile { get; private set; }

    public LogParser(string filePath)
    {
        this.filePath = filePath;
    }

    public void Dispose()
    {
        if (reader != null)
        {
            reader.Dispose();
        }
    }

    public void Process()
    {
        var fileName = Path.GetFileName(filePath);
        LogFile = new LogFile() { Name = fileName };
        reader = new StreamReader(filePath);
        try
        {
            ProcessAccounts();
            ProcessCalendars();
            ProcessSyncQueues();
            ProcessErrors();
        }
        finally {
            reader.Dispose();
        }
    }

    private void ProcessAccounts()
    {
        SearchFor("Calendar store state");
        ReadLine();
        AssertLineContains("Accounts:");
        var lines = ReadLinesWhileContains(@"^[^\t]*\t");

        List<Account> accounts = new List<Account>();
        accountDic = new Dictionary<string, Account>();
        foreach (var line in lines)
        {
            var tokens = line.Split(", ");
            var account = new Account { 
                Id = tokens[1], 
                Name = tokens[0], 
                Enabled = tokens[2].Equals("enabled")
            };
            accounts.Add(account);
            LogFile.Accounts.Add(account);
            accountDic.Add(account.Id, account);
        }
    }

    private void ProcessCalendars()
    {
        AssertLineContains("Calendars:");
        var lines = ReadLinesWhileContains(@"^[^\t]*\t");

        foreach (var line in lines)
        {
            var tokens = line.Split(", ");
            var calendar = new Calendar { Id = tokens[2], Name = tokens[0] };
            var accountId = tokens[1];
            var account = FindAccount(accountId);
            account.Calendars.Add(calendar);
        }
    }

    private void ProcessSyncQueues()
    {
        var syncQueueTag = "Sync queues:";
        SearchFor(syncQueueTag);
        var firstLine = Regex.Replace(currentLine, $@"^[\s\S]*{syncQueueTag} ", "");
        var lines = ReadLinesUntilContains("Verbose sources:");
        lines.Insert(0, firstLine);

        foreach (var line in lines)
        {
            var accountId = GetRegexGroup(line, @"^\S*\s/\s(\S+)", 1);
            if (accountId == null)
            {
                continue;
            }

            var account = FindAccount(accountId);
            var strLastSync = GetRegexGroup(line, @"last sync: (\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2})", 1);
            if (strLastSync == null)
            {
                throw new Exception("Invalid log structure: [Sync Queues Section] Last sync date not found.");
            }

            DateTime lastSync = DateTime.ParseExact(strLastSync, "yyyy-MM-dd HH:mm:ss", null);
            account.SyncInfo = new SyncInfo { LastSync = lastSync };
        }
    }

    private void ProcessErrors()
    {
        SearchFor("Unresolved errors:");
        ReadLine();
        var lines = ReadLinesWhileContains(@"^\s+");

        foreach (var line in lines)
        {
            var accountId = GetRegexGroup(line, @"FBFantasticalSyncErrorSourceIdentifierKey=([^,]+),", 1);
            if (accountId == null)
            {
                throw new Exception($"Invalid log structure: [Errors Section] Account Id not defined.");
            }

            var account = FindAccount(accountId);
            account.SyncInfo.ExistError = true;
            account.SyncInfo.ErrorDesc = Regex.Unescape(line.Substring(1, line.Length - 2));
            LogFile.ExistSyncErrors = true;
        }
    }

    private string GetRegexGroup(string text, string pattern, int groupIndex)
    {
        var match = Regex.Match(text, pattern);
        if (!match.Success) { return null; }

        var value = match.Groups[groupIndex].Value;

        return value;
    }

    private Account FindAccount(string Id)
    {
        Account account;
        if (!accountDic.TryGetValue(Id, out account))
        {
            throw new Exception($"Invalid log structure: Account Id:'{Id}' not found.");
        }

        return account;
    }

    private void SearchFor(string text)
    {
        while ((currentLine = reader.ReadLine()) != null)
        {
            if (currentLine.IndexOf(text) >= 0)
            {
                return;
            }
        }

        throw new Exception($"Invalid log structure: '{text}' not found.");
    }

    private List<string> ReadLinesWhileContains(string pattern)
    {
        List<string> lines = new List<string>();
        while ((currentLine = reader.ReadLine()) != null)
        {
            if (!Regex.IsMatch(currentLine, pattern))
            {
                break;
            }
            var formatedLine = Regex.Replace(currentLine, pattern, "");
            lines.Add(formatedLine);
        }

        return lines;
    }


    private List<string> ReadLinesUntilContains(string pattern)
    {
        List<string> lines = new List<string>();
        while ((currentLine = reader.ReadLine()) != null)
        {
            if (Regex.IsMatch(currentLine, pattern))
            {
                break;
            }
            lines.Add(currentLine);
        }

        return lines;
    }

    private void ReadLine()
    {
        currentLine = reader.ReadLine();
        if (currentLine == null)
        {
            throw new Exception("Invalid log structure: No more lines to read.");
        }
    }

    private void AssertLineContains(string text)
    {
        if (!currentLine.Contains(text))
        {
            throw new Exception($"Invalid log structure: '{text}' not found.");
        }
    }
}
