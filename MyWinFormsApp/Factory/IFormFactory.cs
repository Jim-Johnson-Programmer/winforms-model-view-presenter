using MyWinFormsApp.View;

namespace MyWinFormsApp.Factory
{
  /// <summary>
  /// Factory interface for creating forms with dependency injection
  /// </summary>
  public interface IFormFactory
  {
    /// <summary>
    /// Creates a form of the specified type with all dependencies injected
    /// </summary>
    /// <typeparam name="T">The form type to create</typeparam>
    /// <returns>An instance of the requested form type</returns>
    T CreateForm<T>() where T : Form;

    /// <summary>
    /// Creates a form of the specified type with all dependencies injected
    /// </summary>
    /// <param name="formType">The form type to create</param>
    /// <returns>An instance of the requested form type</returns>
    Form CreateForm(Type formType);

    /// <summary>
    /// Creates a customer form with dependencies injected
    /// </summary>
    /// <returns>CustomerForm instance</returns>
    CustomerForm CreateCustomerForm();

    /// <summary>
    /// Shows a form as a dialog and handles disposal
    /// </summary>
    /// <typeparam name="T">The form type to show</typeparam>
    /// <returns>DialogResult from the form</returns>
    DialogResult ShowFormAsDialog<T>() where T : Form;

    /// <summary>
    /// Shows a form modeless and returns the instance
    /// </summary>
    /// <typeparam name="T">The form type to show</typeparam>
    /// <returns>The form instance</returns>
    T ShowForm<T>() where T : Form;
  }
}