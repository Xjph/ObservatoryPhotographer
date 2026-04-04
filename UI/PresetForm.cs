namespace Observatory.Photographer.UI
{
    public partial class PresetForm : Form
    {
        private PhotoData _photoData;
        public string PresetName { get; private set; } = string.Empty;
        public List<PhotoAction> PresetAction
        {
            get => _presetAction;
            init { _presetAction = value; }
        }

        private List<PhotoAction> _presetAction = [];

        public PresetForm(PhotoData photoData, bool save = false)
        {
            InitializeComponent();
            _photoData = photoData;
            if (save)
            {
                Text = "Save Preset";
                SaveLoadButton.Text = "Save";
                SaveNameLabel.Enabled = true;
                SaveNameLabel.Visible = true;
                PresetNameText.Enabled = true;
                PresetNameText.Visible = true;
            }
            else
            {
                Height -= 23;
            }
            LoadPresets();
        }

        private void LoadPresets()
        {
            SavedPresetDropdown.Items.Clear();
            foreach (var preset in _photoData.SavedPresets.Keys)
            {
                SavedPresetDropdown.Items.Add(preset);
            }
        }

        private void SaveLoadButton_Click(object? sender, EventArgs e)
        {
            try
            {
                if (SaveLoadButton.Text == "Save")
                {
                    if (_photoData.SavedPresets.ContainsKey(PresetNameText.Text))
                    {
                        var result = MessageBox.Show(
                            "A preset with this name already exists. Do you want to overwrite it?",
                            "Confirm Overwrite",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning
                        );
                        if (result == DialogResult.No)
                        {
                            return;
                        }
                        _photoData.SavedPresets[PresetNameText.Text] = PresetAction;
                    }
                    else
                    {
                        _photoData.SavedPresets.Add(PresetNameText.Text, PresetAction);
                    }
                    _photoData.SavePresets();
                    PresetName = PresetNameText.Text;
                }
                else
                {
                    _presetAction =
                    [
                        .. _photoData.SavedPresets[
                            SavedPresetDropdown.SelectedItem?.ToString() ?? string.Empty
                        ],
                    ];
                    PresetName = SavedPresetDropdown.SelectedItem?.ToString() ?? string.Empty;
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                var saving = SaveLoadButton.Text == "Save";
                MessageBox.Show(
                    $"An error occurred while {(saving ? "sav" : "load")}ing the preset: {ex.Message}{Environment.NewLine}No action has been {(saving ? "sav" : "load")}ed.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void CancelButton_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void SavedPresetDropdown_SelectedIndexChanged(object? sender, EventArgs e)
        {
            PresetNameText.TextChanged -= PresetNameText_TextChanged;
            PresetNameText.Text = SavedPresetDropdown.SelectedItem?.ToString() ?? string.Empty;
            PresetNameText.TextChanged += PresetNameText_TextChanged;
        }

        private void PresetNameText_TextChanged(object? sender, EventArgs e)
        {
            SavedPresetDropdown.SelectedItem = null;
        }
    }
}
