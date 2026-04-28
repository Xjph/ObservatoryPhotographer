namespace Observatory.Photographer.UI
{
    partial class MainPanel
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
            FlowPanel = new Panel();
            InfoBox = new GroupBox();
            InfoLabel = new Label();
            ProcessButton = new Button();
            PhotoView = new ListView();
            ProcessingLabel = new Label();
            FlowPanel.SuspendLayout();
            InfoBox.SuspendLayout();
            SuspendLayout();
            // 
            // FlowPanel
            // 
            FlowPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            FlowPanel.Controls.Add(InfoBox);
            FlowPanel.Controls.Add(ProcessButton);
            FlowPanel.Controls.Add(PhotoView);
            FlowPanel.Controls.Add(ProcessingLabel);
            FlowPanel.Location = new Point(12, 12);
            FlowPanel.Name = "FlowPanel";
            FlowPanel.Size = new Size(776, 426);
            FlowPanel.TabIndex = 0;
            // 
            // InfoBox
            // 
            InfoBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            InfoBox.Controls.Add(InfoLabel);
            InfoBox.Location = new Point(483, 3);
            InfoBox.Name = "InfoBox";
            InfoBox.Size = new Size(290, 390);
            InfoBox.TabIndex = 5;
            InfoBox.TabStop = false;
            InfoBox.Text = "Image Metadata";
            // 
            // InfoLabel
            // 
            InfoLabel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            InfoLabel.Location = new Point(6, 19);
            InfoLabel.Name = "InfoLabel";
            InfoLabel.Size = new Size(278, 368);
            InfoLabel.TabIndex = 0;
            // 
            // ProcessButton
            // 
            ProcessButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            ProcessButton.FlatAppearance.BorderSize = 0;
            ProcessButton.FlatStyle = FlatStyle.Flat;
            ProcessButton.Location = new Point(3, 399);
            ProcessButton.Name = "ProcessButton";
            ProcessButton.Size = new Size(75, 23);
            ProcessButton.TabIndex = 1;
            ProcessButton.Text = "Process";
            ProcessButton.UseVisualStyleBackColor = true;
            ProcessButton.Click += ProcessButton_Click;
            // 
            // PhotoView
            // 
            PhotoView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PhotoView.Location = new Point(3, 3);
            PhotoView.Name = "PhotoView";
            PhotoView.Size = new Size(474, 390);
            PhotoView.Sorting = SortOrder.Ascending;
            PhotoView.TabIndex = 0;
            PhotoView.UseCompatibleStateImageBehavior = false;
            // 
            // ProcessingLabel
            // 
            ProcessingLabel.Dock = DockStyle.Fill;
            ProcessingLabel.Enabled = false;
            ProcessingLabel.Location = new Point(0, 0);
            ProcessingLabel.Name = "ProcessingLabel";
            ProcessingLabel.Size = new Size(776, 426);
            ProcessingLabel.TabIndex = 6;
            ProcessingLabel.Text = "Processing...";
            ProcessingLabel.TextAlign = ContentAlignment.MiddleCenter;
            ProcessingLabel.Visible = false;
            // 
            // MainPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(FlowPanel);
            Name = "MainPanel";
            Text = "DevForm";
            FlowPanel.ResumeLayout(false);
            InfoBox.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel FlowPanel;
        private ListView PhotoView;
        private Button ProcessButton;
        private GroupBox InfoBox;
        private Label InfoLabel;
        private Label ProcessingLabel;
    }
}