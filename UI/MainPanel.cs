using ImageMagick;
using Observatory.Framework.Interfaces;

namespace Observatory.Photographer.UI
{
    public partial class MainPanel : Form
    {
        public IObservatoryCore? Core;
        public IObservatoryWorker? Worker;

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
                var processForm = new ProcessForm(PhotoView.SelectedItems[0].ImageKey, Core, Worker);
                Core.RegisterControl(processForm);
                processForm.ShowDialog();
            }
        }
    }
}
