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
                    "Bitmap (*.bmp)|*.bmp"
                ];
            var dialog = new OpenFileDialog
            {
                CheckFileExists = true,
                Title = "Select Watermark Image",
                Filter = string.Join('|', fileTypes)
            };
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                PathTextbox.Text = dialog.FileName;
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
                AutomaticLocationRadio.Checked ? LocationMethod.Automatic :
                LocationQuadRadio.Checked ? LocationMethod.Quadrant :
                LocationMethod.Manual;
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

        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
