using FantasticalLog.Core.Contracts;

namespace FantasticalLog.Core.Models;
public class LogFile: IItem
{
    public string Name { get; set; }
    public List<Account> Accounts { get; } = new();
    public bool ExistSyncErrors { get; set; }
}
