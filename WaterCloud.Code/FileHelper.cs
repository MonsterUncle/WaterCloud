using System;
using System.IO;
using System.Text;
using System.Data;
using System.Drawing;
using System.Linq;

namespace WaterCloud.Code
{
    public class FileHelper
    {
        public static string MapPath(string path)
        {
            try
            {
                string rootdir = Directory.GetCurrentDirectory();
                //DirectoryInfo Dir = Directory.GetParent(rootdir);
                //string root = Dir.Parent.Parent.Parent.FullName;
                return rootdir + path;
            }
            catch (Exception ex)
            {
                return path;
            }
        }
        #region 获取文件到集合中
        /// <summary>
        /// 读取指定位置文件列表到集合中
        /// </summary>
        /// <param name="path">指定路径</param>
        /// <returns></returns>
        public static DataTable GetFileTable(string path)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("ext", typeof(string));
            dt.Columns.Add("size", typeof(long));
            dt.Columns.Add("time", typeof(DateTime));

            if (Directory.Exists(path))
            {
                DirectoryInfo dirinfo = new DirectoryInfo(path);
                FileInfo fi;
                DirectoryInfo dir;
                string FileName, FileExt;
                long FileSize = 0;
                DateTime FileModify;
                try
                {
                    foreach (FileSystemInfo fsi in dirinfo.GetFileSystemInfos())
                    {
                        FileName = string.Empty;
                        FileExt = string.Empty;
                        if (fsi is FileInfo)
                        {
                            fi = (FileInfo)fsi;
                            //获取文件名称
                            FileName = fi.Name;
                            //获取文件扩展名
                            FileExt = fi.Extension;
                            //获取文件大小
                            FileSize = fi.Length;
                            //获取文件最后修改时间
                            FileModify = fi.LastWriteTime;
                        }
                        else
                        {
                            dir = (DirectoryInfo)fsi;
                            //获取目录名
                            FileName = dir.Name;
                            //获取目录最后修改时间
                            FileModify = dir.LastWriteTime;
                            //设置目录文件为文件夹
                            FileExt = "文件夹";
                        }
                        DataRow dr = dt.NewRow();
                        dr["name"] = FileName;
                        dr["ext"] = FileExt;
                        dr["size"] = FileSize;
                        dr["time"] = FileModify;
                        dt.Rows.Add(dr);
                    }
                }
                catch
                {

                    throw;
                }
            }
            return dt;
        }

        #endregion

        #region 检测指定路径是否存在
        /// <summary>
        /// 检测指定路径是否存在
        /// </summary>
        /// <param name="path">目录的绝对路径</param> 
        public static bool IsExistDirectory(string path)
        {
            return Directory.Exists(path);
        }
        #endregion

        #region 检测指定文件是否存在,如果存在则返回true
        /// <summary>
        /// 检测指定文件是否存在,如果存在则返回true
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>  
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }
        #endregion

        #region 创建文件夹
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="folderPath">文件夹的绝对路径</param>
        public static void CreateFolder(string folderPath)
        {
            if (!IsExistDirectory(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }
        #endregion

        #region 判断上传文件后缀名
        /// <summary>
        /// 判断上传文件后缀名
        /// </summary>
        /// <param name="strExtension">后缀名</param>
        public static bool IsCanEdit(string strExtension)
        {
            strExtension = strExtension.ToLower();
            if (strExtension.LastIndexOf(".", StringComparison.Ordinal) >= 0)
            {
                strExtension = strExtension.Substring(strExtension.LastIndexOf(".", StringComparison.Ordinal));
            }
            else
            {
                strExtension = ".txt";
            }
            string[] strArray = new string[] { ".htm", ".html", ".txt", ".js", ".css", ".xml", ".sitemap" };
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strExtension.Equals(strArray[i]))
                {
                    return true;
                }
            }
            return false;
        }


        public static bool IsSafeName(string strExtension)
        {
            strExtension = strExtension.ToLower();
            if (strExtension.LastIndexOf(".") >= 0)
            {
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
            }
            else
            {
                strExtension = ".txt";
            }
            string[] strArray = new string[] { ".jpg", ".gif", ".png" };
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strExtension.Equals(strArray[i]))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsZipName(string strExtension)
        {
            strExtension = strExtension.ToLower();
            if (strExtension.LastIndexOf(".") >= 0)
            {
                strExtension = strExtension.Substring(strExtension.LastIndexOf("."));
            }
            else
            {
                strExtension = ".txt";
            }
            string[] strArray = new string[] { ".zip", ".rar" };
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strExtension.Equals(strArray[i]))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 创建文件夹
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="fileName">文件的绝对路径</param>
        public static void CreateSuffic(string fileName)
        {
            try
            {
                if (!Directory.Exists(fileName))
                {
                    Directory.CreateDirectory(fileName);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="fileName">文件的绝对路径</param>
        public static void CreateFiles(string fileName)
        {
            try
            {
                //判断文件是否存在，不存在创建该文件
                if (!IsExistFile(fileName))
                {
                    FileInfo file = new FileInfo(fileName);
                    FileStream fs = file.Create();
                    fs.Close();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #region 创建文本文件
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void CreateFile(string path, string content)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                sw.Write(content);
            }
        }
        #endregion
        /// <summary>
        /// 创建一个文件,并将字节流写入文件。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="buffer">二进制流数据</param>
        public static void CreateFile(string filePath, byte[] buffer)
        {
            try
            {
                //判断文件是否存在，不存在创建该文件
                if (!IsExistFile(filePath))
                {
                    FileInfo file = new FileInfo(filePath);
                    FileStream fs = file.Create();
                    fs.Write(buffer, 0, buffer.Length);
                    fs.Close();
                }
                else
                {
                    File.WriteAllBytes(filePath, buffer);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 将文件移动到指定目录
        /// <summary>
        /// 将文件移动到指定目录
        /// </summary>
        /// <param name="sourceFilePath">需要移动的源文件的绝对路径</param>
        /// <param name="descDirectoryPath">移动到的目录的绝对路径</param>
        public static void Move(string sourceFilePath, string descDirectoryPath)
        {
            string sourceName = GetFileName(sourceFilePath);
            if (IsExistDirectory(descDirectoryPath))
            {
                //如果目标中存在同名文件,则删除
                if (IsExistFile(descDirectoryPath + "\\" + sourceFilePath))
                {
                    DeleteFile(descDirectoryPath + "\\" + sourceFilePath);
                }
                else
                {
                    //将文件移动到指定目录
                    File.Move(sourceFilePath, descDirectoryPath + "\\" + sourceFilePath);
                }
            }
        }
        #endregion

        #region 将源文件的内容复制到目标文件中
        /// <summary>
        /// 将源文件的内容复制到目标文件中
        /// </summary>
        /// <param name="sourceFilePath">源文件的绝对路径</param>
        /// <param name="descDirectoryPath">目标文件的绝对路径</param>
        public static void Copy(string sourceFilePath, string descDirectoryPath)
        {
            File.Copy(sourceFilePath, descDirectoryPath, true);
        }
        #endregion

        #region 从文件的绝对路径中获取文件名( 不包含扩展名 )
        /// <summary>
        /// 从文件的绝对路径中获取文件名( 不包含扩展名 )
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param> 
        public static string GetFileName(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            return file.Name;
        }
        #endregion

        #region 获取文件的后缀名
        /// <summary>
        /// 获取文件的后缀名
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static string GetExtension(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            return file.Extension;
        }

        /// <summary>
        /// 返回文件扩展名，不含“.”
        /// </summary>
        /// <param name="filepath">文件全名称</param>
        /// <returns>string</returns>
        public static string GetFileExt(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                return "";
            }
            if (filepath.LastIndexOf(".", StringComparison.Ordinal) > 0)
            {
                return filepath.Substring(filepath.LastIndexOf(".", StringComparison.Ordinal) + 1); //文件扩展名，不含“.”
            }
            return "";
        }
        #endregion

        #region 删除指定文件
        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void DeleteFile(string filePath)
        {
            if (IsExistFile(filePath))
            {
                File.Delete(filePath);
            }
        }
        #endregion

        #region 删除指定目录及其所有子目录
        /// <summary>
        /// 删除指定目录及其所有子目录
        /// </summary>
        /// <param name="directoryPath">文件的绝对路径</param>
        public static void DeleteDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                Directory.Delete(directoryPath);
            }
        }
        #endregion

        #region 清空指定目录下所有文件及子目录,但该目录依然保存.
        /// <summary>
        /// 清空指定目录下所有文件及子目录,但该目录依然保存.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static void ClearDirectory(string directoryPath)
        {
            if (!IsExistDirectory(directoryPath)) return;
            //删除目录中所有的文件
            string[] fileNames = GetFileNames(directoryPath);
            for (int i = 0; i < fileNames.Length; i++)
            {
                DeleteFile(fileNames[i]);
            }
            //删除目录中所有的子目录
            string[] directoryNames = GetDirectories(directoryPath);
            for (int i = 0; i < directoryNames.Length; i++)
            {
                DeleteDirectory(directoryNames[i]);
            }
        }
        #endregion

        #region  剪切  粘贴
        /// <summary>
        /// 剪切文件
        /// </summary>
        /// <param name="source">原路径</param> 
        /// <param name="destination">新路径</param> 
        public bool FileMove(string source, string destination)
        {
            bool ret = false;
            FileInfo file_s = new FileInfo(source);
            FileInfo file_d = new FileInfo(destination);
            if (file_s.Exists)
            {
                if (!file_d.Exists)
                {
                    file_s.MoveTo(destination);
                    ret = true;
                }
            }
            if (ret == true)
            {
                //Response.Write("<script>alert('剪切文件成功！');</script>");
            }
            else
            {
                //Response.Write("<script>alert('剪切文件失败！');</script>");
            }
            return ret;
        }
        #endregion

        #region 检测指定目录是否为空
        /// <summary>
        /// 检测指定目录是否为空
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>  
        public static bool IsEmptyDirectory(string directoryPath)
        {
            try
            {
                //判断文件是否存在
                string[] fileNames = GetFileNames(directoryPath);
                if (fileNames.Length > 0)
                {
                    return false;
                }
                //判断是否存在文件夹
                string[] directoryNames = GetDirectories(directoryPath);
                if (directoryNames.Length > 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {

                return true;
            }
        }
        #endregion

        #region 获取指定目录中所有文件列表
        /// <summary>
        /// 获取指定目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>  
        public static string[] GetFileNames(string directoryPath)
        {
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }
            return Directory.GetFiles(directoryPath);
        }
        #endregion

        #region 获取指定目录中的子目录列表
        /// <summary>
        /// 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static string[] GetDirectories(string directoryPath)
        {
            try
            {
                return Directory.GetDirectories(directoryPath);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 获取指定目录及子目录中所有子目录列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                if (isSearchChild)
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 获取一个文件的长度
        /// <summary> 
        /// 获取一个文件的长度,单位为Byte 
        /// </summary> 
        /// <param name="filePath">文件的绝对路径</param>         
        public static int GetFileSize(string filePath)
        {
            //创建一个文件对象 
            FileInfo fi = new FileInfo(filePath);
            //获取文件的大小 
            return (int)fi.Length;
        }
        /// <summary> 
        /// 获取一个文件的长度,单位为KB 
        /// </summary> 
        /// <param name="filePath">文件的路径</param>         
        public static double GetFileSizeByKb(string filePath)
        {
            //创建一个文件对象 
            FileInfo fi = new FileInfo(filePath);
            //获取文件的大小 
            return Math.Round(Convert.ToDouble(filePath.Length) / 1024, 2);// ConvertHelper.ToDouble(ConvertHelper.ToDouble(fi.Length) / 1024, 1);
        }

        /// <summary> 
        /// 获取一个文件的长度,单位为MB 
        /// </summary> 
        /// <param name="filePath">文件的路径</param>         
        public static double GetFileSizeByMb(string filePath)
        {
            //创建一个文件对象 
            FileInfo fi = new FileInfo(filePath);
            //获取文件的大小 
            return Math.Round(Convert.ToDouble(Convert.ToDouble(fi.Length) / 1024 / 1024), 2);
        }

        #endregion

        #region 获取文件大小并以B，KB，GB，TB
        /// <summary>
        /// 计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        /// <param name="size">初始文件大小</param>
        /// <returns></returns>
        public static string ToFileSize(long size)
        {
            string m_strSize = "";
            long FactSize = 0;
            FactSize = size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " 字节";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " KB";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " MB";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " GB";
            return m_strSize;
        }
        #endregion

        #region 将文件读取到字符串中
        /// <summary>
        /// 将文件读取到字符串中
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static string FileToString(string filePath)
        {
            return FileToString(filePath, Encoding.UTF8);
        }
        /// <summary>
        /// 将文件读取到字符串中
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="encoding">字符编码</param>
        public static string FileToString(string filePath, Encoding encoding)
        {
            //创建流读取器
            StreamReader reader = new StreamReader(filePath, encoding);
            try
            {
                //读取流
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //关闭流读取器
                reader.Close();
            }
        }
        #endregion

        #region 判断文件
        // 判断文件是否是bai图片
        public static bool IsPicture(string fileName)
        {
            string strFilter = ".jpeg|.gif|.jpg|.png|.bmp|.pic|.tiff|.ico|.iff|.lbm|.mag|.mac|.mpt|.opt|";
            char[] separtor = { '|' };
            string[] tempFileds = StringSplit(strFilter, separtor);
            foreach (string str in tempFileds)
            {
                if (str.ToUpper() == fileName.Substring(fileName.LastIndexOf("."), fileName.Length - fileName.LastIndexOf(".")).ToUpper()) { return true; }
            }
            return false;
        }
        // 判断文件是否是excle
        public static bool IsExcel(string fileName)
        {
            string strFilter = ".xls|.xlsx|";
            char[] separtor = { '|' };
            string[] tempFileds = StringSplit(strFilter, separtor);
            foreach (string str in tempFileds)
            {
                if (str.ToUpper() == fileName.Substring(fileName.LastIndexOf("."), fileName.Length - fileName.LastIndexOf(".")).ToUpper()) { return true; }
            }
            return false;
        }
        // 通过字符串，分隔符返回zhistring[]数组 
        public static string[] StringSplit(string s, char[] separtor)
        {
            string[] tempFileds = s.Trim().Split(separtor); return tempFileds;
        }
        #endregion
    }
}
