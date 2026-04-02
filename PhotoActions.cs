using ImageMagick;

namespace Observatory.Photographer
{
    public enum PhotoActionKind
    {
        Save,
        Resize,
        Caption,
        Watermark,
        Meta,
    }

    public enum LocationMethod
    {
        Manual,
        Quadrant,
        Automatic,
    }

    public abstract class PhotoAction
    {
        public abstract PhotoActionKind Action { get; }
        // public required ImageWithMetadata Metadata { get; set; }
    }

    public class MetaAction : PhotoAction
    {
        public override PhotoActionKind Action
        {
            get => PhotoActionKind.Meta;
        }
    }

    public class SaveAction : PhotoAction
    {
        public override PhotoActionKind Action
        {
            get => PhotoActionKind.Save;
        }
        public required MagickFormat Format { get; set; }
        public uint Quality { get; set; } = 75;
        public required string FolderPath { get; set; }
        public required string FilePattern { get; set; }
        public required string CmdrName { get; set; }
    }

    public class ResizeAction : PhotoAction
    {
        public override PhotoActionKind Action
        {
            get => PhotoActionKind.Resize;
        }
        public bool Relative { get; set; } = false;
        public required int X { get; set; }
        public required int Y { get; set; }
    }

    public class CaptionAction : PhotoAction
    {
        public override PhotoActionKind Action
        {
            get => PhotoActionKind.Caption;
        }
        public string Text { get; set; } = string.Empty;
        public required Font Font { get; set; }
        public MagickColor Color { get; set; } = new();
        public int QuadValue { get; set; }
        public ImageUtils.Quad Quad
        {
            get { return (ImageUtils.Quad)QuadValue; }
        }
        public ImageUtils.SecondOrderQuad SecondOrderQuad
        {
            get { return (ImageUtils.SecondOrderQuad)QuadValue; }
        }
        public Point Location { get; set; }
        public bool Relative { get; set; }
        public LocationMethod LocationMethod { get; set; }
        public bool SecondOrder { get; set; }
        public required string CmdrName { get; set; }
    }

    public class WatermarkAction : PhotoAction
    {
        public override PhotoActionKind Action
        {
            get => PhotoActionKind.Watermark;
        }
        public required string WatermarkImagePath { get; set; }
        public int QuadValue { get; set; }
        public ImageUtils.Quad Quad
        {
            get { return (ImageUtils.Quad)QuadValue; }
        }
        public ImageUtils.SecondOrderQuad SecondOrderQuad
        {
            get { return (ImageUtils.SecondOrderQuad)QuadValue; }
        }
        public Point Location { get; set; }
        public bool Relative { get; set; }
        public LocationMethod LocationMethod { get; set; }
        public bool SecondOrder { get; set; }
    }
}
