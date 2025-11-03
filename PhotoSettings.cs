using Observatory.Framework;

namespace Observatory.Photographer
{
    public class PhotoSettings
    {
        [SettingDisplayName("Process While Monitoring")]
        public bool ProcessWhileMonitoring { get; set; }

        [SettingDisplayName("Import During Read All")]
        public bool ImportDuringReadAll { get; set; }

        [SettingDisplayName("Process During Read All")]
        public bool ProcessDuringReadAll { get; set; }

        [SettingDisplayName("Separate Output")]
        public bool SeparateOutput { get; set; }

        [SettingDisplayName("Screenshot Location")]
        [System.Text.Json.Serialization.JsonIgnore]
        public DirectoryInfo ScreenshotLocation 
        { 
            get => new(ScreenshotLocationPath); 
            set => ScreenshotLocationPath = value.FullName; 
        }

        [SettingDisplayName("Output Folder")]
        [System.Text.Json.Serialization.JsonIgnore]
        public DirectoryInfo OutputLocation
        {
            get => new(OutputLocationPath);
            set => OutputLocationPath = value.FullName;
        }

        [SettingIgnore]
        public string ScreenshotLocationPath { get; set; }

        [SettingIgnore]
        public string OutputLocationPath { get; set; }

        [SettingIgnore]
        public object[] SavedActions { get; set; }
    }
}
