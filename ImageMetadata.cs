using ImageMagick;
using Observatory.Framework.Files;
using Observatory.Framework.Files.Journal;

namespace Observatory.Photographer
{
    public class ImageWithMetadata
    {
        public ImageWithMetadata(MagickImage image, Screenshot screenshot)
        {
            Image = image;
            Screenshot = screenshot;
            Overlay = new MagickImage(MagickColors.Transparent, image.Width, image.Height);
        }

        public ImageWithMetadata(MagickImage image, Screenshot screenshot, Status? status)
        {
            Image = image;
            Screenshot = screenshot;
            Status = status;
            Overlay = new MagickImage(MagickColors.Transparent, image.Width, image.Height);
        }

        public bool HasStatus => Status is not null;

        // Screenshot metadata
        public Screenshot Screenshot { get; private set; }

        // Status metadata
        public Status? Status { get; private set; }

        public MagickImage Image { get; private set; }

        public MagickImage Overlay { get; private set; }
    }
}
