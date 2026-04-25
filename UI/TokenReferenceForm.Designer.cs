namespace Observatory.Photographer.UI
{
    partial class TokenReferenceForm
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
            LinkLayoutPanel = new FlowLayoutPanel();
            SuspendLayout();
            // 
            // LinkLayoutPanel
            // 
            LinkLayoutPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            LinkLayoutPanel.FlowDirection = FlowDirection.TopDown;
            LinkLayoutPanel.Location = new Point(12, 12);
            LinkLayoutPanel.Name = "LinkLayoutPanel";
            LinkLayoutPanel.Size = new Size(296, 443);
            LinkLayoutPanel.TabIndex = 0;
            // 
            // TokenReferenceForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(320, 467);
            Controls.Add(LinkLayoutPanel);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Name = "TokenReferenceForm";
            Text = "Metadata Tokens";
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel LinkLayoutPanel;
    }
}