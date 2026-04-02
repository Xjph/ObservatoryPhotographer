namespace Observatory.Photographer.UI
{
    public partial class WatermarkForm : Form
    {
        public WatermarkAction WatermarkAction { get; private set; }

        public WatermarkForm(WatermarkAction watermarkAction)
        {
            bool proceed = true;
            foreach (Form form in Application.OpenForms)
            {
                if (form is WatermarkForm)
                {
                    form.Activate();
                    Close();
                    proceed = false;
                }
            }
            if (proceed)
            {
                WatermarkAction = watermarkAction;
                InitializeComponent();

                // Override theme
                // Using text color to indicate good path
                PathTextbox.BackColor = Color.White;
                PopulateQuads();
                LoadAction();
            }
        }

        private void LoadAction()
        {
            PathTextbox.Text = WatermarkAction.WatermarkImagePath;
            QuadOrderCheckbox.Checked = WatermarkAction.SecondOrder;
            AutoOrderCheckbox.Checked = WatermarkAction.SecondOrder;
            LocationQuadDropdown.SelectedIndex = WatermarkAction.QuadValue;
            LocationRelativeCheckbox.Checked = WatermarkAction.Relative;
            LocationXSpinner.Value = WatermarkAction.Location.X;
            LocationYSpinner.Value = WatermarkAction.Location.Y;
            switch (WatermarkAction.LocationMethod)
            {
                case LocationMethod.Automatic:
                    AutomaticLocationRadio.Checked = true;
                    break;
                case LocationMethod.Quadrant:
                    LocationQuadRadio.Checked = true;
                    break;
                case LocationMethod.Manual:
                    LocationSpecificRadio.Checked = true;
                    break;
            }
        }

        private void PopulateQuads()
        {
            LocationQuadDropdown.Items.Clear();
            int prevIndex = LocationQuadDropdown.SelectedIndex;
            if (QuadOrderCheckbox.Checked)
            {
                LocationQuadDropdown.Items.AddRange([
                    "Top-Left Of Top-Left",
                    "Top-Right Of Top-Left",
                    "Bottom-Left Of Top-Left",
                    "Bottom-Right Of Top-Left",
                    "Top-Left Of Top-Right",
                    "Top-Right Of Top-Right",
                    "Bottom-Left Of Top-Right",
                    "Bottom-Right Of Top-Right",
                    "Top-Left Of Bottom-Left",
                    "Top-Right Of Bottom-Left",
                    "Bottom-Left Of Bottom-Left",
                    "Bottom-Right Of Bottom-Left",
                    "Top-Left Of Bottom-Right",
                    "Top-Right Of Bottom-Right",
                    "Bottom-Left Of Bottom-Right",
                    "Bottom-Right Of Bottom-Right",
                ]);
                LocationQuadDropdown.SelectedIndex = prevIndex * 4;
            }
            else
            {
                LocationQuadDropdown.Items.AddRange([
                    "Top-Left",
                    "Top-Right",
                    "Bottom-Left",
                    "Bottom-Right",
                ]);
                LocationQuadDropdown.SelectedIndex = prevIndex / 4;
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            string[] fileTypes =
            [
                "Image Types|*.png;*.jpg;*.jpeg;*.gif;*.webp;*.heic;*.avif;*.bmp",
                "PNG (*.png)|*.png",
                "JPEG (*.jpg, *.jpeg)|*.jpg;*.jpeg",
                "GIF (*.gif)|*.gif",
                "WebP (*.webp)|*.webp",
                "HEIC (*.heic)|*.heic",
                "AVIF (*.avif)|*.avif",
                "Bitmap (*.bmp)|*.bmp",
            ];
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Title = "Select Watermark Image",
                Filter = string.Join('|', fileTypes),
            };
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                PathTextbox.Text = dialog.FileName;
                WatermarkAction.WatermarkImagePath = dialog.FileName;
            }
        }

        private void LocationRadioChanged(object sender, EventArgs e)
        {
            AutoOrderCheckbox.Enabled = AutomaticLocationRadio.Checked;
            LocationQuadDropdown.Enabled = LocationQuadRadio.Checked;
            QuadOrderCheckbox.Enabled = LocationQuadRadio.Checked;
            LocationXSpinner.Enabled = LocationSpecificRadio.Checked;
            LocationYSpinner.Enabled = LocationSpecificRadio.Checked;
            LocationRelativeCheckbox.Enabled = LocationSpecificRadio.Checked;

            WatermarkAction.LocationMethod =
                AutomaticLocationRadio.Checked ? LocationMethod.Automatic
                : LocationQuadRadio.Checked ? LocationMethod.Quadrant
                : LocationMethod.Manual;
        }

        private void QuadOrderCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            PopulateQuads();
            WatermarkAction.SecondOrder = QuadOrderCheckbox.Checked;
        }

        private void AutoOrderCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            WatermarkAction.SecondOrder = AutoOrderCheckbox.Checked;
        }

        private void PathTextbox_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists(PathTextbox.Text))
            {
                WatermarkAction.WatermarkImagePath = PathTextbox.Text;
                PathTextbox.Font = new Font(PathTextbox.Font, FontStyle.Regular);
                PathTextbox.ForeColor = Color.Black;
            }
            else
            {
                PathTextbox.Font = new Font(PathTextbox.Font, FontStyle.Bold);
                PathTextbox.ForeColor = Color.Red;
            }
        }

        private void LocationRelativeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            LocationXLabel.Text = LocationRelativeCheckbox.Checked ? "%" : "px";
            LocationYLabel.Text = LocationRelativeCheckbox.Checked ? "%" : "px";
            WatermarkAction.Relative = LocationRelativeCheckbox.Checked;
        }

        private void LocationQuadDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            WatermarkAction.QuadValue = LocationQuadDropdown.SelectedIndex;
        }

        private void LocationXSpinner_ValueChanged(object sender, EventArgs e)
        {
            WatermarkAction.Location = new((int)LocationXSpinner.Value, WatermarkAction.Location.Y);
        }

        private void LocationYSpinner_ValueChanged(object sender, EventArgs e)
        {
            WatermarkAction.Location = new(WatermarkAction.Location.X, (int)LocationYSpinner.Value);
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
