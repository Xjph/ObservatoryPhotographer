namespace Observatory.Photographer.UI
{
    partial class ProcessForm
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
            ResizeCheckbox = new CheckBox();
            ConvertDropdown = new ComboBox();
            ResizeFixed = new RadioButton();
            ResizeScale = new RadioButton();
            ResizePanel = new Panel();
            PercentLabel = new Label();
            ResizePercentSpinner = new NumericUpDown();
            ResizeYSpinner = new NumericUpDown();
            xLabel = new Label();
            ResizeXSpinner = new NumericUpDown();
            QualityLabel = new Label();
            QualitySpinner = new NumericUpDown();
            CaptionCheckbox = new CheckBox();
            CaptionButton = new Button();
            CaptionTextbox = new TextBox();
            WatermarkCheckbox = new CheckBox();
            WatermarkButton = new Button();
            CancelBtn = new Button();
            OkButton = new Button();
            SetDefaultButton = new Button();
            MetadataCheckbox = new CheckBox();
            FilenameLabel = new Label();
            FilenameLink = new LinkLabel();
            FilenameTextbox = new TextBox();
            ExampleBox = new GroupBox();
            ExampleLabel = new Label();
            FormatLabel = new Label();
            ProcessingLabel = new Label();
            LoadPresetButton = new Button();
            SavePresetButton = new Button();
            ResizePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ResizePercentSpinner).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ResizeYSpinner).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ResizeXSpinner).BeginInit();
            ((System.ComponentModel.ISupportInitialize)QualitySpinner).BeginInit();
            ExampleBox.SuspendLayout();
            SuspendLayout();
            // 
            // ResizeCheckbox
            // 
            ResizeCheckbox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ResizeCheckbox.AutoSize = true;
            ResizeCheckbox.Location = new Point(12, 37);
            ResizeCheckbox.Name = "ResizeCheckbox";
            ResizeCheckbox.Size = new Size(58, 19);
            ResizeCheckbox.TabIndex = 1;
            ResizeCheckbox.Text = "Resize";
            ResizeCheckbox.UseVisualStyleBackColor = true;
            ResizeCheckbox.CheckedChanged += ResizeCheckbox_CheckedChanged;
            // 
            // ConvertDropdown
            // 
            ConvertDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            ConvertDropdown.FormattingEnabled = true;
            ConvertDropdown.Items.AddRange(new object[] { "JPEG", "PNG", "HEIC", "WEBP", "BMP" });
            ConvertDropdown.Location = new Point(63, 10);
            ConvertDropdown.Name = "ConvertDropdown";
            ConvertDropdown.Size = new Size(126, 23);
            ConvertDropdown.TabIndex = 2;
            ConvertDropdown.SelectedIndexChanged += ConvertDropdown_SelectedIndexChanged;
            // 
            // ResizeFixed
            // 
            ResizeFixed.AutoSize = true;
            ResizeFixed.Location = new Point(3, 3);
            ResizeFixed.Name = "ResizeFixed";
            ResizeFixed.Size = new Size(87, 19);
            ResizeFixed.TabIndex = 3;
            ResizeFixed.TabStop = true;
            ResizeFixed.Text = "Dimensions";
            ResizeFixed.UseVisualStyleBackColor = true;
            ResizeFixed.CheckedChanged += ResizeFixed_CheckedChanged;
            // 
            // ResizeScale
            // 
            ResizeScale.AutoSize = true;
            ResizeScale.Location = new Point(3, 28);
            ResizeScale.Name = "ResizeScale";
            ResizeScale.Size = new Size(65, 19);
            ResizeScale.TabIndex = 4;
            ResizeScale.TabStop = true;
            ResizeScale.Text = "Percent";
            ResizeScale.UseVisualStyleBackColor = true;
            ResizeScale.CheckedChanged += ResizeScale_CheckedChanged;
            // 
            // ResizePanel
            // 
            ResizePanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ResizePanel.Controls.Add(PercentLabel);
            ResizePanel.Controls.Add(ResizePercentSpinner);
            ResizePanel.Controls.Add(ResizeYSpinner);
            ResizePanel.Controls.Add(xLabel);
            ResizePanel.Controls.Add(ResizeXSpinner);
            ResizePanel.Controls.Add(ResizeFixed);
            ResizePanel.Controls.Add(ResizeScale);
            ResizePanel.Location = new Point(12, 55);
            ResizePanel.Name = "ResizePanel";
            ResizePanel.Size = new Size(298, 55);
            ResizePanel.TabIndex = 5;
            // 
            // PercentLabel
            // 
            PercentLabel.AutoSize = true;
            PercentLabel.Location = new Point(142, 30);
            PercentLabel.Name = "PercentLabel";
            PercentLabel.Size = new Size(17, 15);
            PercentLabel.TabIndex = 8;
            PercentLabel.Text = "%";
            // 
            // ResizePercentSpinner
            // 
            ResizePercentSpinner.Location = new Point(96, 28);
            ResizePercentSpinner.Name = "ResizePercentSpinner";
            ResizePercentSpinner.Size = new Size(47, 23);
            ResizePercentSpinner.TabIndex = 8;
            ResizePercentSpinner.Value = new decimal(new int[] { 100, 0, 0, 0 });
            ResizePercentSpinner.ValueChanged += ResizePercentSpinner_ValueChanged;
            // 
            // ResizeYSpinner
            // 
            ResizeYSpinner.Location = new Point(184, 3);
            ResizeYSpinner.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            ResizeYSpinner.Name = "ResizeYSpinner";
            ResizeYSpinner.Size = new Size(63, 23);
            ResizeYSpinner.TabIndex = 8;
            ResizeYSpinner.ValueChanged += ResizeYSpinner_ValueChanged;
            // 
            // xLabel
            // 
            xLabel.AutoSize = true;
            xLabel.Location = new Point(165, 7);
            xLabel.Name = "xLabel";
            xLabel.Size = new Size(12, 15);
            xLabel.TabIndex = 8;
            xLabel.Text = "x";
            // 
            // ResizeXSpinner
            // 
            ResizeXSpinner.Location = new Point(96, 3);
            ResizeXSpinner.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            ResizeXSpinner.Name = "ResizeXSpinner";
            ResizeXSpinner.Size = new Size(63, 23);
            ResizeXSpinner.TabIndex = 5;
            ResizeXSpinner.ValueChanged += ResizeXSpinner_ValueChanged;
            // 
            // QualityLabel
            // 
            QualityLabel.AutoSize = true;
            QualityLabel.Location = new Point(196, 13);
            QualityLabel.Name = "QualityLabel";
            QualityLabel.Size = new Size(48, 15);
            QualityLabel.TabIndex = 6;
            QualityLabel.Text = "Quality:";
            // 
            // QualitySpinner
            // 
            QualitySpinner.Location = new Point(250, 11);
            QualitySpinner.Name = "QualitySpinner";
            QualitySpinner.Size = new Size(60, 23);
            QualitySpinner.TabIndex = 7;
            QualitySpinner.ValueChanged += QualitySpinner_ValueChanged;
            // 
            // CaptionCheckbox
            // 
            CaptionCheckbox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            CaptionCheckbox.AutoSize = true;
            CaptionCheckbox.Location = new Point(12, 116);
            CaptionCheckbox.Name = "CaptionCheckbox";
            CaptionCheckbox.Size = new Size(68, 19);
            CaptionCheckbox.TabIndex = 8;
            CaptionCheckbox.Text = "Caption";
            CaptionCheckbox.UseVisualStyleBackColor = true;
            // 
            // CaptionButton
            // 
            CaptionButton.FlatAppearance.BorderSize = 0;
            CaptionButton.FlatStyle = FlatStyle.Flat;
            CaptionButton.Location = new Point(86, 113);
            CaptionButton.Name = "CaptionButton";
            CaptionButton.Size = new Size(72, 23);
            CaptionButton.TabIndex = 9;
            CaptionButton.Text = "Configure";
            CaptionButton.UseVisualStyleBackColor = true;
            CaptionButton.Click += CaptionButton_Click;
            // 
            // CaptionTextbox
            // 
            CaptionTextbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            CaptionTextbox.Location = new Point(12, 142);
            CaptionTextbox.Multiline = true;
            CaptionTextbox.Name = "CaptionTextbox";
            CaptionTextbox.Size = new Size(298, 108);
            CaptionTextbox.TabIndex = 10;
            CaptionTextbox.TextChanged += CaptionTextbox_TextChanged;
            // 
            // WatermarkCheckbox
            // 
            WatermarkCheckbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            WatermarkCheckbox.AutoSize = true;
            WatermarkCheckbox.Location = new Point(12, 259);
            WatermarkCheckbox.Name = "WatermarkCheckbox";
            WatermarkCheckbox.Size = new Size(84, 19);
            WatermarkCheckbox.TabIndex = 11;
            WatermarkCheckbox.Text = "Watermark";
            WatermarkCheckbox.UseVisualStyleBackColor = true;
            // 
            // WatermarkButton
            // 
            WatermarkButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            WatermarkButton.FlatAppearance.BorderSize = 0;
            WatermarkButton.FlatStyle = FlatStyle.Flat;
            WatermarkButton.Location = new Point(102, 256);
            WatermarkButton.Name = "WatermarkButton";
            WatermarkButton.Size = new Size(72, 23);
            WatermarkButton.TabIndex = 12;
            WatermarkButton.Text = "Configure";
            WatermarkButton.UseVisualStyleBackColor = true;
            WatermarkButton.Click += WatermarkButton_Click;
            // 
            // CancelBtn
            // 
            CancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            CancelBtn.FlatAppearance.BorderSize = 0;
            CancelBtn.FlatStyle = FlatStyle.Flat;
            CancelBtn.Location = new Point(238, 433);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(72, 23);
            CancelBtn.TabIndex = 13;
            CancelBtn.Text = "Close";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // OkButton
            // 
            OkButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            OkButton.FlatAppearance.BorderSize = 0;
            OkButton.FlatStyle = FlatStyle.Flat;
            OkButton.Location = new Point(157, 433);
            OkButton.Name = "OkButton";
            OkButton.Size = new Size(72, 23);
            OkButton.TabIndex = 14;
            OkButton.Text = "Process";
            OkButton.UseVisualStyleBackColor = true;
            OkButton.Click += OkButton_Click;
            // 
            // SetDefaultButton
            // 
            SetDefaultButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            SetDefaultButton.FlatAppearance.BorderSize = 0;
            SetDefaultButton.FlatStyle = FlatStyle.Flat;
            SetDefaultButton.Location = new Point(12, 433);
            SetDefaultButton.Name = "SetDefaultButton";
            SetDefaultButton.Size = new Size(84, 23);
            SetDefaultButton.TabIndex = 15;
            SetDefaultButton.Text = "Set Default";
            SetDefaultButton.UseVisualStyleBackColor = true;
            SetDefaultButton.Click += SetDefaultButton_Click;
            // 
            // MetadataCheckbox
            // 
            MetadataCheckbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MetadataCheckbox.AutoSize = true;
            MetadataCheckbox.Location = new Point(12, 284);
            MetadataCheckbox.Name = "MetadataCheckbox";
            MetadataCheckbox.Size = new Size(116, 19);
            MetadataCheckbox.TabIndex = 16;
            MetadataCheckbox.Text = "Embed Metadata";
            MetadataCheckbox.UseVisualStyleBackColor = true;
            // 
            // FilenameLabel
            // 
            FilenameLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            FilenameLabel.AutoSize = true;
            FilenameLabel.Location = new Point(12, 306);
            FilenameLabel.Name = "FilenameLabel";
            FilenameLabel.Size = new Size(138, 15);
            FilenameLabel.TabIndex = 17;
            FilenameLabel.Text = "Output filename pattern:";
            // 
            // FilenameLink
            // 
            FilenameLink.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            FilenameLink.AutoSize = true;
            FilenameLink.Location = new Point(290, 306);
            FilenameLink.Name = "FilenameLink";
            FilenameLink.Size = new Size(20, 15);
            FilenameLink.TabIndex = 18;
            FilenameLink.TabStop = true;
            FilenameLink.Text = "(?)";
            FilenameLink.LinkClicked += FilenameLink_LinkClicked;
            // 
            // FilenameTextbox
            // 
            FilenameTextbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            FilenameTextbox.Location = new Point(12, 324);
            FilenameTextbox.Name = "FilenameTextbox";
            FilenameTextbox.Size = new Size(298, 23);
            FilenameTextbox.TabIndex = 19;
            FilenameTextbox.TextChanged += FilenameTextbox_TextChanged;
            // 
            // ExampleBox
            // 
            ExampleBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ExampleBox.Controls.Add(ExampleLabel);
            ExampleBox.Location = new Point(12, 353);
            ExampleBox.Name = "ExampleBox";
            ExampleBox.Size = new Size(298, 43);
            ExampleBox.TabIndex = 20;
            ExampleBox.TabStop = false;
            ExampleBox.Text = "Filename Example";
            // 
            // ExampleLabel
            // 
            ExampleLabel.Location = new Point(6, 19);
            ExampleLabel.Name = "ExampleLabel";
            ExampleLabel.Size = new Size(286, 15);
            ExampleLabel.TabIndex = 0;
            ExampleLabel.Text = "Filename Example Text";
            // 
            // FormatLabel
            // 
            FormatLabel.AutoSize = true;
            FormatLabel.Location = new Point(12, 13);
            FormatLabel.Name = "FormatLabel";
            FormatLabel.Size = new Size(45, 15);
            FormatLabel.TabIndex = 21;
            FormatLabel.Text = "Format";
            // 
            // ProcessingLabel
            // 
            ProcessingLabel.BorderStyle = BorderStyle.FixedSingle;
            ProcessingLabel.Dock = DockStyle.Fill;
            ProcessingLabel.Enabled = false;
            ProcessingLabel.Location = new Point(0, 0);
            ProcessingLabel.Name = "ProcessingLabel";
            ProcessingLabel.Size = new Size(324, 468);
            ProcessingLabel.TabIndex = 22;
            ProcessingLabel.Text = "Processing...";
            ProcessingLabel.TextAlign = ContentAlignment.MiddleCenter;
            ProcessingLabel.Visible = false;
            // 
            // LoadPresetButton
            // 
            LoadPresetButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            LoadPresetButton.FlatAppearance.BorderSize = 0;
            LoadPresetButton.FlatStyle = FlatStyle.Flat;
            LoadPresetButton.Location = new Point(12, 404);
            LoadPresetButton.Name = "LoadPresetButton";
            LoadPresetButton.Size = new Size(84, 23);
            LoadPresetButton.TabIndex = 23;
            LoadPresetButton.Text = "Load Preset";
            LoadPresetButton.UseVisualStyleBackColor = true;
            LoadPresetButton.Click += LoadPresetButton_Click;
            // 
            // SavePresetButton
            // 
            SavePresetButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            SavePresetButton.FlatAppearance.BorderSize = 0;
            SavePresetButton.FlatStyle = FlatStyle.Flat;
            SavePresetButton.Location = new Point(102, 404);
            SavePresetButton.Name = "SavePresetButton";
            SavePresetButton.Size = new Size(84, 23);
            SavePresetButton.TabIndex = 24;
            SavePresetButton.Text = "Save Preset";
            SavePresetButton.UseVisualStyleBackColor = true;
            SavePresetButton.Click += SavePresetButton_Click;
            // 
            // ProcessForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(324, 468);
            Controls.Add(SavePresetButton);
            Controls.Add(LoadPresetButton);
            Controls.Add(FormatLabel);
            Controls.Add(ExampleBox);
            Controls.Add(FilenameTextbox);
            Controls.Add(FilenameLink);
            Controls.Add(FilenameLabel);
            Controls.Add(MetadataCheckbox);
            Controls.Add(SetDefaultButton);
            Controls.Add(OkButton);
            Controls.Add(CancelBtn);
            Controls.Add(WatermarkButton);
            Controls.Add(WatermarkCheckbox);
            Controls.Add(CaptionTextbox);
            Controls.Add(CaptionButton);
            Controls.Add(CaptionCheckbox);
            Controls.Add(QualitySpinner);
            Controls.Add(QualityLabel);
            Controls.Add(ResizeCheckbox);
            Controls.Add(ResizePanel);
            Controls.Add(ConvertDropdown);
            Controls.Add(ProcessingLabel);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            MinimumSize = new Size(340, 360);
            Name = "ProcessForm";
            Text = "Process Image";
            ResizePanel.ResumeLayout(false);
            ResizePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ResizePercentSpinner).EndInit();
            ((System.ComponentModel.ISupportInitialize)ResizeYSpinner).EndInit();
            ((System.ComponentModel.ISupportInitialize)ResizeXSpinner).EndInit();
            ((System.ComponentModel.ISupportInitialize)QualitySpinner).EndInit();
            ExampleBox.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private CheckBox ResizeCheckbox;
        private ComboBox ConvertDropdown;
        private RadioButton ResizeFixed;
        private RadioButton ResizeScale;
        private Panel ResizePanel;
        private Label QualityLabel;
        private NumericUpDown QualitySpinner;
        private Label PercentLabel;
        private NumericUpDown ResizePercentSpinner;
        private NumericUpDown ResizeYSpinner;
        private Label xLabel;
        private NumericUpDown ResizeXSpinner;
        private CheckBox CaptionCheckbox;
        private Button CaptionButton;
        private TextBox CaptionTextbox;
        private CheckBox WatermarkCheckbox;
        private Button WatermarkButton;
        private Button CancelBtn;
        private Button OkButton;
        private Button SetDefaultButton;
        private CheckBox MetadataCheckbox;
        private Label FilenameLabel;
        private LinkLabel FilenameLink;
        private TextBox FilenameTextbox;
        private GroupBox ExampleBox;
        private Label ExampleLabel;
        private Label FormatLabel;
        private Label ProcessingLabel;
        private Button LoadPresetButton;
        private Button SavePresetButton;
    }
}