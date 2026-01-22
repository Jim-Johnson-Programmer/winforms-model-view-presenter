using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyWinFormsApp.Factory;
using MyWinFormsApp.Model;
using MyWinFormsApp.Presenter;
using MyWinFormsApp.Services;
using MyWinFormsApp.View;

namespace MyWinFormsApp
{
    public static class ExtensionClass
    {
        public static IHostBuilder UseDiBootStrapper(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices((context, services) =>
            {
                services.DiBootStrapper();
            });
        }

        public static void DiBootStrapper(this IServiceCollection services)
        {
            // Register application services (Singleton for shared state/resources)
            services.AddSingleton<ILoggingService, LoggingService>();
            services.AddSingleton<IConfigurationService, ConfigurationService>();

            // Register Form Factory (Singleton to ensure proper resource management)
            services.AddSingleton<IFormFactory, FormFactory>();

            // Register MVP components (Transient for new instances)
            services.AddTransient<ICustomerPresenter, CustomerPresenter>();
            services.AddTransient<ICustomerView, CustomerForm>();
            services.AddTransient<CustomerModel>();

            // Register example services (kept for compatibility)
            services.AddTransient<IMyClass, MyClass>();
            services.AddTransient<MySimpleClass>();

            // Register all forms that will be created by the factory
            // Most forms should be Transient to get fresh instances each time
            services.AddTransient<MainForm>();
            services.AddTransient<CustomerForm>();
            services.AddTransient<ProductForm>();

            // Service lifetime explanations:
            // - Singleton: Single instance for the entire application lifetime (services, factories)
            // - Scoped: One instance per scope/request (not commonly used in WinForms)
            // - Transient: New instance every time it's requested (forms, presenters, models)
        }
    }

    public class MySimpleClass
    {
        public string Name { get; set; } = "John";
    }

    public interface IMyClass
    {
        int Id { get; set; }

        string MyMethod();
    }

    public class MyClass : IMyClass
    {
        public int Id { get; set; } = 1;

        public string MyMethod()
        {
            return "Hello World";
        }
    }
}