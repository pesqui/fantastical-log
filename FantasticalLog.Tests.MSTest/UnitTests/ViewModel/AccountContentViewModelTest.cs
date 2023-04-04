using FantasticalLog.Core.Contracts;
using FantasticalLog.ViewModels;
using Moq;

namespace FantasticalLog.Tests.MSTest.UnitTests.ViewModel;

[TestClass]
public class AccountContentViewModelTest
{
    [TestMethod]
    public void WhenViewModelIsCreated_PropertiesAreInitialized()
    {
        // Arrange
        var mockLogFileService = new Mock<ILogFileService>();

        // When
        var viewModel = new AccountContentViewModel(mockLogFileService.Object);

        // Then
        Assert.IsNull(viewModel.Account);
        Assert.AreEqual(viewModel.Id, string.Empty);
        Assert.AreEqual(viewModel.Name, string.Empty);
        Assert.IsFalse(viewModel.Enabled);
        Assert.AreEqual(viewModel.CalendarCount, 0);
        Assert.AreEqual(viewModel.LastSyncDate, "(Not Synced)");
        Assert.IsFalse(viewModel.ExistError);
        Assert.AreEqual(viewModel.ErrorDesc, string.Empty);
    }

    [TestMethod]
    public void WhenAccountIsSet_PropertiesAreSet()
    {
        // Arrange
        var mockLogFileService = new Mock<ILogFileService>();
        var viewModel = new AccountContentViewModel(mockLogFileService.Object);

        // Act
        var account = StubFactory.CreateAccount();
        viewModel.Account = account;

        // Assert
        Assert.IsNotNull(viewModel.Account);
        Assert.AreEqual(viewModel.Id, account.Id);
        Assert.AreEqual(viewModel.Name, account.Name);
        Assert.AreEqual(viewModel.Enabled, account.Enabled);
        Assert.AreEqual(viewModel.CalendarCount, account.Calendars.Count);
        Assert.AreEqual(viewModel.LastSyncDate, account.SyncInfo.LastSync.ToString());
        Assert.AreEqual(viewModel.ExistError, account.SyncInfo.ExistError);
        Assert.AreEqual(viewModel.ErrorDesc, account.SyncInfo.ErrorDesc);
    }
}
