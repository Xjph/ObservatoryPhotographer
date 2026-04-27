namespace Observatory.Photographer.UI
{
    partial class PresetForm
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
            SavedPresetLabel = new Label();
            SavedPresetDropdown = new ComboBox();
            CancelButton = new Button();
            SaveLoadButton = new Button();
            DeleteButton = new Button();
            SuspendLayout();
            // 
            // SavedPresetLabel
            // 
            SavedPresetLabel.AutoSize = true;
            SavedPresetLabel.Location = new Point(12, 9);
            SavedPresetLabel.Name = "SavedPresetLabel";
            SavedPresetLabel.Size = new Size(81, 15);
            SavedPresetLabel.TabIndex = 0;
            SavedPresetLabel.Text = "Saved Presets:";
            // 
            // SavedPresetDropdown
            // 
            SavedPresetDropdown.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            SavedPresetDropdown.FormattingEnabled = true;
            SavedPresetDropdown.Location = new Point(115, 6);
            SavedPresetDropdown.Name = "SavedPresetDropdown";
            SavedPresetDropdown.Size = new Size(216, 23);
            SavedPresetDropdown.TabIndex = 1;
            // 
            // CancelButton
            // 
            CancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            CancelButton.FlatAppearance.BorderSize = 0;
            CancelButton.FlatStyle = FlatStyle.Flat;
            CancelButton.Location = new Point(256, 38);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new Size(75, 23);
            CancelButton.TabIndex = 4;
            CancelButton.Text = "Cancel";
            CancelButton.UseVisualStyleBackColor = true;
            CancelButton.Click += CancelButton_Click;
            // 
            // SaveLoadButton
            // 
            SaveLoadButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            SaveLoadButton.FlatAppearance.BorderSize = 0;
            SaveLoadButton.FlatStyle = FlatStyle.Flat;
            SaveLoadButton.Location = new Point(175, 38);
            SaveLoadButton.Name = "SaveLoadButton";
            SaveLoadButton.Size = new Size(75, 23);
            SaveLoadButton.TabIndex = 5;
            SaveLoadButton.Text = "Load";
            SaveLoadButton.UseVisualStyleBackColor = true;
            SaveLoadButton.Click += SaveLoadButton_Click;
            // 
            // DeleteButton
            // 
            DeleteButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            DeleteButton.FlatAppearance.BorderSize = 0;
            DeleteButton.FlatStyle = FlatStyle.Flat;
            DeleteButton.Location = new Point(12, 38);
            DeleteButton.Name = "DeleteButton";
            DeleteButton.Size = new Size(75, 23);
            DeleteButton.TabIndex = 6;
            DeleteButton.Text = "Delete";
            DeleteButton.UseVisualStyleBackColor = true;
            DeleteButton.Click += DeleteButton_Click;
            // 
            // PresetForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(343, 72);
            Controls.Add(DeleteButton);
            Controls.Add(SaveLoadButton);
            Controls.Add(CancelButton);
            Controls.Add(SavedPresetDropdown);
            Controls.Add(SavedPresetLabel);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "PresetForm";
            Text = "Load Preset";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label SavedPresetLabel;
        private ComboBox SavedPresetDropdown;
        private Button CancelButton;
        private Button SaveLoadButton;
        private Button DeleteButton;
    }
}