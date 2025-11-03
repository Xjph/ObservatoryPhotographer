using ImageMagick;
using Observatory.Framework.Interfaces;

namespace Observatory.Photographer.UI
{
    public partial class ProcessForm : Form
    {
        private MagickImage _image;
        private CaptionForm _captionForm;
        private WatermarkForm _watermarkForm;
        private MainPanel? _mainPanel;
        private CaptionAction _captionAction;
        private WatermarkAction _watermarkAction;
        private SaveAction _saveAction;
        private ResizeAction _resizeAction;
        private readonly IObservatoryCore _core;
        private readonly IObservatoryWorker _worker;
        
        public ProcessForm(string imagePath, IObservatoryCore core, IObservatoryWorker worker)
        {
            RestoreSavedProcess();
            _image = new(imagePath);
            _captionAction = new() { Font = Font };
            _watermarkAction = new() { WatermarkImagePath = string.Empty };
            _saveAction = new() { Format = MagickFormat.Png, Path = ((PhotoSettings)worker.Settings).OutputLocationPath + Path.DirectorySeparatorChar + "image.png" };
            _resizeAction = new() { X = 100, Y = 100, Relative = true };
            _captionForm = new(_captionAction);
            _watermarkForm = new(_watermarkAction);
            _core = core;
            _worker = worker;
            core.RegisterControl(_captionForm);
            core.RegisterControl(_watermarkForm);
            CancelButton = CancelBtn;
            InitializeComponent();
        }

        private void RestoreSavedProcess()
        {
            IEnumerable<PhotoAction> actionList;
            try
            {
                actionList = ((PhotoSettings)_worker.Settings).SavedActions.Cast<PhotoAction>();
            }
            catch
            {
                actionList = [];
            }

            foreach (PhotoAction action in actionList)
            {
                switch (action)
                {
                    case SaveAction saveAction:
                        _saveAction = saveAction;
                        break;
                    case WatermarkAction watermarkAction:
                        _watermarkAction = watermarkAction;
                        break;
                    case CaptionAction captionAction:
                        _captionAction = captionAction;
                        break;
                    case ResizeAction resizeAction:
                        _resizeAction= resizeAction;
                        break;
                    case MetaAction metaAction:
                        MetadataCheckbox.Checked = true;
                        break;
                }
            }
        }

        private void CaptionButton_Click(object sender, EventArgs e)
        {
            _captionForm.ShowDialog();
        }

        private void WatermarkButton_Click(object sender, EventArgs e)
        {
            _watermarkForm.ShowDialog();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            ImageUtils.PerformPhotoActions(_image, BuildActionList());
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var actionList = BuildActionList();
            
            ((PhotoSettings)_worker.Settings).SavedActions = [.. actionList];

            _core.SaveSettings(_worker);
        }

        private List<PhotoAction> BuildActionList()
        {
            List<PhotoAction> actionList = [];

            if (WatermarkCheckbox.Checked)
                actionList.Add(_watermarkAction);

            if (CaptionCheckbox.Checked)
                actionList.Add(_captionAction);

            if (ResizeCheckbox.Checked)
                actionList.Add(_resizeAction);

            if (MetadataCheckbox.Checked)
                actionList.Add(new MetaAction());

            if (ConvertCheckbox.Checked)
                actionList.Add(_saveAction);

            return actionList;
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
