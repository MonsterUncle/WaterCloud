using System;
using System.IO;
using WaterCloud.Code;

namespace WaterCloud.Code.Templates
{
	public class TemplateConfingItem
	{
		private ConvertToAnyValue _tempbankPath;

		private PlatformInfoHandler.SiteInfoHandlerParaNode _nodeItem;

		private string _currentName = string.Empty;

		private TemplateModel[] _items;

		public _Path Root
		{
			get
			{
				return new _Path(this._tempbankPath.VirtualPath, "");
			}
		}

		public TemplateModel Default
		{
			get
			{
				string attr = this._nodeItem.GetAttr("default");
				TemplateModel result = null;
				TemplateModel[] items = this.Items;
				for (int i = 0; i < items.Length; i++)
				{
					TemplateModel templateBank = items[i];
					if (templateBank.Tag.ToLower() == attr.ToLower().Trim())
					{
						result = templateBank;
						break;
					}
				}
				return result;
			}
		}

		public TemplateModel Current
		{
			get
			{
				string text = this._currentName;
				if (string.IsNullOrWhiteSpace(this._currentName))
				{
					text = this._nodeItem.GetAttr("default");
				}
				TemplateModel result = null;
				TemplateModel[] items = this.Items;
				for (int i = 0; i < items.Length; i++)
				{
					TemplateModel templateBank = items[i];
					if (templateBank.Tag.ToLower() == text.ToLower().Trim())
					{
						result = templateBank;
						templateBank.IsCurrent = true;
					}
					else
					{
						templateBank.IsCurrent = false;
					}
				}
				return result;
			}
		}

		public TemplateModel[] Items
		{
			get
			{
				return this._items;
			}
		}

		public TemplateConfingItem(string bankName)
		{
			string attr = PlatformInfoHandler.Node(Template.TemplateNodeName).GetAttr("path");
			string @string = PlatformInfoHandler.Node(Template.TemplateNodeName)[bankName].Value.String;
			this._tempbankPath = new ConvertToAnyValue(attr + @string);
			this._nodeItem = PlatformInfoHandler.Node(Template.TemplateNodeName)[bankName];
			if (Directory.Exists(this.Root.Physics))
			{
				DirectoryInfo[] directories = new DirectoryInfo(this.Root.Physics).GetDirectories();
				TemplateModel[] array = new TemplateModel[directories.Length];
				for (int i = 0; i < directories.Length; i++)
				{
					DirectoryInfo directoryInfo = directories[i];
					array[i] = new TemplateModel(directoryInfo)
					{
						Path = this.GetPath(directoryInfo.Name),
						Tag = directoryInfo.Name
					};
				}
				this._items = array;
				string attr2 = this._nodeItem.GetAttr("default");
				TemplateModel[] items = this.Items;
				for (int j = 0; j < items.Length; j++)
				{
					TemplateModel templateBank = items[j];
					templateBank.IsCurrent = (templateBank.Tag.ToLower() == attr2.ToLower());
				}
			}
		}

		public _Path GetPath(string templateName)
		{
			return new _Path(this._tempbankPath.VirtualPath, templateName);
		}

		public TemplateModel SetCurrent(string tag)
		{
			this._currentName = tag;
			return this.Current;
		}

		public TemplateModel GetTemplate(string tag)
		{
			TemplateModel templateBank = null;
			TemplateModel[] items = this.Items;
			for (int i = 0; i < items.Length; i++)
			{
				TemplateModel templateBank2 = items[i];
				if (templateBank2.Tag.ToLower() == tag.ToLower().Trim())
				{
					templateBank = templateBank2;
					break;
				}
			}
			if (templateBank == null)
			{
				templateBank = this.Default;
			}
			return templateBank;
		}
	}
}
