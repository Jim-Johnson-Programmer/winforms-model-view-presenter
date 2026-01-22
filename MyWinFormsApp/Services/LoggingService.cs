namespace MyWinFormsApp.Services
{
  /// <summary>
  /// Service for logging application events and user interactions
  /// </summary>
  public interface ILoggingService
  {
    void LogInfo(string message);
    void LogWarning(string message);
    void LogError(string message, Exception? exception = null);
    void LogFormOpened(string formName);
    void LogFormClosed(string formName);
  }

  /// <summary>
  /// Simple implementation of logging service for demonstration
  /// In production, you might use Microsoft.Extensions.Logging or NLog
  /// </summary>
  public class LoggingService : ILoggingService
  {
    private readonly string _logFile;

    public LoggingService()
    {
      // In a real app, this would be configured via appsettings or injected
      var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
      var logDirectory = Path.Combine(appDataPath, "MyWinFormsApp", "Logs");
      Directory.CreateDirectory(logDirectory);
      _logFile = Path.Combine(logDirectory, $"app-{DateTime.Now:yyyyMMdd}.log");
    }

    public void LogInfo(string message)
    {
      WriteLog("INFO", message);
    }

    public void LogWarning(string message)
    {
      WriteLog("WARN", message);
    }

    public void LogError(string message, Exception? exception = null)
    {
      var fullMessage = exception != null ? $"{message} - Exception: {exception}" : message;
      WriteLog("ERROR", fullMessage);
    }

    public void LogFormOpened(string formName)
    {
      LogInfo($"Form opened: {formName}");
    }

    public void LogFormClosed(string formName)
    {
      LogInfo($"Form closed: {formName}");
    }

    private void WriteLog(string level, string message)
    {
      try
      {
        var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}{Environment.NewLine}";
        File.AppendAllText(_logFile, logEntry);

        // Also write to debug output for development
        System.Diagnostics.Debug.WriteLine($"[{level}] {message}");
      }
      catch
      {
        // Ignore logging errors to prevent application crashes
      }
    }
  }
}