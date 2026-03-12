using ImageMagick;
using ImageMagick.Drawing;
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
            BottomRight = 3
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
            BottomRightOfBottomRight = 15
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

        public static void PerformPhotoActions(List<PhotoAction> actions, ImageWithMetadata imageData)
        {
            if (actions.Count == 0 || actions.Last().Action != PhotoActionKind.Save) 
                throw new ArgumentException("Final Action Must Be SaveAction");

            bool embedMeta = false;

            foreach (var action in actions)
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
                    case MetaAction metaAction:
                        embedMeta = true;
                        // EmbedMetadata(metaAction);
                        break;
                    case SaveAction saveAction:
                        SaveImage(saveAction, imageData, embedMeta);
                        break;
                }
            }
        }

        public static string FillTokenizedString(string tokenizedString, ImageWithMetadata metadata, string cmdrName = "")
        {
            string resolveFlag(Framework.Files.Status? status, StatusFlags flag)
            {
                if (status is null) return string.Empty;
                return flag switch
                {
                    StatusFlags.Docked => status.Flags.HasFlag(StatusFlags.Docked) ? "Docked" : "Undocked",
                    StatusFlags.Landed => status.Flags.HasFlag(StatusFlags.Landed) ? "Landed" : "In Flight",
                    StatusFlags.LandingGear => status.Flags.HasFlag(StatusFlags.LandingGear) ? "Down" : "Raised",
                    StatusFlags.Shields => status.Flags.HasFlag(StatusFlags.Shields) ? "Up" : "Down",
                    StatusFlags.Supercruise => status.Flags.HasFlag(StatusFlags.Supercruise) ? "Supercruise" : "Normal Space",
                    StatusFlags.FAOff => status.Flags.HasFlag(StatusFlags.FAOff) ? "Off" : "On",
                    StatusFlags.Hardpoints => status.Flags.HasFlag(StatusFlags.Hardpoints) ? "Deployed" : "Retracted",
                    StatusFlags.Wing => status.Flags.HasFlag(StatusFlags.Wing) ? "Wing" : "Solo",
                    StatusFlags.Lights => status.Flags.HasFlag(StatusFlags.Lights) ? "On" : "Off",
                    StatusFlags.CargoScoop => status.Flags.HasFlag(StatusFlags.CargoScoop) ? "Deployed" : "Retracted",
                    StatusFlags.SilentRunning => status.Flags.HasFlag(StatusFlags.SilentRunning) ? "Silent Running" : "",
                    StatusFlags.FuelScooping => status.Flags.HasFlag(StatusFlags.FuelScooping) ? "Scooping" : "",
                    StatusFlags.SRVBrake => status.Flags.HasFlag(StatusFlags.SRVBrake) ? "On" : "Off",
                    StatusFlags.SRVTurret => status.Flags.HasFlag(StatusFlags.SRVTurret) ? "Active" : "Fixed",
                    StatusFlags.SRVProximity => status.Flags.HasFlag(StatusFlags.SRVProximity) ? "Close" : "Clear",
                    StatusFlags.SRVDriveAssist => status.Flags.HasFlag(StatusFlags.SRVDriveAssist) ? "On" : "Off",
                    StatusFlags.Masslock => status.Flags.HasFlag(StatusFlags.Masslock) ? "Mass Locked" : "",
                    StatusFlags.FSDCharging => status.Flags.HasFlag(StatusFlags.FSDCharging) ? "FSD Charging" : "FSD Idle",
                    StatusFlags.FSDCooldown => status.Flags.HasFlag(StatusFlags.FSDCooldown) ? "FSD Cooldown" : "FSD Ready",
                    StatusFlags.LowFuel => status.Flags.HasFlag(StatusFlags.LowFuel) ? "Low Fuel" : "Fuel OK",
                    StatusFlags.Overheat => status.Flags.HasFlag(StatusFlags.Overheat) ? "Overheating" : "Temperature Normal",
                    StatusFlags.LatLongValid => status.Flags.HasFlag(StatusFlags.LatLongValid) ? "Lat/Long Valid" : "Lat/Long Invalid",
                    StatusFlags.InDanger => status.Flags.HasFlag(StatusFlags.InDanger) ? "In Danger" : "",
                    StatusFlags.Interdiction => status.Flags.HasFlag(StatusFlags.Interdiction) ? "Interdicted" : "",
                    StatusFlags.MainShip => status.Flags.HasFlag(StatusFlags.MainShip) ? "Ship" : "",
                    StatusFlags.Fighter => status.Flags.HasFlag(StatusFlags.Fighter) ? "Fighter" : "",
                    StatusFlags.SRV => status.Flags.HasFlag(StatusFlags.SRV) ? "SRV" : "",
                    StatusFlags.AnalysisHUD => status.Flags.HasFlag(StatusFlags.AnalysisHUD) ? "Analysis" : "Combat",
                    StatusFlags.NightVision => status.Flags.HasFlag(StatusFlags.NightVision) ? "On" : "Off",
                    StatusFlags.RadialAltitude => status.Flags.HasFlag(StatusFlags.RadialAltitude) ? "Radial" : "Terrain",
                    StatusFlags.FSDJump => status.Flags.HasFlag(StatusFlags.FSDJump) ? "FSD Jumping" : "",
                    StatusFlags.SRVHighBeam => status.Flags.HasFlag(StatusFlags.SRVHighBeam) ? "On" : "Off",
                    _ => string.Empty,
                };
            }

            string resolveFlag2(Framework.Files.Status? status, StatusFlags2 flag)
            {
                if (status is null) return string.Empty;
                return flag switch
                {
                    StatusFlags2.OnFoot => status.Flags2.HasFlag(StatusFlags2.OnFoot) ? "On Foot" : "",
                    StatusFlags2.InTaxi => status.Flags2.HasFlag(StatusFlags2.InTaxi) ? "In Taxi" : "",
                    StatusFlags2.InMulticrew => status.Flags2.HasFlag(StatusFlags2.InMulticrew) ? "In Multicrew" : "",
                    StatusFlags2.OnFootInStation => status.Flags2.HasFlag(StatusFlags2.OnFootInStation) ? "In Station" : "",
                    StatusFlags2.OnFootOnPlanet => status.Flags2.HasFlag(StatusFlags2.OnFootOnPlanet) ? "On Planet" : "",
                    StatusFlags2.AimDownSight => status.Flags2.HasFlag(StatusFlags2.AimDownSight) ? "Aiming Down Sight" : "",
                    StatusFlags2.LowOxygen => status.Flags2.HasFlag(StatusFlags2.LowOxygen) ? "Low Oxygen" : "",
                    StatusFlags2.LowHealth => status.Flags2.HasFlag(StatusFlags2.LowHealth) ? "Low Health" : "",
                    StatusFlags2.Cold => status.Flags2.HasFlag(StatusFlags2.Cold) ? "Cold" : "",
                    StatusFlags2.Hot => status.Flags2.HasFlag(StatusFlags2.Hot) ? "Hot" : "",
                    StatusFlags2.VeryCold => status.Flags2.HasFlag(StatusFlags2.VeryCold) ? "Very Cold" : "",
                    StatusFlags2.VeryHot => status.Flags2.HasFlag(StatusFlags2.VeryHot) ? "Very Hot" : "",
                    StatusFlags2.GlideMode => status.Flags2.HasFlag(StatusFlags2.GlideMode) ? "Gliding" : "",
                    StatusFlags2.OnFootInHangar => status.Flags2.HasFlag(StatusFlags2.OnFootInHangar) ? "In Hangar" : "",
                    StatusFlags2.OnFootInSocialSpace => status.Flags2.HasFlag(StatusFlags2.OnFootInSocialSpace) ? "In Social Space" : "",
                    StatusFlags2.OnFootExterior => status.Flags2.HasFlag(StatusFlags2.OnFootExterior) ? "Exterior" : "",
                    StatusFlags2.BreathableAtmosphere => status.Flags2.HasFlag(StatusFlags2.BreathableAtmosphere) ? "Breathable Atmosphere" : "",
                    StatusFlags2.TelepresenceMulticrew => status.Flags2.HasFlag(StatusFlags2.TelepresenceMulticrew) ? "Telepresence" : "",
                    StatusFlags2.PhysicalMulticrew => status.Flags2.HasFlag(StatusFlags2.PhysicalMulticrew) ? "Physical Multicrew" : "",
                    StatusFlags2.FsdHyperdriveCharging => status.Flags2.HasFlag(StatusFlags2.FsdHyperdriveCharging) ? "FSD Charging" : "",
                    _ => string.Empty,
                };
            }

            Dictionary<string, string> tokenLookup = new(StringComparer.InvariantCultureIgnoreCase)
            {
                { "cmdr", cmdrName ?? string.Empty },

                // Screenshot properties
                { "latitude", metadata.Screenshot.Latitude.ToString() },
                { "longitude", metadata.Screenshot.Longitude.ToString() },
                { "system", metadata.Screenshot.System ?? string.Empty },
                { "body", metadata.Screenshot.Body ?? string.Empty },
                { "altitude", metadata.Screenshot.Altitude.ToString() },
                { "heading", metadata.Screenshot.Heading.ToString() },
                { "timestamp", metadata.Screenshot.TimestampDateTime.ToString("s").Replace(':', '-') },

                // Status properties
                { "guifocus", metadata.Status?.GuiFocus.ToString() ?? string.Empty },
                { "balance", metadata.Status?.Balance.ToString() ?? string.Empty },
                { "cargo", metadata.Status?.Cargo.ToString("N0") ?? string.Empty },
                { "mainfuel", metadata.Status?.Fuel.FuelMain.ToString("N1") ?? string.Empty },
                { "reservoirfuel", metadata.Status?.Fuel.FuelReservoir.ToString("N1") ?? string.Empty },
                { "health", (metadata.Status?.Health * 100)?.ToString("N0") ?? string.Empty },
                { "oxygen", (metadata.Status?.Oxygen * 100)?.ToString("N0") ?? string.Empty },
                { "destination", metadata.Status?.Destination.Name ?? string.Empty },
                { "gravity-g", metadata.Status?.Gravity.ToString("N2") ?? string.Empty },
                { "gravity-mps2", (metadata.Status?.Gravity * 9.81)?.ToString("N2") ?? string.Empty },
                { "legalstate", metadata.Status?.LegalState.ToString() ?? string.Empty },
                { "radius", (metadata.Status?.PlanetRadius / 1000)?.ToString("N0") ?? string.Empty },
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
                { "onfootinsocialspace", resolveFlag2(metadata.Status, StatusFlags2.OnFootInSocialSpace) },
                { "onfootexterior", resolveFlag2(metadata.Status, StatusFlags2.OnFootExterior) },
                { "breathableatmosphere", resolveFlag2(metadata.Status, StatusFlags2.BreathableAtmosphere) },
                { "telepresencemulticrew", resolveFlag2(metadata.Status, StatusFlags2.TelepresenceMulticrew) },
                { "physicalmulticrew", resolveFlag2(metadata.Status, StatusFlags2.PhysicalMulticrew) },
                { "fsdhyperdrivecharging", resolveFlag2(metadata.Status, StatusFlags2.FsdHyperdriveCharging) },
            };

            foreach (var kvp in tokenLookup)
            {
                tokenizedString = tokenizedString.Replace("{" + kvp.Key + "}", kvp.Value);
            }

            // Finally check for timestamp tokens in format {timestamp:FORMAT}
            var timestampTokenStart = tokenizedString.IndexOf("{timestamp:");
            while (timestampTokenStart != -1)
            {
                var timestampTokenEnd = tokenizedString.IndexOf('}', timestampTokenStart);
                if (timestampTokenEnd == -1) break; // No closing brace found
                var format = tokenizedString[(timestampTokenStart + 11)..timestampTokenEnd];
                var formattedTimestamp = metadata.Screenshot.TimestampDateTime.ToString(format);
                tokenizedString = string.Concat(
                    tokenizedString.AsSpan()[..timestampTokenStart], 
                    formattedTimestamp, 
                    tokenizedString.AsSpan(timestampTokenEnd + 1));
                timestampTokenStart = tokenizedString.IndexOf("{timestamp:", timestampTokenStart + formattedTimestamp.Length);
            }

            return tokenizedString;
        }

        public static void EmbedMetadata(MetaAction metaAction, string filename)
        {
            var exifData = new CompactExifLib.ExifData(filename);
            exifData.SetTagValue(CompactExifLib.ExifTag.Software, "Observatory Photographer", CompactExifLib.StrCoding.Utf8);
            exifData.Save(filename);
            // metaAction.Metadata.Image.SetAttribute("Exif:Software", "Observatory Photographer");
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

        public static void AddTextToImage(CaptionAction action, ImageWithMetadata imageData)
        {
            int quad = 0;
            var captionSettings = new MagickReadSettings()
            {
                Font = action.Font.Name,
                TextGravity = Gravity.Northwest,
                BackgroundColor = MagickColors.Transparent,
                FontPointsize = action.Font.SizeInPoints,
            };

            using var caption = new MagickImage($"caption:{FillTokenizedString(action.Text, imageData, action.CmdrName)}", captionSettings);

            if (action.LocationMethod == LocationMethod.Automatic)
            {
                quad = FindOpenQuad(imageData.Image, action.SecondOrder);
            }
            else if (action.LocationMethod == LocationMethod.Quadrant)
            {
                quad = action.SecondOrder
                    ? (int)action.SecondOrderQuad
                    : (int)action.Quad;
            }

            int x = 0, y = 0;
            if (action.LocationMethod == LocationMethod.Manual)
            {
                x = action.Location.X;
                y = action.Location.Y;
            }
            else
            {
                if (action.SecondOrder)
                {
                    var bounds = GetBoundsFromQuad(imageData.Image, quad, action.SecondOrder);
                    x = bounds.X;
                    y = bounds.Y;
                }
                else
                {
                    var gravity = quad switch
                    {
                        3 => Gravity.Southeast,
                        2 => Gravity.Southwest,
                        1 => Gravity.Northeast,
                        _ => Gravity.Northwest,
                    };
                    imageData.Image.Composite(caption, gravity, CompositeOperator.Over);
                }
            }
            imageData.Image.Composite(caption, x, y, CompositeOperator.Over);
        }

        public static void AddWatermarkToImage(WatermarkAction action, ImageWithMetadata imageData)
        {
            int quad = 0;
            if (action.LocationMethod == LocationMethod.Automatic)
            {
                quad = FindOpenQuad(imageData.Image, action.SecondOrder);
            }
            else if (action.LocationMethod == LocationMethod.Quadrant)
            {
                quad = action.SecondOrder
                    ? (int)action.SecondOrderQuad
                    : (int)action.Quad;
            }

            int x, y;
            MagickImage watermark = new(action.WatermarkImagePath);

            if (action.LocationMethod == LocationMethod.Manual)
            {
                x = action.Location.X; 
                y = action.Location.Y;
                imageData.Image.Composite(watermark, x, y);
            }
            else
            {
                if (action.SecondOrder)
                {
                    var bounds = GetBoundsFromQuad(imageData.Image, quad, action.SecondOrder);
                    x = bounds.X;
                    y = bounds.Y;
                    imageData.Image.Composite(watermark, x, y);
                }
                else
                {
                    var gravity = quad switch
                    {
                        3 => Gravity.Southeast,
                        2 => Gravity.Southwest,
                        1 => Gravity.Northeast,
                        _ => Gravity.Northwest,
                    };
                    imageData.Image.Composite(watermark, gravity);
                }
            }
        }

        public static MagickImage DrawBox(MagickImage image, Rectangle bounds)
        {
            var d = new Drawables()
                .FillColor(MagickColors.Transparent)
                .StrokeColor(MagickColors.OrangeRed)
                .StrokeWidth(5)
                .Rectangle(bounds.X, bounds.Y, bounds.X + bounds.Width, bounds.Y + bounds.Height);
            image.Draw(d);
            return image;
        }

        public static void Resize(ResizeAction sizeAction, ImageWithMetadata imageData)
        {
            var image = imageData.Image;
            if (sizeAction.Relative)
            {
                uint newHeight = (uint)(image.Height * (sizeAction.Y / 100));
                uint newWidth = (uint)(image.Width * (sizeAction.X / 100));
                image.Resize(newWidth, newHeight);
            }
            else
            {
                image.Resize((uint)sizeAction.X, (uint)sizeAction.Y);
            }
        }

        public static void SaveImage(SaveAction action, ImageWithMetadata imageData, bool embedMeta)
        {
            var image = imageData.Image;
            image.Format = action.Format;
            image.Quality = action.Quality;
            var filename = FillTokenizedString(action.FilePattern, imageData, action.CmdrName);
            var sanitizedCharacters = filename.Where(c => !Path.GetInvalidFileNameChars().Contains(c)).ToArray();
            filename = new string(sanitizedCharacters);

            var (extension, validExtensions) = action.Format switch
            {
                MagickFormat.Jpeg => (".jpg", new string[] { ".jpg", ".jpeg", ".jpe", ".jfif" }),
                MagickFormat.Png => (".png", [".png"]),
                MagickFormat.Heic => (".heic", [".heic", ".heif"]),
                MagickFormat.WebP => (".webp", [".webp"]),
                MagickFormat.Bmp => (".bmp", [ ".bmp", ".dib" ]),
                _ => throw new ArgumentException("Unsupported format")
            };

            if (!validExtensions.Any(ext => filename.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
            {
                filename += extension;
            }

            var fullPath = action.FolderPath + Path.DirectorySeparatorChar + filename;

            image.Write(fullPath);

            if (embedMeta)
            {
                var exifData = new CompactExifLib.ExifData(fullPath);
                exifData.SetTagValue(CompactExifLib.ExifTag.Software, "Observatory Photographer", CompactExifLib.StrCoding.Utf8);
                exifData.SetTagValue(CompactExifLib.ExifTag.DocumentName, imageData.Screenshot.Body, CompactExifLib.StrCoding.Utf8);
                exifData.SetDateTaken(imageData.Screenshot.TimestampDateTime);
                exifData.SetGpsDateTimeStamp(imageData.Screenshot.TimestampDateTime);
                exifData.SetGpsLatitude(CompactExifLib.GeoCoordinate.FromDecimal((decimal)imageData.Screenshot.Latitude, true));
                exifData.SetGpsLongitude(CompactExifLib.GeoCoordinate.FromDecimal((decimal)imageData.Screenshot.Longitude, false));
                exifData.SetGpsAltitude((decimal)imageData.Screenshot.Altitude);
                exifData.SetTagValue(CompactExifLib.ExifTag.XpTitle, imageData.Screenshot.System, CompactExifLib.StrCoding.Utf8);
                exifData.SetTagValue(CompactExifLib.ExifTag.XpSubject, imageData.Screenshot.Body ?? string.Empty, CompactExifLib.StrCoding.Utf8);
                exifData.SetTagValue(CompactExifLib.ExifTag.XpAuthor, action.CmdrName ?? string.Empty, CompactExifLib.StrCoding.Utf8);
                // exifData.SetTagValue(CompactExifLib.ExifTag.UserComment, jsonMeta, CompactExifLib.StrCoding.Utf8);
                exifData.Save(fullPath);
            }
        }
    }
}
