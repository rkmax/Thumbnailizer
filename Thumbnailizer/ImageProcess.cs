﻿
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System;

namespace Thumbnailizer
{
    /// <summary>
    /// Clase auxiliar para el procesamiento de imagenes
    /// </summary>
    class ImageProcess
    {
        /// <summary>
        /// Crea un thumbnail a partir de una Image
        /// </summary>
        /// <param name="imageOriginal">Image a convertir</param>
        /// <param name="thumbPath">ruta donde se va a guardar el thumbnail</param>
        /// <param name="thumbWidth">Ancho del thumbnail</param>
        /// <param name="thumbHeight">Alto del thumbnail</param>
        /// <param name="quality">Calidad</param>
        public static void GenerateThumbnail(
            Image imageOriginal, string thumbPath, int thumbWidth = 0, int thumbHeight = 0, long quality = 100L)
        {
            if (thumbHeight == 0 && thumbWidth == 0) return;

            if (thumbWidth == 0)
            {
                thumbWidth = (int) ((double)thumbHeight / (double)imageOriginal.Height * imageOriginal.Width);
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

                ImageCodecInfo[] info;
                info = ImageCodecInfo.GetImageEncoders();
                EncoderParameters encoderParameters;
                encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                thumbnail.Save(thumbPath, info[1], encoderParameters);
            }            
        }

        public static BitmapImage LoadBitmapSourceFromStringPath(string path)
        {
            return new BitmapImage(new System.Uri(path));
        }

        public static Image LoadImageFromStringPath(string path)
        {
            return Image.FromFile(path);
        }
           
    }
}
