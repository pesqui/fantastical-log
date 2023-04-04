using System.Text.Json;
using FantasticalLog.Core.Models;
using FantasticalLog.Core.Services;
using FantasticalLog.Core.Tests.MSTest.IntegrationTests.Stubs;

namespace FantasticalLog.Core.Tests.MSTest.IntegrationTests.Services;

[TestClass]
public class LogFileServiceTest
{
    [TestMethod]
    public async Task WhenExportToJsonFileRuns_DataIsExported()
    {
        // Arrange
        var stubFileService = new StubFileService();
        var logFileService = new LogFileService(stubFileService);
        var logFile = StubFactory.CreateLogFile();
        var outputFilePath = Path.Combine(stubFileService.TemporaryTestFolder, "Log1.json");

        // Act
        await logFileService.ExportToJsonFile(outputFilePath, logFile);

        // Assert
        var strJSON = File.ReadAllText(outputFilePath);
        var logFileLoaded = JsonSerializer.Deserialize<LogFile>(strJSON);
        
        Assert.IsNotNull(logFileLoaded);
        Assert.AreEqual(logFileLoaded.Name, logFile.Name);
        Assert.AreEqual(logFileLoaded.ExistSyncErrors, logFile.ExistSyncErrors);
    }

    [TestMethod]
    public async Task WhenGetLogFilesFromZipRunsForZipFile_LogFilesAreLoaded()
    {
        // Arrange
        var stubFileService = new StubFileService();
        var logFileService = new LogFileService(stubFileService);
        var zipFilePath = Path.Combine(stubFileService.ResourcesTestFolder, Constants.ZipFile);

        // Act
        List<LogFile> logFiles = await logFileService.GetLogFiles(zipFilePath);

        // Assert
        Assert.AreEqual(logFiles.Count, 3);
        Assert.AreEqual(logFiles[0].Name, Constants.FileLog1);
        Assert.AreEqual(logFiles[1].Name, Constants.FileLog2);
        Assert.AreEqual(logFiles[2].Name, Constants.FileLog3);
    }

    [TestMethod]
    public async Task WhenGetLogFilesFromZipRunsForLogFile_LogFilesIsLoaded()
    {
        // Arrange
        var stubFileService = new StubFileService();
        var logFileService = new LogFileService(stubFileService);
        var logFilePath = Path.Combine(stubFileService.ResourcesTestFolder, Constants.FileLog1);

        // Act
        List<LogFile> logFiles = await logFileService.GetLogFiles(logFilePath);

        // Assert
        Assert.AreEqual(logFiles.Count, 1);
        Assert.AreEqual(logFiles[0].Name, Constants.FileLog1);
    }

    [TestMethod]
    public async Task WhenGetLogFilesFromZipRunsForInvalidFile_ExceptionIsExpected()
    {
        // Arrange
        var stubFileService = new StubFileService();
        var logFileService = new LogFileService(stubFileService);
        var filePath = Path.Combine(stubFileService.ResourcesTestFolder, Constants.InvalidFile);

        // Act and Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(() => logFileService.GetLogFiles(filePath));
    }

}