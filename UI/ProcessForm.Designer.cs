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
            ConvertCheckbox = new CheckBox();
            ResizeCheckbox = new CheckBox();
            ConvertDropdown = new ComboBox();
            ResizeFixed = new RadioButton();
            ResizeScale = new RadioButton();
            panel1 = new Panel();
            PercentLabel = new Label();
            numericUpDown3 = new NumericUpDown();
            numericUpDown2 = new NumericUpDown();
            xLabel = new Label();
            numericUpDown1 = new NumericUpDown();
            QualityLabel = new Label();
            QualitySpinner = new NumericUpDown();
            CaptionCheckbox = new CheckBox();
            CaptionButton = new Button();
            CaptionTextbox = new TextBox();
            WatermarkCheckbox = new CheckBox();
            WatermarkButton = new Button();
            CancelBtn = new Button();
            OkButton = new Button();
            SaveButton = new Button();
            MetadataCheckbox = new CheckBox();
            FilenameLabel = new Label();
            FilenameLink = new LinkLabel();
            FilenameTextbox = new TextBox();
            ExampleBox = new GroupBox();
            ExampleLabel = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)QualitySpinner).BeginInit();
            ExampleBox.SuspendLayout();
            SuspendLayout();
            // 
            // ConvertCheckbox
            // 
            ConvertCheckbox.AutoSize = true;
            ConvertCheckbox.Location = new Point(12, 12);
            ConvertCheckbox.Name = "ConvertCheckbox";
            ConvertCheckbox.Size = new Size(68, 19);
            ConvertCheckbox.TabIndex = 0;
            ConvertCheckbox.Text = "Convert";
            ConvertCheckbox.UseVisualStyleBackColor = true;
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
            // 
            // ConvertDropdown
            // 
            ConvertDropdown.DropDownStyle = ComboBoxStyle.DropDownList;
            ConvertDropdown.FormattingEnabled = true;
            ConvertDropdown.Items.AddRange(new object[] { "JPEG", "PNG", "HEIC", "WEBP", "BMP" });
            ConvertDropdown.Location = new Point(86, 10);
            ConvertDropdown.Name = "ConvertDropdown";
            ConvertDropdown.Size = new Size(118, 23);
            ConvertDropdown.TabIndex = 2;
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
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.Controls.Add(PercentLabel);
            panel1.Controls.Add(numericUpDown3);
            panel1.Controls.Add(numericUpDown2);
            panel1.Controls.Add(xLabel);
            panel1.Controls.Add(numericUpDown1);
            panel1.Controls.Add(ResizeFixed);
            panel1.Controls.Add(ResizeScale);
            panel1.Location = new Point(12, 55);
            panel1.Name = "panel1";
            panel1.Size = new Size(298, 55);
            panel1.TabIndex = 5;
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
            // numericUpDown3
            // 
            numericUpDown3.Location = new Point(96, 28);
            numericUpDown3.Name = "numericUpDown3";
            numericUpDown3.Size = new Size(47, 23);
            numericUpDown3.TabIndex = 8;
            numericUpDown3.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // numericUpDown2
            // 
            numericUpDown2.Location = new Point(184, 3);
            numericUpDown2.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            numericUpDown2.Name = "numericUpDown2";
            numericUpDown2.Size = new Size(63, 23);
            numericUpDown2.TabIndex = 8;
            // 
            // xLabel
            // 
            xLabel.AutoSize = true;
            xLabel.Location = new Point(165, 7);
            xLabel.Name = "xLabel";
            xLabel.Size = new Size(13, 15);
            xLabel.TabIndex = 8;
            xLabel.Text = "x";
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(96, 3);
            numericUpDown1.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(63, 23);
            numericUpDown1.TabIndex = 5;
            // 
            // QualityLabel
            // 
            QualityLabel.AutoSize = true;
            QualityLabel.Location = new Point(213, 13);
            QualityLabel.Name = "QualityLabel";
            QualityLabel.Size = new Size(48, 15);
            QualityLabel.TabIndex = 6;
            QualityLabel.Text = "Quality:";
            // 
            // QualitySpinner
            // 
            QualitySpinner.Location = new Point(267, 11);
            QualitySpinner.Name = "QualitySpinner";
            QualitySpinner.Size = new Size(43, 23);
            QualitySpinner.TabIndex = 7;
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
            CaptionTextbox.Size = new Size(298, 95);
            CaptionTextbox.TabIndex = 10;
            // 
            // WatermarkCheckbox
            // 
            WatermarkCheckbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            WatermarkCheckbox.AutoSize = true;
            WatermarkCheckbox.Location = new Point(12, 243);
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
            WatermarkButton.Location = new Point(102, 240);
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
            CancelBtn.Location = new Point(238, 496);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(72, 23);
            CancelBtn.TabIndex = 13;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            CancelBtn.Click += CancelBtn_Click;
            // 
            // OkButton
            // 
            OkButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            OkButton.FlatAppearance.BorderSize = 0;
            OkButton.FlatStyle = FlatStyle.Flat;
            OkButton.Location = new Point(157, 496);
            OkButton.Name = "OkButton";
            OkButton.Size = new Size(72, 23);
            OkButton.TabIndex = 14;
            OkButton.Text = "Process";
            OkButton.UseVisualStyleBackColor = true;
            OkButton.Click += OkButton_Click;
            // 
            // SaveButton
            // 
            SaveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            SaveButton.FlatAppearance.BorderSize = 0;
            SaveButton.FlatStyle = FlatStyle.Flat;
            SaveButton.Location = new Point(12, 496);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(84, 23);
            SaveButton.TabIndex = 15;
            SaveButton.Text = "Save Preset";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // MetadataCheckbox
            // 
            MetadataCheckbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MetadataCheckbox.AutoSize = true;
            MetadataCheckbox.Location = new Point(12, 268);
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
            FilenameLabel.Location = new Point(12, 290);
            FilenameLabel.Name = "FilenameLabel";
            FilenameLabel.Size = new Size(138, 15);
            FilenameLabel.TabIndex = 17;
            FilenameLabel.Text = "Output filename pattern:";
            // 
            // FilenameLink
            // 
            FilenameLink.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            FilenameLink.AutoSize = true;
            FilenameLink.Location = new Point(290, 290);
            FilenameLink.Name = "FilenameLink";
            FilenameLink.Size = new Size(20, 15);
            FilenameLink.TabIndex = 18;
            FilenameLink.TabStop = true;
            FilenameLink.Text = "(?)";
            // 
            // FilenameTextbox
            // 
            FilenameTextbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            FilenameTextbox.Location = new Point(12, 308);
            FilenameTextbox.Name = "FilenameTextbox";
            FilenameTextbox.Size = new Size(298, 23);
            FilenameTextbox.TabIndex = 19;
            // 
            // ExampleBox
            // 
            ExampleBox.Controls.Add(ExampleLabel);
            ExampleBox.Location = new Point(12, 337);
            ExampleBox.Name = "ExampleBox";
            ExampleBox.Size = new Size(298, 43);
            ExampleBox.TabIndex = 20;
            ExampleBox.TabStop = false;
            ExampleBox.Text = "Filename Example";
            // 
            // ExampleLabel
            // 
            ExampleLabel.AutoSize = true;
            ExampleLabel.Location = new Point(6, 19);
            ExampleLabel.Name = "ExampleLabel";
            ExampleLabel.Size = new Size(127, 15);
            ExampleLabel.TabIndex = 0;
            ExampleLabel.Text = "Filename Example Text";
            // 
            // ProcessForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(324, 531);
            Controls.Add(ExampleBox);
            Controls.Add(FilenameTextbox);
            Controls.Add(FilenameLink);
            Controls.Add(FilenameLabel);
            Controls.Add(MetadataCheckbox);
            Controls.Add(SaveButton);
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
            Controls.Add(panel1);
            Controls.Add(ConvertDropdown);
            Controls.Add(ConvertCheckbox);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            MinimumSize = new Size(340, 360);
            Name = "ProcessForm";
            Text = "Process Image";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown3).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown2).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)QualitySpinner).EndInit();
            ExampleBox.ResumeLayout(false);
            ExampleBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox ConvertCheckbox;
        private CheckBox ResizeCheckbox;
        private ComboBox ConvertDropdown;
        private RadioButton ResizeFixed;
        private RadioButton ResizeScale;
        private Panel panel1;
        private Label QualityLabel;
        private NumericUpDown QualitySpinner;
        private Label PercentLabel;
        private NumericUpDown numericUpDown3;
        private NumericUpDown numericUpDown2;
        private Label xLabel;
        private NumericUpDown numericUpDown1;
        private CheckBox CaptionCheckbox;
        private Button CaptionButton;
        private TextBox CaptionTextbox;
        private CheckBox WatermarkCheckbox;
        private Button WatermarkButton;
        private Button CancelBtn;
        private Button OkButton;
        private Button SaveButton;
        private CheckBox MetadataCheckbox;
        private Label FilenameLabel;
        private LinkLabel FilenameLink;
        private TextBox FilenameTextbox;
        private GroupBox ExampleBox;
        private Label ExampleLabel;
    }
}