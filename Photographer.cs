using System.Text;
using System.Text.Json;
using ImageMagick;
using Observatory.Framework;
using Observatory.Framework.Files;
using Observatory.Framework.Files.Journal;
using Observatory.Framework.Interfaces;

namespace Observatory.Photographer
{
    public class Photographer
    {
        private IObservatoryCore _core;
        private UI.MainPanel _ui;
        private ImageList _imageList;
        private PhotoSettings _settings;
        private Dictionary<string, ImageWithMetadata> _imageData;
        private List<Task> _processingTasks = [];
        private PhotoData _photoData;
        public readonly Action<Exception, string> Errorlogger;

        public Photographer(
            IObservatoryCore core,
            PhotoWorker worker,
            UI.MainPanel ui,
            PhotoSettings photoSettings,
            Action<Exception, string> errorLogger
        )
        {
            _core = core;
            Errorlogger = errorLogger;
            _ui = ui;
            ui.Core = core;
            ui.Photographer = this;
            ui.Worker = worker;
            _imageList = new();
            _ui.PhotoListView.LargeImageList = _imageList;
            _ui.PhotoListView.View = View.LargeIcon;
            _ui.PhotoListView.LargeImageList.ImageSize = new Size(256, 256);
            _settings = photoSettings;
            _imageData = [];
            _photoData = new(core, errorLogger);
            _ui.PhotoListView.SelectedIndexChanged += SelectedImageChanged;
        }

        private void SelectedImageChanged(object? sender, EventArgs e)
        {
            if (_ui.PhotoListView.SelectedItems.Count > 0)
                _ui.DataLabel.Text = CreateImageDataText(
                    _imageData[_ui.PhotoListView.SelectedItems[0].ImageKey].Screenshot,
                    _ui.PhotoListView.SelectedItems[0].ImageKey
                );
        }

        private void TaskCleanup()
        {
            _processingTasks = [.. _processingTasks.Where(t => !t.IsCompleted)];
            if (
                _processingTasks.Count == 1
                && !_core.CurrentLogMonitorState.HasFlag(LogMonitorState.Batch)
            )
            {
                UiExec(() => _ui.ShowProcessing(false));
            }
        }

        private void UiExec(Action action) => _core.ExecuteOnUIThread(action);

        public void HandleScreenshot(Screenshot screenshot)
        {
            if (ProceedWithImport(_core.CurrentLogMonitorState))
            {
                var picturesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

                var file = FindImage(screenshot);
                if (file != null)
                    _processingTasks.Add(
                        Task.Run(() =>
                        {
                            var largeImageKeys =
                                _ui.PhotoListView.LargeImageList?.Images.Keys.Cast<string>()
                                    .ToList()
                                ?? [];
                            List<ListViewItem> imageItems = [];
                            UiExec(() =>
                                imageItems = [.. _ui.PhotoListView.Items.Cast<ListViewItem>()]
                            );

                            using MagickImage original = new(file.FullName);
                            if (largeImageKeys.Contains(file.FullName))
                            {
                                var staleItem = imageItems.Where(item =>
                                    item.ImageKey == file.FullName
                                );
                                if (staleItem.Any())
                                {
                                    UiExec(() => _ui.PhotoListView.Items.Remove(staleItem.First()));
                                }
                            }
                            else
                            {
                                var aspect = original.Width / (double)original.Height;
                                int thumbWidth = 256;
                                int thumbHeight = (int)Math.Floor(256 / aspect);
                                var resized = MagickToBitmap(original, thumbWidth, thumbHeight);
                                UiExec(() =>
                                {
                                    _ui.PhotoListView.LargeImageList!.ImageSize = new Size(
                                        thumbWidth,
                                        thumbHeight
                                    );
                                    _ui.PhotoListView.LargeImageList!.Images.Add(
                                        file.FullName,
                                        resized
                                    );
                                });
                            }

                            string itemLabel;

                            if (!string.IsNullOrEmpty(screenshot.System))
                                itemLabel = screenshot.System;
                            else
                                itemLabel = screenshot.Timestamp;

                            Status? status = null;
                            var isRealtime = StatusIsCurrent(_core.CurrentLogMonitorState);

                            if (isRealtime)
                            {
                                status = _core.GetStatus();
                                if (status != null)
                                {
                                    _photoData.ScreenshotStatus[file.FullName] = status;
                                    _photoData.SaveStatus();
                                }
                            }
                            else
                            {
                                _photoData.ScreenshotStatus.TryGetValue(file.FullName, out status);
                            }

                            UiExec(() =>
                                _ui.PhotoListView.Items.Add(
                                    new ListViewItem(screenshot.System) { ImageKey = file.FullName }
                                )
                            );
                            _imageData[file.FullName] = new(original, screenshot, status);

                            if (ProceedWithProcessing(_core.CurrentLogMonitorState))
                            {
                                ImageUtils.PerformPhotoActions(
                                    GetDefaultActions(),
                                    _imageData[file.FullName]
                                );
                            }
                        }).ContinueWith(t =>
                        {
                            if (t.Exception != null)
                            {
                                Errorlogger(t.Exception, $"Error processing screenshot {file.FullName}");
                            }
                            TaskCleanup();
                        })
                    );
            }
        }

        private bool ProceedWithProcessing(LogMonitorState state) =>
            (
                !state.HasFlag(LogMonitorState.PreRead)
                && state.HasFlag(LogMonitorState.Batch)
                && _settings.ProcessDuringReadAll
            ) || (state.HasFlag(LogMonitorState.Realtime) && _settings.ProcessWhileMonitoring);

        private IEnumerable<PhotoAction> GetDefaultActions()
        {
            _photoData.SavedPresets.TryGetValue(
                _settings.DefaultPreset,
                out IEnumerable<PhotoAction>? defaultActions
            );
            return defaultActions ?? [];
        }

        public ImageWithMetadata? GetImageMetadata(string imageKey)
        {
            if (_imageData.TryGetValue(imageKey, out ImageWithMetadata? value))
                return value;
            return null;
        }

        private static Bitmap MagickToBitmap(MagickImage magickImage, int width, int height)
        {
            var resized = magickImage.Clone();
            resized.Resize((uint)width, (uint)height);
            using (var ms = new MemoryStream())
            {
                resized.Write(ms, MagickFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                return new Bitmap(ms);
            }
        }

        private bool ProceedWithImport(LogMonitorState state)
        {
            var batchCheck =
                _settings.ImportDuringReadAll
                || _settings.ProcessDuringReadAll
                || !state.HasFlag(LogMonitorState.Batch);
            var realtimeCheck =
                _settings.ProcessWhileMonitoring || !state.HasFlag(LogMonitorState.Realtime);

            // Don't ever process from pre-read
            return (batchCheck || realtimeCheck) && !state.HasFlag(LogMonitorState.PreRead);
        }

        private bool StatusIsCurrent(LogMonitorState state) =>
            state.HasFlag(LogMonitorState.Realtime);

        private string CreateImageDataText(Screenshot screenshot, string fullFilename)
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
                    dataText.AppendLine(
                        $"  Lat/Long: {Math.Abs(screenshot.Latitude):N3}°{(screenshot.Latitude >= 0 ? 'N' : 'S')}  {Math.Abs(screenshot.Longitude):N3}°{(screenshot.Longitude >= 0 ? 'E' : 'W')}"
                    );
                    dataText.AppendLine(
                        $"  Facing Direction: {HeadingToCompass(screenshot.Heading)}"
                    );
                }
            }
            dataText.AppendLine($"Width: {screenshot.Width}");
            dataText.AppendLine($"Height: {screenshot.Height}");
            dataText.AppendLine($"File Size: {new FileInfo(fullFilename).Length}");
            dataText.AppendLine($"Full file path: {fullFilename}");
            if (
                _imageData.TryGetValue(fullFilename, out ImageWithMetadata? metadata)
                && metadata.Status != null
            )
            {
                dataText.AppendLine();
                dataText.AppendLine($"Extended Metadata Available");
            }

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
            else
                return "NW";
        }

        private FileInfo? FindImage(Screenshot screenshot)
        {
            var filename = screenshot.Filename
                .Replace("\\ED_Pictures", string.Empty);

            if (CheckAndSetPath(ref filename))
            {
                if (!_settings.AllowTimestampMismatch && 
                    Math.Abs((screenshot.TimestampDateTime - File.GetCreationTimeUtc(filename)).TotalDays) > 1)
                    return null;
                return new FileInfo(filename);
            }

            return null;
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

        public void UpdateContext(Status status) { }
    }
}
