using MyWinFormsApp.Model;

namespace MyWinFormsApp.Tests
{
    public class CustomerModelTests
    {
        [Fact]
        public void IsValidEmail_ReturnsTrue_WhenEmailIsValid()
        {
            var model = new CustomerModel { Email = "test@example.com" };
            Assert.True(model.IsValidEmail());
        }
    }
}
