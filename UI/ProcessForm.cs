using System.Diagnostics;
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
        private readonly PhotoWorker _worker;
        private string _currentPreset = string.Empty;

        public ProcessForm(ImageWithMetadata metadata, IObservatoryCore core, PhotoWorker worker, PhotoData photoData)
        {
            _image = metadata;
            _captionAction = new()
            {
                FontPath = GetFontPath(Font),
                FontFamily = Font.FontFamily.Name,
                FontSize = (uint)Font.Size,
                CmdrName = worker.CmdrName ?? "CMDR",
            };
            _watermarkAction = new() { WatermarkImagePath = string.Empty };
            _saveAction = new()
            {
                Format = MagickFormat.Png,
                FolderPath = ((PhotoSettings)worker.Settings).OutputLocationPath,
                FilePattern = "{cmdr}-{system}-{timestamp}",
                CmdrName = worker.CmdrName ?? "CMDR",
                IncludeMetadata = true,
                SeparateOutput = false,
            };
            _resizeAction = new()
            {
                X = 100,
                Y = 100,
                Relative = true,
            };
            _core = core;
            _worker = worker;
            _photoData = photoData;
            CancelButton = CancelBtn;
            InitializeComponent();
            RestoreSavedProcess();
            _captionForm = new(_captionAction);
            _watermarkForm = new(_watermarkAction);
            core.RegisterControl(_captionForm);
            core.RegisterControl(_watermarkForm);
            FilenameTooltip.SetToolTip(ExampleLabel, string.Empty);
        }

        public ProcessForm(IObservatoryCore core, PhotoWorker worker)
        {
            _image = new(
                new(MagickColors.White, 1, 1),
                new()
                {
                    System = "ExampleSystem",
                    Body = "ExampleBody",
                    Timestamp = DateTime.Now.ToString(),
                    Latitude = 12.34f,
                    Longitude = 56.78f,
                    Altitude = 910f,
                    Heading = 180,
                }
            );

            _captionAction = new()
            {
                FontPath = GetFontPath(Font),
                FontFamily = Font.FontFamily.Name,
                FontSize = (uint)Font.Size,
                CmdrName = worker.CmdrName ?? "CMDR",
            };
            _watermarkAction = new() { WatermarkImagePath = string.Empty };
            _saveAction = new()
            {
                Format = MagickFormat.Png,
                FolderPath = ((PhotoSettings)worker.Settings).OutputLocationPath,
                FilePattern = "{cmdr}-{system}-{timestamp}",
                CmdrName = worker.CmdrName ?? "CMDR",
                IncludeMetadata = true,
                SeparateOutput = false,
            };
            _resizeAction = new()
            {
                X = 100,
                Y = 100,
                Relative = true,
            };
            _core = core;
            _worker = worker;
            _photoData = new(core, core.GetPluginErrorLogger(worker));
            InitializeComponent();
            RestoreSavedProcess();
            CancelButton = CancelBtn;
            OkButton.Visible = false;
            OkButton.Enabled = false;
            Text = "Manage Process Preset";
            _captionForm = new(_captionAction);
            _watermarkForm = new(_watermarkAction);
            core.RegisterControl(_captionForm);
            core.RegisterControl(_watermarkForm);
            FilenameTooltip.SetToolTip(ExampleLabel, string.Empty);
        }

        private void UpdateUIFromActionList(IEnumerable<PhotoAction> actionList)
        {
            foreach (var action in actionList)
            {
                switch (action)
                {
                    case SaveAction saveAction:
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
                        _saveAction = saveAction;
                        break;
                    case WatermarkAction watermarkAction:
                        WatermarkCheckbox.Checked = true;
                        _watermarkAction = watermarkAction;
                        break;
                    case CaptionAction captionAction:
                        CaptionTextbox.Text = captionAction.Text;
                        CaptionCheckbox.Checked = true;
                        _captionAction = captionAction;
                        break;
                    case ResizeAction resizeAction:
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
                        _resizeAction = resizeAction;
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
                _photoData.SavedPresets.TryGetValue(
                    Photographer.DEFAULT_PRESET,
                    out IEnumerable<PhotoAction>? defaultActions
                );
                actionList = defaultActions ?? [];
            }
            catch (Exception ex)
            {
                _mainPanel?.Photographer?.Errorlogger(ex, "Failed to restore saved process.");
                actionList = [_saveAction];
            }

            UpdateUIFromActionList(actionList);
        }

        private void CaptionButton_Click(object sender, EventArgs e)
        {
            _captionForm.StartPosition = FormStartPosition.Manual;
            _captionForm.Location = Point.Add(Location, new Size(100, 100));
            _captionForm.ShowDialog();
        }

        private void WatermarkButton_Click(object sender, EventArgs e)
        {
            _watermarkForm.StartPosition = FormStartPosition.Manual;
            _watermarkForm.Location = Point.Add(Location, new Size(100, 100));
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
                })
                .ContinueWith(t =>
                {
                    if (t.Exception != null)
                    {
                        _mainPanel?.Photographer?.Errorlogger(
                            t.Exception,
                            "Error performing photo actions."
                        );
                    }
                });
        }

        private void SetDefaultButton_Click(object sender, EventArgs e)
        {
            if (_photoData.SavedPresets.ContainsKey(Photographer.DEFAULT_PRESET))
                _photoData.SavedPresets[Photographer.DEFAULT_PRESET] = BuildActionList();
            else
                _photoData.SavedPresets.Add(Photographer.DEFAULT_PRESET, BuildActionList());

            _photoData.SavePresets();
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
                case "JPEG-XL":
                    _saveAction.Format = MagickFormat.Jxl;
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
            FilenameTooltip.SetToolTip(ExampleLabel, ExampleLabel.Text);
        }

        private void OpenReferenceForm(TextBox textBox)
        {
            var referenceForm = new TokenReferenceForm(textBox);
            _core?.RegisterControl(referenceForm);
            referenceForm.StartPosition = FormStartPosition.Manual;
            referenceForm.Location = Point.Add(Location, new Size(Width, 0));
            referenceForm.Show();
        }

        private void CaptionLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenReferenceForm(CaptionTextbox);
        }

        private void FilenameLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenReferenceForm(FilenameTextbox);
        }

        private void LoadPresetButton_Click(object sender, EventArgs e)
        {
            var presetForm = new PresetForm(_photoData);
            _core?.RegisterControl(presetForm);
            presetForm.StartPosition = FormStartPosition.Manual;
            presetForm.Location = Point.Add(Location, new Size(100, 100));
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
            _core?.RegisterControl(presetForm);
            presetForm.StartPosition = FormStartPosition.Manual;
            presetForm.Location = Point.Add(Location, new Size(100, 100));
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

        public static string GetFontPath(Font font)
        {
            var userFontDir = Path.Join(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Microsoft",
                "Windows",
                "Fonts"
            );

            List<string> allFonts =
            [
                .. Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Fonts)),
                .. Directory.Exists(userFontDir)
                    ? Directory.GetFiles(
                        Path.Join(
                            Environment.GetFolderPath(
                                Environment.SpecialFolder.LocalApplicationData
                            ),
                            "Microsoft",
                            "Windows",
                            "Fonts"
                        )
                    )
                    : [],
            ];

            var fontMatch =
                SearchForFontMatch(allFonts, font, false)
                ?? SearchForFontMatch(allFonts, font, true);

            return fontMatch ?? string.Empty;
        }

        private static string? SearchForFontMatch(List<string> files, Font font, bool matchPartial)
        {
            foreach (var fontFile in files)
            {
                try
                {
                    using var privateFontCollection =
                        new System.Drawing.Text.PrivateFontCollection();
                    privateFontCollection.AddFontFile(fontFile);
                    if (
                        privateFontCollection.Families.Any(f =>
                            matchPartial
                                ? f.Name.Contains(font.FontFamily.Name)
                                : f.Name == font.FontFamily.Name
                        )
                    )
                    {
                        return fontFile;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error loading font file {fontFile}: {ex.Message}");
                }
            }
            return null;
        }
    }
}
