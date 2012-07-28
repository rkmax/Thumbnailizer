using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace Thumbnailizer
{
    /// <summary>
    /// Clase auxiliar para el procesamiento de imagenes
    /// </summary>
    internal class ImageProcess
    {
        private static ImageCodecInfo[] _imageEncoder = ImageCodecInfo.GetImageEncoders();

        /// <summary>
        /// Callback cuando falla la creacion de la imagen
        /// </summary>
        /// <returns>siempre falso</returns>
        public static bool CallBack() { return false; }

        /// <summary>
        /// Crea un thumbnail a partir de una Image
        /// </summary>
        /// <param name="imageOriginal">Image a convertir</param>
        /// <param name="thumbPath">ruta donde se va a guardar el thumbnail</param>
        /// <param name="thumbWidth">Ancho del thumbnail</param>
        /// <param name="thumbHeight">Alto del thumbnail</param>
        /// <param name="quality">Calidad</param>
        public static void GenerateThumbnail(
            Image imageOriginal, string thumbPath, int thumbWidth = 0, int thumbHeight = 0, long quality = 90L)
        {
            if (thumbHeight == 0 && thumbWidth == 0) return;

            if (thumbWidth == 0)
            {
                thumbWidth = (int)((double)thumbHeight / (double)imageOriginal.Height * imageOriginal.Width);
            }

            if (thumbHeight == 0)
            {
                thumbHeight = (int)((double)thumbWidth / (double)imageOriginal.Width * imageOriginal.Height);
            }

            using (Image thumbnail = new Bitmap(thumbWidth, thumbHeight))
            {
                Graphics graphic = Graphics.FromImage(thumbnail);

                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.DrawImage(imageOriginal, 0, 0, thumbWidth, thumbHeight);

                var info = GetCodec(imageOriginal);

                EncoderParameters encoderParameters = new EncoderParameters(2);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                encoderParameters.Param[1] = new EncoderParameter(Encoder.Compression, (long)EncoderValue.CompressionCCITT4);

                // Cierra la imagen original y elimina la imagen de la ruta
                imageOriginal.Dispose();
                if (File.Exists(thumbPath)) File.Delete(thumbPath);

                thumbnail.Save(thumbPath, info, encoderParameters);
            }
        }

        /// <summary>
        /// Obtiene el ImageCodecInfo de la imagen pasada
        /// </summary>
        /// <param name="image">imagen</param>
        /// <returns>Informacion del codec</returns>
        public static ImageCodecInfo GetCodec(Image image)
        {
            ImageCodecInfo result = null;

            foreach (var item in _imageEncoder)
            {
                if (item.FormatID == image.RawFormat.Guid)
                    result = item;
            }

            return result;
        }

        /// <summary>
        /// Carga una imagen desde la ruta deseada
        /// </summary>
        /// <param name="path">ruta deseada</param>
        /// <returns>regresa la imagen como BitmapImage</returns>
        public static BitmapImage LoadBitmapSourceFromStringPath(string path)
        {
            return new BitmapImage(new System.Uri(path));
        }

        /// <summary>
        /// Carga una imagen desde la ruta deseada
        /// </summary>
        /// <param name="path">ruta deseada</param>
        /// <returns>regresa la imagen como System.Drawing.Image</returns>
        public static Image LoadImageFromStringPath(string path)
        {
            return Image.FromFile(path);
        }
    }
}