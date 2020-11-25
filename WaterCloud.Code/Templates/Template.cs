using System;
using System.IO;
using WaterCloud.Code.Method;
using WaterCloud.Code.Templates;

namespace WaterCloud.Code
{
	public class Template
	{
		public static readonly string TemplateNodeName = "Template";

        private static TemplateConfingItem _forWeb = new TemplateConfingItem("Web");
        //private static TemplateConfingItem _forWeb = new TemplateConfingItem("HomeSite"); 
        private static TemplateConfingItem _forMobi = new TemplateConfingItem("Mobile");

        private static TemplateConfingItem _forSite;

        public static TemplateConfingItem ForWeb
		{
			get
			{
				return Template._forWeb;
			}
		}
       
        public static TemplateConfingItem ForMobile
		{
			get
			{
				return Template._forMobi;
			}
		}

        public static TemplateConfingItem ForSite(string siteName)
        {
            return _forSite = new TemplateConfingItem(siteName);
        }
        public static TemplateModel GetTemplate(string type, string tag)
		{
			string attr = PlatformInfoHandler.Node(Template.TemplateNodeName).GetAttr("path");
			string @string = PlatformInfoHandler.Node(Template.TemplateNodeName)[type].Value.String;
			string virtualPath = new ConvertToAnyValue(attr + @string).VirtualPath;
			DirectoryInfo directoryInfo = new DirectoryInfo(Server.MapPath(virtualPath) + tag);
			return new TemplateModel(directoryInfo)
			{
				Path = new _Path(virtualPath, tag),
				Tag = directoryInfo.Name
			};
		}
	}
}
