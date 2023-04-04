using FantasticalLog.Core.Contracts;

namespace FantasticalLog.Models;

public class TreeviewItem
{
    public string Name { get; set; } = "";
    public string Glyph { get; set; } = "";
    public List<TreeviewItem> Children { get; set; } = new();
    public bool ExistError { get; set; }
    public IItem? Reference { get; set; }
}
