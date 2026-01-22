using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyWinFormsApp.Factory;
using MyWinFormsApp.Services;
using MyWinFormsApp.View;
using System.Windows.Forms;

namespace MyWinFormsApp.Tests
{
  public class FormFactoryTests : IDisposable
  {
    private readonly IServiceProvider _serviceProvider;
    private readonly IFormFactory _formFactory;

    public FormFactoryTests()
    {
      var services = new ServiceCollection();
      services.DiBootStrapper();

      _serviceProvider = services.BuildServiceProvider();
      _formFactory = _serviceProvider.GetRequiredService<IFormFactory>();
    }

    [Fact]
    public void FormFactory_Should_Create_CustomerForm()
    {
      // Act
      var form = _formFactory.CreateForm<CustomerForm>();

      // Assert
      Assert.NotNull(form);
      Assert.IsType<CustomerForm>(form);

      // Cleanup
      form.Dispose();
    }

    [Fact]
    public void FormFactory_Should_Create_ProductForm()
    {
      // Act
      var form = _formFactory.CreateForm<ProductForm>();

      // Assert
      Assert.NotNull(form);
      Assert.IsType<ProductForm>(form);

      // Cleanup
      form.Dispose();
    }

    [Fact]
    public void FormFactory_Should_Create_MainForm()
    {
      // Act
      var form = _formFactory.CreateForm<MainForm>();

      // Assert
      Assert.NotNull(form);
      Assert.IsType<MainForm>(form);

      // Cleanup
      form.Dispose();
    }

    [Fact]
    public void FormFactory_Should_Throw_For_Unregistered_Form()
    {
      // Act & Assert
      Assert.Throws<InvalidOperationException>(() =>
          _formFactory.CreateForm<UnregisteredForm>());
    }

    [Fact]
    public void ServiceProvider_Should_Have_Required_Services()
    {
      // Act & Assert - These should not throw exceptions
      var loggingService = _serviceProvider.GetRequiredService<ILoggingService>();
      var configService = _serviceProvider.GetRequiredService<IConfigurationService>();
      var formFactory = _serviceProvider.GetRequiredService<IFormFactory>();

      Assert.NotNull(loggingService);
      Assert.NotNull(configService);
      Assert.NotNull(formFactory);
    }

    public void Dispose()
    {
      (_serviceProvider as IDisposable)?.Dispose();
    }
  }

  // Test form that's not registered - for negative testing
  public class UnregisteredForm : Form
  {
    public UnregisteredForm()
    {
      this.Text = "Unregistered Form";
    }
  }
}