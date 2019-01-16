using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace JOBS.Web.Ui.Helper
{
    public static class ImageHelper
    {
        public static RotateFlipType GetOrientationToFlipType(int orientationValue)
        {
            RotateFlipType rotateFlipType = RotateFlipType.RotateNoneFlipNone;

            switch (orientationValue)
            {
                case 1:
                    rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                    break;
                case 2:
                    rotateFlipType = RotateFlipType.RotateNoneFlipX;
                    break;
                case 3:
                    rotateFlipType = RotateFlipType.Rotate180FlipNone;
                    break;
                case 4:
                    rotateFlipType = RotateFlipType.Rotate180FlipX;
                    break;
                case 5:
                    rotateFlipType = RotateFlipType.Rotate90FlipX;
                    break;
                case 6:
                    rotateFlipType = RotateFlipType.Rotate90FlipNone;
                    break;
                case 7:
                    rotateFlipType = RotateFlipType.Rotate270FlipX;
                    break;
                case 8:
                    rotateFlipType = RotateFlipType.Rotate270FlipNone;
                    break;
                default:
                    rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                    break;
            }

            return rotateFlipType;
        }
        public static Image RotateImage(Image img, float rotationAngle)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, new Point(0, 0));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }
        public static int MaxHeight()
        {
            //return new GlobalSettingFacade().GetImageMaxHeight();
            return  300;
        }
        public static int MaxWidth()
        {
            //return new GlobalSettingFacade().GetImageMaxWidth();
            return 300;

        }
        
        public static byte[] CropImage(byte[] content, int x, int y, int width, int height)
        {
            using (MemoryStream stream = new MemoryStream(content))
            {
                return CropImage(stream, x, y, width, height);
            }
        }

        public static byte[] CropImage(Stream content, int x, int y, int width, int height)
        {
            //Parsing stream to bitmap
            using (Bitmap sourceBitmap = new Bitmap(content))
            {
                //Get new dimensions
                double sourceWidth = Convert.ToDouble(sourceBitmap.Size.Width);
                double sourceHeight = Convert.ToDouble(sourceBitmap.Size.Height);
                Rectangle cropRect = new Rectangle(x, y, width, height);

                //Creating new bitmap with valid dimensions
                using (Bitmap newBitMap = new Bitmap(cropRect.Width, cropRect.Height))
                {
                    using (Graphics g = Graphics.FromImage(newBitMap))
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.CompositingQuality = CompositingQuality.HighQuality;

                        g.DrawImage(sourceBitmap, new Rectangle(0, 0, newBitMap.Width, newBitMap.Height), cropRect, GraphicsUnit.Pixel);

                        return GetBitmapBytes(newBitMap);
                    }
                }
            }
        }

        public static byte[] GetBitmapBytes(Bitmap source)
        {
            //Settings to increase quality of the image
            ImageCodecInfo codec = ImageCodecInfo.GetImageEncoders()[4];
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);

            //Temporary stream to save the bitmap
            using (MemoryStream tmpStream = new MemoryStream())
            {
                source.Save(tmpStream, codec, parameters);

                //Get image bytes from temporary stream
                byte[] result = new byte[tmpStream.Length];
                tmpStream.Seek(0, SeekOrigin.Begin);
                tmpStream.Read(result, 0, (int)tmpStream.Length);

                return result;
            }
        }


        public static Image CropByUserCoord(Image image, int width, int height, int x, int y)
        {
            try
            {
                if ((height + y) > image.Height)
                {
                    y = image.Height - height;
                    //height = image.Height;
                }
                if ((width + x) > image.Width)
                {
                    x = image.Width - width;
                }
                Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb); 
                //bmp.SetResolution(80, 60);
                Graphics gfx = Graphics.FromImage(bmp);
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gfx.CompositingMode = CompositingMode.SourceCopy;
                gfx.DrawImage(image, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel); 
                
                //Image watermarkImage = Image.FromFile(HttpContext.Current.Server.MapPath("~/Content/logo/watermark-logo.png"));
                //int xpoint = 0;
                //int ypoint = 0;
                //WaterMarkPosition(gfx.DpiX, gfx.DpiY, width, height, watermarkImage, out xpoint, out ypoint);
                //gfx.DrawImageUnscaled(watermarkImage, xpoint, ypoint);
                //gfx.Dispose();
                //watermarkImage.Dispose(); 
                // Dispose to free up resources
                image.Dispose();
                //bmp.Dispose();
                gfx.Dispose();
               // bmp.MakeTransparent();
                return bmp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static Image SetWaterMark(Image image)
        {
            int width = image.Width;
            int height = image.Height;
            //Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            Graphics gfx = Graphics.FromImage(image); 
            Image watermarkImage = Image.FromFile(HttpContext.Current.Server.MapPath("~/Content/logo/watermark-logo.png"));
            int xpoint = 0;
            int ypoint = 0;
            WaterMarkPosition(gfx.DpiX, gfx.DpiY, width, height, watermarkImage, out xpoint, out ypoint);
            gfx.DrawImageUnscaled(watermarkImage, xpoint, ypoint);
            gfx.Dispose();
            watermarkImage.Dispose();
            return image;
        }
        #region classnew
        /* string watermarkimage = "watermark.png";
         string soldoutStampUrl = "";
         int resizeWidth = 728;
         int resizeHeight = 497;
         bool skipHeight = false;
         int compressionLevel = 80;
         bool dontCache = true;

         private void SetImageCacheability(HttpContext context, DateTime? lastUpdatedDate = null)
         {
             int expiretime = (365 * 24 * 60);
             context.Response.ContentType = "image/jpeg";
             DateTime dateTime = (lastUpdatedDate == null || !lastUpdatedDate.HasValue || lastUpdatedDate.Value == DateTime.MinValue) ? DateTime.Now : lastUpdatedDate.Value;
             lastUpdatedDate = dateTime;
             context.Response.Cache.SetLastModified(dateTime);

             if (dontCache)
                 expiretime = 5;
             context.Response.Cache.SetCacheability(HttpCacheability.Public);
             context.Response.Cache.SetMaxAge(TimeSpan.FromMinutes(expiretime));
             context.Response.Cache.SetSlidingExpiration(true);

         }

         static byte[] GetBytes(string str)
         {
             byte[] bytes = new byte[str.Length * sizeof(char)];
             System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
             return bytes;
         }

         private void WriteToCache(string imgID, byte[] imageArray)
         {
             if (string.IsNullOrWhiteSpace(imgID) || HttpContext.Current == null) return;
             if (HttpContext.Current.Cache["imageCache"] == null)
                 HttpContext.Current.Cache["imageCache"] = new Dictionary<string, byte[]>();
             var imageCache = HttpContext.Current.Cache["imageCache"] as Dictionary<string, byte[]>;
             if (!imageCache.ContainsKey(imgID))
             {
                 imageCache.Add(imgID, imageArray);
             }
         }

         private byte[] ReadFromCache(string imgID)
         {
             if (string.IsNullOrWhiteSpace(imgID) || HttpContext.Current == null) return null;
             var imageCache = HttpContext.Current.Cache["imageCache"] as Dictionary<string, byte[]>;
             if (imageCache == null) return null;
             if (imageCache.ContainsKey(imgID))
                 return imageCache[imgID];
             else return null;
         }

         public bool IsReusable
         {
             get
             {
                 return false;
             }
         }

         public void AddStampOnImage(Image img, HttpContext context, DateTime? lastUpdatedDate)
         {

             Image modiFiedImage = GetScaledImage(img, resizeWidth, resizeHeight);
             int width = modiFiedImage.Width;
             int height = modiFiedImage.Height;
             //int compressionLevel = 100;

             Image bitmapImg = new Bitmap(width, height, modiFiedImage.PixelFormat);
             if (modiFiedImage.PixelFormat.ToString().Contains("Indexed"))
                 bitmapImg = new Bitmap(width, height, PixelFormat.Format24bppRgb);


             PropertyItem[] propItems = img.PropertyItems;

             foreach (var item in propItems)
             {
                 PropertyItem pItem = img.GetPropertyItem(item.Id);
                 bitmapImg.SetPropertyItem(pItem);
             }

             Graphics oGraphic = Graphics.FromImage(bitmapImg);
             oGraphic.CompositingQuality = CompositingQuality.HighQuality;
             oGraphic.SmoothingMode = SmoothingMode.HighQuality;
             oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
             oGraphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
             Rectangle oRectangle = new Rectangle(0, 0, width, height);
             oGraphic.DrawImage(modiFiedImage, oRectangle);

             int x;
             int y;
             Image watermarkImg = Image.FromFile(HttpContext.Current.Server.MapPath("~/images/" +   watermarkimage +  ""));
             GetWatermarkPlacement(oGraphic.DpiX, oGraphic.DpiY, width, height, watermarkImg, out x, out y);
             oGraphic.DrawImageUnscaled(watermarkImg, x, y);
             oGraphic.Dispose();
             watermarkImg.Dispose();

             long[] quality = new long[1];

             //if (compressionLevel > 100 || compressionLevel < 0)
             //    compressionLevel = 40;

             quality[0] = compressionLevel;

             EncoderParameters encoderParams = new EncoderParameters();
             EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
             encoderParams.Param[0] = encoderParam;

             ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
             ImageCodecInfo jpegICI = null;

             for (int i = 0; i < arrayICI.Length; i++  )
             {
                 if (arrayICI[i].FormatDescription.Equals("JPEG"))
                 {
                     jpegICI = arrayICI[i];
                     break;
                 }
             }

             SetImageCacheability(context, lastUpdatedDate);
             bitmapImg.Save(context.Response.OutputStream, jpegICI, encoderParams);

             encoderParam.Dispose();
         }
         public void AddSoldoutStampOnImage(Image img, HttpContext context, string thumb)
         {

             Image modiFiedImage = (thumb != null && thumb == "true") ? img : GetScaledImage(img, resizeWidth, resizeHeight);

             int width = modiFiedImage.Width;
             int height = modiFiedImage.Height;
             //int compressionLevel = 100;

             Image bitmapImg = new Bitmap(width, height, modiFiedImage.PixelFormat);
             if (modiFiedImage.PixelFormat.ToString().Contains("Indexed"))
                 bitmapImg = new Bitmap(width, height, PixelFormat.Format24bppRgb);


             PropertyItem[] propItems = img.PropertyItems;

             foreach (var item in propItems)
             {
                 PropertyItem pItem = img.GetPropertyItem(item.Id);
                 bitmapImg.SetPropertyItem(pItem);
             }

             Graphics oGraphic = Graphics.FromImage(bitmapImg);
             oGraphic.CompositingQuality = CompositingQuality.HighQuality;
             oGraphic.SmoothingMode = SmoothingMode.HighQuality;
             oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
             oGraphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
             Rectangle oRectangle = new Rectangle(0, 0, width, height);
             oGraphic.DrawImage(modiFiedImage, oRectangle);

             int x;
             int y;
             Image soldoutStamp = Image.FromFile(HttpContext.Current.Server.MapPath(soldoutStampUrl   +""));
             GetSoldoutStampPlacement(oGraphic.DpiX, oGraphic.DpiY, width, height, soldoutStamp, out x, out y);
             oGraphic.DrawImageUnscaled(soldoutStamp, x, y);
             oGraphic.Dispose();
             soldoutStamp.Dispose();

             long[] quality = new long[1];

             //if (compressionLevel > 100 || compressionLevel < 0)
             //    compressionLevel = 40;

             quality[0] = compressionLevel;

             EncoderParameters encoderParams = new EncoderParameters();
             EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
             encoderParams.Param[0] = encoderParam;

             ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
             ImageCodecInfo jpegICI = null;

             for (int i = 0; i < arrayICI.Length; i++ )
             {
                 if (arrayICI[i].FormatDescription.Equals("JPEG"))
                 {
                     jpegICI = arrayICI[i];
                     break;
                 }
             }

             SetImageCacheability(context);
             bitmapImg.Save(context.Response.OutputStream, jpegICI, encoderParams);

             encoderParam.Dispose();
         }
         public Image AddStampOnImageForTestGallerySuiteImage(Image img, int resizeWidth, int resizeHeight)
         {

             Image modiFiedImage = GetScaledImage(img, resizeWidth, resizeHeight);
             int width = modiFiedImage.Width;
             int height = modiFiedImage.Height;
             //int compressionLevel = 40;

             Image bitmapImg = new Bitmap(width, height, modiFiedImage.PixelFormat);
             if (modiFiedImage.PixelFormat.ToString().Contains("Indexed"))
                 bitmapImg = new Bitmap(width, height, PixelFormat.Format24bppRgb);

             Graphics oGraphic = Graphics.FromImage(bitmapImg);
             oGraphic.CompositingQuality = CompositingQuality.HighQuality;
             oGraphic.SmoothingMode = SmoothingMode.HighQuality;
             oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
             oGraphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
             Rectangle oRectangle = new Rectangle(0, 0, width, height);
             oGraphic.DrawImage(modiFiedImage, oRectangle);

             int x;
             int y;
             Image watermarkImg = Image.FromFile(HttpContext.Current.Server.MapPath("~/images/"+   watermarkimage +  ""));
             GetWatermarkPlacement(oGraphic.DpiX, oGraphic.DpiY, width, height, watermarkImg, out x, out y);
             oGraphic.DrawImageUnscaled(watermarkImg, x, y);
             oGraphic.Dispose();
             watermarkImg.Dispose();

             long[] quality = new long[1];
             //if (compressionLevel > 100 || compressionLevel < 0)
             //    compressionLevel = 40;

             quality[0] = compressionLevel;

             EncoderParameters encoderParams = new EncoderParameters();
             EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
             encoderParams.Param[0] = encoderParam;

             ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
             ImageCodecInfo jpegICI = null;

             for (int i = 0; i < arrayICI.Length; i ++ )
             {
                 if (arrayICI[i].FormatDescription.Equals("JPEG"))
                 {
                     jpegICI = arrayICI[i];
                     break;
                 }
             }

             return bitmapImg;
         }

         private void ResizeImage(Image img, HttpContext context, DateTime? lastUpdatedDate)
         {

             //Image modiFiedImage = GetScaledImage(img, 728, 497);
             Image modiFiedImage = GetScaledImage(img, resizeWidth, resizeHeight);
             int width = modiFiedImage.Width;
             int height = modiFiedImage.Height;
             //int compressionLevel = 40;

             Image bitmapImg = new Bitmap(width, height, modiFiedImage.PixelFormat);
             if (modiFiedImage.PixelFormat.ToString().Contains("Indexed"))
                 bitmapImg = new Bitmap(width, height, PixelFormat.Format24bppRgb);

             Graphics oGraphic = Graphics.FromImage(bitmapImg);
             oGraphic.CompositingQuality = CompositingQuality.HighQuality;
             oGraphic.SmoothingMode = SmoothingMode.HighQuality;
             oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
             oGraphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
             Rectangle oRectangle = new Rectangle(0, 0, width, height);
             oGraphic.DrawImage(modiFiedImage, oRectangle);


             long[] quality = new long[1];
             //if (compressionLevel > 100 || compressionLevel < 0)
             //    compressionLevel = 40;

             quality[0] = compressionLevel;

             EncoderParameters encoderParams = new EncoderParameters();
             EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
             encoderParams.Param[0] = encoderParam;

             ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
             ImageCodecInfo jpegICI = null;

             for (int i = 0; i < arrayICI.Length; i++)
             {
                 if (arrayICI[i].FormatDescription.Equals("JPEG"))
                 {
                     jpegICI = arrayICI[i];
                     break;
                 }
             }
             SetImageCacheability(context, lastUpdatedDate);
             bitmapImg.Save(context.Response.OutputStream, jpegICI, encoderParams);

             encoderParam.Dispose();
         }

         private void GetWatermarkPlacement(float graphicDpiX, float graphicDpiY, int finalWidth, int finalHeight, Image watermarkImg, out int x, out int y)
         {
             float propX = graphicDpiX / watermarkImg.HorizontalResolution;
             float propY = graphicDpiY / watermarkImg.VerticalResolution;

             int watermarkImgWidth = Convert.ToInt32((float)watermarkImg.Width * propX);
             int watermarkImgHeight = Convert.ToInt32((float)watermarkImg.Height * propY);
             x = 20;
             y = finalHeight - watermarkImgHeight;
         }
         private void GetSoldoutStampPlacement(float graphicDpiX, float graphicDpiY, int finalWidth, int finalHeight, Image soldoutStamp, out int x, out int y)
         {
             float propX = graphicDpiX / soldoutStamp.HorizontalResolution;
             float propY = graphicDpiY / soldoutStamp.VerticalResolution;

             int soldoutImgWidth = Convert.ToInt32((float)soldoutStamp.Width * propX);
             int soldoutImgHeight = Convert.ToInt32((float)soldoutStamp.Height * propY);

             x = (finalWidth - soldoutImgWidth) / 2;
             y = (finalHeight - soldoutImgHeight) / 2;
         }*/
        #endregion
        /// <summary>
        /// Returns Resized Image Based On Min Height / width
        /// </summary>
        /// <param name="minWidth">Min Width</param>
        /// <param name="minHeight">Min Height</param>
        /// <param name="image">Image File In Bitmap</param>
        public static Image GetImageResizeMin(float minWidth, float minHeight, Image image)
        {
            var ratioX = (double)minWidth / image.Width;
            var ratioY = (double)minHeight / image.Height;
            var ratio = Math.Max(ratioX, ratioY);
            var newWidth = (int)Math.Ceiling(image.Width * ratio);
            var newHeight = (int)Math.Ceiling(image.Height * ratio);
            var newImage = new Bitmap(newWidth , newHeight);
            Graphics thumbGraph = Graphics.FromImage(newImage);

            thumbGraph.CompositingQuality = CompositingQuality.Default;
            thumbGraph.SmoothingMode = SmoothingMode.Default;

            //thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            thumbGraph.DrawImage(image, 0, 0, newWidth, newHeight);
            thumbGraph.Dispose();

            return newImage; 
        }
        public static Bitmap GetImageResize(int maxWidth, int maxHeight, Image image)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);
            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            var newImage = new Bitmap(newWidth, newHeight);
            Graphics thumbGraph = Graphics.FromImage(newImage);

            thumbGraph.CompositingQuality = CompositingQuality.Default;
            thumbGraph.SmoothingMode = SmoothingMode.Default;

            //thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            thumbGraph.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }
        public static Bitmap GetImageResizeForHomeSlider(int maxWidth, int maxHeight, Image image)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);
            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            if (newHeight < maxHeight)
            {
                ratio = Math.Max(ratioX, ratioY);
                newHeight = (int)(image.Height * ratio);
            }
            else if (newWidth < maxWidth)
            {
                ratio = Math.Max(ratioX, ratioY);
                newWidth = (int)(image.Width * ratio);
            }
            var newImage = new Bitmap(newWidth, newHeight);
            newImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            Graphics thumbGraph = Graphics.FromImage(newImage);

            thumbGraph.CompositingMode = CompositingMode.SourceCopy;
            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbGraph.PixelOffsetMode = PixelOffsetMode.HighQuality;


            //thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            thumbGraph.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        } 
        public static string CreateThumbnail(int maxWidth, int maxHeight, string path, string savePath, Image image,string ContentType)
        {

            //var image = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(path));
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);
            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            var newImage = new Bitmap(newWidth, newHeight);
            Graphics thumbGraph = Graphics.FromImage(newImage);

            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;

            //thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic; 
            //foreach (PropertyItem pi in image.PropertyItems)
            //{
            //    if (pi.Id == 0x0112)
            //    {
            //        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //    }
            //}
            //image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            //image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            foreach (var prop in image.PropertyItems)
            {
                if (prop.Id == 0x0112) //value of EXIF
                {
                    int orientationValue = image.GetPropertyItem(prop.Id).Value[0];
                    RotateFlipType rotateFlipType = ImageHelper.GetOrientationToFlipType(orientationValue);
                    image.RotateFlip(rotateFlipType);

                    break;
                }
            }
            thumbGraph.DrawImage(image, 0, 0, newWidth, newHeight);
            image.Dispose();

            string fileRelativePath = "newsizeimages/" + maxWidth + Path.GetFileName(path);
            //newImage.Save(HttpContext.Current.Server.MapPath(savePath), newImage.RawFormat);
            //thumbGraph.Dispose();
            if (string.IsNullOrWhiteSpace(ContentType) || ContentType.ToLower() != "propertylogo")
            {
                AddWaterMarkLogo(newImage, savePath);
            }
            else
            {
                SaveImage(newImage,savePath);
            }
            thumbGraph.Dispose();
            newImage.Dispose();
            return savePath;
        }
        //private Image GetScaledImage(Image imgPhoto, int Width, int Height)
        //{
        //    int sourceWidth = imgPhoto.Width;
        //    int sourceHeight = imgPhoto.Height;
        //    int sourceX = 0;
        //    int sourceY = 0;
        //    bool isTransparent;
        //    if ((imgPhoto.Flags & 0x2) != 0)
        //    {
        //        isTransparent = true;
        //    }
        //    else
        //    {
        //        isTransparent = false;
        //    }

        //    Size newSiz = skipHeight ? CalcSize(new Size(sourceWidth, sourceHeight), new Size(Width, Height), true) : CalcNewSize(new Size(sourceWidth, sourceHeight), new Size(Width, Height), true);

        //    Bitmap bmPhoto = new Bitmap(newSiz.Width, newSiz.Height,
        //                      PixelFormat.Format24bppRgb);
        //    bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
        //                     imgPhoto.VerticalResolution);

        //    Graphics grPhoto = Graphics.FromImage(bmPhoto);

        //    grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //    grPhoto.CompositingQuality = CompositingQuality.HighQuality;
        //    grPhoto.SmoothingMode = SmoothingMode.HighQuality;
        //}
        public static void AddWaterMarkLogo(Image image, string filepath)
        {
            /*Image image = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(path));*/
            int width = image.Width;
            int height = image.Height;

            Image bitmapImg = new Bitmap(width, height, image.PixelFormat);
            if (image.PixelFormat.ToString().Contains("Indexed"))
                bitmapImg = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            PropertyItem[] propertyitems = image.PropertyItems;

            foreach (var item in propertyitems)
            {
                bitmapImg.SetPropertyItem(image.GetPropertyItem(item.Id));
            }

            Graphics oGraphics = Graphics.FromImage(bitmapImg);
            oGraphics.CompositingQuality = CompositingQuality.HighQuality;
            oGraphics.SmoothingMode = SmoothingMode.HighQuality;
            oGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            oGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle oRectanlge = new Rectangle(0, 0, width, height);
            oGraphics.DrawImage(image, oRectanlge);

            int xpoint = 0;
            int ypoint = 0;

            Image watermarkImage = Image.FromFile(HttpContext.Current.Server.MapPath("~/Content/logo/watermark-logo.png"));

            WaterMarkPosition(oGraphics.DpiX, oGraphics.DpiY, width, height, watermarkImage, out xpoint, out ypoint);

            oGraphics.DrawImageUnscaled(watermarkImage, xpoint, ypoint);
            oGraphics.Dispose();
            watermarkImage.Dispose();

            long[] quality = new long[1];
            quality[0] = 80;//compression level
            EncoderParameters encoderParamerters = new EncoderParameters();
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParamerters.Param[0] = encoderParam;

            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegICI = null;

            for (int i = 0; i < arrayICI.Length; i++)
            {
                if (arrayICI[i].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[i];
                    break;
                }
            }
            bitmapImg.Save(HttpContext.Current.Server.MapPath(filepath), bitmapImg.RawFormat);
            encoderParamerters.Dispose();
            //Save
        }
        public static void AddWaterMarkLogo(string path, string filepath)
        {
            Image image = System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(path));
            int width = image.Width;
            int height = image.Height;

            Image bitmapImg = new Bitmap(width, height, image.PixelFormat);
            if (image.PixelFormat.ToString().Contains("Indexed"))
                bitmapImg = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            PropertyItem[] propertyitems = image.PropertyItems;

            foreach (var item in propertyitems)
            {
                bitmapImg.SetPropertyItem(image.GetPropertyItem(item.Id));
            }

            Graphics oGraphics = Graphics.FromImage(bitmapImg);
            oGraphics.CompositingQuality = CompositingQuality.HighQuality;
            oGraphics.SmoothingMode = SmoothingMode.HighQuality;
            oGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            oGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle oRectanlge = new Rectangle(0, 0, width, height);
            oGraphics.DrawImage(image, oRectanlge);

            int xpoint = 0;
            int ypoint = 0;

            Image watermarkImage = Image.FromFile(HttpContext.Current.Server.MapPath("~/Content/logo/watermark-logo.png"));

            WaterMarkPosition(oGraphics.DpiX, oGraphics.DpiY, width, height, watermarkImage, out xpoint, out ypoint);

            oGraphics.DrawImageUnscaled(watermarkImage, xpoint, ypoint);
            oGraphics.Dispose();
            watermarkImage.Dispose();

            long[] quality = new long[1];
            quality[0] = 80;//compression level
            EncoderParameters encoderParamerters = new EncoderParameters();
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParamerters.Param[0] = encoderParam;

            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegICI = null;

            for (int i = 0; i < arrayICI.Length; i++)
            {
                if (arrayICI[i].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[i];
                    break;
                }
            }
            bitmapImg.Save(HttpContext.Current.Server.MapPath(filepath), bitmapImg.RawFormat);
            encoderParamerters.Dispose();
            //Save
        }
        public static void SaveImage(Image image,string filepath)
        {
            long[] quality = new long[1];
            quality[0] = 80;//compression level
            EncoderParameters encoderParamerters = new EncoderParameters();
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParamerters.Param[0] = encoderParam;

            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegICI = null;

            for (int i = 0; i < arrayICI.Length; i++)
            {
                if (arrayICI[i].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[i];
                    break;
                }
            }
            image.Save(HttpContext.Current.Server.MapPath(filepath), image.RawFormat);
            encoderParamerters.Dispose();
        }
        private static void WaterMarkPosition(float graphicsDpiX, float graphicsDpiY, int finalWidth, int finalHeight, Image watermarkImage, out int x, out int y)
        {
            float propx = graphicsDpiX / watermarkImage.HorizontalResolution;
            float propy = graphicsDpiY / watermarkImage.HorizontalResolution;

            int watermarkWidth = Convert.ToInt32((float)watermarkImage.Width * propx);
            int watermarkHeight = Convert.ToInt32((float)watermarkImage.Height * propy);

            x = (finalWidth / 2) - (watermarkWidth / 2);
            y = (finalHeight / 2) - (watermarkHeight / 2);

        }

        
    }
    public static class ExifPatcher
    {
        public static Stream PatchAwayExif(Stream inStream, Stream outStream)
        {
            byte[] jpegHeader = new byte[2];
            jpegHeader[0] = (byte)inStream.ReadByte();
            jpegHeader[1] = (byte)inStream.ReadByte();
            if (jpegHeader[0] == 0xff && jpegHeader[1] == 0xd8)
            {
                SkipExifSection(inStream);
            }

            outStream.WriteByte(0xff);
            outStream.WriteByte(0xd8);

            int readCount;
            byte[] readBuffer = new byte[4096];
            while ((readCount = inStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                outStream.Write(readBuffer, 0, readCount);

            return outStream;
        }

        private static void SkipExifSection(Stream inStream)
        {
            byte[] header = new byte[2];
            header[0] = (byte)inStream.ReadByte();
            header[1] = (byte)inStream.ReadByte();
            if (header[0] == 0xff && header[1] == 0xe1)
            {
                int exifLength = inStream.ReadByte();
                exifLength = exifLength << 8;
                exifLength |= inStream.ReadByte();

                for (int i = 0; i < exifLength - 2; i++)
                {
                    inStream.ReadByte();
                }
            }
        }
    }
}