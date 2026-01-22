using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyWinFormsApp.Factory;
using MyWinFormsApp.Model;
using MyWinFormsApp.View;
using System.Security.Policy;

namespace MyWinFormsApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // ConfigureServices() must be called before building the host
            var host = CreateHostBuilder().UseDiBootStrapper().Build();

            var services = host.Services;

            // Get the form factory and create the main form
            var formFactory = services.GetRequiredService<IFormFactory>();
            var mainForm = services.GetRequiredService<MainForm>();

            // Alternative ways to start your application:

            // Option 1: Start with MainForm (recommended for multi-form apps)
            Application.Run(mainForm);

            // Option 2: Start with CustomerForm directly using factory
            // var customerForm = formFactory.CreateCustomerForm();
            // Application.Run(customerForm);

            // Option 3: Start with any form using generic factory method
            // var customerForm = formFactory.CreateForm<CustomerForm>();
            // Application.Run(customerForm);
        }



        static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder();
        //.ConfigureServices((context, services) =>
        //{
        //    ConfigureServices(services);
        //});
    }
}