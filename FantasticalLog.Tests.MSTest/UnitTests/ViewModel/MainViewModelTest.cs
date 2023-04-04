using System.Collections.Generic;
using System.Security.Principal;
using FantasticalLog.Core.Contracts;
using FantasticalLog.Core.Models;
using FantasticalLog.Models;
using FantasticalLog.ViewModels;
using Moq;

namespace FantasticalLog.Tests.MSTest.UnitTests.ViewModel;

[TestClass]
public class MainViewModelTest
{
    [TestMethod]
    public void WhenViewModelIsCreated_PropertiesAreInitialized()
    {
        // Arrange
        var mockLogFileService = new Mock<ILogFileService>();

        // When
        var viewModel = new MainViewModel(mockLogFileService.Object);

        // Then
        Assert.IsNull(viewModel.CurrentItem);
        Assert.IsFalse(viewModel.IsLogFileContentVisible);
        Assert.IsFalse(viewModel.IsAccountContentVisible);
        Assert.IsFalse(viewModel.IsCalendarContentVisible);
        Assert.IsFalse(viewModel.IsErrorMsgVisible);
        Assert.AreEqual(viewModel.ErrorMsg, string.Empty);
        Assert.IsTrue(viewModel.IsNoFilesLoadedMsgVisible);
        Assert.IsFalse(viewModel.IsNoItemSelectedMsgVisible);
    }

    [TestMethod]
    public async Task WhenOpenFile_DataIsLoaded()
    {
        // Arrange
        var logFile = StubFactory.CreateLogFile();
        var logFiles = new List<LogFile> { logFile };
        var mockLogFileService = new Mock<ILogFileService>();
        mockLogFileService
            .Setup(o => o.GetLogFiles(It.IsAny<string>()))
            .ReturnsAsync(logFiles);
        var viewModel = new MainViewModel(mockLogFileService.Object);
        var filePath = logFile.Name;

        // Act
        await viewModel.onOpenFile(filePath);

        // Assert
        mockLogFileService.Verify(o => o.GetLogFiles(filePath), Times.Once());
        Assert.IsTrue(viewModel.Files.Count > 0);
        Assert.AreEqual(viewModel.Files[0].Name, filePath);
        Assert.IsFalse(viewModel.IsNoFilesLoadedMsgVisible);
        Assert.IsTrue(viewModel.IsNoItemSelectedMsgVisible);
        Assert.IsFalse(viewModel.IsErrorMsgVisible);
        Assert.AreEqual(viewModel.ErrorMsg, string.Empty);
    }

    [TestMethod]
    public async Task WhenOpenFile_OperationFails()
    {
        // Arrange
        var mockLogFileService = new Mock<ILogFileService>();
        var errorMessage = "File not found.";
        mockLogFileService
            .Setup(o => o.GetLogFiles(It.IsAny<string>()))
            .ThrowsAsync(new FileNotFoundException(errorMessage));
        var viewModel = new MainViewModel(mockLogFileService.Object);
        var filePath = "file1.log";

        // Act
        await viewModel.onOpenFile(filePath);

        // Assert
        Assert.IsTrue(viewModel.IsErrorMsgVisible);
        Assert.AreEqual(viewModel.ErrorMsg, errorMessage);
        Assert.IsTrue(viewModel.IsNoFilesLoadedMsgVisible);
        Assert.IsFalse(viewModel.IsNoItemSelectedMsgVisible);
    }

    [TestMethod]
    public void WhenLogFileIsSelected_LogFileContentPanelIsDisplayed()
    {
        // Arrange
        var mockLogFileService = new Mock<ILogFileService>();
        var viewModel = new MainViewModel(mockLogFileService.Object);
        var logFile = StubFactory.CreateLogFile();
        var treeviewItem = new TreeviewItem
        {
            Name = logFile.Name,
            Reference = logFile
        };

        // Act
        viewModel.onItemSelected(treeviewItem);

        // Assert
        Assert.AreSame(viewModel.CurrentItem, logFile);
        Assert.IsTrue(viewModel.IsLogFileContentVisible);
        Assert.IsFalse(viewModel.IsAccountContentVisible);
        Assert.IsFalse(viewModel.IsCalendarContentVisible);
        Assert.IsFalse(viewModel.IsNoItemSelectedMsgVisible);
    }

    [TestMethod]
    public void WhenAccountIsSelected_AccountContentPanelIsDisplayed()
    {
        // Arrange
        var mockLogFileService = new Mock<ILogFileService>();
        var viewModel = new MainViewModel(mockLogFileService.Object);
        var account = StubFactory.CreateAccount();
        var treeviewItem = new TreeviewItem
        {
            Name = account.Name,
            Reference = account
        };

        // Act
        viewModel.onItemSelected(treeviewItem);

        // Assert
        Assert.AreSame(viewModel.CurrentItem, account);
        Assert.IsFalse(viewModel.IsLogFileContentVisible);
        Assert.IsTrue(viewModel.IsAccountContentVisible);
        Assert.IsFalse(viewModel.IsCalendarContentVisible);
        Assert.IsFalse(viewModel.IsNoItemSelectedMsgVisible);
    }

    [TestMethod]
    public void WhenCalendarIsSelected_CalendarContentPanelIsDisplayed()
    {
        // Arrange
        var mockLogFileService = new Mock<ILogFileService>();
        var viewModel = new MainViewModel(mockLogFileService.Object);
        var calendar = StubFactory.CreateCalendar();
        var treeviewItem = new TreeviewItem
        {
            Name = calendar.Name,
            Reference = calendar
        };

        // Act
        viewModel.onItemSelected(treeviewItem);

        // Assert
        Assert.AreSame(viewModel.CurrentItem, calendar);
        Assert.IsFalse(viewModel.IsLogFileContentVisible);
        Assert.IsFalse(viewModel.IsAccountContentVisible);
        Assert.IsTrue(viewModel.IsCalendarContentVisible);
        Assert.IsFalse(viewModel.IsNoItemSelectedMsgVisible);
    }

    [TestMethod]
    public void WhenCloseFiles_LogFilesAreUnloaded()
    {
        // Arrange
        var mockLogFileService = new Mock<ILogFileService>();
        var viewModel = new MainViewModel(mockLogFileService.Object);
        var logFile = StubFactory.CreateLogFile();
        var treeviewItem = new TreeviewItem
        {
            Name = logFile.Name,
            Reference = logFile
        };
        viewModel.onItemSelected(treeviewItem);

        // Act
        viewModel.onCloseFiles();

        // Assert
        Assert.AreEqual(viewModel.Files.Count, 0);
        Assert.IsNull(viewModel.CurrentItem);
        Assert.IsFalse(viewModel.IsLogFileContentVisible);
        Assert.IsFalse(viewModel.IsAccountContentVisible);
        Assert.IsFalse(viewModel.IsCalendarContentVisible);
        Assert.IsTrue(viewModel.IsNoFilesLoadedMsgVisible);
        Assert.IsFalse(viewModel.IsNoItemSelectedMsgVisible);
    }
}
