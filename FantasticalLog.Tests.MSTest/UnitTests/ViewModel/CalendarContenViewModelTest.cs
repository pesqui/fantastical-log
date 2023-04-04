using FantasticalLog.Core.Contracts;
using FantasticalLog.ViewModels;
using Moq;

namespace FantasticalLog.Tests.MSTest.UnitTests.ViewModel;

[TestClass]
public class CalendarContenViewModelTest
{
    [TestMethod]
    public void WhenViewModelIsCreated_PropertiesAreInitialized()
    {
        // Arrange
        var mockLogFileService = new Mock<ILogFileService>();

        // When
        var viewModel = new CalendarContentViewModel(mockLogFileService.Object);

        // Then
        Assert.IsNull(viewModel.Calendar);
        Assert.AreEqual(viewModel.Id, string.Empty);
        Assert.AreEqual(viewModel.Name, string.Empty);
    }

    [TestMethod]
    public void WhenLogFileIsSet_PropertiesAreSet()
    {
        // Arrange
        var mockLogFileService = new Mock<ILogFileService>();
        var viewModel = new CalendarContentViewModel(mockLogFileService.Object);

        // Act
        var calendar = StubFactory.CreateCalendar();
        viewModel.Calendar = calendar;

        // Assert
        Assert.IsNotNull(viewModel.Calendar);
        Assert.AreEqual(viewModel.Id, calendar.Id);
        Assert.AreEqual(viewModel.Name, calendar.Name);
    }
}
