using ImageMagick;
using Observatory.Framework.Interfaces;

namespace Observatory.Photographer.UI
{
    public partial class ProcessForm : Form
    {
        private ImageWithMetadata _image;
        private CaptionForm _captionForm;
        private WatermarkForm _watermarkForm;
        private MainPanel? _mainPanel;
        private CaptionAction _captionAction;
        private WatermarkAction _watermarkAction;
        private SaveAction _saveAction;
        private ResizeAction _resizeAction;
        private readonly IObservatoryCore? _core;
        private readonly PhotoData _photoData;
        private readonly PhotoWorker? _worker;
        private string _currentPreset = string.Empty;

        public ProcessForm(ImageWithMetadata metadata, IObservatoryCore? core, PhotoWorker? worker)
        {
            _image = metadata;
            _captionAction = new() { Font = Font, CmdrName = worker?.CmdrName ?? "CMDR" };
            _watermarkAction = new() { WatermarkImagePath = string.Empty };
            _saveAction = new()
            {
                Format = MagickFormat.Png,
                FolderPath = ((PhotoSettings)worker.Settings).OutputLocationPath,
                FilePattern = "image",
                CmdrName = worker?.CmdrName ?? "CMDR",
                IncludeMetadata = false,
                SeparateOutput = false,
            };
            _resizeAction = new()
            {
                X = 100,
                Y = 100,
                Relative = true,
            };
            _captionForm = new(_captionAction);
            _watermarkForm = new(_watermarkAction);
            _core = core;
            _worker = worker;
            _photoData = new(core, core.GetPluginErrorLogger(worker));
            core?.RegisterControl(_captionForm);
            core?.RegisterControl(_watermarkForm);
            CancelButton = CancelBtn;
            InitializeComponent();
            RestoreSavedProcess();
            FilenameTooltip.SetToolTip(
                ExampleLabel,
                string.Empty
            );
        }

        private void UpdateUIFromActionList(IEnumerable<PhotoAction> actionList)
        {
            foreach (var action in actionList)
            {
                switch (action)
                {
                    case SaveAction saveAction:
                        _saveAction = saveAction;
                        FilenameTextbox.Text = saveAction.FilePattern;
                        MetadataCheckbox.Checked = saveAction.IncludeMetadata;
                        switch (saveAction.Format)
                        {
                            case MagickFormat.Jpeg:
                                ConvertDropdown.SelectedItem = "JPEG";
                                QualitySpinner.Value =
                                    saveAction.Quality > 0 ? saveAction.Quality : 85;
                                QualitySpinner.Visible = true;
                                break;
                            case MagickFormat.Png:
                                ConvertDropdown.SelectedItem = "PNG";
                                QualitySpinner.Value = 100;
                                QualitySpinner.Visible = false;
                                break;
                            case MagickFormat.Heic:
                                ConvertDropdown.SelectedItem = "HEIC";
                                QualitySpinner.Value =
                                    saveAction.Quality > 0 ? saveAction.Quality : 85;
                                QualitySpinner.Visible = true;
                                break;
                            case MagickFormat.WebP:
                                ConvertDropdown.SelectedItem = "WEBP";
                                QualitySpinner.Value =
                                    saveAction.Quality > 0 ? saveAction.Quality : 85;
                                QualitySpinner.Visible = true;
                                break;
                            case MagickFormat.Bmp:
                                ConvertDropdown.SelectedItem = "BMP";
                                QualitySpinner.Value = 100;
                                QualitySpinner.Visible = false;
                                break;
                        }
                        break;
                    case WatermarkAction watermarkAction:
                        _watermarkAction = watermarkAction;
                        WatermarkCheckbox.Checked = true;
                        break;
                    case CaptionAction captionAction:
                        _captionAction = captionAction;
                        CaptionCheckbox.Checked = true;
                        break;
                    case ResizeAction resizeAction:
                        _resizeAction = resizeAction;
                        ResizeCheckbox.Checked = true;
                        if (resizeAction.Relative)
                        {
                            ResizeScale.Checked = true;
                            ResizePercentSpinner.Value = resizeAction.X;
                        }
                        else
                        {
                            ResizeFixed.Checked = true;
                            ResizeXSpinner.Value = resizeAction.X;
                            ResizeYSpinner.Value = resizeAction.Y;
                        }
                        break;
                }
            }

            if (FilenameTextbox.Text.Trim() == string.Empty)
                FilenameTextbox.Text = "{timestamp}-{system}";
            ConvertDropdown.SelectedItem ??= "PNG";
        }

        private void RestoreSavedProcess()
        {
            IEnumerable<PhotoAction> actionList;
            try
            {
                var defaultAction =
                    ((PhotoSettings?)_worker?.Settings)?.DefaultPreset ?? string.Empty;
                _photoData.SavedPresets.TryGetValue(
                    defaultAction,
                    out IEnumerable<PhotoAction>? defaultActions
                );
                actionList = defaultActions ?? [];
            }
            catch (Exception ex) 
            {
                _mainPanel?.Photographer?.Errorlogger(ex, "Failed to restore saved process.");
                actionList = [];
            }

            UpdateUIFromActionList(actionList);
        }

        private void CaptionButton_Click(object sender, EventArgs e)
        {
            _captionForm.ShowDialog();
        }

        private void WatermarkButton_Click(object sender, EventArgs e)
        {
            _watermarkForm.ShowDialog();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            ProcessingLabel.BringToFront();
            ProcessingLabel.Enabled = true;
            ProcessingLabel.Visible = true;
            Task.Run(() =>
            {
                ImageUtils.PerformPhotoActions(BuildActionList(), _image);
                _core?.ExecuteOnUIThread(() => Close());
            }).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    _mainPanel?.Photographer?.Errorlogger(t.Exception, "Error performing photo actions.");
                }
            });
        }

        private void SetDefaultButton_Click(object sender, EventArgs e)
        {
            var settings = (PhotoSettings?)_worker?.Settings;
            if (settings is null)
                return;
            settings.DefaultPreset = string.IsNullOrEmpty(_currentPreset)
                ? "Default"
                : _currentPreset;

            _core?.SaveSettings(_worker);
        }

        private List<PhotoAction> BuildActionList()
        {
            List<PhotoAction> actionList = [];

            if (WatermarkCheckbox.Checked)
                actionList.Add(_watermarkAction);

            if (CaptionCheckbox.Checked)
                actionList.Add(_captionAction);

            if (ResizeCheckbox.Checked)
                actionList.Add(_resizeAction);

            actionList.Add(_saveAction);

            return actionList;
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ConvertDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ConvertDropdown.SelectedItem?.ToString() ?? "BMP")
            {
                case "JPEG":
                    _saveAction.Format = MagickFormat.Jpeg;
                    QualitySpinner.Value = 85;
                    QualitySpinner.Visible = true;
                    break;
                case "PNG":
                    _saveAction.Format = MagickFormat.Png;
                    QualitySpinner.Value = 100;
                    QualitySpinner.Visible = false;
                    break;
                case "HEIC":
                    _saveAction.Format = MagickFormat.Heic;
                    QualitySpinner.Value = 85;
                    QualitySpinner.Visible = true;
                    break;
                case "WEBP":
                    _saveAction.Format = MagickFormat.WebP;
                    QualitySpinner.Value = 85;
                    QualitySpinner.Visible = true;
                    break;
                case "BMP":
                    _saveAction.Format = MagickFormat.Bmp;
                    QualitySpinner.Value = 100;
                    QualitySpinner.Visible = false;
                    break;
            }
        }

        private void QualitySpinner_ValueChanged(object sender, EventArgs e)
        {
            _saveAction.Quality = (uint)QualitySpinner.Value;
        }

        private void ResizeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            ResizeFixed.Enabled = ResizeCheckbox.Checked;
            ResizeScale.Enabled = ResizeCheckbox.Checked;
            ResizeXSpinner.Enabled = ResizeCheckbox.Checked;
            ResizeYSpinner.Enabled = ResizeCheckbox.Checked;
            ResizePercentSpinner.Enabled = ResizeCheckbox.Checked;
        }

        private void ResizeFixed_CheckedChanged(object sender, EventArgs e)
        {
            SetEnabledResizeControls();
        }

        private void ResizeScale_CheckedChanged(object sender, EventArgs e)
        {
            SetEnabledResizeControls();
        }

        private void SetEnabledResizeControls()
        {
            _resizeAction.Relative = ResizeScale.Checked;
            ResizeXSpinner.Enabled = ResizeFixed.Checked;
            ResizeYSpinner.Enabled = ResizeFixed.Checked;
            ResizePercentSpinner.Enabled = ResizeScale.Checked;
        }

        private void ResizeXSpinner_ValueChanged(object sender, EventArgs e)
        {
            _resizeAction.X = (int)ResizeXSpinner.Value;
        }

        private void ResizeYSpinner_ValueChanged(object sender, EventArgs e)
        {
            _resizeAction.Y = (int)ResizeYSpinner.Value;
        }

        private void ResizePercentSpinner_ValueChanged(object sender, EventArgs e)
        {
            _resizeAction.X = (int)ResizePercentSpinner.Value;
        }

        private void CaptionTextbox_TextChanged(object sender, EventArgs e)
        {
            _captionAction.Text = CaptionTextbox.Text;
        }

        private void FilenameTextbox_TextChanged(object sender, EventArgs e)
        {
            _saveAction.FilePattern = FilenameTextbox.Text;
            ExampleLabel.Text = ImageUtils.FillTokenizedString(
                FilenameTextbox.Text,
                _image,
                _saveAction.CmdrName
            );
            FilenameTooltip.SetToolTip(
                ExampleLabel,
                ExampleLabel.Text
            );
        }

        private void FilenameLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(
                "SCREENSHOT PROPERTIES:\n"
                    + "{cmdr} - Commander name\n"
                    + "{latitude} - Latitude coordinate\n"
                    + "{longitude} - Longitude coordinate\n"
                    + "{system} - System name\n"
                    + "{body} - Body name\n"
                    + "{altitude} - Altitude\n"
                    + "{heading} - Heading direction\n"
                    + "{timestamp} - ISO timestamp (YYYY-MM-DDTHH-MM-SS)\n"
                    + "{timestamp:FORMAT} - Custom formatted timestamp\n\n"
                    + "STATUS PROPERTIES (Not always available):\n"
                    + "{guifocus} - Current GUI focus\n"
                    + "{balance} - Credit balance\n"
                    + "{cargo} - Cargo amount\n"
                    + "{mainfuel} - Main fuel level\n"
                    + "{reservoirfuel} - Reservoir fuel level\n"
                    + "{health} - Health percentage\n"
                    + "{oxygen} - Oxygen percentage\n"
                    + "{destination} - Destination name\n"
                    + "{gravity-g} - Gravity in G\n"
                    + "{gravity-mps2} - Gravity in m/s²\n"
                    + "{legalstate} - Legal state\n"
                    + "{radius} - Planet radius in km\n"
                    + "{temperature} - Temperature\n"
                    + "{hud} - HUD mode (Analysis/Combat)\n"
                    + "{docked} - Docked/Undocked\n"
                    + "{landed} - Landed/In Flight\n"
                    + "{landinggear} - Landing gear (Down/Raised)\n"
                    + "{shields} - Shields (Up/Down)\n"
                    + "{supercruise} - Supercruise/Normal Space\n"
                    + "{faoff} - Flight assist (On/Off)\n"
                    + "{hardpoints} - Hardpoints (Deployed/Retracted)\n"
                    + "{wing} - Wing/Solo\n"
                    + "{lights} - Lights (On/Off)\n"
                    + "{cargoscoop} - Cargo scoop (Deployed/Retracted)\n"
                    + "{silentrunning} - Silent running status\n"
                    + "{fuelscooping} - Fuel scooping status\n"
                    + "{srvbrake} - SRV brake (On/Off)\n"
                    + "{srvturret} - SRV turret (Active/Fixed)\n"
                    + "{srvproximity} - SRV proximity (Close/Clear)\n"
                    + "{srvdriveassist} - SRV drive assist (On/Off)\n"
                    + "{masslock} - Mass lock status\n"
                    + "{fsdcharging} - FSD charging/idle\n"
                    + "{fsdcooldown} - FSD cooldown/ready\n"
                    + "{lowfuel} - Low fuel warning\n"
                    + "{overheat} - Overheat warning\n"
                    + "{latlongvalid} - Lat/Long validity\n"
                    + "{indanger} - In danger status\n"
                    + "{interdiction} - Interdiction status\n"
                    + "{mainship} - In main ship\n"
                    + "{fighter} - In fighter\n"
                    + "{srv} - In SRV\n"
                    + "{nightvision} - Night vision (On/Off)\n"
                    + "{radialaltitude} - Altitude mode (Radial/Terrain)\n"
                    + "{fsdjump} - FSD jumping status\n"
                    + "{srvhighbeam} - SRV high beam (On/Off)\n"
                    + "{onfoot} - On foot status\n"
                    + "{intaxi} - In taxi status\n"
                    + "{inmulticrew} - In multicrew\n"
                    + "{onfootinstation} - On foot in station\n"
                    + "{onfootonplanet} - On foot on planet\n"
                    + "{aimdownsight} - Aiming down sight\n"
                    + "{lowoxygen} - Low oxygen warning\n"
                    + "{lowhealth} - Low health warning\n"
                    + "{cold} - Cold status\n"
                    + "{hot} - Hot status\n"
                    + "{verycold} - Very cold status\n"
                    + "{veryhot} - Very hot status\n"
                    + "{glidemode} - Gliding status\n"
                    + "{onfootinhangar} - On foot in hangar\n"
                    + "{onfootinsocialspace} - On foot in social space\n"
                    + "{onfootexterior} - On foot exterior\n"
                    + "{breathableatmosphere} - Breathable atmosphere\n"
                    + "{telepresencemulticrew} - Telepresence multicrew\n"
                    + "{physicalmulticrew} - Physical multicrew\n"
                    + "{fsdhyperdrivecharging} - FSD hyperdrive charging",
                "Image Metadata Tokens",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void LoadPresetButton_Click(object sender, EventArgs e)
        {
            var presetForm = new PresetForm(_photoData);
            var result = presetForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                _currentPreset = presetForm.PresetName;
                UpdateUIFromActionList(presetForm.PresetAction);
            }
        }

        private void SavePresetButton_Click(object sender, EventArgs e)
        {
            var presetForm = new PresetForm(_photoData, true) { PresetAction = BuildActionList() };
            var result = presetForm.ShowDialog();
            if (result == DialogResult.OK)
                _currentPreset = presetForm.PresetName;
        }

        private void MetadataCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            _saveAction.IncludeMetadata = MetadataCheckbox.Checked;
        }

        private void SeparateCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            _saveAction.SeparateOutput = SeparateCheckbox.Checked;
        }
    }
}
