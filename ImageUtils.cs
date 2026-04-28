using System.Text;
using System.Text.Json;
using ImageMagick;
using Observatory.Framework.Files.ParameterTypes;

namespace Observatory.Photographer
{
    public class ImageUtils
    {
        public enum Quad
        {
            TopLeft = 0,
            TopRight = 1,
            BottomLeft = 2,
            BottomRight = 3,
        }

        public enum SecondOrderQuad
        {
            TopLeftOfTopLeft = 0,
            TopRightOfTopLeft = 1,
            BottomLeftOfTopLeft = 2,
            BottomRightOfTopLeft = 3,
            TopLeftOfTopRight = 4,
            TopRightOfTopRight = 5,
            BottomLeftOfTopRight = 6,
            BottomRightOfTopRight = 7,
            TopLeftOfBottomLeft = 8,
            TopRightOfBottomLeft = 9,
            BottomLeftOfBottomLeft = 10,
            BottomRightOfBottomLeft = 11,
            TopLeftOfBottomRight = 12,
            TopRightOfBottomRight = 13,
            BottomLeftOfBottomRight = 14,
            BottomRightOfBottomRight = 15,
        }

        private static Rectangle GetBoundsFromQuad(MagickImage image, int quad, bool secondOrder)
        {
            var firstQuad = secondOrder ? quad / 4 : quad;

            int x = 0;
            int y = 0;
            int width = (int)(secondOrder ? image.Width / 4 : image.Width / 2);
            int height = (int)(secondOrder ? image.Height / 4 : image.Height / 2);

            switch (firstQuad)
            {
                case 1:
                    x = (int)image.Width / 2 + 1;
                    break;
                case 2:
                    y = (int)image.Height / 2 + 1;
                    break;
                case 3:
                    x = (int)image.Width / 2 + 1;
                    y = (int)image.Height / 2 + 1;
                    break;
            }

            if (secondOrder)
            {
                var secondQuad = quad % 4;
                switch (secondQuad)
                {
                    case 1:
                        x += width / 2 + 1;
                        break;
                    case 2:
                        y += height / 2 + 1;
                        break;
                    case 3:
                        x += width / 2 + 1;
                        y += height / 2 + 1;
                        break;
                }
            }

            return new Rectangle(x, y, width, height);
        }

        public static void PerformPhotoActions(
            IEnumerable<PhotoAction> actions,
            ImageWithMetadata imageData
        )
        {
            if (!actions.Any() || actions.Last().Action != PhotoActionKind.Save)
                throw new ArgumentException("Final Action Must Be SaveAction");

            foreach (var action in actions)
            {
                try
                {
                    switch (action)
                    {
                        case WatermarkAction watermarkAction:
                            AddWatermarkToImage(watermarkAction, imageData);
                            break;
                        case CaptionAction captionAction:
                            AddTextToImage(captionAction, imageData);
                            break;
                        case ResizeAction sizeAction:
                            Resize(sizeAction, imageData);
                            break;
                        case SaveAction saveAction:
                            SaveImage(saveAction, imageData);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // Additional context for exception logging at call site
                    throw new Exception(
                        $"Error performing action {action.Action} on image {imageData.Screenshot.Filename}",
                        ex
                    );
                }
            }
        }

        private static string resolveFlag(Framework.Files.Status? status, StatusFlags flag)
        {
            if (status is null)
                return string.Empty;
            return flag switch
            {
                StatusFlags.Docked => status.Flags.HasFlag(StatusFlags.Docked)
                    ? "Docked"
                    : "Undocked",
                StatusFlags.Landed => status.Flags.HasFlag(StatusFlags.Landed)
                    ? "Landed"
                    : "In Flight",
                StatusFlags.LandingGear => status.Flags.HasFlag(StatusFlags.LandingGear)
                    ? "Down"
                    : "Raised",
                StatusFlags.Shields => status.Flags.HasFlag(StatusFlags.Shields) ? "Up" : "Down",
                StatusFlags.Supercruise => status.Flags.HasFlag(StatusFlags.Supercruise)
                    ? "Supercruise"
                    : "Normal Space",
                StatusFlags.FAOff => status.Flags.HasFlag(StatusFlags.FAOff) ? "Off" : "On",
                StatusFlags.Hardpoints => status.Flags.HasFlag(StatusFlags.Hardpoints)
                    ? "Deployed"
                    : "Retracted",
                StatusFlags.Wing => status.Flags.HasFlag(StatusFlags.Wing) ? "Wing" : "Solo",
                StatusFlags.Lights => status.Flags.HasFlag(StatusFlags.Lights) ? "On" : "Off",
                StatusFlags.CargoScoop => status.Flags.HasFlag(StatusFlags.CargoScoop)
                    ? "Deployed"
                    : "Retracted",
                StatusFlags.SilentRunning => status.Flags.HasFlag(StatusFlags.SilentRunning)
                    ? "Silent Running"
                    : "",
                StatusFlags.FuelScooping => status.Flags.HasFlag(StatusFlags.FuelScooping)
                    ? "Scooping"
                    : "",
                StatusFlags.SRVBrake => status.Flags.HasFlag(StatusFlags.SRVBrake) ? "On" : "Off",
                StatusFlags.SRVTurret => status.Flags.HasFlag(StatusFlags.SRVTurret)
                    ? "Active"
                    : "Fixed",
                StatusFlags.SRVProximity => status.Flags.HasFlag(StatusFlags.SRVProximity)
                    ? "Close"
                    : "Clear",
                StatusFlags.SRVDriveAssist => status.Flags.HasFlag(StatusFlags.SRVDriveAssist)
                    ? "On"
                    : "Off",
                StatusFlags.Masslock => status.Flags.HasFlag(StatusFlags.Masslock)
                    ? "Mass Locked"
                    : "",
                StatusFlags.FSDCharging => status.Flags.HasFlag(StatusFlags.FSDCharging)
                    ? "FSD Charging"
                    : "FSD Idle",
                StatusFlags.FSDCooldown => status.Flags.HasFlag(StatusFlags.FSDCooldown)
                    ? "FSD Cooldown"
                    : "FSD Ready",
                StatusFlags.LowFuel => status.Flags.HasFlag(StatusFlags.LowFuel)
                    ? "Low Fuel"
                    : "Fuel OK",
                StatusFlags.Overheat => status.Flags.HasFlag(StatusFlags.Overheat)
                    ? "Overheating"
                    : "Temperature Normal",
                StatusFlags.LatLongValid => status.Flags.HasFlag(StatusFlags.LatLongValid)
                    ? "Lat/Long Valid"
                    : "Lat/Long Invalid",
                StatusFlags.InDanger => status.Flags.HasFlag(StatusFlags.InDanger)
                    ? "In Danger"
                    : "",
                StatusFlags.Interdiction => status.Flags.HasFlag(StatusFlags.Interdiction)
                    ? "Interdicted"
                    : "",
                StatusFlags.MainShip => status.Flags.HasFlag(StatusFlags.MainShip) ? "Ship" : "",
                StatusFlags.Fighter => status.Flags.HasFlag(StatusFlags.Fighter) ? "Fighter" : "",
                StatusFlags.SRV => status.Flags.HasFlag(StatusFlags.SRV) ? "SRV" : "",
                StatusFlags.AnalysisHUD => status.Flags.HasFlag(StatusFlags.AnalysisHUD)
                    ? "Analysis"
                    : "Combat",
                StatusFlags.NightVision => status.Flags.HasFlag(StatusFlags.NightVision)
                    ? "On"
                    : "Off",
                StatusFlags.RadialAltitude => status.Flags.HasFlag(StatusFlags.RadialAltitude)
                    ? "Radial"
                    : "Terrain",
                StatusFlags.FSDJump => status.Flags.HasFlag(StatusFlags.FSDJump)
                    ? "FSD Jumping"
                    : "",
                StatusFlags.SRVHighBeam => status.Flags.HasFlag(StatusFlags.SRVHighBeam)
                    ? "On"
                    : "Off",
                _ => string.Empty,
            };
        }

        private static string resolveFlag2(Framework.Files.Status? status, StatusFlags2 flag)
        {
            if (status is null)
                return string.Empty;
            return flag switch
            {
                StatusFlags2.OnFoot => status.Flags2.HasFlag(StatusFlags2.OnFoot) ? "On Foot" : "",
                StatusFlags2.InTaxi => status.Flags2.HasFlag(StatusFlags2.InTaxi) ? "In Taxi" : "",
                StatusFlags2.InMulticrew => status.Flags2.HasFlag(StatusFlags2.InMulticrew)
                    ? "In Multicrew"
                    : "",
                StatusFlags2.OnFootInStation => status.Flags2.HasFlag(StatusFlags2.OnFootInStation)
                    ? "In Station"
                    : "",
                StatusFlags2.OnFootOnPlanet => status.Flags2.HasFlag(StatusFlags2.OnFootOnPlanet)
                    ? "On Planet"
                    : "",
                StatusFlags2.AimDownSight => status.Flags2.HasFlag(StatusFlags2.AimDownSight)
                    ? "Aiming Down Sight"
                    : "",
                StatusFlags2.LowOxygen => status.Flags2.HasFlag(StatusFlags2.LowOxygen)
                    ? "Low Oxygen"
                    : "",
                StatusFlags2.LowHealth => status.Flags2.HasFlag(StatusFlags2.LowHealth)
                    ? "Low Health"
                    : "",
                StatusFlags2.Cold => status.Flags2.HasFlag(StatusFlags2.Cold) ? "Cold" : "",
                StatusFlags2.Hot => status.Flags2.HasFlag(StatusFlags2.Hot) ? "Hot" : "",
                StatusFlags2.VeryCold => status.Flags2.HasFlag(StatusFlags2.VeryCold)
                    ? "Very Cold"
                    : "",
                StatusFlags2.VeryHot => status.Flags2.HasFlag(StatusFlags2.VeryHot)
                    ? "Very Hot"
                    : "",
                StatusFlags2.GlideMode => status.Flags2.HasFlag(StatusFlags2.GlideMode)
                    ? "Gliding"
                    : "",
                StatusFlags2.OnFootInHangar => status.Flags2.HasFlag(StatusFlags2.OnFootInHangar)
                    ? "In Hangar"
                    : "",
                StatusFlags2.OnFootInSocialSpace => status.Flags2.HasFlag(
                    StatusFlags2.OnFootInSocialSpace
                )
                    ? "In Social Space"
                    : "",
                StatusFlags2.OnFootExterior => status.Flags2.HasFlag(StatusFlags2.OnFootExterior)
                    ? "Exterior"
                    : "",
                StatusFlags2.BreathableAtmosphere => status.Flags2.HasFlag(
                    StatusFlags2.BreathableAtmosphere
                )
                    ? "Breathable Atmosphere"
                    : "",
                StatusFlags2.TelepresenceMulticrew => status.Flags2.HasFlag(
                    StatusFlags2.TelepresenceMulticrew
                )
                    ? "Telepresence"
                    : "",
                StatusFlags2.PhysicalMulticrew => status.Flags2.HasFlag(
                    StatusFlags2.PhysicalMulticrew
                )
                    ? "Physical Multicrew"
                    : "",
                StatusFlags2.FsdHyperdriveCharging => status.Flags2.HasFlag(
                    StatusFlags2.FsdHyperdriveCharging
                )
                    ? "FSD Charging"
                    : "",
                _ => string.Empty,
            };
        }

        private static string FormatDegrees(float degrees, bool isLatitude, bool useDMS)
        {
            var absDegrees = Math.Abs(degrees);
            var direction = isLatitude
                ? degrees >= 0
                    ? "N"
                    : "S"
                : degrees >= 0
                    ? "E"
                    : "W";
            if (!useDMS)
                return $"{absDegrees:0.####}° {direction}";

            var d = Math.Floor(absDegrees);
            var m = Math.Floor((absDegrees - d) * 60);
            var s = (absDegrees - d - m / 60) * 3600;

            return $"{d}°{m}'{s:0.##}\" {direction}";
        }

        private static Dictionary<string, string> lookupDict(
            string cmdrName,
            ImageWithMetadata metadata
        ) =>
            new(StringComparer.InvariantCultureIgnoreCase)
            {
                { "cmdr", cmdrName ?? string.Empty },
                // Screenshot properties
                { "latitude", FormatDegrees((float?)metadata.Status?.Latitude ?? metadata.Screenshot.Latitude, true, true) },
                { "longitude", FormatDegrees((float?)metadata.Status?.Longitude ?? metadata.Screenshot.Longitude, false, true) },
                { "latitudeDMS", FormatDegrees((float?)metadata.Status?.Latitude ?? metadata.Screenshot.Latitude, true, false) },
                { "longitudeDMS", FormatDegrees((float?)metadata.Status?.Longitude ?? metadata.Screenshot.Longitude, false, false) },
                { "system", metadata.Screenshot.System ?? string.Empty },
                { "body", metadata.Screenshot.Body ?? string.Empty },
                { "altitude", metadata.Screenshot.Altitude.ToString() },
                { "heading", metadata.Status?.Heading.ToString() ?? metadata.Screenshot.Heading.ToString() },
                {
                    "timestamp",
                    metadata.Screenshot.TimestampDateTime.ToString("s").Replace(':', '-')
                },
                // Status properties
                { "guifocus", metadata.Status?.GuiFocus.ToString() ?? string.Empty },
                { "balance", metadata.Status?.Balance.ToString() ?? string.Empty },
                { "cargo", metadata.Status?.Cargo.ToString("N0") ?? string.Empty },
                { "mainfuel", metadata.Status?.Fuel?.FuelMain.ToString("N1") ?? string.Empty },
                {
                    "reservoirfuel",
                    metadata.Status?.Fuel?.FuelReservoir.ToString("N1") ?? string.Empty
                },
                { "health", (metadata.Status?.Health * 100)?.ToString("N0") ?? string.Empty },
                { "oxygen", (metadata.Status?.Oxygen * 100)?.ToString("N0") ?? string.Empty },
                { "destination", metadata.Status?.Destination?.Name ?? string.Empty },
                { "gravity-g", metadata.Status?.Gravity.ToString("N2") ?? string.Empty },
                {
                    "gravity-mps2",
                    (metadata.Status?.Gravity * 9.81)?.ToString("N2") ?? string.Empty
                },
                { "legalstate", metadata.Status?.LegalState.ToString() ?? string.Empty },
                {
                    "radius",
                    (metadata.Status?.PlanetRadius / 1000)?.ToString("N0") ?? string.Empty
                },
                { "temperature", metadata.Status?.Temperature.ToString("N1") ?? string.Empty },
                // Status flags
                { "hud", resolveFlag(metadata.Status, StatusFlags.AnalysisHUD) },
                { "docked", resolveFlag(metadata.Status, StatusFlags.Docked) },
                { "landed", resolveFlag(metadata.Status, StatusFlags.Landed) },
                { "landinggear", resolveFlag(metadata.Status, StatusFlags.LandingGear) },
                { "shields", resolveFlag(metadata.Status, StatusFlags.Shields) },
                { "supercruise", resolveFlag(metadata.Status, StatusFlags.Supercruise) },
                { "faoff", resolveFlag(metadata.Status, StatusFlags.FAOff) },
                { "hardpoints", resolveFlag(metadata.Status, StatusFlags.Hardpoints) },
                { "wing", resolveFlag(metadata.Status, StatusFlags.Wing) },
                { "lights", resolveFlag(metadata.Status, StatusFlags.Lights) },
                { "cargoscoop", resolveFlag(metadata.Status, StatusFlags.CargoScoop) },
                { "silentrunning", resolveFlag(metadata.Status, StatusFlags.SilentRunning) },
                { "fuelscooping", resolveFlag(metadata.Status, StatusFlags.FuelScooping) },
                { "srvbrake", resolveFlag(metadata.Status, StatusFlags.SRVBrake) },
                { "srvturret", resolveFlag(metadata.Status, StatusFlags.SRVTurret) },
                { "srvproximity", resolveFlag(metadata.Status, StatusFlags.SRVProximity) },
                { "srvdriveassist", resolveFlag(metadata.Status, StatusFlags.SRVDriveAssist) },
                { "masslock", resolveFlag(metadata.Status, StatusFlags.Masslock) },
                { "fsdcharging", resolveFlag(metadata.Status, StatusFlags.FSDCharging) },
                { "fsdcooldown", resolveFlag(metadata.Status, StatusFlags.FSDCooldown) },
                { "lowfuel", resolveFlag(metadata.Status, StatusFlags.LowFuel) },
                { "overheat", resolveFlag(metadata.Status, StatusFlags.Overheat) },
                { "latlongvalid", resolveFlag(metadata.Status, StatusFlags.LatLongValid) },
                { "indanger", resolveFlag(metadata.Status, StatusFlags.InDanger) },
                { "interdiction", resolveFlag(metadata.Status, StatusFlags.Interdiction) },
                { "mainship", resolveFlag(metadata.Status, StatusFlags.MainShip) },
                { "fighter", resolveFlag(metadata.Status, StatusFlags.Fighter) },
                { "srv", resolveFlag(metadata.Status, StatusFlags.SRV) },
                { "nightvision", resolveFlag(metadata.Status, StatusFlags.NightVision) },
                { "radialaltitude", resolveFlag(metadata.Status, StatusFlags.RadialAltitude) },
                { "fsdjump", resolveFlag(metadata.Status, StatusFlags.FSDJump) },
                { "srvhighbeam", resolveFlag(metadata.Status, StatusFlags.SRVHighBeam) },
                // StatusFlags2
                { "onfoot", resolveFlag2(metadata.Status, StatusFlags2.OnFoot) },
                { "intaxi", resolveFlag2(metadata.Status, StatusFlags2.InTaxi) },
                { "inmulticrew", resolveFlag2(metadata.Status, StatusFlags2.InMulticrew) },
                { "onfootinstation", resolveFlag2(metadata.Status, StatusFlags2.OnFootInStation) },
                { "onfootonplanet", resolveFlag2(metadata.Status, StatusFlags2.OnFootOnPlanet) },
                { "aimdownsight", resolveFlag2(metadata.Status, StatusFlags2.AimDownSight) },
                { "lowoxygen", resolveFlag2(metadata.Status, StatusFlags2.LowOxygen) },
                { "lowhealth", resolveFlag2(metadata.Status, StatusFlags2.LowHealth) },
                { "cold", resolveFlag2(metadata.Status, StatusFlags2.Cold) },
                { "hot", resolveFlag2(metadata.Status, StatusFlags2.Hot) },
                { "verycold", resolveFlag2(metadata.Status, StatusFlags2.VeryCold) },
                { "veryhot", resolveFlag2(metadata.Status, StatusFlags2.VeryHot) },
                { "glidemode", resolveFlag2(metadata.Status, StatusFlags2.GlideMode) },
                { "onfootinhangar", resolveFlag2(metadata.Status, StatusFlags2.OnFootInHangar) },
                {
                    "onfootinsocialspace",
                    resolveFlag2(metadata.Status, StatusFlags2.OnFootInSocialSpace)
                },
                { "onfootexterior", resolveFlag2(metadata.Status, StatusFlags2.OnFootExterior) },
                {
                    "breathableatmosphere",
                    resolveFlag2(metadata.Status, StatusFlags2.BreathableAtmosphere)
                },
                {
                    "telepresencemulticrew",
                    resolveFlag2(metadata.Status, StatusFlags2.TelepresenceMulticrew)
                },
                {
                    "physicalmulticrew",
                    resolveFlag2(metadata.Status, StatusFlags2.PhysicalMulticrew)
                },
                {
                    "fsdhyperdrivecharging",
                    resolveFlag2(metadata.Status, StatusFlags2.FsdHyperdriveCharging)
                },
            };

        public static string FillTokenizedString(
            string tokenizedString,
            ImageWithMetadata metadata,
            string cmdrName = ""
        )
        {
            foreach (var kvp in lookupDict(cmdrName, metadata))
            {
                tokenizedString = tokenizedString.Replace("{" + kvp.Key + "}", kvp.Value);
            }

            // Check for timestamp tokens in format {timestamp:FORMAT}
            var timestampTokenStart = tokenizedString.IndexOf("{timestamp:");
            while (timestampTokenStart != -1)
            {
                var timestampTokenEnd = tokenizedString.IndexOf('}', timestampTokenStart);
                if (timestampTokenEnd == -1)
                    break; // No closing brace found
                var format = tokenizedString[(timestampTokenStart + 11)..timestampTokenEnd];
                var formattedTimestamp = metadata.Screenshot.TimestampDateTime.ToString(format);
                tokenizedString = string.Concat(
                    tokenizedString.AsSpan()[..timestampTokenStart],
                    formattedTimestamp,
                    tokenizedString.AsSpan(timestampTokenEnd + 1)
                );
                timestampTokenStart = tokenizedString.IndexOf(
                    "{timestamp:",
                    timestampTokenStart + formattedTimestamp.Length
                );
            }

            return tokenizedString;
        }

        public static int FindOpenQuad(MagickImage image, bool twoPass = false)
        {
            var quads = image.CropToTiles(image.Width / 2, image.Height / 2);
            var quadSizes = quads.Select(q =>
            {
                using MemoryStream qStream = new();
                q.Format = MagickFormat.Jpeg;
                q.Write(qStream);
                return qStream.Length;
            });
            long minSize = quadSizes.Min();
            var minIndex = quadSizes.ToList().IndexOf(minSize);
            if (twoPass)
            {
                var nextOrderIndex = FindOpenQuad(new MagickImage(quads[minIndex]));
                minIndex = minIndex * 4 + nextOrderIndex;
            }
            return minIndex;
        }

        private static void LocateAndComposite(
            LocationMethod locationMethod,
            Quad quad,
            SecondOrderQuad secondOrderQuad,
            Point location,
            bool secondOrder,
            MagickImage baseImage,
            MagickImage overlayImage,
            MagickImage compImage
        )
        {
            int quadInt = 0;
            if (locationMethod == LocationMethod.Automatic)
            {
                quadInt = FindOpenQuad(baseImage, secondOrder);
            }
            else if (locationMethod == LocationMethod.Quadrant)
            {
                quadInt = secondOrder ? (int)secondOrderQuad : (int)quad;
            }

            if (locationMethod == LocationMethod.Manual)
            {
                overlayImage.Composite(compImage, location.X, location.Y, CompositeOperator.Over);
            }
            else
            {
                if (secondOrder)
                {
                    var bounds = GetBoundsFromQuad(baseImage, quadInt, secondOrder);
                    overlayImage.Composite(compImage, bounds.X, bounds.Y, CompositeOperator.Over);
                }
                else
                {
                    var gravity = quadInt switch
                    {
                        3 => Gravity.Southeast,
                        2 => Gravity.Southwest,
                        1 => Gravity.Northeast,
                        _ => Gravity.Northwest,
                    };
                    overlayImage.Composite(compImage, gravity, CompositeOperator.Over);
                }
            }
        }

        public static void AddTextToImage(CaptionAction action, ImageWithMetadata imageData)
        {
            // Scale font based on image height to maintain
            // uniformity of relative size across resolutions.
            var fontScale = imageData.Image.Height / 720f;

            var captionSettings = new MagickReadSettings()
            {
                Font = action.FontPath,
                TextGravity = Gravity.Northwest,
                FillColor = action.Color,
                BackgroundColor = MagickColors.Transparent,
                FontPointsize = action.FontSize * fontScale,
            };

            using var caption = new MagickImage(
                $"label:{FillTokenizedString(action.Text, imageData, action.CmdrName)}",
                captionSettings
            );

            // Put it exactly where the user specified, otherwise
            // add padding to avoid text being right on the edge.
            if (action.LocationMethod != LocationMethod.Manual)
            {
                caption.BorderColor = MagickColors.Transparent;
                caption.Border((uint)Math.Round(10 * fontScale));
            }

            LocateAndComposite(
                action.LocationMethod,
                action.Quad,
                action.SecondOrderQuad,
                action.Location,
                action.SecondOrder,
                imageData.Image,
                imageData.Overlay,
                caption
            );
        }

        public static void AddWatermarkToImage(WatermarkAction action, ImageWithMetadata imageData)
        {
            MagickImage watermark;
            try
            {
                watermark = new(action.WatermarkImagePath);
            }
            catch (Exception ex)
            {
                // Rethrow for higher level logging and to halt further processing.
                throw new Exception(
                    $"Failed to load watermark image from path: {action.WatermarkImagePath}",
                    ex
                );
            }

            LocateAndComposite(
                action.LocationMethod,
                action.Quad,
                action.SecondOrderQuad,
                action.Location,
                action.SecondOrder,
                imageData.Image,
                imageData.Overlay,
                watermark
            );

            watermark.Dispose();
        }

        public static void Resize(ResizeAction sizeAction, ImageWithMetadata imageData)
        {
            var image = imageData.Image;
            var overlay = imageData.Overlay;
            if (sizeAction.Relative)
            {
                // Relative scaling stores percentage in sizeAction.X, ignore sizeAction.Y
                uint newHeight = (uint)(image.Height * (sizeAction.X / 100f));
                uint newWidth = (uint)(image.Width * (sizeAction.X / 100f));
                image.Resize(newWidth, newHeight);
                overlay.Resize(newWidth, newHeight);
            }
            else
            {
                image.Resize((uint)sizeAction.X, (uint)sizeAction.Y);
                overlay.Resize((uint)sizeAction.X, (uint)sizeAction.Y);
            }
        }

        public static void SaveImage(SaveAction action, ImageWithMetadata imageData)
        {
            if (action.IncludeMetadata)
            {
                EmbedMeta(imageData, action);
            }

            var image = imageData.Image;
            var overlay = imageData.Overlay;

            var filename = FillTokenizedString(action.FilePattern, imageData, action.CmdrName);
            var sanitizedCharacters = filename
                .Where(c => !Path.GetInvalidPathChars().Contains(c))
                .ToArray();

            filename = new string(sanitizedCharacters);

            if (!action.SeparateOutput)
            {
                imageData.Image.Composite(imageData.Overlay, CompositeOperator.Over);
            }
            else
            {
                imageData.Overlay.Format = MagickFormat.Png;
                imageData.Overlay.Write(Path.Combine(action.FolderPath, $"{filename}_overlay.png"));
            }

            var (extension, validExtensions) = action.Format switch
            {
                MagickFormat.Jpeg => (".jpg", new string[] { ".jpg", ".jpeg", ".jpe", ".jfif" }),
                MagickFormat.Png => (".png", [".png"]),
                MagickFormat.Heic => (".heic", [".heic", ".heif"]),
                MagickFormat.WebP => (".webp", [".webp"]),
                MagickFormat.Bmp => (".bmp", [".bmp", ".dib"]),
                _ => throw new ArgumentException("Unsupported format"),
            };

            if (
                !validExtensions.Any(ext =>
                    filename.EndsWith(ext, StringComparison.OrdinalIgnoreCase)
                )
            )
            {
                filename += extension;
            }

            var fullPath = Path.Combine(action.FolderPath, filename);
            var dirPath = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrWhiteSpace(dirPath) && !Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            image.Format = action.Format;
            image.Quality = action.Quality;
            image.Write(fullPath);
        }

        private static void EmbedMeta(ImageWithMetadata image, SaveAction action)
        {
            Dictionary<string, object?> allMeta = new()
            {
                { "Screenshot", image.Screenshot },
                { "Status", image.Status },
            };

            if (action.Format != MagickFormat.Png)
            {
                var exifData = image.Image.GetExifProfile() ?? new ExifProfile();
                exifData.SetValue(ExifTag.Software, "Observatory Photographer");
                exifData.SetValue(
                    ExifTag.ImageDescription,
                    image.Screenshot.System ?? string.Empty
                );
                exifData.SetValue(
                    ExifTag.XPSubject,
                    Encoding.Unicode.GetBytes(image.Screenshot.Body ?? string.Empty)
                );
                exifData.SetValue(ExifTag.Artist, action.CmdrName ?? string.Empty);
                exifData.SetValue(ExifTag.DateTimeOriginal, image.Screenshot.Timestamp);

                // Convert decimal coordinates to DMS format for EXIF GPS tags
                var absLat = Math.Abs(image.Screenshot.Latitude);
                Rational latDegress = new(Math.Floor(absLat));
                Rational latMinutes = new(Math.Floor(absLat * 60 % 60));
                Rational latSeconds = new(absLat * 3600 % 60);

                var absLong = Math.Abs(image.Screenshot.Longitude);
                Rational longDegress = new(Math.Floor(absLong));
                Rational longMinutes = new(Math.Floor(absLong * 60 % 60));
                Rational longSeconds = new(absLong * 3600 % 60);

                exifData.SetValue(ExifTag.GPSLatitude, [latDegress, latMinutes, latSeconds]);
                exifData.SetValue(
                    ExifTag.GPSLatitudeRef,
                    image.Screenshot.Latitude >= 0 ? "N" : "S"
                );
                exifData.SetValue(ExifTag.GPSLongitude, [longDegress, longMinutes, longSeconds]);
                exifData.SetValue(
                    ExifTag.GPSLongitudeRef,
                    image.Screenshot.Longitude >= 0 ? "E" : "W"
                );

                // Place all available metadata in UserComment as JSON
                exifData.SetValue(
                    ExifTag.UserComment,
                    [
                        // "UNICODE\0" in ASCII/UTF8 followed by the UTF16 encoded string
                        0x55,
                        0x4E,
                        0x49,
                        0x43,
                        0x4F,
                        0x44,
                        0x45,
                        0x00,
                        .. Encoding.Unicode.GetBytes(JsonSerializer.Serialize(allMeta)),
                    ]
                );

                image.Image.SetProfile(exifData);
            }
            else
            {
                image.Image.SetAttribute("Software", "Observatory Photographer");
                image.Image.SetAttribute("Author", action.CmdrName ?? string.Empty);
                image.Image.SetAttribute("Description", image.Screenshot.System);
                image.Image.SetAttribute("Create Time", image.Screenshot.Timestamp);
                image.Image.SetAttribute("ObservatoryMetadata", JsonSerializer.Serialize(allMeta));
            }
        }
    }
}
