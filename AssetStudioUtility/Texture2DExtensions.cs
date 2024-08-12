using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace AssetStudio
{
    public static class Texture2DExtensions
    {
        public static Image<Bgra32> ConvertToImage(this Texture2D m_Texture2D, bool flip)
        {
            var converter = new Texture2DConverter(m_Texture2D);
            var uncroppedSize = converter.GetUncroppedSize();
            var buff = BigArrayPool<byte>.Shared.Rent(converter.OutputDataSize);
            try
            {
                if (!converter.DecodeTexture2D(buff)) 
                    return null;

                Image<Bgra32> image;
                if (converter.UsesSwitchSwizzle)
                {
                    image = Image.LoadPixelData<Bgra32>(buff, uncroppedSize.Width, uncroppedSize.Height);
                    image.Mutate(x => x.Crop(m_Texture2D.m_Width, m_Texture2D.m_Height));
                }
                else
                {
                    image = Image.LoadPixelData<Bgra32>(buff, m_Texture2D.m_Width, m_Texture2D.m_Height);
                }

                if (flip)
                {
                    image.Mutate(x => x.Flip(FlipMode.Vertical));
                }
                return image;
            }
            finally
            {
                BigArrayPool<byte>.Shared.Return(buff, clearArray: true);
            }
        }

        public static MemoryStream ConvertToStream(this Texture2D m_Texture2D, ImageFormat imageFormat, bool flip)
        {
            var image = ConvertToImage(m_Texture2D, flip);
            if (image != null)
            {
                using (image)
                {
                    return image.ConvertToStream(imageFormat);
                }
            }
            return null;
        }
    }
}
