using Observatory.Framework.Files.Journal;
using Observatory.Framework.Files;
using ImageMagick;

namespace Observatory.Photographer
{
    public class ImageWithMetadata
    {
        public ImageWithMetadata(MagickImage image, Screenshot screenshot)
        {
            Image = image;
            Screenshot = screenshot;
        }

        public ImageWithMetadata(MagickImage image, Screenshot screenshot, Status? status)
        {
            Image = image;
            Screenshot = screenshot;
            Status = status;
        }

        public bool HasStatus => Status is not null;

        // Screenshot metadata
        public Screenshot Screenshot { get; private set; }

        // Status metadata
        public Status? Status { get; private set; }

        public MagickImage Image { get; private set; }
    }
}
