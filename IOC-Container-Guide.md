# WinForms IOC Container Implementation Guide

This document explains how to use Inversion of Control (IOC) containers in WinForms applications with a factory pattern for creating multiple forms.

## Overview

The IOC container pattern in WinForms provides several benefits:

- **Dependency Injection**: Forms and services receive their dependencies automatically
- **Loose Coupling**: Forms don't directly instantiate their dependencies
- **Testability**: Easy to mock dependencies for unit testing
- **Maintainability**: Central registration of all dependencies
- **Scalability**: Easy to add new forms and services

## Key Components

### 1. Form Factory (`IFormFactory`)

The factory is responsible for creating forms with their dependencies injected:

```csharp
public interface IFormFactory
{
    T CreateForm<T>() where T : Form;
    Form CreateForm(Type formType);
    DialogResult ShowFormAsDialog<T>() where T : Form;
    T ShowForm<T>() where T : Form;
}
```

### 2. Service Registration

All dependencies are registered in the `DiBootStrapper` method:

```csharp
public static void DiBootStrapper(this IServiceCollection services)
{
    // Singleton services (shared across the application)
    services.AddSingleton<ILoggingService, LoggingService>();
    services.AddSingleton<IConfigurationService, ConfigurationService>();
    services.AddSingleton<IFormFactory, FormFactory>();

    // Transient services (new instance each time)
    services.AddTransient<MainForm>();
    services.AddTransient<CustomerForm>();
    services.AddTransient<ProductForm>();

    // MVP pattern components
    services.AddTransient<ICustomerPresenter, CustomerPresenter>();
    services.AddTransient<ICustomerView, CustomerForm>();
    services.AddTransient<CustomerModel>();
}
```

## Service Lifetimes

### Singleton

- **Usage**: Services that maintain state or are expensive to create
- **Examples**: `ILoggingService`, `IConfigurationService`, `IFormFactory`
- **Benefit**: Single instance shared across the application

### Transient

- **Usage**: Forms, presenters, models that should have fresh state
- **Examples**: Forms, presenters, business models
- **Benefit**: New instance every time, ensuring clean state

### Scoped

- **Usage**: Rarely used in WinForms (more common in web applications)
- **Examples**: Database contexts in specific scenarios

## Usage Examples

### 1. Basic Form Creation

```csharp
// Inject the factory into your main form or service
public MainForm(IFormFactory formFactory)
{
    _formFactory = formFactory;
}

// Create and show a form
var customerForm = _formFactory.CreateForm<CustomerForm>();
customerForm.Show();

// Show as dialog
var result = _formFactory.ShowFormAsDialog<CustomerForm>();
```

### 2. Form with Dependencies

```csharp
public partial class CustomerForm : Form, ICustomerView
{
    private readonly ILoggingService _loggingService;
    private readonly IConfigurationService _configurationService;

    // Constructor injection
    public CustomerForm(ILoggingService loggingService, IConfigurationService configurationService)
    {
        _loggingService = loggingService;
        _configurationService = configurationService;
        InitializeComponent();
    }
}
```

### 3. Dynamic Form Creation

```csharp
// Create form by type
Type formType = typeof(ProductForm);
var form = _formFactory.CreateForm(formType);

// Create form based on user selection
var formTypes = new Dictionary<string, Type>
{
    { "Customer", typeof(CustomerForm) },
    { "Product", typeof(ProductForm) },
    { "Order", typeof(OrderForm) }
};

if (formTypes.TryGetValue(selectedFormName, out var type))
{
    var form = _formFactory.CreateForm(type);
    form.Show();
}
```

## Best Practices

### 1. Constructor Design

```csharp
public CustomerForm(ILoggingService loggingService, IConfigurationService configurationService)
{
    // Always validate dependencies
    _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
    _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));

    InitializeComponent();

    // Initialize with dependencies
    LoadSettings();
    _loggingService.LogFormOpened(this.GetType().Name);
}

// Keep default constructor for designer support
public CustomerForm() : this(null!, null!)
{
    // Only for designer - not used at runtime with IOC
}
```

### 2. Error Handling in Factory

```csharp
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
```

### 3. Resource Management

```csharp
// For dialogs, use the factory's ShowFormAsDialog method
public DialogResult ShowCustomerDialog()
{
    // Factory handles disposal automatically
    return _formFactory.ShowFormAsDialog<CustomerForm>();
}

// For modeless forms, track instances if needed
private readonly List<Form> _openForms = new();

public void ShowCustomerForm()
{
    var form = _formFactory.CreateForm<CustomerForm>();
    _openForms.Add(form);

    form.FormClosed += (s, e) => _openForms.Remove(form);
    form.Show();
}
```

### 4. Service Interface Design

```csharp
// Good: Focused interface
public interface ILoggingService
{
    void LogInfo(string message);
    void LogError(string message, Exception exception = null);
}

// Good: Configuration service
public interface IConfigurationService
{
    T GetSetting<T>(string key, T defaultValue = default);
    void SetSetting<T>(string key, T value);
    void SaveSettings();
}
```

## Advanced Scenarios

### 1. Conditional Registration

```csharp
public static void DiBootStrapper(this IServiceCollection services)
{
    // Register different implementations based on environment
    #if DEBUG
    services.AddSingleton<ILoggingService, DebugLoggingService>();
    #else
    services.AddSingleton<ILoggingService, FileLoggingService>();
    #endif

    // Register services based on configuration
    var useFileConfig = Environment.GetEnvironmentVariable("USE_FILE_CONFIG") == "true";
    if (useFileConfig)
        services.AddSingleton<IConfigurationService, FileConfigurationService>();
    else
        services.AddSingleton<IConfigurationService, RegistryConfigurationService>();
}
```

### 2. Decorator Pattern

```csharp
// Add logging decorator around existing service
services.AddSingleton<IConfigurationService, ConfigurationService>();
services.Decorate<IConfigurationService, LoggingConfigurationServiceDecorator>();
```

### 3. Factory Pattern for Complex Objects

```csharp
public interface IReportFormFactory
{
    ReportForm CreateSalesReport(DateTime fromDate, DateTime toDate);
    ReportForm CreateInventoryReport(string location);
}

public class ReportFormFactory : IReportFormFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ReportFormFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ReportForm CreateSalesReport(DateTime fromDate, DateTime toDate)
    {
        var form = _serviceProvider.GetRequiredService<ReportForm>();
        form.InitializeForSalesReport(fromDate, toDate);
        return form;
    }
}
```

## Testing with IOC

### 1. Unit Testing Forms

```csharp
[Test]
public void CustomerForm_Should_LogFormOpened_When_Created()
{
    // Arrange
    var mockLogging = new Mock<ILoggingService>();
    var mockConfig = new Mock<IConfigurationService>();

    // Act
    var form = new CustomerForm(mockLogging.Object, mockConfig.Object);

    // Assert
    mockLogging.Verify(x => x.LogFormOpened("CustomerForm"), Times.Once);
}
```

### 2. Integration Testing with Test Container

```csharp
[Test]
public void FormFactory_Should_Create_CustomerForm_With_Dependencies()
{
    // Arrange
    var services = new ServiceCollection();
    services.DiBootStrapper();

    // Override with mocks for testing
    services.AddSingleton(Mock.Of<ILoggingService>());
    services.AddSingleton(Mock.Of<IConfigurationService>());

    var provider = services.BuildServiceProvider();
    var factory = provider.GetRequiredService<IFormFactory>();

    // Act
    var form = factory.CreateForm<CustomerForm>();

    // Assert
    Assert.IsNotNull(form);
    Assert.IsInstanceOf<CustomerForm>(form);
}
```

## Troubleshooting

### Common Issues

1. **Form not registered**: Register all forms in `DiBootStrapper`
2. **Circular dependencies**: Review constructor dependencies
3. **Designer issues**: Keep parameterless constructor for designer support
4. **Memory leaks**: Properly dispose forms, especially for long-running applications
5. **Service not found**: Ensure service is registered with correct lifetime

### Error Messages

- `InvalidOperationException`: Service not registered or circular dependency
- `ArgumentNullException`: Dependency injection failed, check registrations
- `ObjectDisposedException`: Service disposed too early, check lifetimes

## Performance Considerations

1. **Use Singleton for expensive services** (database connections, logging)
2. **Use Transient for forms** to ensure clean state
3. **Avoid heavy constructors** in Transient services
4. **Consider lazy initialization** for expensive dependencies
5. **Profile memory usage** in long-running applications

This pattern provides a robust foundation for scalable WinForms applications with proper separation of concerns and dependency management.
