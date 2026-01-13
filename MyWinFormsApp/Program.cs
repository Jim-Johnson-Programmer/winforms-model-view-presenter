using Microsoft.Extensions.DependencyInjection;
using MyWinFormsApp.Model;
using MyWinFormsApp.Presenter;
using MyWinFormsApp.View;

namespace MyWinFormsApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var services = new ServiceCollection();
            ConfigureServices(services);

            var serviceProvider = services.BuildServiceProvider();
            
            // Get the CustomerForm (which implements ICustomerView)
            CustomerForm customerForm = (CustomerForm)serviceProvider.GetRequiredService<ICustomerView>();
            
            // IMPORTANT: Create the presenter to wire up the MVP pattern
            var presenter = serviceProvider.GetRequiredService<ICustomerPresenter>();
            
            Application.Run(customerForm);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Register the concrete form
            services.AddScoped<CustomerForm>();
            
            // Register ICustomerView to resolve to CustomerForm
            services.AddScoped<ICustomerView,CustomerForm>();
            
            // Register model and presenter
            services.AddTransient<CustomerModel>();
            services.AddTransient<ICustomerPresenter,CustomerPresenter>();
        }
    }
}