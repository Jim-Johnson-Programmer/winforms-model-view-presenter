namespace MyWinFormsApp.Services
{
  /// <summary>
  /// Service for managing application configuration settings
  /// </summary>
  public interface IConfigurationService
  {
    string GetSetting(string key, string defaultValue = "");
    T GetSetting<T>(string key, T defaultValue = default);
    void SetSetting(string key, string value);
    void SetSetting<T>(string key, T value);
    void SaveSettings();
  }

  /// <summary>
  /// Simple configuration service that stores settings in a JSON file
  /// </summary>
  public class ConfigurationService : IConfigurationService
  {
    private readonly Dictionary<string, string> _settings;
    private readonly string _configFile;

    public ConfigurationService()
    {
      var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
      var configDirectory = Path.Combine(appDataPath, "MyWinFormsApp");
      Directory.CreateDirectory(configDirectory);
      _configFile = Path.Combine(configDirectory, "appsettings.json");

      _settings = LoadSettings();
    }

    public string GetSetting(string key, string defaultValue = "")
    {
      return _settings.TryGetValue(key, out var value) ? value : defaultValue;
    }

    public T GetSetting<T>(string key, T defaultValue = default)
    {
      if (!_settings.TryGetValue(key, out var value))
        return defaultValue;

      try
      {
        return (T)Convert.ChangeType(value, typeof(T));
      }
      catch
      {
        return defaultValue;
      }
    }

    public void SetSetting(string key, string value)
    {
      _settings[key] = value;
    }

    public void SetSetting<T>(string key, T value)
    {
      _settings[key] = value?.ToString() ?? "";
    }

    public void SaveSettings()
    {
      try
      {
        var json = System.Text.Json.JsonSerializer.Serialize(_settings, new System.Text.Json.JsonSerializerOptions
        {
          WriteIndented = true
        });
        File.WriteAllText(_configFile, json);
      }
      catch
      {
        // Ignore save errors
      }
    }

    private Dictionary<string, string> LoadSettings()
    {
      try
      {
        if (File.Exists(_configFile))
        {
          var json = File.ReadAllText(_configFile);
          return System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json)
                 ?? new Dictionary<string, string>();
        }
      }
      catch
      {
        // Ignore load errors, return empty settings
      }

      return new Dictionary<string, string>();
    }
  }
}