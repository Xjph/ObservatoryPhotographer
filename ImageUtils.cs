using ImageMagick;
using ImageMagick.Drawing;

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

        public static void PerformPhotoActions(MagickImage image, List<PhotoAction> actions)
        {
            if (actions.Count == 0 || actions.Last().Action != PhotoActionKind.Save) 
                throw new ArgumentException("Final Action Must Be SaveAction");
          
            foreach (var action in actions)
            {
                switch (action)
                {
                    case WatermarkAction watermarkAction:
                        AddWatermarkToImage(image, watermarkAction);
                        break;
                    case CaptionAction captionAction:
                        AddTextToImage(image, captionAction);
                        break;
                    case ResizeAction sizeAction:
                        Resize(image, sizeAction);
                        break;
                    case SaveAction saveAction:
                        SaveImage(image, saveAction);
                        break;
                }
            }
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

        public static MagickImage AddTextToImage(MagickImage image, CaptionAction action)
        {
            int quad = 0;
            var captionSettings = new MagickReadSettings()
            {
                Font = action.Font.Name,
                TextGravity = Gravity.Northwest,
                BackgroundColor = MagickColors.Transparent,
                FontPointsize = action.Font.SizeInPoints,
            };

            using var caption = new MagickImage($"caption:{action.Text}", captionSettings);

            if (action.LocationMethod == LocationMethod.Automatic)
            {
                quad = FindOpenQuad(image, action.SecondOrder);
            }
            else if (action.LocationMethod == LocationMethod.Quadrant)
            {
                quad = action.SecondOrder
                    ? (int)action.SecondOrderQuad
                    : (int)action.Quad;
            }

            int x, y;
            if (action.LocationMethod == LocationMethod.Manual)
            {
                x = action.Location.X;
                y = action.Location.Y;
            }
            else
            {
                if (action.SecondOrder)
                {
                    var bounds = GetBoundsFromQuad(image, quad, action.SecondOrder);
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
                    image.Composite(caption, gravity, CompositeOperator.Over);

                    return image;
                }
            }
            image.Composite(caption, x, y, CompositeOperator.Over);
            return image;
        }

        public static MagickImage AddWatermarkToImage(MagickImage image, WatermarkAction action)
        {
            int quad = 0;
            if (action.LocationMethod == LocationMethod.Automatic)
            {
                quad = FindOpenQuad(image, action.SecondOrder);
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
            }
            else
            {
                if (action.SecondOrder)
                {
                    var bounds = GetBoundsFromQuad(image, quad, action.SecondOrder);
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
                    image.Composite(watermark, gravity);
                    return image;
                }
            }

            image.Composite(watermark, x, y);
            return image;
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

        public static MagickImage Resize(MagickImage image, ResizeAction sizeAction)
        {
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
            return image;
        }

        public static void SaveImage(MagickImage image, SaveAction action)
        {
            image.Format = action.Format;
            image.Quality = action.Quality;
            image.Write(action.Path);
        }
    }
}
