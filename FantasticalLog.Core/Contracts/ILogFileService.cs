using FantasticalLog.Core.Models;

namespace FantasticalLog.Core.Contracts;
public interface ILogFileService
{
    Task<List<LogFile>> GetLogFiles(string filePath);
    Task ExportToJsonFile(string filePath, LogFile logFile);
}
