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
        private PhotoWorker _worker;
        private UI.MainPanel _ui;
        private ImageList _imageList;
        private PhotoSettings _settings;
        private Dictionary<string, ImageWithMetadata> _imageData;
        private List<Task> _processingTasks = [];
        private Dictionary<string, Status> _persistedStatus;
        private readonly string _statusStoragePath;

        public Photographer(
            IObservatoryCore core,
            PhotoWorker worker,
            UI.MainPanel ui,
            PhotoSettings photoSettings
        )
        {
            _core = core;
            _worker = worker;
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
            _persistedStatus = [];
            _statusStoragePath = Path.Combine(_core.PluginStorageFolder, "screenshot_status.json");
            LoadPersistedStatus();
            _ui.PhotoListView.SelectedIndexChanged += SelectedImageChanged;
        }

        private void SelectedImageChanged(object? sender, EventArgs e)
        {
            if (_ui.PhotoListView.SelectedItems.Count > 0)
                _ui.DataLabel.Text = CreateImageDataText(
                    _imageData[_ui.PhotoListView.SelectedItems[0].ImageKey].Screenshot,
                    _ui.PhotoListView.SelectedItems[0].ImageKey
                );
            //BuildImageCaption(_imageData[_ui.PhotoListView.SelectedItems[0].ImageKey]);
        }

        private static string BuildImageCaption(ImageWithMetadata metadata)
        {
            // Simple one-line title to display under thumbnail
            // System - Body - Time with fallback to filename
            StringBuilder caption = new();
            if (!string.IsNullOrEmpty(metadata.Screenshot.System))
            {
                caption.Append(metadata.Screenshot.System);
                if (!string.IsNullOrEmpty(metadata.Screenshot.Body))
                {
                    caption.Append(" - ");
                    caption.Append(metadata.Screenshot.Body);
                }
                caption.Append(" - ");
                caption.Append(metadata.Screenshot.TimestampDateTime.ToString("g"));
            }
            else
            {
                caption.Append(metadata.Screenshot.Filename.Split('\\').Last());
            }

            return caption.ToString();
        }

        private void LoadPersistedStatus()
        {
            if (File.Exists(_statusStoragePath))
            {
                try
                {
                    var json = File.ReadAllText(_statusStoragePath);
                    _persistedStatus =
                        JsonSerializer.Deserialize<Dictionary<string, Status>>(json) ?? [];
                }
                catch
                {
                    _persistedStatus = [];
                }
            }
        }

        private void SavePersistedStatus()
        {
            try
            {
                var json = JsonSerializer.Serialize(
                    _persistedStatus,
                    new JsonSerializerOptions { WriteIndented = true }
                );
                File.WriteAllText(_statusStoragePath, json);
            }
            catch
            {
                // Silently fail if unable to save
            }
        }

        private void TaskCleanup()
        {
            _processingTasks = [.. _processingTasks.Where(t => !t.IsCompleted)];
        }

        public void HandleScreenshot(Screenshot screenshot)
        {
            TaskCleanup();
            if (Proceed(_core.CurrentLogMonitorState))
            {
                var picturesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                var filename = screenshot.Filename
                // .Replace("\\\\", "\\")
                .Replace("\\ED_Pictures", string.Empty);

                var fullFilename = FindImagePath(filename);

                _processingTasks.Add(
                    Task.Run(() =>
                    {
                        if (!string.IsNullOrEmpty(fullFilename))
                        {
                            var file = new FileInfo(fullFilename);

                            var uiExec = (Action action) => _core.ExecuteOnUIThread(action);

                            var largeImageKeys =
                                _ui.PhotoListView.LargeImageList?.Images.Keys.Cast<string>()
                                    .ToList()
                                ?? [];
                            List<ListViewItem> imageItems = [];
                            uiExec(() =>
                                imageItems = [.. _ui.PhotoListView.Items.Cast<ListViewItem>()]
                            );

                            MagickImage original = new(fullFilename);
                            if (largeImageKeys.Contains(fullFilename))
                            {
                                var staleItem = imageItems.Where(item =>
                                    item.ImageKey == fullFilename
                                );
                                if (staleItem.Any())
                                {
                                    uiExec(() => _ui.PhotoListView.Items.Remove(staleItem.First()));
                                }
                            }
                            else
                            {
                                var aspect = original.Width / (double)original.Height;
                                int thumbWidth = 256;
                                int thumbHeight = (int)Math.Floor(256 / aspect);
                                var resized = MagickToBitmap(original, thumbWidth, thumbHeight);
                                uiExec(() =>
                                {
                                    _ui.PhotoListView.LargeImageList!.ImageSize = new Size(
                                        thumbWidth,
                                        thumbHeight
                                    );
                                    _ui.PhotoListView.LargeImageList!.Images.Add(
                                        fullFilename,
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
                                    _persistedStatus[fullFilename] = status;
                                    SavePersistedStatus();
                                }
                            }
                            else
                            {
                                _persistedStatus.TryGetValue(fullFilename, out status);
                            }

                            uiExec(() =>
                                _ui.PhotoListView.Items.Add(
                                    new ListViewItem(screenshot.System) { ImageKey = fullFilename }
                                )
                            );
                            _imageData[fullFilename] = new(original, screenshot, status);

                            if (ProceedWithProcessing(_core.CurrentLogMonitorState))
                            {
                                ImageUtils.PerformPhotoActions(
                                    GetSavedActions(),
                                    _imageData[fullFilename]
                                );
                            }
                        }
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

        private IEnumerable<PhotoAction> GetSavedActions() =>
            _settings.SavedActions.Select<object, PhotoAction>(action =>
            {
                var jsonAction = (JsonElement)action;
                var actionType = (PhotoActionKind)jsonAction.GetProperty("Action").GetInt32();
                return actionType switch
                {
                    PhotoActionKind.Save => jsonAction.Deserialize<SaveAction>()!,
                    PhotoActionKind.Resize => jsonAction.Deserialize<ResizeAction>()!,
                    PhotoActionKind.Caption => jsonAction.Deserialize<CaptionAction>()!,
                    PhotoActionKind.Watermark => jsonAction.Deserialize<WatermarkAction>()!,
                    PhotoActionKind.Meta => jsonAction.Deserialize<MetaAction>()!,
                    _ => throw new InvalidOperationException("Unknown action type in settings"),
                };
            });

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

        private bool Proceed(LogMonitorState state)
        {
            var batchCheck =
                _settings.ProcessDuringReadAll || !state.HasFlag(LogMonitorState.Batch);
            var realtimeCheck =
                _settings.ProcessWhileMonitoring || !state.HasFlag(LogMonitorState.Realtime);

            // Don't ever process from pre-read
            return batchCheck && realtimeCheck && !state.HasFlag(LogMonitorState.PreRead);
        }

        private bool StatusIsCurrent(LogMonitorState state) =>
            state.HasFlag(LogMonitorState.Realtime);

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

        public void UpdateContext(Status status) { }
    }
}
