using System;
using System.Data;
using System.Collections;
using System.IO;
using System.Drawing; 

namespace WaterCloud.Code
{
    class ThumbnailByLp
    {
        private int _height = 60;
        private int _width = 45;

        public int Height
        {
            get
            {
                return this._height;
            }
            set
            {
                this._height = value;
            }
        }

        public int Width
        {
            get
            {
                return this._width;
            }
            set
            {
                this._width = value;
            }
        }

    }

    /// <summary>
    /// Busi_UploadHelp 的摘要说明。
    /// </summary>
    public class UploadHelp
    {
        Hashtable ht;
        //#region 【上传文件相关】
        //private string _webDirPath = System.Configuration.ConfigurationManager.AppSettings["WebDirPath"];

        ///// <summary>
        ///// 新增或者删除的时候用的路径
        ///// </summary>
        //public string UpLoadPath
        //{
        //    get
        //    {
        //        string temp = WebDirPath + "\\UpLoadPath\\";
        //        return temp;
        //    }

        //}
        ///// <summary>
        ///// 保存到数据库 的FilePath
        ///// </summary>
        //public string UpLoadUrlPath
        //{
        //    get
        //    {
        //        return "/UpLoadPath/";
        //    }
        //}
        //public string WebDirPath
        //{
        //    get
        //    {
        //        return _webDirPath;
        //    }

        //}
        //#endregion

        //#region 【缩略图相关】

        //private bool _IsThumbnail = false;
        //private Thumbnail _smallThumbnail = new Thumbnail();
        ///// <summary>
        ///// 是否生成缩略图
        ///// </summary>
        //public bool IsThumbnail
        //{
        //    get
        //    {
        //        return this._IsThumbnail;
        //    }
        //    set
        //    {
        //        this._IsThumbnail = value;
        //    }
        //}

        ///// <summary>
        ///// 小缩略图对象
        ///// </summary>
        //public Thumbnail SmallThumbnail
        //{
        //    get
        //    {
        //        return this._smallThumbnail;
        //    }
        //    set
        //    {
        //        this._smallThumbnail = value;
        //    }
        //}

        //#endregion

        //public Busi_UploadHelp()
        //{
        //    //
        //    // TODO: 在此处添加构造函数逻辑
        //    //
        //}

        //#region 【文件上传相关】

        ///// <summary>
        ///// 生成文件名 【ID+文件的扩展名　作为文件名】
        ///// </summary>
        ///// <param name="dtoPubattach"></param>
        ///// <returns></returns>
        //private string GetFileName(DTOWeb_Pubattach dtoPubattach)
        //{
        //    return dtoPubattach.TABLEID + GetRandomNumber() + GetExtension(dtoPubattach.FILENAME);
        //}

        ///// <summary>
        ///// 上传文件　的部分命名
        ///// </summary>
        ///// <returns></returns>
        //private string GetRandomNumber()
        //{
        //    return System.Guid.NewGuid().ToString();
        //}

        ///// <summary>
        ///// 给出文件的扩展名
        ///// </summary>
        ///// <param name="FILENAME">文件路径</param>
        ///// <returns></returns>
        //private string GetExtension(string FILENAME)
        //{
        //    int strIndex = FILENAME.LastIndexOf(".");
        //    string strFileName = FILENAME;
        //    string strExtension = strFileName.Substring(strIndex, strFileName.Length - strIndex);
        //    return strExtension;
        //}

        ///// <summary>
        ///// 写文件函数
        ///// </summary>
        ///// <param name="b"></param>
        ///// <param name="filePath"></param>
        ///// <param name="path"></param>
        ///// <returns></returns>
        //private bool WriteFile(byte[] b, string filePath, string path)
        //{

        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    FileStream fs = null;
        //    try
        //    {
        //        fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
        //        for (int i = 0; i < b.Length; i++)
        //        {
        //            fs.WriteByte(b[i]);
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (fs != null)
        //            fs.Close();
        //    }
        //}

        //public string GetFilePath(string UpLoadPath, string strTime, string strFileName)
        //{
        //    return UpLoadPath + strTime + "\\" + strFileName;
        //}

        //public string GetFilePath(string filePath)
        //{
        //    string temp = filePath.Replace("/", "\\");
        //    return WebDirPath + temp;
        //}

        //private string GetDirPath(string UpLoadPath, string strTime)
        //{
        //    return UpLoadPath + strTime + "\\";
        //}

        //private string GetSavaToDBFilePath(string UpLoadUrlPath, string strTime, string strFileName)
        //{
        //    return UpLoadUrlPath + strTime + "/" + strFileName;
        //}

        //public bool AddFile(DTOWeb_Pubattach dtoPubattach)
        //{
        //    string strTime = DateTime.Now.ToShortDateString();

        //    string strFileName = GetFileName(dtoPubattach);

        //    string strDirPath = GetDirPath(UpLoadPath, strTime);

        //    string strFilePath = GetFilePath(UpLoadPath, strTime, strFileName);

        //    dtoPubattach.FILEPATH = GetSavaToDBFilePath(UpLoadUrlPath, strTime, strFileName);


        //    if (WriteFile(dtoPubattach.BINARY, strFilePath, strDirPath))
        //    {
        //        //2010-10-11 王大鹏修改 生成缩略图
        //        this.CreateThumbnail(dtoPubattach, strFilePath);
        //        return true;
        //    }
        //    return false;
        //}


        ////wangdpeng 2011-01-12 水印
        //public bool AddFile(DTOWeb_Pubattach dtoPubattach,int _isNeedWatermark,int _watermarkType,int _watermarkPosition,string _watermarkText,int _watermarkTextSize,string _watermarkPicPath)
        //{
        //    string strTime = DateTime.Now.ToShortDateString();

        //    string strFileName = GetFileName(dtoPubattach);

        //    string strDirPath = GetDirPath(UpLoadPath, strTime);

        //    string strFilePath = GetFilePath(UpLoadPath, strTime, strFileName);

        //    dtoPubattach.FILEPATH = GetSavaToDBFilePath(UpLoadUrlPath, strTime, strFileName);


        //    if (WriteFile(dtoPubattach.BINARY, strFilePath, strDirPath))
        //    {
        //        //2010-10-11 wangdapeng 生成缩略图
        //        this.CreateThumbnail(dtoPubattach, strFilePath);
        //        //2011-01-12 wangdapeng 添加水印
        //        if (Convert.ToInt32(System.Web.HttpContext.Current.Session["IsNeedWatermark"]) == 1)
        //        {
        //            this.AddWatermark(dtoPubattach, strFilePath, Convert.ToInt32(System.Web.HttpContext.Current.Session["WatermarkType"]), Convert.ToInt32(System.Web.HttpContext.Current.Session["WatermarkPosition"]), System.Web.HttpContext.Current.Session["WatermarkText"].ToString(), Convert.ToInt32(System.Web.HttpContext.Current.Session["WatermarkTextSize"]), System.Web.HttpContext.Current.Session["WatermarkPicPath"].ToString());
        //        }

        //        return true;
        //    }
        //    return false;
        //}
        ////webeditor中的图片加水印 wangdapeng 2010-01-12
        //public void AddWatermark(string filePath)
        //{
        //    if (this.isPicWatermarkOfWebeditor(filePath.Substring(filePath.LastIndexOf(".")+1,filePath.Length-filePath.LastIndexOf(".")-1)))
        //    {
        //        string mark_ = filePath.Insert(filePath.LastIndexOf('\\') + 1, "mark_");

        //        if (Convert.ToInt32(System.Web.HttpContext.Current.Session["WatermarkType"]) == 0)
        //        {
        //            AddWatermarkWord(filePath, mark_, System.Web.HttpContext.Current.Session["WatermarkText"].ToString(), Convert.ToInt32(System.Web.HttpContext.Current.Session["WatermarkTextSize"]), Convert.ToInt32(System.Web.HttpContext.Current.Session["WatermarkPosition"]));
        //        }
        //        else
        //        {
        //            AddWatermarkPic(filePath, mark_, System.Web.HttpContext.Current.Session["WatermarkPicPath"].ToString(), Convert.ToInt32(System.Web.HttpContext.Current.Session["WatermarkPosition"]));
        //        }
        //    }
        //}

        //#endregion
        //#region 水印相关 filePath.Insert(filePath.LastIndexOf('.'),"mark")
        //private void AddWatermark(DTOWeb_Pubattach dtoPubattach, string filePath,int _watermarkType,int _watermarkPosition,string _watermarkText,int _watermarkTextSize,string _watermarkPicPath)
        //{
        //    //if (dtoPubattach.CONTENTTYPE== "image/jpg" || dtoPubattach.CONTENTTYPE== "image/jpeg" )
        //    if (this.isPicWatermark(dtoPubattach.CONTENTTYPE))
        //    {
        //        string mark_ = filePath.Insert(filePath.LastIndexOf('\\') + 1, "mark_");
        //        if (_watermarkType == 0)
        //        {
        //            AddWatermarkWord(filePath, mark_, _watermarkText, _watermarkTextSize, _watermarkPosition);
        //        }
        //        else
        //        {
        //            AddWatermarkPic(filePath, mark_, _watermarkPicPath, _watermarkPosition);
        //        }
        //    }
        //}
        ////文字水印
        //private static void AddWatermarkWord(string Path, string MarkedPath, string Text, int Size, int location)
        //{
        //    System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
        //    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
        //    g.DrawImage(image, 0, 0, image.Width, image.Height);
        //    System.Drawing.Font f = new System.Drawing.Font("Verdana", Size);
        //    System.Drawing.Brush b = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
        //    int sWidth = image.Width;
        //    int sHeight = image.Height;
        //    int wWidth = Size * Text.Length;
        //    int wHeight = f.Height;

        //    int x1 = 0; //原图和水印图的宽度差,即开始绘图的X位置
        //    int y1 = 0; //原图和水印图的高度差,即开始绘图的Y位置

        //    switch (location)
        //    {
        //        case 1: //左上
        //            x1 = 0;
        //            y1 = 0;
        //            break;
        //        case 2: //左下
        //            x1 = 0;
        //            if ((sHeight - wHeight) > 0) //源图比水印图高
        //                y1 = sHeight - wHeight;
        //            else
        //                y1 = sWidth;
        //            break;
        //        case 3: //右上
        //            y1 = 0;
        //            if ((sWidth - wWidth) > 0) // 源图比水印图宽
        //                x1 = sWidth - wWidth;
        //            else
        //                x1 = sWidth;
        //            break;
        //        case 4: //右下
        //            //计算高度
        //            if ((sHeight - wHeight) > 0) //源图比水印图高
        //                y1 = sHeight - wHeight;
        //            else
        //                y1 = sWidth;
        //            //计算宽度
        //            if ((sWidth - wWidth) > 0) // 源图比水印图宽
        //                x1 = sWidth - wWidth;
        //            else
        //                x1 = sWidth;
        //            break;
        //        case 0: //居中
        //            //计算高度
        //            if ((sHeight - wHeight) > 0) //源图比水印图高
        //                y1 = (sHeight - wHeight) / 2;
        //            else
        //                y1 = sWidth;
        //            //计算宽度
        //            if ((sWidth - wWidth) > 0) // 源图比水印图宽
        //                x1 = (sWidth - wWidth) / 2;
        //            else
        //                x1 = sWidth;
        //            break;
        //    }

        //    g.DrawString(Text, f, b, x1, y1);
        //    g.Dispose();

        //    image.Save(MarkedPath);
        //    image.Dispose();
        //}
        ////图片水印
        //private static void AddWatermarkPic(string Path, string MarkedPath, string MarkFilePath, int location)
        //{
        //    System.Drawing.Image image = System.Drawing.Image.FromFile(Path);
        //    System.Drawing.Image copyImage = System.Drawing.Image.FromFile(MarkFilePath);
        //    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
        //    int sWidth = image.Width;
        //    int sHeight = image.Height;
        //    int wWidth = copyImage.Width;
        //    int wHeight = copyImage.Height;

        //    int x1 = 0; //原图和水印图的宽度差,即开始绘图的X位置
        //    int y1 = 0; //原图和水印图的高度差,即开始绘图的Y位置



        //    if (sWidth > wWidth && sHeight > wHeight) //如果源图比水印图大
        //    {
        //        switch (location)
        //        {
        //            case 1: //左上
        //                x1 = 0;
        //                y1 = 0;
        //                break;
        //            case 2: //左下
        //                x1 = 0;
        //                if ((sHeight - wHeight) > 0) //源图比水印图高
        //                    y1 = sHeight - wHeight;
        //                else
        //                    y1 = sWidth;
        //                break;
        //            case 3: //右上
        //                y1 = 0;
        //                if ((sWidth - wWidth) > 0) // 源图比水印图宽
        //                    x1 = sWidth - wWidth;
        //                else
        //                    x1 = sWidth;
        //                break;
        //            case 4: //右下
        //                //计算高度
        //                if ((sHeight - wHeight) > 0) //源图比水印图高
        //                    y1 = sHeight - wHeight;
        //                else
        //                    y1 = sWidth;
        //                //计算宽度
        //                if ((sWidth - wWidth) > 0) // 源图比水印图宽
        //                    x1 = sWidth - wWidth;
        //                else
        //                    x1 = sWidth;
        //                break;
        //            case 0: //居中
        //                //计算高度
        //                if ((sHeight - wHeight) > 0) //源图比水印图高
        //                    y1 = (sHeight - wHeight) / 2;
        //                else
        //                    y1 = sWidth;
        //                //计算宽度
        //                if ((sWidth - wWidth) > 0) // 源图比水印图宽
        //                    x1 = (sWidth - wWidth) / 2;
        //                else
        //                    x1 = sWidth;
        //                break;
        //        }
        //        g.DrawImage(copyImage, new System.Drawing.Rectangle(x1, y1, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, System.Drawing.GraphicsUnit.Pixel);
        //    }
        //    g.Dispose();
        //    image.Save(MarkedPath);

        //    //image.Save(Path);
        //    image.Dispose();
        //    ////
        //    if (File.Exists(Path) && File.Exists(MarkedPath))
        //    {
        //        System.IO.File.Replace(MarkedPath, Path, System.Configuration.ConfigurationManager.AppSettings["WebDirImgBack"].ToString() + Path.Substring(Path.LastIndexOf('\\') + 1));
        //    }
        //    //image.Save(Path);
        //}

        //#endregion
        #region 【缩略图相关】

        ///// <summary>
        ///// 生成缩略图
        ///// </summary>
        //private void CreateThumbnail(DTOWeb_Pubattach dtoPubattach, string filePath)
        //{
        //    if (this.IsThumbnail && this.isPic(dtoPubattach.CONTENTTYPE))
        //    {
        //        this.ShowThumbnail(filePath, GetMiddlePicFilePath(filePath), 225, 281);//生成缩略图
        //        this.ShowThumbnail(filePath, GetSmallPicFilePath(filePath));//生成小图
        //        //this.ShowThumbnail(filePath,GetSmallPicFilePath(filePath),SmallThumbnail.Height,SmallThumbnail.Width);//生成小图

        //        //this.ShowThumbnail(filePath,GetOlympicFilePath(filePath),205,290);//生成奥运图//
        //    }
        //}

        ////2010-10-11 王大鹏修改 Webeditor缩略图生成
        //public void CreateThumbnail(string filePath)
        //{
        //    //if (this.IsThumbnail && this.isPic(dtoPubattach.CONTENTTYPE))
        //    //{
        //    this.ShowThumbnail(filePath, GetMiddlePicFilePath(filePath), 225, 281);//生成缩略图
        //    this.ShowThumbnail(filePath, GetSmallPicFilePath(filePath));//生成小图
        //    //this.ShowThumbnail(filePath,GetSmallPicFilePath(filePath),SmallThumbnail.Height,SmallThumbnail.Width);//生成小图

        //    //this.ShowThumbnail(filePath,GetOlympicFilePath(filePath),205,290);//生成奥运图//
        //    //}
        //}
        ////2010-10-11 王大鹏修改 生成缩略图
        //public string GetMiddlePicFilePath(string filePath)
        //{
        //    string temp = filePath;
        //    if (temp.LastIndexOf("\\") + 1 == -1) return "";
        //    //temp = temp.Substring(0, temp.LastIndexOf(".")) + "_middle" + GetExtension(temp);
        //    temp = temp.Insert(temp.LastIndexOf('\\') + 1, "middle_");
        //    return temp;
        //}
        ////2010-10-11 王大鹏修改 生成小图
        //public string GetSmallPicFilePath(string filePath)
        //{
        //    string temp = filePath;
        //    if (temp.LastIndexOf("\\") == -1) return "";
        //    //temp = temp.Substring(0, temp.LastIndexOf(".")) + "_small" + GetExtension(temp);
        //    temp = temp.Insert(temp.LastIndexOf('\\') + 1, "small_");
        //    return temp;
        //}

        //public string GetOlympicFilePath(string filePath)
        //{
        //    string temp = filePath;
        //    temp = temp.Substring(0, temp.LastIndexOf(".")) + "_Olympic" + GetExtension(temp);
        //    return temp;
        //}

        /// <summary>
        /// 判断是否是图片
        /// </summary>
        /// <returns></returns>
        private bool isPic(string contentType)
        {
            if (contentType == "image/jpg" || contentType == "image/jpeg" ||
                contentType == "image/png" || contentType == "image/pjpeg" ||
                contentType == "image/gif" || contentType == "image/bmp" ||
                contentType == "image/x-png")
                return true;
            else
                return false;
        }
        //判断是否是可以加水印的图片
        private bool isPicWatermark(string contentType)
        {
            if (contentType == "image/jpg" || contentType == "image/jpeg" ||
                contentType == "image/png" || contentType == "image/pjpeg" ||
                contentType == "image/bmp" || contentType == "image/x-png")
                return true;
            else
                return false;
        }
        private bool isPicWatermarkOfWebeditor(string contentType)
        {
            if (contentType == "jpg")
                return true;
            else
                return false;
        }
        ///   <summary> 
        ///   生成缩略图 
        ///   </summary> 
        ///   <param   name= "originalImagePath "> 源图路径（物理路径） </param> 
        ///   <param   name= "thumbnailPath "> 缩略图路径（物理路径） </param> 
        ///   <param   name= "width "> 缩略图宽度 </param> 
        ///   <param   name= "height "> 缩略图高度 </param> 
        ///   <param   name= "mode "> 生成缩略图的方式 </param>   
        public void ShowThumbnail(string originalImagePath, string thumbnailPath, int height, int width)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);
            string mode = "HW";
            int sWidth = originalImage.Width;
            int sHeight = originalImage.Height;
            int towidth = width;
            int toheight = height;
            //int wWidth = copyImage.Width;
            //int wHeight = copyImage.Height;
            if (sWidth > sHeight)
            {
                if (sWidth > width)
                {
                    height = width * sHeight / sWidth;
                }
                else
                {
                    width = sWidth;
                    height = sHeight;
                }
                if (height > toheight)
                {
                    width = toheight * width / height;
                    height = toheight;
                }

            }
            else
            {
                if (sHeight > height)
                {
                    width = height * sWidth / sHeight;
                }
                else
                {
                    width = sWidth;
                    height = sHeight;
                }
                if (width > towidth)
                {
                    height = towidth * height / width;
                    width = towidth;
                }
            }
            towidth = width;
            toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW "://指定高宽缩放（可能变形）                                 
                    break;
                case "W "://指定宽，高按比例                                         
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H "://指定高，宽按比例 
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut "://指定高宽裁减（不变形）                                 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片 
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板 
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);

            try
            {
                originalImage.Dispose();
                //以jpg格式保存缩略图 
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                bitmap.Dispose();
                g.Dispose();
            }

        }



        /// <summary>
        /// 生成小图等比例缩放，宽度和高度都不超过160px
        /// </summary>
        /// <param name="originalImagePath"></param>
        /// <param name="thumbnailPath"></param>
        public void SaveThumbnail(string originalImagePath, string thumbnailPath, int maxlength)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);
            int sWidth = originalImage.Width;
            int sHeight = originalImage.Height;
            int width = 160;
            int height = 160;
            if (sWidth > sHeight)
            {
                if (sWidth > maxlength)
                {
                    width = maxlength;
                    height = width * sHeight / sWidth;
                }
                else
                {
                    width = sWidth;
                    height = sHeight;
                }
            }
            else
            {
                if (sHeight > maxlength)
                {
                    height = maxlength;
                    width = height * sWidth / sHeight;
                }
                else
                {
                    width = sWidth;
                    height = sHeight;
                }
            }
            int towidth = width;
            int toheight = height;
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            //新建一个bmp图片 
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板 
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充 
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);

            try
            {
                originalImage.Dispose();
                //以jpg格式保存缩略图 
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                bitmap.Dispose();
                g.Dispose();
            }

        }


        private bool ThumbnailCallback()
        {
            return false;
        }
        #endregion
    }
}
