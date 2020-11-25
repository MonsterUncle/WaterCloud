using System;
using System.IO;
using System.Text;
using System.Xml;
using WaterCloud.Code.Method;


namespace WaterCloud.Code.Templates
{
	public class TemplateModel
	{
		private static string _xmlName = "_self.xml";

		private string _name;

		private string _tag;

		private string _author;

		private DateTime _crtTime;

		private string _qq;

		private string _intro;

		private string _size;

		private ulong _length;

		private _Path _path;

		private bool _isCurrent = false;

		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		public string Tag
		{
			get
			{
				return this._tag;
			}
			set
			{
				this._tag = value;
			}
		}

		public string Author
		{
			get
			{
				return this._author;
			}
			set
			{
				this._author = value;
			}
		}

		public DateTime CrtTime
		{
			get
			{
				return this._crtTime;
			}
			set
			{
				this._crtTime = value;
			}
		}

		public string QQ
		{
			get
			{
				return this._qq;
			}
			set
			{
				this._qq = value;
			}
		}

		public string Intro
		{
			get
			{
				return this._intro;
			}
			set
			{
				this._intro = value;
			}
		}

		public string Size
		{
			get
			{
				return this._size;
			}
		}

		public ulong Length
		{
			get
			{
				return this._length;
			}
		}

		public string Logo
		{
			get
			{
				return this.Path.Virtual + "logo.jpg";
			}
		}

		public _Path Path
		{
			get
			{
				return this._path;
			}
			set
			{
				this._path = value;
			}
		}

		public bool IsCurrent
		{
			get
			{
				return this._isCurrent;
			}
			set
			{
				this._isCurrent = value;
			}
		}

		public TemplateModel(DirectoryInfo di)
		{
			this._init(di);
		}

		private void _init(DirectoryInfo di)
		{
			string text = di.FullName + "\\" + TemplateModel._xmlName;
			if (!File.Exists(text))
			{
				this._creatXML(text);
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(text);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("Template");
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("Info");
			foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
			{
				XmlElement xmlElement = (XmlElement)xmlNode3;
				if (xmlElement.Name == "Name")
				{
					this._name = (string.IsNullOrWhiteSpace(xmlElement.InnerText) ? di.Name : xmlElement.InnerText);
				}
				if (xmlElement.Name == "Author")
				{
					this._author = xmlElement.InnerText;
				}
				if (xmlElement.Name == "CrtTime")
				{
					try
					{
						this._crtTime = (string.IsNullOrWhiteSpace(xmlElement.InnerText) ? DateTime.Now : Convert.ToDateTime(xmlElement.InnerText));
					}
					catch
					{
						this._crtTime = DateTime.Now;
					}
				}
				if (xmlElement.Name == "QQ")
				{
					this._qq = xmlElement.InnerText;
				}
				if (xmlElement.Name == "Intro")
				{
					this._intro = xmlElement.InnerText;
				}
			}
			this._length =HttpHelper.Path(di.FullName).Length;
			this._size = HttpHelper.Path("").GetSize(this._length);
		}

		private void _creatXML(string xmlPath)
		{
			StreamWriter streamWriter = new StreamWriter(xmlPath, true, Encoding.UTF8);
			streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
			streamWriter.WriteLine("<Template>");
			streamWriter.WriteLine("<Info>");
			streamWriter.WriteLine("<Name/>");
			streamWriter.WriteLine("<Author/>");
			streamWriter.WriteLine("<CrtTime/>");
			streamWriter.WriteLine("<QQ/>");
			streamWriter.WriteLine("<Intro/>");
			streamWriter.WriteLine("</Info>");
			streamWriter.WriteLine("</Template>");
			streamWriter.Close();
			streamWriter.Dispose();
		}

		public void Save()
		{
			string text = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
			text += "<Template>";
			text += "<Info>";
			text = text + "<Name>" + this._name + "</Name>";
			text = text + "<Author>" + this._author + "</Author>";
			text = text + "<CrtTime>" + this.CrtTime.ToString() + "</CrtTime>";
			text = text + "<QQ>" + this.QQ + "</QQ>";
			text = text + "<Intro>" + this.Intro + "</Intro>";
			text += "</Info>";
			text += "</Template>";
			string path = this.Path.Physics + "\\" + TemplateModel._xmlName;
			using (StreamWriter streamWriter = new StreamWriter(path, false, Encoding.UTF8))
			{
				streamWriter.Write(text);
			}
		}
	}
}
