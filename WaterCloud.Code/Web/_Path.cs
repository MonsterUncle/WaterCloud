using System;
using System.IO;

namespace WaterCloud.Code.Method
{
	public class _Path
	{
		private string _path = "";

		private string _virtualPath = "";

		private string _value = "";

		public string Physics
		{
			get
			{
				return Server.MapPath(this._virtualPath);
			}
		}

		public string Virtual
		{
			get
			{
				return Server.VirtualPath(this._virtualPath);
			}
		}

		public string String
		{
			get
			{
				return this._value;
			}
		}

		public ulong Length
		{
			get
			{
				return this.getLength(this._path);
			}
		}

		public _Path(string root, string path)
		{
			path = (string.IsNullOrWhiteSpace(path) ? "" : path);
			this._value = path;
			if (root == null)
			{
				root = "/";
			}
			if (root.Length < 1)
			{
				root = "/";
			}
			string a = root.Substring(root.Length - 1, 1);
			if (a != "\\" && a != "/")
			{
				root += "/";
			}
			this._path = root + path;
			this._virtualPath = root + path;
			if (!Directory.Exists(this.Physics))
			{
				Directory.CreateDirectory(this.Physics);
			}
		}

		private ulong getLength(string path)
		{
			ulong result;
			if (!Directory.Exists(path))
			{
				result = 0uL;
			}
			else
			{
				ulong num = 0uL;
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				FileInfo[] files = directoryInfo.GetFiles();
				for (int i = 0; i < files.Length; i++)
				{
					FileInfo fileInfo = files[i];
					num += (ulong)fileInfo.Length;
				}
				DirectoryInfo[] directories = directoryInfo.GetDirectories();
				for (int i = 0; i < directories.Length; i++)
				{
					DirectoryInfo directoryInfo2 = directories[i];
					num += this.getLength(directoryInfo2.FullName);
				}
				result = num;
			}
			return result;
		}

		public string GetSize(ulong size)
		{
			string result = "";
			if (size < 1024uL)
			{
				result = size.ToString() + " Bit";
			}
			else
			{
				double num = size / 1024uL;
				num = Math.Round(num * 100.0) / 100.0;
				if (num < 1024.0)
				{
					result = num.ToString() + " Kb";
				}
				else
				{
					double num2 = num / 1024.0;
					num2 = Math.Round(num2 * 100.0) / 100.0;
					if (num2 < 1024.0)
					{
						result = num2.ToString() + " Mb";
					}
				}
			}
			return result;
		}

		public int DeleteFile(string filename)
		{
			string text = this.Physics + filename;
			string path = text;
			if (text.IndexOf(".") > -1)
			{
				string str = text.Substring(text.LastIndexOf("."));
				path = text.Substring(0, text.LastIndexOf(".")) + "_small" + str;
			}
			try
			{
				File.Delete(text);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return 1;
		}
	}
}
