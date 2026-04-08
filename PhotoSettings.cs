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
        public bool ProcessDuringReadAll
        {
            get => _processDuringReadAll;
            set
            {
                if (value)
                {
                    MessageBox.Show(
                        "\"Process During Read All\" is resource intensive and may cause performance degradation during the read all operation."
                            + Environment.NewLine
                            + "Depending on the number of screenshots and the specifications of your computer it may take up to several minutes to complete."
                            + Environment.NewLine
                            + Environment.NewLine
                            + "This setting will automatically disable after the next Read All operation finishes.",
                        "Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
                _processDuringReadAll = value;
            }
        }

        [SettingDisplayName("Allow Timestamp Mismatch")]
        public bool AllowTimestampMismatch { get; set; }

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
        public required string ScreenshotLocationPath { get; set; }

        [SettingIgnore]
        public required string OutputLocationPath { get; set; }

        [SettingIgnore]
        public required string DefaultPreset { get; set; }

        private bool _processDuringReadAll;
    }
}
