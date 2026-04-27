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
                    var presetName = SavedPresetDropdown.Text;
                    if (_photoData.SavedPresets.ContainsKey(presetName))
                    {
                        var result = MessageBox.Show(
                            $"A preset named \"{presetName}\" already exists. Do you want to overwrite?",
                            "Confirm Overwrite",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning
                        );
                        if (result == DialogResult.No)
                        {
                            return;
                        }
                        _photoData.SavedPresets[presetName] = PresetAction;
                    }
                    else
                    {
                        _photoData.SavedPresets.Add(presetName, PresetAction);
                    }
                    _photoData.SavePresets();
                    PresetName = presetName;
                }
                else
                {
                    _presetAction =
                    [
                        .. _photoData.SavedPresets[
                            SavedPresetDropdown.Text
                        ],
                    ];
                    PresetName = SavedPresetDropdown.Text;
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

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                var presetName = SavedPresetDropdown.Text;
                if (_photoData.SavedPresets.ContainsKey(presetName))
                {
                    var result = MessageBox.Show(
                        $"Are you sure you want to delete the \"{presetName}\" preset?",
                        "Confirm Delete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );
                    if (result == DialogResult.No)
                    {
                        return;
                    }
                    _photoData.SavedPresets.Remove(presetName);
                    _photoData.SavePresets();
                    SavedPresetDropdown.Items.Remove(presetName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred while deleting the preset: {ex.Message}{Environment.NewLine}No preset has been deleted.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
