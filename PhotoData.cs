using System.Text.Json;
using Observatory.Framework.Files;
using Observatory.Framework.Interfaces;

namespace Observatory.Photographer
{
    public class PhotoData
    {
        public PhotoData(IObservatoryCore core, Action<Exception, string> errorLogger)
        {
            _core = core;
            _errorLogger = errorLogger;
            ScreenshotStatus = LoadPersistedStatus();
            SavedPresets = LoadSavedPresets();
        }

        private IObservatoryCore _core { get; init; }
        private Action<Exception, string> _errorLogger { get; }
        private string _statusStoragePath =>
            Path.Combine(_core.PluginStorageFolder, "screenshot_status.json");
        private string _presetsStoragePath =>
            Path.Combine(_core.PluginStorageFolder, "presets.json");
        public Dictionary<string, Status> ScreenshotStatus { get; set; }
        public Dictionary<string, IEnumerable<PhotoAction>> SavedPresets { get; set; }

        public void SaveStatus()
        {
            var json = JsonSerializer.Serialize(ScreenshotStatus);
            File.WriteAllText(_statusStoragePath, json);
        }

        public void SavePresets()
        {
            var json = JsonSerializer.Serialize(SavedPresets);
            File.WriteAllText(_presetsStoragePath, json);
        }

        public void SaveAll()
        {
            SaveStatus();
            SavePresets();
        }

        public void Reload()
        {
            ScreenshotStatus = LoadPersistedStatus();
            SavedPresets = LoadSavedPresets();
        }

        private Dictionary<string, Status> LoadPersistedStatus()
        {
            if (!File.Exists(_statusStoragePath))
            {
                return [];
            }
            var json = File.ReadAllText(_statusStoragePath);
            return DeserializeOrDefault<Dictionary<string, Status>>(json) ?? [];
        }

        private Dictionary<string, IEnumerable<PhotoAction>> LoadSavedPresets()
        {
            if (!File.Exists(_presetsStoragePath))
            {
                return [];
            }
            var json = File.ReadAllText(_presetsStoragePath);
            var savedActions = DeserializeOrDefault<Dictionary<string, object[]>>(json) ?? [];
            return savedActions.ToDictionary(kvp => kvp.Key, kvp => DeserializeActions(kvp.Value));
        }

        private IEnumerable<PhotoAction> DeserializeActions(object[] actions) =>
            actions
                .Select<object, PhotoAction>(action =>
                {
                    try
                    {
                        var jsonAction = (JsonElement)action;
                        var actionType = (PhotoActionKind)
                            jsonAction.GetProperty("Action").GetInt32();
                        return actionType switch
                        {
                            PhotoActionKind.Save => jsonAction.Deserialize<SaveAction>()!,
                            PhotoActionKind.Resize => jsonAction.Deserialize<ResizeAction>()!,
                            PhotoActionKind.Caption => jsonAction.Deserialize<CaptionAction>()!,
                            PhotoActionKind.Watermark => jsonAction.Deserialize<WatermarkAction>()!,
                            _ => throw new InvalidOperationException(
                                "Unknown action type in settings"
                            ),
                        };
                    }
                    catch (Exception ex)
                    {
                        // If deserialization fails for any reason, skip this action.
                        _errorLogger(ex, "Failed to deserialize photo action.");
                        return null!;
                    }
                })
                .Where(action => action != null)!;

        private T? DeserializeOrDefault<T>(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                _errorLogger(ex, "Failed to deserialize photographer JSON data.");
                return default;
            }
        }
    }
}
