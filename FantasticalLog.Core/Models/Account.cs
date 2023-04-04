using FantasticalLog.Core.Contracts;

namespace FantasticalLog.Core.Models;
public class Account: IItem
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; }
    public List<Calendar> Calendars { get; } = new();
    public SyncInfo SyncInfo { get; set; }
}
