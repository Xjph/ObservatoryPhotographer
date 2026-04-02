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
            var defaultScreenshotLocation =
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
                + Path.DirectorySeparatorChar
                + "Frontier Developments"
                + Path.DirectorySeparatorChar
                + "Elite Dangerous";

            var defaultOutputLocation =
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
                + Path.DirectorySeparatorChar
                + "Elite Observatory Screenshots";

            _settings = new()
            {
                ScreenshotLocationPath = defaultScreenshotLocation,
                OutputLocationPath = defaultOutputLocation,
                ProcessDuringReadAll = false,
                ProcessWhileMonitoring = true,
                SeparateOutput = false,
                SavedActions = [],
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
            };

        public static Guid Guid => new("AEA9A421-94FB-40D9-BCFF-AB899B279325");

        string IObservatoryPlugin.Version =>
            typeof(PhotoWorker).Assembly.GetName().Version?.ToString() ?? "0";

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

        void IObservatoryWorker.StatusChange(Observatory.Framework.Files.Status status)
        {
            _photographer?.UpdateContext(status);
        }

        void IObservatoryPlugin.Load(IObservatoryCore observatoryCore)
        {
            _photographer = new(observatoryCore, this, _mainPanel, _settings);
            _mainPanel.Core = observatoryCore;
            _mainPanel.Worker = this;
            _mainPanel.Photographer = _photographer;
        }

        internal string CmdrName { get; set; } = "CMDR";
    }
}
