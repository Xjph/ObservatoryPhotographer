using ImageMagick;
using Observatory.Framework.Interfaces;

namespace Observatory.Photographer.UI
{
    public partial class MainPanel : Form
    {
        public IObservatoryCore? Core;
        public PhotoWorker? Worker;
        public Photographer? Photographer;

        public MainPanel()
        {
            InitializeComponent();
            var comparer = new PhotoViewComparer();
            PhotoView.ListViewItemSorter = new PhotoViewComparer();
        }

        public Panel PhotoPanel => FlowPanel;
        public Label DataLabel => InfoLabel;
        public ListView PhotoListView => PhotoView;

        public void ShowProcessing(bool show)
        {
            Core?.ExecuteOnUIThread(() =>
            {
                ProcessingLabel.Enabled = show;
                ProcessingLabel.Visible = show;
                if (show)
                    ProcessingLabel.BringToFront();
                else
                    ProcessingLabel.SendToBack();
            });
        }

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            if (PhotoView.SelectedItems.Count > 0)
            {
                var meta = Photographer?.GetImageMetadata(PhotoView.SelectedItems[0].ImageKey);
                if (meta is not null)
                {
                    // Clone image so original isn't modified by processing.
                    var clonedMeta = new ImageWithMetadata(
                        meta.Filename,
                        meta.Screenshot,
                        meta.Status
                    );
                    var processForm = new ProcessForm(clonedMeta, Core!, Worker!);
                    Core?.RegisterControl(processForm);
                    processForm.StartPosition = FormStartPosition.Manual;
                    processForm.Location = Point.Add(
                        Application.OpenForms[0]?.Location ?? Point.Empty,
                        new Size(100, 100)
                    );
                    processForm.ShowDialog();
                }
            }
            else
            {
                var processForm = new ProcessForm(Core!, Worker!);
                Core?.RegisterControl(processForm);
                processForm.StartPosition = FormStartPosition.Manual;
                processForm.Location = Point.Add(
                    Application.OpenForms[0]?.Location ?? Point.Empty,
                    new Size(100, 100)
                );
                processForm.ShowDialog();
            }
        }
    }
}
