namespace Observatory.Photographer.UI
{
    partial class CaptionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            FontLabel = new Label();
            FontDialog = new FontDialog();
            FontSampleLabel = new Label();
            FontButton = new Button();
            LocationGroup = new GroupBox();
            LocationRelativeCheckbox = new CheckBox();
            LocationYLabel = new Label();
            LocationXLabel = new Label();
            LocationYSpinner = new NumericUpDown();
            LocationXSpinner = new NumericUpDown();
            LocationSpecificRadio = new RadioButton();
            QuadOrderCheckbox = new CheckBox();
            LocationQuadDropdown = new ComboBox();
            LocationQuadRadio = new RadioButton();
            AutoOrderCheckbox = new CheckBox();
            AutomaticLocationRadio = new RadioButton();
            FontColourButton = new Button();
            OkButton = new Button();
            LocationGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)LocationYSpinner).BeginInit();
            ((System.ComponentModel.ISupportInitialize)LocationXSpinner).BeginInit();
            SuspendLayout();
            // 
            // FontLabel
            // 
            FontLabel.AutoSize = true;
            FontLabel.Location = new Point(12, 9);
            FontLabel.Name = "FontLabel";
            FontLabel.Size = new Size(34, 15);
            FontLabel.TabIndex = 0;
            FontLabel.Text = "Font:";
            // 
            // FontSampleLabel
            // 
            FontSampleLabel.AutoSize = true;
            FontSampleLabel.Location = new Point(52, 9);
            FontSampleLabel.Name = "FontSampleLabel";
            FontSampleLabel.Size = new Size(73, 15);
            FontSampleLabel.TabIndex = 1;
            FontSampleLabel.Text = "Font Sample";
            // 
            // FontButton
            // 
            FontButton.FlatAppearance.BorderSize = 0;
            FontButton.FlatStyle = FlatStyle.Flat;
            FontButton.Location = new Point(12, 27);
            FontButton.Name = "FontButton";
            FontButton.Size = new Size(92, 23);
            FontButton.TabIndex = 2;
            FontButton.Text = "Change Font";
            FontButton.UseVisualStyleBackColor = true;
            FontButton.Click += FontButton_Click;
            // 
            // LocationGroup
            // 
            LocationGroup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            LocationGroup.Controls.Add(LocationRelativeCheckbox);
            LocationGroup.Controls.Add(LocationYLabel);
            LocationGroup.Controls.Add(LocationXLabel);
            LocationGroup.Controls.Add(LocationYSpinner);
            LocationGroup.Controls.Add(LocationXSpinner);
            LocationGroup.Controls.Add(LocationSpecificRadio);
            LocationGroup.Controls.Add(QuadOrderCheckbox);
            LocationGroup.Controls.Add(LocationQuadDropdown);
            LocationGroup.Controls.Add(LocationQuadRadio);
            LocationGroup.Controls.Add(AutoOrderCheckbox);
            LocationGroup.Controls.Add(AutomaticLocationRadio);
            LocationGroup.Location = new Point(12, 56);
            LocationGroup.Name = "LocationGroup";
            LocationGroup.Size = new Size(239, 271);
            LocationGroup.TabIndex = 3;
            LocationGroup.TabStop = false;
            LocationGroup.Text = "Location";
            // 
            // LocationRelativeCheckbox
            // 
            LocationRelativeCheckbox.AutoSize = true;
            LocationRelativeCheckbox.Location = new Point(25, 234);
            LocationRelativeCheckbox.Name = "LocationRelativeCheckbox";
            LocationRelativeCheckbox.Size = new Size(67, 19);
            LocationRelativeCheckbox.TabIndex = 11;
            LocationRelativeCheckbox.Text = "Relative";
            LocationRelativeCheckbox.UseVisualStyleBackColor = true;
            LocationRelativeCheckbox.CheckedChanged += LocationRelativeCheckbox_CheckedChanged;
            // 
            // LocationYLabel
            // 
            LocationYLabel.AutoSize = true;
            LocationYLabel.Location = new Point(98, 207);
            LocationYLabel.Name = "LocationYLabel";
            LocationYLabel.Size = new Size(13, 15);
            LocationYLabel.TabIndex = 10;
            LocationYLabel.Text = "y";
            // 
            // LocationXLabel
            // 
            LocationXLabel.AutoSize = true;
            LocationXLabel.Location = new Point(98, 178);
            LocationXLabel.Name = "LocationXLabel";
            LocationXLabel.Size = new Size(12, 15);
            LocationXLabel.TabIndex = 9;
            LocationXLabel.Text = "x";
            // 
            // LocationYSpinner
            // 
            LocationYSpinner.Location = new Point(25, 205);
            LocationYSpinner.Name = "LocationYSpinner";
            LocationYSpinner.Size = new Size(67, 23);
            LocationYSpinner.TabIndex = 8;
            LocationYSpinner.ValueChanged += LocationYSpinner_ValueChanged;
            // 
            // LocationXSpinner
            // 
            LocationXSpinner.Location = new Point(25, 176);
            LocationXSpinner.Name = "LocationXSpinner";
            LocationXSpinner.Size = new Size(67, 23);
            LocationXSpinner.TabIndex = 7;
            LocationXSpinner.ValueChanged += LocationXSpinner_ValueChanged;
            // 
            // LocationSpecificRadio
            // 
            LocationSpecificRadio.AutoSize = true;
            LocationSpecificRadio.Location = new Point(6, 151);
            LocationSpecificRadio.Name = "LocationSpecificRadio";
            LocationSpecificRadio.Size = new Size(114, 19);
            LocationSpecificRadio.TabIndex = 6;
            LocationSpecificRadio.TabStop = true;
            LocationSpecificRadio.Text = "Manual Location";
            LocationSpecificRadio.UseVisualStyleBackColor = true;
            LocationSpecificRadio.CheckedChanged += LocationRadioChanged;
            // 
            // QuadOrderCheckbox
            // 
            QuadOrderCheckbox.AutoSize = true;
            QuadOrderCheckbox.Location = new Point(24, 126);
            QuadOrderCheckbox.Name = "QuadOrderCheckbox";
            QuadOrderCheckbox.Size = new Size(98, 19);
            QuadOrderCheckbox.TabIndex = 5;
            QuadOrderCheckbox.Text = "Second Order";
            QuadOrderCheckbox.UseVisualStyleBackColor = true;
            QuadOrderCheckbox.CheckedChanged += QuadOrderCheckbox_CheckedChanged;
            // 
            // LocationQuadDropdown
            // 
            LocationQuadDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            LocationQuadDropdown.FormattingEnabled = true;
            LocationQuadDropdown.Location = new Point(24, 97);
            LocationQuadDropdown.Name = "LocationQuadDropdown";
            LocationQuadDropdown.Size = new Size(205, 23);
            LocationQuadDropdown.TabIndex = 4;
            LocationQuadDropdown.SelectedIndexChanged += LocationQuadDropdown_SelectedIndexChanged;
            // 
            // LocationQuadRadio
            // 
            LocationQuadRadio.AutoSize = true;
            LocationQuadRadio.Location = new Point(6, 72);
            LocationQuadRadio.Name = "LocationQuadRadio";
            LocationQuadRadio.Size = new Size(105, 19);
            LocationQuadRadio.TabIndex = 3;
            LocationQuadRadio.TabStop = true;
            LocationQuadRadio.Text = "Fixed Quadrant";
            LocationQuadRadio.UseVisualStyleBackColor = true;
            LocationQuadRadio.CheckedChanged += LocationRadioChanged;
            // 
            // AutoOrderCheckbox
            // 
            AutoOrderCheckbox.AutoSize = true;
            AutoOrderCheckbox.Location = new Point(24, 47);
            AutoOrderCheckbox.Name = "AutoOrderCheckbox";
            AutoOrderCheckbox.Size = new Size(74, 19);
            AutoOrderCheckbox.TabIndex = 2;
            AutoOrderCheckbox.Text = "Two Pass";
            AutoOrderCheckbox.UseVisualStyleBackColor = true;
            AutoOrderCheckbox.CheckedChanged += AutoOrderCheckbox_CheckedChanged;
            // 
            // AutomaticLocationRadio
            // 
            AutomaticLocationRadio.AutoSize = true;
            AutomaticLocationRadio.Location = new Point(6, 22);
            AutomaticLocationRadio.Name = "AutomaticLocationRadio";
            AutomaticLocationRadio.Size = new Size(223, 19);
            AutomaticLocationRadio.TabIndex = 0;
            AutomaticLocationRadio.TabStop = true;
            AutomaticLocationRadio.Text = "Automatically Determine Open Space";
            AutomaticLocationRadio.UseVisualStyleBackColor = true;
            AutomaticLocationRadio.CheckedChanged += LocationRadioChanged;
            // 
            // FontColourButton
            // 
            FontColourButton.FlatAppearance.BorderSize = 0;
            FontColourButton.FlatStyle = FlatStyle.Flat;
            FontColourButton.Location = new Point(110, 27);
            FontColourButton.Name = "FontColourButton";
            FontColourButton.Size = new Size(102, 23);
            FontColourButton.TabIndex = 4;
            FontColourButton.Text = "Change Colour";
            FontColourButton.UseVisualStyleBackColor = true;
            FontColourButton.Click += FontColourButton_Click;
            // 
            // OkButton
            // 
            OkButton.FlatAppearance.BorderSize = 0;
            OkButton.FlatStyle = FlatStyle.Flat;
            OkButton.Location = new Point(176, 333);
            OkButton.Name = "OkButton";
            OkButton.Size = new Size(75, 23);
            OkButton.TabIndex = 5;
            OkButton.Text = "OK";
            OkButton.UseVisualStyleBackColor = true;
            OkButton.Click += OkButton_Click;
            // 
            // CaptionForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(263, 370);
            Controls.Add(OkButton);
            Controls.Add(FontColourButton);
            Controls.Add(LocationGroup);
            Controls.Add(FontButton);
            Controls.Add(FontSampleLabel);
            Controls.Add(FontLabel);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "CaptionForm";
            Text = "Caption Setup";
            LocationGroup.ResumeLayout(false);
            LocationGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)LocationYSpinner).EndInit();
            ((System.ComponentModel.ISupportInitialize)LocationXSpinner).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label FontLabel;
        private FontDialog FontDialog;
        private Label FontSampleLabel;
        private Button FontButton;
        private GroupBox LocationGroup;
        private RadioButton AutomaticLocationRadio;
        private CheckBox AutoOrderCheckbox;
        private CheckBox QuadOrderCheckbox;
        private ComboBox LocationQuadDropdown;
        private RadioButton LocationQuadRadio;
        private NumericUpDown LocationYSpinner;
        private NumericUpDown LocationXSpinner;
        private RadioButton LocationSpecificRadio;
        private CheckBox LocationRelativeCheckbox;
        private Label LocationYLabel;
        private Label LocationXLabel;
        private Button FontColourButton;
        private Button OkButton;
    }
}