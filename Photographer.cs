using ImageMagick;
using Observatory.Framework;
using Observatory.Framework.Files;
using Observatory.Framework.Files.Journal;
using Observatory.Framework.Interfaces;
using System.Text;

namespace Observatory.Photographer
{
    public class Photographer
    {
        private IObservatoryCore _core;
        private PhotoWorker _worker;
        private UI.MainPanel _ui;
        private ImageList _imageList;
        private PhotoSettings _settings;
        private Dictionary<string, string> _imageData;

        public Photographer(IObservatoryCore core, PhotoWorker worker, UI.MainPanel ui, PhotoSettings photoSettings)
        {
            _core = core;
            _worker = worker;
            _ui = ui;
            ui.Core = core;
            ui.Worker = worker;
            _imageList = new();
            _ui.PhotoListView.LargeImageList = _imageList;
            _ui.PhotoListView.View = View.LargeIcon;
            _ui.PhotoListView.LargeImageList.ImageSize = new Size(256, 256);
            _settings = photoSettings;
            _imageData = [];
            _ui.PhotoListView.SelectedIndexChanged += SelectedImageChanged;
        }

        private void SelectedImageChanged(object? sender, EventArgs e)
        {
            if (_ui.PhotoListView.SelectedItems.Count > 0)
                _ui.DataLabel.Text = _imageData[_ui.PhotoListView.SelectedItems[0].ImageKey];
        }

        public void HandleScreenshot(Screenshot screenshot)
        {
            if (Proceed(_core.CurrentLogMonitorState))
            {
                var picturesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                var filename = screenshot.Filename
                    .Replace("\\\\", "\\")
                    .Replace("\\ED_Pictures", string.Empty);

                var fullFilename = FindImagePath(filename);

                if (!string.IsNullOrEmpty(fullFilename))
                {
                    var file = new FileInfo(fullFilename);

                    _core.ExecuteOnUIThread(() =>
                    {
                        if (_ui.PhotoListView.LargeImageList?.Images.ContainsKey(fullFilename) ?? false)
                        {
                            var staleItem = _ui.PhotoListView.Items.Cast<ListViewItem>().Where(item => item.ImageKey == fullFilename);
                            if (staleItem.Any())
                            {
                                _ui.PhotoListView.Items.Remove(staleItem.First());
                            }
                        }
                        else
                        {
                            var original = Image.FromFile(fullFilename);
                            var aspect = original.Width / (double)original.Height;
                            var resized = new Bitmap(original, new Size(256, (int)Math.Floor(256 / aspect)));
                            _ui.PhotoListView.LargeImageList.ImageSize = new Size(256, (int)Math.Floor(256 / aspect));
                            _ui.PhotoListView.LargeImageList?.Images.Add(fullFilename, resized);
                        }

                        string itemLabel;

                        if (!string.IsNullOrEmpty(screenshot.System))
                            itemLabel = screenshot.System;
                        else
                            itemLabel = screenshot.Timestamp;

                        _ui.PhotoListView.Items.Add(new ListViewItem(screenshot.System) { ImageKey = fullFilename });
                        _imageData[fullFilename] = CreateImageDataText(screenshot, fullFilename);

                    });

                    var image = new MagickImage(File.ReadAllBytes(fullFilename));
                    var openQuad = ImageUtils.FindOpenQuad(image);
                    Rectangle quadBounds;
                    var width = (int)image.Width;
                    var height = (int)image.Height;
                    switch (openQuad)
                    {
                        case 0: // Top left
                            quadBounds = new(0, 0, width / 2, height / 2);
                            break;
                        case 1: // Top right
                            quadBounds = new(width / 2, 0, width / 2, height / 2);
                            break;
                        case 2: // Bottom left
                            quadBounds = new(0, height / 2, width / 2, height / 2);
                            break;
                        case 3: // Bottom right
                        default:
                            quadBounds = new(width / 2, height / 2, width / 2, height / 2);
                            break;
                    }
                }
            }
        }

        private bool Proceed(LogMonitorState state)
        {
            var batchCheck = _settings.ProcessDuringReadAll || !state.HasFlag(LogMonitorState.Batch);
            var realtimeCheck = _settings.ProcessWhileMonitoring || !state.HasFlag(LogMonitorState.Realtime);

            // Don't ever process from pre-read
            return batchCheck && realtimeCheck && !state.HasFlag(LogMonitorState.PreRead);    
        }

        private static string CreateImageDataText(Screenshot screenshot, string fullFilename)
        {
            StringBuilder dataText = new();
            dataText.AppendLine($"Time taken: {screenshot.Timestamp}");
            dataText.AppendLine($"Original filename: {screenshot.Filename.Split('\\').Last()}");
            if (screenshot.System?.Length > 0)
            {
                dataText.AppendLine($"Location");
                dataText.AppendLine($"  System: {screenshot.System}");
                
                if (screenshot.Body.Length > 0 && screenshot.Body != screenshot.System)
                    dataText.AppendLine($"  Body: {screenshot.Body}");

                if (screenshot.Latitude != 0 && screenshot.Longitude != 0)
                {
                    dataText.AppendLine($"  Lat/Long: {Math.Abs(screenshot.Latitude):N3}°{(screenshot.Latitude >= 0 ? 'N' : 'S')}  {Math.Abs(screenshot.Longitude):N3}°{(screenshot.Longitude >= 0 ? 'E' : 'W')}");
                    dataText.AppendLine($"  Facing Direction: {HeadingToCompass(screenshot.Heading)}");
                }
            }
            dataText.AppendLine($"Width: {screenshot.Width}");
            dataText.AppendLine($"Height: {screenshot.Height}");
            dataText.AppendLine($"File Size: {new FileInfo(fullFilename).Length}");
            dataText.AppendLine($"Full file path: {fullFilename}");
            return dataText.ToString();
        }

        private static string HeadingToCompass(int heading)
        {
            if (heading < 22.5 || heading > 337.5)
                return "N";
            else if (heading >= 22.5 && heading < 67.5)
                return "NE";
            else if (heading >= 67.5 && heading < 112.5)
                return "E";
            else if (heading >= 112.5 && heading < 157.5)
                return "SE";
            else if (heading >= 157.5 && heading < 202.5)
                return "S";
            else if (heading >= 202.5 && heading < 247.5)
                return "SW";
            else if (heading >= 247.5 && heading < 292.5)
                return "W";
            else return "NW";
        }

        private string FindImagePath(string initialPath)
        {
            if (CheckAndSetPath(ref initialPath))
            {
                return initialPath;
            }

            return string.Empty;
        }

        private bool CheckAndSetPath(ref string filename)
        {
            var fullPath = _settings.ScreenshotLocation + filename;
            if (File.Exists(fullPath))
            {
                filename = fullPath;
                return true;
            }
            return false;
        }

        public void UpdateContext(Status status)
        {
            
        }
    }
}
