using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace WaterCloud.Code.Images
{
    public class ImageTo
    {
        public static Image Cute(Image source, int x, int y, int width, int height)
        {
            int width2 = source.Width;
            int height2 = source.Height;
            width = ((width >= width2) ? width2 : width);
            height = ((height >= height2) ? height2 : height);
            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.DrawImage(source, 0, 0, new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
            graphics.Dispose();
            return bitmap;
        }

        public static Image Zoom(Image source, int width, int height, bool isDeformation)
        {
            Image result;
            if (isDeformation)
            {
                result = ImageTo.Zoom(source, width, height);
            }
            else
            {
                double num = (double)width / (double)source.Width;
                double num2 = (double)height / (double)source.Height;
                double num3 = (num > num2) ? num : num2;
                int num4 = Convert.ToInt32((double)source.Width * num3);
                int num5 = Convert.ToInt32((double)source.Height * num3);
                num4 = ((num4 >= source.Width) ? source.Width : num4);
                num5 = ((num5 >= source.Height) ? source.Height : num5);
                Bitmap bitmap = new Bitmap(width, height);
                Graphics graphics = Graphics.FromImage(bitmap);
                graphics.Clear(Color.White);
                graphics.DrawImage(source, (width - num4) / 2, (height - num5) / 2, num4, num5);
                graphics.Dispose();
                result = bitmap;
            }
            return result;
        }

        public static Image Zoom(Image source, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = InterpolationMode.High;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(source, 0, 0, width, height);
                graphics.Dispose();
            }
            return bitmap;
        }

        public static Image Zoom(Image source, int percent)
        {
            int width = source.Width * percent / 100;
            int height = source.Height * percent / 100;
            return ImageTo.Zoom(source, width, height);
        }

        public static Image Thumbnail(Image source, int width, int height)
        {
            int num = source.Width;
            int num2 = source.Height;
            double num3 = (double)width / (double)source.Width;
            double num4 = (double)height / (double)source.Height;
            double num5 = (num3 > num4) ? num3 : num4;
            num = Convert.ToInt32((double)source.Width * num5);
            num2 = Convert.ToInt32((double)source.Height * num5);
            num = ((num >= source.Width) ? source.Width : num);
            num2 = ((num2 >= source.Height) ? source.Height : num2);
            Bitmap bitmap = new Bitmap(num, num2);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.InterpolationMode = InterpolationMode.High;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.DrawImage(source, 0, 0, num, num2);
            graphics.Dispose();
            return ImageTo.Cute(bitmap, (bitmap.Width - width) / 2, (bitmap.Height - height) / 2, width, height);
        }

        public static Image Thumbnail(Image source, int percent)
        {
            int width = source.Width * percent / 100;
            int height = source.Height * percent / 100;
            return ImageTo.Thumbnail(source, width, height);
        }

        public static Image Thumbnail(Image source, int width, int height, int restrainObj)
        {
            int num = width;
            int num2 = height;
            if (restrainObj == 0)
            {
                double num3 = (double)width / (double)source.Width;
                double num4 = (double)height / (double)source.Height;
                double num5 = (num3 > num4) ? num3 : num4;
                num = Convert.ToInt32((double)source.Width * num5);
                num2 = Convert.ToInt32((double)source.Height * num5);
            }
            if (restrainObj == 1)
            {
                double num3 = (double)width / (double)source.Width;
                num2 = Convert.ToInt32((double)source.Height * num3);
            }
            if (restrainObj == 2)
            {
                double num4 = (double)height / (double)source.Height;
                num = Convert.ToInt32((double)source.Width * num4);
            }
            num = ((num >= source.Width) ? source.Width : num);
            num2 = ((num2 >= source.Height) ? source.Height : num2);
            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            graphics.DrawImage(source, (width - num) / 2, (height - num2) / 2, num, num2);
            graphics.Dispose();
            return bitmap;
        }

        public static Image Overlay(Image source, Image img, int x, int y)
        {
            Graphics graphics = Graphics.FromImage(source);
            graphics.DrawImage(img, new Rectangle(x, y, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel);
            graphics.Dispose();
            return source;
        }

        public static Image Overlay(Image source, Image img, int x, int y, int opacity)
        {
            img = ImageTo.Transparent(img, opacity);
            return ImageTo.Overlay(source, img, x, y);
        }

        public static Image Overlay(Image source, Image img, string overType, int opacity)
        {
            string text = overType.ToLower();
            int x;
            int y;
            switch (text)
            {
                case "left":
                    x = 0;
                    y = (source.Height - img.Height) / 2;
                    goto IL_1B6;
                case "right":
                    x = source.Width - img.Width;
                    y = (source.Height - img.Height) / 2;
                    goto IL_1B6;
                case "top":
                    x = (source.Width - img.Width) / 2;
                    y = 0;
                    goto IL_1B6;
                case "down":
                    x = (source.Width - img.Width) / 2;
                    y = source.Height - img.Height;
                    goto IL_1B6;
                case "lefttop":
                    x = 0;
                    y = 0;
                    goto IL_1B6;
                case "leftdown":
                    x = 0;
                    y = source.Height - img.Height;
                    goto IL_1B6;
                case "righttop":
                    x = source.Width - img.Width;
                    y = 0;
                    goto IL_1B6;
                case "rightdown":
                    x = source.Width - img.Width;
                    y = source.Height - img.Height;
                    goto IL_1B6;
            }
            x = (source.Width - img.Width) / 2;
            y = (source.Height - img.Height) / 2;
            IL_1B6:
            return ImageTo.Overlay(source, img, x, y, opacity);
        }

        public static Image Overlay(Image source, string text, string overType, int opacity)
        {
            Image img = ImageTo.FromString(text, 12, "宋体", "#ff0000");
            return ImageTo.Overlay(source, img, overType, opacity);
        }

        public static Image Overlay(Image source, string text, string overType, int opacity, int size, string font, string color)
        {
            Image img = ImageTo.FromString(text, size, font, color);
            return ImageTo.Overlay(source, img, overType, opacity);
        }

        public static Image FromString(string text)
        {
            int width = text.Length * 18;
            Bitmap bitmap = new Bitmap(width, 18);
            Graphics graphics = Graphics.FromImage(bitmap);
            Font font = new Font("宋体", 12f, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.Black);
            graphics.DrawString(text, font, brush, 0f, 0f);
            return bitmap;
        }

        public static Image FromString(string text, int size, string font, string color)
        {
            string[] array = text.Split(new char[]
            {
                '\n'
            });
            int num = 0;
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string text2 = array2[i];
                num = ((text2.Length > num) ? text2.Length : num);
            }
            int num2 = num * size;
            num2 = ((num2 < 1) ? 1 : num2);
            Bitmap bitmap = new Bitmap(num2, (int)Math.Floor((double)(size * array.Length) * 1.5));
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            Font font2 = new Font(font, (float)size, FontStyle.Bold);
            Color color2 = ColorTranslator.FromHtml(color);
            Brush brush = new SolidBrush(color2);
            graphics.DrawString(text, font2, brush, 0f, 0f);
            graphics.Dispose();
            return bitmap;
        }

        public static Image Transparent(Image img, int opacity)
        {
            opacity = ((opacity < 0) ? 0 : opacity);
            opacity = ((opacity > 100) ? 100 : opacity);
            float[][] array = new float[5][];
            float[][] arg_35_0 = array;
            int arg_35_1 = 0;
            float[] array2 = new float[5];
            array2[0] = 1f;
            arg_35_0[arg_35_1] = array2;
            float[][] arg_4C_0 = array;
            int arg_4C_1 = 1;
            array2 = new float[5];
            array2[1] = 1f;
            arg_4C_0[arg_4C_1] = array2;
            float[][] arg_63_0 = array;
            int arg_63_1 = 2;
            array2 = new float[5];
            array2[2] = 1f;
            arg_63_0[arg_63_1] = array2;
            float[][] arg_7D_0 = array;
            int arg_7D_1 = 3;
            array2 = new float[5];
            array2[3] = (float)opacity / 100f;
            arg_7D_0[arg_7D_1] = array2;
            array[4] = new float[]
            {
                0f,
                0f,
                0f,
                0f,
                1f
            };
            float[][] newColorMatrix = array;
            ColorMatrix newColorMatrix2 = new ColorMatrix(newColorMatrix);
            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(newColorMatrix2, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            Bitmap bitmap = new Bitmap(img.Width, img.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imageAttributes);
            graphics.Dispose();
            return bitmap;
        }

        public static Image Border(Image img)
        {
            Graphics graphics = Graphics.FromImage(img);
            graphics.DrawRectangle(new Pen(Color.Black, 0f), 0, 0, img.Width - 1, img.Height - 1);
            graphics.Dispose();
            return img;
        }
    }
}
