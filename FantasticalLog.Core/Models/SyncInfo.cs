using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasticalLog.Core.Models;
public class SyncInfo
{
    public DateTime LastSync { get; set; }
    public bool ExistError { get; set; }
    public string ErrorDesc { get; set; } = "";
}
