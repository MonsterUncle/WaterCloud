using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using WaterCloud.Code.Images;

namespace WaterCloud.Code.QrCode
{
    /// <summary>
    ///  二维码，条码生成器类
    ///  结合 Google.ZXing的.Net版本ZXing.Net动态库使用
    /// </summary>
    public class Create
    {
        public static string QrImg(string content, string qrcodeImgPath, int wh, string color, string bgcolor)
        {
             BarcodeWriter writer = new BarcodeWriter();
            BitMatrix matrix = new MultiFormatWriter().encode(content, BarcodeFormat.QR_CODE, wh, wh, null);
            Image image = Create.toBitmap(matrix, color, bgcolor);
            image.Save(qrcodeImgPath, ImageFormat.Png);
            image.Dispose();
            return qrcodeImgPath;
        }

        public static string QrImg(string content, string qrcodeImgPath, string corrLevel, int wh, string color, string bgcolor)
        {
            ZXing.QrCode.Internal.ErrorCorrectionLevel corrLevel2 = ZXing.QrCode.Internal.ErrorCorrectionLevel.L;
            if (corrLevel != null)
            {
                string text = corrLevel.ToLower();
                if (text != null)
                {
                    if (text == "l")
                    {
                        corrLevel2 = ZXing.QrCode.Internal.ErrorCorrectionLevel.L;
                        goto IL_80;
                    }
                    if (text == "m")
                    {
                        corrLevel2 = ZXing.QrCode.Internal.ErrorCorrectionLevel.M;
                        goto IL_80;
                    }
                    if (text == "q")
                    {
                        corrLevel2 = ZXing.QrCode.Internal.ErrorCorrectionLevel.Q;
                        goto IL_80;
                    }
                    if (text == "h")
                    {
                        corrLevel2 = ZXing.QrCode.Internal.ErrorCorrectionLevel.H;
                        goto IL_80;
                    }
                }
                corrLevel2 = ZXing.QrCode.Internal.ErrorCorrectionLevel.L;
                IL_80:;
            }
            BitMatrix matrix = new QRCodeWriter().encode(content, BarcodeFormat.QR_CODE, wh, wh, null);
            Image image = Create.toBitmap(matrix, color, bgcolor);
            image.Save(qrcodeImgPath, ImageFormat.Png);
            image.Dispose();
            return qrcodeImgPath;
        }

        public static string QrImg(string content, string qrcodeImgPath, int wh, string centerImgPath, string color, string bgcolor)
        {
            BitMatrix matrix = new MultiFormatWriter().encode(content, BarcodeFormat.QR_CODE, wh, wh, null);
            Image image = Create.toBitmap(matrix, color, bgcolor);
            if (centerImgPath != null && File.Exists(centerImgPath))
            {
                Image image2 = Image.FromFile(centerImgPath);
                Graphics graphics = Graphics.FromImage(image);
                int width = (image2.Width > image.Width / 3) ? (image.Width / 3) : image2.Width;
                int height = (image2.Height > image.Height / 3) ? (image.Height / 3) : image2.Height;
                image2 = ImageTo.Zoom(image2, width, height, false);
                graphics.DrawImage(image2, new Rectangle((image.Width - image2.Width) / 2, (image.Height - image2.Height) / 2, image2.Width, image2.Height), 0, 0, image2.Width, image2.Height, GraphicsUnit.Pixel);
                graphics.Dispose();
            }
            image.Save(qrcodeImgPath, ImageFormat.Png);
            image.Dispose();
            return qrcodeImgPath;
        }

        public static string QrImg(string content, string qrcodeImgPath, int wh, Image centerImg, string color, string bgcolor)
        {
            BitMatrix matrix = new MultiFormatWriter().encode(content, BarcodeFormat.QR_CODE, wh, wh, null);
            Image image = Create.toBitmap(matrix, color, bgcolor);
            if (centerImg != null)
            {
                int num = (int)((double)image.Width * 0.3);
                int num2 = (int)((double)image.Height * 0.3);
                Bitmap image2 = new Bitmap(num, num2);
                using (Graphics graphics = Graphics.FromImage(image2))
                {
                    graphics.InterpolationMode = InterpolationMode.High;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.DrawImage(centerImg, 0, 0, num, num2);
                    graphics.DrawRectangle(new Pen(Color.Black, 0f), 0, 0, num - 1, num2 - 1);
                    graphics.Dispose();
                }
                Graphics graphics2 = Graphics.FromImage(image);
                graphics2.DrawImage(image2, new Rectangle((image.Width - num) / 2, (image.Height - num) / 2, num, num2), 0, 0, num, num2, GraphicsUnit.Pixel);
                graphics2.Dispose();
            }
            image.Save(qrcodeImgPath, ImageFormat.Png);
            image.Dispose();
            return qrcodeImgPath;
        }

        private static Bitmap toBitmap(BitMatrix matrix, string color, string bgcolor)
        {
            int width = matrix.Width;
            int height = matrix.Height;
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Color foregroundcolor = (color == null || color == "") ? ColorTranslator.FromHtml("#000000") : ColorTranslator.FromHtml(color);
            Color backgroundcolor = (color == null || color == "") ? Color.Transparent : ColorTranslator.FromHtml(bgcolor);
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            barcodeWriter.Renderer = new ZXing.Rendering.BitmapRenderer { Background = backgroundcolor, Foreground = foregroundcolor };//设置颜色
            Bitmap map = barcodeWriter.Write(matrix);
            return bitmap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="qrcodeImgPath"></param>
        /// <param name="wh"></param>
        /// <param name="centerImg"></param>
        /// <param name="color"></param>
        /// <param name="bgcolor"></param>
        /// <returns></returns>
        public static string QrCodeImg(string content, string qrcodeImgPath, int wh, Image centerImg, string color, string bgcolor)
        {
            Color foregroundcolor = (color == null || color == "") ? ColorTranslator.FromHtml("#000000") : ColorTranslator.FromHtml(color);
            Color backgroundcolor = (color == null || color == "") ? Color.Transparent : ColorTranslator.FromHtml(bgcolor);
            //Logo 图片
            Bitmap logo = new Bitmap(centerImg);
            //构造二维码写码器
            MultiFormatWriter writer = new MultiFormatWriter();
            Dictionary<EncodeHintType, object> hint = new Dictionary<EncodeHintType, object>();
            hint.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
            hint.Add(EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.H);
            
            //生成二维码 
            BitMatrix bm = writer.encode(content, BarcodeFormat.QR_CODE, 300, 300, hint);
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            barcodeWriter.Renderer = new ZXing.Rendering.BitmapRenderer { Background = backgroundcolor, Foreground = foregroundcolor };//设置颜色
            Bitmap map = barcodeWriter.Write(bm);
            

            //获取二维码实际尺寸（去掉二维码两边空白后的实际尺寸）
            int[] rectangle = bm.getEnclosingRectangle();

            //计算插入图片的大小和位置
            int middleW = Math.Min((int)(rectangle[2] / 3.5), logo.Width);
            int middleH = Math.Min((int)(rectangle[3] / 3.5), logo.Height);
            int middleL = (map.Width - middleW) / 2;
            int middleT = (map.Height - middleH) / 2;

            //将img转换成bmp格式，否则后面无法创建Graphics对象
            Bitmap bmpimg = new Bitmap(map.Width, map.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmpimg))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(map, 0, 0);
            }
            //将二维码插入图片
            Graphics myGraphic = Graphics.FromImage(bmpimg);
            //白底
            myGraphic.FillRectangle(Brushes.White, middleL, middleT, middleW, middleH);
            myGraphic.DrawImage(logo, middleL, middleT, middleW, middleH);

            //保存成图片
            bmpimg.Save(qrcodeImgPath, ImageFormat.Png);
            bmpimg.Dispose();
            return qrcodeImgPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="qrcodeImgPath"></param>
        /// <param name="wh"></param>
        /// <param name="centerImg"></param>
        /// <param name="color"></param>
        /// <param name="bgcolor"></param>
        /// <returns></returns>
        public static string QrCodeImg(string content, string qrcodeImgPath, int wh, string centerImg, string color, string bgcolor)
        {
            Color foregroundcolor = (color == null || color == "") ? ColorTranslator.FromHtml("#000000") : ColorTranslator.FromHtml(color);
            Color backgroundcolor = (color == null || color == "") ? Color.Transparent : ColorTranslator.FromHtml(bgcolor);
            //Logo 图片
            Bitmap logo = new Bitmap(centerImg);
            //构造二维码写码器
            MultiFormatWriter writer = new MultiFormatWriter();
            Dictionary<EncodeHintType, object> hint = new Dictionary<EncodeHintType, object>();
            hint.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
            hint.Add(EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.H);

            //生成二维码 
            BitMatrix bm = writer.encode(content, BarcodeFormat.QR_CODE, 300, 300, hint);
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            barcodeWriter.Renderer = new ZXing.Rendering.BitmapRenderer { Background = backgroundcolor, Foreground = foregroundcolor };//设置颜色
            Bitmap map = barcodeWriter.Write(bm);


            //获取二维码实际尺寸（去掉二维码两边空白后的实际尺寸）
            int[] rectangle = bm.getEnclosingRectangle();

            //计算插入图片的大小和位置
            int middleW = Math.Min((int)(rectangle[2] / 3.5), logo.Width);
            int middleH = Math.Min((int)(rectangle[3] / 3.5), logo.Height);
            int middleL = (map.Width - middleW) / 2;
            int middleT = (map.Height - middleH) / 2;

            //将img转换成bmp格式，否则后面无法创建Graphics对象
            Bitmap bmpimg = new Bitmap(map.Width, map.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmpimg))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(map, 0, 0);
            }
            //将二维码插入图片
            Graphics myGraphic = Graphics.FromImage(bmpimg);
            //白底
            myGraphic.FillRectangle(Brushes.White, middleL, middleT, middleW, middleH);
            myGraphic.DrawImage(logo, middleL, middleT, middleW, middleH);

            //保存成图片
            bmpimg.Save(qrcodeImgPath, ImageFormat.Png);
            bmpimg.Dispose();
            return qrcodeImgPath;
        }
    }
}
