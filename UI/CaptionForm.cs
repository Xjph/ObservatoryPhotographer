namespace Observatory.Photographer.UI
{
    public partial class CaptionForm : Form
    {
        public CaptionAction CaptionAction { get; private set; }

        public CaptionForm(CaptionAction captionAction)
        {
            bool proceed = true;
            CaptionAction = captionAction;
            foreach (Form form in Application.OpenForms)
            {
                if (form is CaptionForm)
                {
                    form.Activate();
                    Close();
                    proceed = false;
                }
            }
            if (proceed)
            {
                InitializeComponent();
                PopulateQuads();
                LoadAction();
            }
        }

        private void LoadAction()
        {
            QuadOrderCheckbox.Checked = CaptionAction.SecondOrder;
            AutoOrderCheckbox.Checked = CaptionAction.SecondOrder;
            LocationQuadDropdown.SelectedIndex = CaptionAction.QuadValue;
            LocationRelativeCheckbox.Checked = CaptionAction.Relative;
            LocationXSpinner.Value = CaptionAction.Location.X;
            LocationYSpinner.Value = CaptionAction.Location.Y;
            switch (CaptionAction.LocationMethod)
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
            FontSampleLabel.Text = CaptionAction.Font.Name;
            FontSampleLabel.Font = CaptionAction.Font;
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
                    "Bottom-Right Of Bottom-Right"
                    ]);
                LocationQuadDropdown.SelectedIndex = prevIndex * 4;
            }
            else
            {
                LocationQuadDropdown.Items.AddRange([
                    "Top-Left",
                    "Top-Right",
                    "Bottom-Left",
                    "Bottom-Right"
                    ]);
                LocationQuadDropdown.SelectedIndex = prevIndex / 4;
            }

        }

        private void FontButton_Click(object sender, EventArgs e)
        {
            var result = FontDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                FontSampleLabel.Text = FontDialog.Font.Name;
                FontSampleLabel.Font = new(FontDialog.Font.FontFamily, FontSampleLabel.Font.Size, FontDialog.Font.Style);
                CaptionAction.Font = FontDialog.Font;
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

            CaptionAction.LocationMethod =
                AutomaticLocationRadio.Checked ? LocationMethod.Automatic :
                LocationQuadRadio.Checked ? LocationMethod.Quadrant :
                LocationMethod.Manual;
        }

        private void QuadOrderCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            PopulateQuads();
            CaptionAction.SecondOrder = QuadOrderCheckbox.Checked;
        }

        private void AutoOrderCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            CaptionAction.SecondOrder = AutoOrderCheckbox.Checked;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LocationRelativeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            LocationXLabel.Text = LocationRelativeCheckbox.Checked ? "%" : "px";
            LocationYLabel.Text = LocationRelativeCheckbox.Checked ? "%" : "px";
            CaptionAction.Relative = LocationRelativeCheckbox.Checked;
        }

        private void LocationQuadDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            CaptionAction.QuadValue = LocationQuadDropdown.SelectedIndex;
        }

        private void LocationXSpinner_ValueChanged(object sender, EventArgs e)
        {
            CaptionAction.Location = new((int)LocationXSpinner.Value, CaptionAction.Location.Y);
        }

        private void LocationYSpinner_ValueChanged(object sender, EventArgs e)
        {
            CaptionAction.Location = new(CaptionAction.Location.X, (int)LocationYSpinner.Value);
        }

        private void FontColourButton_Click(object sender, EventArgs e)
        {
            var colourPicker = new ColorDialog();
            var mColourR = CaptionAction.Color.R;
            var mColourG = CaptionAction.Color.G;
            var mColourB = CaptionAction.Color.B;
            var mColourA = CaptionAction.Color.A;
            colourPicker.Color = Color.FromArgb(mColourA, mColourR, mColourG, mColourB);
            var result = colourPicker.ShowDialog();
            if (result == DialogResult.OK)
            {
                CaptionAction.Color.R = colourPicker.Color.R;
                CaptionAction.Color.G = colourPicker.Color.G;
                CaptionAction.Color.B = colourPicker.Color.B;
                CaptionAction.Color.A = colourPicker.Color.A;
                FontSampleLabel.ForeColor = colourPicker.Color;
            }
        }
    }
}
