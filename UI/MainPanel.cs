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
        }

        public Panel PhotoPanel => FlowPanel;
        public Label DataLabel => InfoLabel;
        public ListView PhotoListView => PhotoView;

        private void ProcessButton_Click(object sender, EventArgs e)
        {
            if (PhotoView.SelectedItems.Count > 0)
            {
                var meta = Photographer?.GetImageMetadata(PhotoView.SelectedItems[0].ImageKey);
                if (meta is not null)
                {
                    var processForm = new ProcessForm(meta, Core, Worker);
                    Core?.RegisterControl(processForm);
                    processForm.ShowDialog();
                }
            }
        }
    }
}
