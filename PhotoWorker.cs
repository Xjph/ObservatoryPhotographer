using Observatory.Framework;
using Observatory.Framework.Files.Journal;
using Observatory.Framework.Interfaces;

namespace Observatory.Photographer
{
    public class PhotoWorker : IObservatoryWorker
    {
        private PhotoSettings _settings;
        private Photographer? _photographer;
        private PluginUI _pluginUI;
        private UI.MainPanel _mainPanel;

        public PhotoWorker()
        {
            var defaultScreenshotLocation = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                "Frontier Developments",
                "Elite Dangerous"
            );

            var defaultOutputLocation = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                "Elite Observatory Screenshots"
            );

            _settings = new()
            {
                ScreenshotLocationPath = defaultScreenshotLocation,
                OutputLocationPath = defaultOutputLocation,
                ProcessDuringReadAll = false,
                ProcessWhileMonitoring = true,
                DefaultPreset = string.Empty,
            };

            _mainPanel = new();
            _mainPanel.PhotoPanel.Location = new(0, 0);
            _pluginUI = new(PluginUI.UIType.Panel, _mainPanel.PhotoPanel);
        }

        AboutInfo IObservatoryPlugin.AboutInfo =>
            new()
            {
                AuthorName = "Vithigar",
                FullName = "Observatory Photographer",
                ShortName = "Photographer",
                Description =
                    "Manages screenshots taken by the user, allowing them to be automatically renamed and organized based on screenshot metadata.\r\n\r\nIt can also apply some simple post-processing, including resizing, addition text captions, or applying a watermark.",
                Links =
                [
                    new("Documentation", "https://observatory.xjph.net/usage/plugins/photographer"),
                ],
            };

        public static Guid Guid => new("AEA9A421-94FB-40D9-BCFF-AB899B279325");

        string IObservatoryPlugin.Version =>
            (typeof(PhotoWorker).Assembly.GetName().Version?.ToString() ?? "0") + "-Patreon";

        PluginUI IObservatoryPlugin.PluginUI => _pluginUI;

        public object Settings
        {
            get => _settings;
            set => _settings = (PhotoSettings)value;
        }

        void IObservatoryWorker.JournalEvent<TJournal>(TJournal journal)
        {
            switch (journal)
            {
                case Screenshot screenshot:
                    _photographer?.HandleScreenshot(screenshot);
                    break;
                case LoadGame loadGame:
                    CmdrName = loadGame.Commander;
                    break;
            }
        }

        void IObservatoryPlugin.Load(IObservatoryCore observatoryCore)
        {
            _photographer = new(
                observatoryCore,
                this,
                _mainPanel,
                _settings,
                observatoryCore.GetPluginErrorLogger(this)
            );
            _mainPanel.Core = observatoryCore;
            _mainPanel.Worker = this;
            _mainPanel.Photographer = _photographer;
        }

        void IObservatoryWorker.LogMonitorStateChanged(LogMonitorStateChangedEventArgs eventArgs)
        {
            if (
                _settings.ProcessDuringReadAll
                && !eventArgs.PreviousState.HasFlag(LogMonitorState.Batch)
                && eventArgs.NewState.HasFlag(LogMonitorState.Batch)
            )
            {
                _mainPanel.Core?.ExecuteOnUIThread(() => _mainPanel.ShowProcessing(true));
            }

            if (
                _settings.ProcessDuringReadAll
                && eventArgs.PreviousState.HasFlag(LogMonitorState.Batch)
                && !eventArgs.NewState.HasFlag(LogMonitorState.Batch)
            )
            {
                _settings.ProcessDuringReadAll = false;
                _mainPanel.Core?.SaveSettings(this);
            }

            if (
                eventArgs.NewState.HasFlag(LogMonitorState.Batch)
                && !eventArgs.NewState.HasFlag(LogMonitorState.PreRead)
            )
            {
                _photographer?.Clear();
            }
        }

        internal string CmdrName { get; set; } = "CMDR";
    }
}
