using FantasticalLog.Core.Services;
using FantasticalLog.Core.Tests.MSTest.IntegrationTests.Stubs;

namespace FantasticalLog.Core.Tests.MSTest.IntegrationTests.Services;

[TestClass]
public class LogParserTest
{
    [TestMethod]
    public void WhenParserLogFile_LogFileIsLoaded()
    {
        // Arrange
        var stubFileService = new StubFileService();
        var logFilePath = Path.Combine(stubFileService.ResourcesTestFolder, Constants.FileLog3);
        var logParser = new LogParser(logFilePath);

        // Act
        logParser.Process();

        // Assert
        Assert.IsNotNull(logParser.LogFile);
        Assert.AreEqual(logParser.LogFile.Name, Constants.FileLog3);
        Assert.AreEqual(logParser.LogFile.Accounts.Count, 15);
        var account = logParser.LogFile.Accounts[3];
        Assert.AreEqual(account.Id, "8f6366e5b6fc682348cd03ddbe799424dccb103f");
        Assert.AreEqual(account.Name, "Google");
        Assert.IsTrue(account.Enabled);
        Assert.AreEqual(account.Calendars.Count, 6);
        Assert.AreEqual(account.SyncInfo.LastSync, DateTime.ParseExact("2023-03-17 13:45:33", "yyyy-MM-dd HH:mm:ss", null));
        Assert.IsTrue(account.SyncInfo.ExistError);
        Assert.IsTrue(account.SyncInfo.ErrorDesc.StartsWith("Error Domain=com.flexibits.fantastical.sync.errormanager Code=500"));
    }

    [TestMethod]
    public void WhenParserInvalidLogFile_LogFileIsLoaded()
    {
        // Arrange
        var stubFileService = new StubFileService();
        var logFilePath = Path.Combine(stubFileService.ResourcesTestFolder, Constants.InvalidFileLog);
        var logParser = new LogParser(logFilePath);

        // Act and Assert
        Assert.ThrowsException<Exception>(() => logParser.Process());
    }
}