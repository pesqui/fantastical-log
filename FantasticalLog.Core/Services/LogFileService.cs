using System.IO.Compression;
using System.Text.Json;
using FantasticalLog.Core.Contracts;
using FantasticalLog.Core.Models;

namespace FantasticalLog.Core.Services;

public class LogFileService : ILogFileService
{
    private readonly IFileService fileService;

    public LogFileService(IFileService fileService)
    {
        this.fileService = fileService;
    }

    public async Task<List<LogFile>> GetLogFiles(string filePath)
    {
        List<LogFile> logFiles;

        var extension = Path.GetExtension(filePath).ToLower();
        if (extension.Equals(".log"))
        {
            LogFile logFile = GetLogFile(filePath);
            logFiles = new List<LogFile> { logFile };
        }
        else if (extension.Equals(".zip"))
        {
            logFiles = await GetLogFilesFromZip(filePath);
        }
        else
        {
            throw new ArgumentException("Invalid file type.");
        }

        return logFiles;
    }
    private LogFile GetLogFile(string filePath)
    {
        LogFile logFile = LoadLogFile(filePath);

        return logFile;
    }

    private async Task<List<LogFile>> GetLogFilesFromZip(string zipFilePath)
    {
        await ExtractZipFile(zipFilePath);

        List<LogFile> logFiles = new List<LogFile>();
        var filePaths = Directory.GetFiles(fileService.TemporaryFolderPath);
        foreach (var filePath in filePaths)
        {
            LogFile logFile = LoadLogFile(filePath);
            logFiles.Add(logFile);
        }

        return logFiles;
    }

    private LogFile LoadLogFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(filePath);
        }

        using var logParser = new LogParser(filePath);
        logParser.Process();
        LogFile logFile = logParser.LogFile;

        return logFile;
    }

    private async Task ExtractZipFile(string zipFilePath)
    {
        await fileService.CreateTemporaryFolder();
        ZipFile.ExtractToDirectory(zipFilePath, fileService.TemporaryFolderPath);
    }

    public async Task ExportToJsonFile(string filePath, LogFile logFile)
    {
        await Task.Run(() =>
        {
            var jsonString = JsonSerializer.Serialize(logFile, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonString);
        });
    }
}
