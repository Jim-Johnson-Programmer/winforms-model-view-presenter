using Microsoft.Extensions.DependencyInjection;
using MyWinFormsApp.View;

namespace MyWinFormsApp.Factory
{
  /// <summary>
  /// Factory for creating WinForms with dependency injection
  /// </summary>
  public class FormFactory : IFormFactory
  {
    private readonly IServiceProvider _serviceProvider;

    public FormFactory(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    /// <summary>
    /// Creates a form of the specified type with all dependencies injected
    /// </summary>
    /// <typeparam name="T">The form type to create</typeparam>
    /// <returns>An instance of the requested form type</returns>
    public T CreateForm<T>() where T : Form
    {
      try
      {
        return _serviceProvider.GetRequiredService<T>();
      }
      catch (InvalidOperationException ex)
      {
        throw new InvalidOperationException($"Form type '{typeof(T).Name}' is not registered in the IoC container.", ex);
      }
    }

    /// <summary>
    /// Creates a form of the specified type with all dependencies injected
    /// </summary>
    /// <param name="formType">The form type to create</param>
    /// <returns>An instance of the requested form type</returns>
    public Form CreateForm(Type formType)
    {
      if (!typeof(Form).IsAssignableFrom(formType))
      {
        throw new ArgumentException($"Type '{formType.Name}' must inherit from Form.", nameof(formType));
      }

      try
      {
        return (Form)_serviceProvider.GetRequiredService(formType);
      }
      catch (InvalidOperationException ex)
      {
        throw new InvalidOperationException($"Form type '{formType.Name}' is not registered in the IoC container.", ex);
      }
    }

    /// <summary>
    /// Creates a customer form with dependencies injected
    /// </summary>
    /// <returns>CustomerForm instance</returns>
    public CustomerForm CreateCustomerForm()
    {
      return CreateForm<CustomerForm>();
    }

    /// <summary>
    /// Shows a form as a dialog and handles disposal
    /// </summary>
    /// <typeparam name="T">The form type to show</typeparam>
    /// <returns>DialogResult from the form</returns>
    public DialogResult ShowFormAsDialog<T>() where T : Form
    {
      using var form = CreateForm<T>();
      return form.ShowDialog();
    }

    /// <summary>
    /// Shows a form modeless and returns the instance
    /// </summary>
    /// <typeparam name="T">The form type to show</typeparam>
    /// <returns>The form instance</returns>
    public T ShowForm<T>() where T : Form
    {
      var form = CreateForm<T>();
      form.Show();
      return form;
    }
  }
}