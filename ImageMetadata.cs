using ImageMagick;
using Observatory.Framework.Files;
using Observatory.Framework.Files.Journal;

namespace Observatory.Photographer
{
    public class ImageWithMetadata : IDisposable
    {
        public ImageWithMetadata(MagickImage image, Screenshot screenshot)
        {
            _image = image;
            Screenshot = screenshot;
            _overlay = new MagickImage(MagickColors.Transparent, image.Width, image.Height);
            Filename = string.Empty;
        }

        public ImageWithMetadata(string filename, Screenshot screenshot, Status? status)
        {
            Filename = filename;
            _image = null;
            _overlay = null;
            Screenshot = screenshot;
            Status = status;
        }

        public bool HasStatus => Status is not null;

        // Screenshot metadata
        public Screenshot Screenshot { get; private set; }

        // Status metadata
        public Status? Status { get; private set; }

        public MagickImage Image 
        { 
            get
            {
                _image ??= new MagickImage(Filename);
                return _image;
            }
        }

        public MagickImage Overlay 
        { 
            get
            {
                _overlay ??= new MagickImage(MagickColors.Transparent, Image.Width, Image.Height);
                return _overlay;
            }
        }

        public void Dispose()
        {
            Image.Dispose();
            Overlay.Dispose();
            GC.SuppressFinalize(this);
        }

        public string Filename { get; private set; }
        private MagickImage? _image;
        private MagickImage? _overlay;
    }
}
