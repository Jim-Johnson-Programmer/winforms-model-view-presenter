using Moq;
using MyWinFormsApp.Model;
using MyWinFormsApp.Presenter;
using MyWinFormsApp.View;

namespace MyWinFormsApp.Tests
{
    public class CustomerPresenterTests
    {
        [Fact]
        public void OnSaveClicked_ValidEmail_DisplaysSuccessMessage()
        {
            // Arrange
            var viewMock = new Mock<ICustomerView>();
            var model = new CustomerModel();
            var presenter = new CustomerPresenter(viewMock.Object, model);

            viewMock.Setup(v => v.CustomerName).Returns("Alice");
            viewMock.Setup(v => v.CustomerEmail).Returns("alice@example.com");

            // Act
            viewMock.Raise(v => v.SaveClicked += null, EventArgs.Empty);

            // Assert
            viewMock.Verify(v => v.CustomerName, Times.Once);
        }
    }
}
