using FantasticalLog.Core.Contracts;
using FantasticalLog.Core.Models;
using FantasticalLog.ViewModels;
using Moq;

namespace FantasticalLog.Tests.MSTest.UnitTests.ViewModel;

[TestClass]
public class LogFileContenViewModelTest
{
    [TestMethod]
    public void WhenViewModelIsCreated_PropertiesAreInitialized()
    {
        // Arrange
        var mockLogFileService = new Mock<ILogFileService>();

        // When
        var viewModel = new LogFileContentViewModel(mockLogFileService.Object);

        // Then
        Assert.IsNull(viewModel.LogFile);
        Assert.AreEqual(viewModel.Name, string.Empty);
        Assert.AreEqual(viewModel.AccountCount, 0);
        Assert.IsFalse(viewModel.IsErrorMsgVisible);
        Assert.AreEqual(viewModel.ErrorMsg, string.Empty);
    }

    [TestMethod]
    public void WhenAccountIsSet_PropertiesAreSet()
    {
        // Arrange
        var mockLogFileService = new Mock<ILogFileService>();
        var viewModel = new LogFileContentViewModel(mockLogFileService.Object);
        var logFile = StubFactory.CreateLogFile();

        // When
        viewModel.LogFile = logFile;

        // Then
        Assert.IsNotNull(viewModel.LogFile);
        Assert.AreEqual(viewModel.Name, logFile.Name);
        Assert.AreEqual(viewModel.AccountCount, logFile.Accounts.Count);
    }

    [TestMethod]
    public async Task WhenExportToJsonFileRuns_DataIsExported()
    {
        // Arrange
        var mockLogFileService = new Mock<ILogFileService>();
        var viewModel = new LogFileContentViewModel(mockLogFileService.Object);
        var logFile = StubFactory.CreateLogFile();
        viewModel.LogFile = logFile;
        var outputFile = "Log1.json";

        // Act
        await viewModel.onExportToJsonFile(outputFile);

        // Assert
        mockLogFileService.Verify(o => o.ExportToJsonFile(outputFile, logFile), Times.Once());
        Assert.IsFalse(viewModel.IsErrorMsgVisible);
    }

    [TestMethod]
    public async Task WhenExportToJsonFile_OperationFails()
    {
        // Arrange
        var mockLogFileService = new Mock<ILogFileService>();
        var errorMessage = "Access denied.";
        mockLogFileService
            .Setup(o => o.ExportToJsonFile(It.IsAny<string>(), It.IsAny<LogFile>()))
            .Throws(new UnauthorizedAccessException(errorMessage));
        var viewModel = new LogFileContentViewModel(mockLogFileService.Object);
        var logFile = StubFactory.CreateLogFile();
        viewModel.LogFile = logFile;
        var outputFile = "Log1.json";

        // Act
        await viewModel.onExportToJsonFile(outputFile);

        // Assert
        Assert.IsTrue(viewModel.IsErrorMsgVisible);
        Assert.AreEqual(viewModel.ErrorMsg, errorMessage);
    }
}
