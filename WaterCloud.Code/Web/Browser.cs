using System;
using System.Text.RegularExpressions;
using System.Web;

namespace WaterCloud.Code
{
	public class Browser
	{
		public static bool IsMobile
		{
			get
			{
				HttpContext current = HttpContext.Current;
				string text = current.Request.ServerVariables["HTTP_USER_AGENT"];
				Regex regex = new Regex("android|avantgo|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\\\\/|plucker|pocket|psp|symbian|treo|up\\\\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
				Regex regex2 = new Regex("1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\\\\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\\\\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\\\\-(n|u)|c55\\\\/|capi|ccwa|cdm\\\\-|cell|chtm|cldc|cmd\\\\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\\\\-s|devi|dica|dmob|do(c|p)o|ds(12|\\\\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\\\\-|_)|g1 u|g560|gene|gf\\\\-5|g\\\\-mo|go(\\\\.w|od)|gr(ad|un)|haie|hcit|hd\\\\-(m|p|t)|hei\\\\-|hi(pt|ta)|hp( i|ip)|hs\\\\-c|ht(c(\\\\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\\\\-(20|go|ma)|i230|iac( |\\\\-|\\\\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\\\\/)|klon|kpt |kwc\\\\-|kyo(c|k)|le(no|xi)|lg( g|\\\\/(k|l|u)|50|54|e\\\\-|e\\\\/|\\\\-[a-w])|libw|lynx|m1\\\\-w|m3ga|m50\\\\/|ma(te|ui|xo)|mc(01|21|ca)|m\\\\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\\\\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\\\\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\\\\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\\\\-2|po(ck|rt|se)|prox|psio|pt\\\\-g|qa\\\\-a|qc(07|12|21|32|60|\\\\-[2-7]|i\\\\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\\\\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\\\\-|oo|p\\\\-)|sdk\\\\/|se(c(\\\\-|0|1)|47|mc|nd|ri)|sgh\\\\-|shar|sie(\\\\-|m)|sk\\\\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\\\\-|v\\\\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\\\\-|tdg\\\\-|tel(i|m)|tim\\\\-|t\\\\-mo|to(pl|sh)|ts(70|m\\\\-|m3|m5)|tx\\\\-9|up(\\\\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\\\\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\\\\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|xda(\\\\-|2|g)|yas\\\\-|your|zeto|zte\\\\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
				return regex.IsMatch(text) || regex2.IsMatch(text.Substring(0, 4));
			}
		}

		public static string IP
		{
			get
			{
				string text = string.Empty;
				text = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
				string result;
				if (text != null && text != string.Empty)
				{
					if (text.IndexOf(".") == -1)
					{
						text = null;
					}
					else if (text.IndexOf(",") != -1)
					{
						text = text.Replace(" ", "").Replace("\"", "");
						string[] array = text.Split(",;".ToCharArray());
						for (int i = 0; i < array.Length; i++)
						{
							if (Browser.IsIPAddress(array[i]) && array[i].Substring(0, 3) != "10." && array[i].Substring(0, 7) != "192.168" && array[i].Substring(0, 7) != "172.16.")
							{
								result = array[i];
								return result;
							}
						}
					}
					else
					{
						if (Browser.IsIPAddress(text))
						{
							result = text;
							return result;
						}
						text = null;
					}
				}
				if (text == null || text == string.Empty)
				{
					text = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
				}
				if (text == null || text == string.Empty)
				{
					text = HttpContext.Current.Request.UserHostAddress;
				}
				result = text;
				return result;
			}
		}

		public static string Type
		{
			get
			{
				HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
				return browser.Type;
			}
		}

		public static string Name
		{
			get
			{
				HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
				return browser.Browser;
			}
		}

		public static string Version
		{
			get
			{
				HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
				return browser.Version;
			}
		}

		public static string OS
		{
			get
			{
				string text = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
				string result;
				if (text.IndexOf("NT 4.0") > 0)
				{
					result = "Windows NT ";
				}
				else if (text.IndexOf("NT 5.0") > 0)
				{
					result = "Windows 2000";
				}
				else if (text.IndexOf("NT 5.1") > 0)
				{
					result = "Windows XP";
				}
				else if (text.IndexOf("NT 5.2") > 0)
				{
					result = "Windows 2003";
				}
				else if (text.IndexOf("NT 6.0") > 0)
				{
					result = "Windows Vista";
				}
				else if (text.IndexOf("NT 6.1") > 0)
				{
					result = "Windows 2008";
				}
				else if (text.IndexOf("WindowsCE") > 0)
				{
					result = "Windows CE";
				}
				else if (text.IndexOf("NT") > 0)
				{
					result = "Windows NT ";
				}
				else if (text.IndexOf("9x") > 0)
				{
					result = "Windows ME";
				}
				else if (text.IndexOf("98") > 0)
				{
					result = "Windows 98";
				}
				else if (text.IndexOf("95") > 0)
				{
					result = "Windows 95";
				}
				else if (text.IndexOf("Win32") > 0)
				{
					result = "Win32";
				}
				else if (text.IndexOf("Linux") > 0)
				{
					result = "Linux";
				}
				else if (text.IndexOf("SunOS") > 0)
				{
					result = "SunOS";
				}
				else if (text.IndexOf("Mac") > 0)
				{
					result = "Mac";
				}
				else if (text.IndexOf("Linux") > 0)
				{
					result = "Linux";
				}
				else if (text.IndexOf("Windows") > 0)
				{
					result = "Windows";
				}
				else
				{
					result = "未知类型";
				}
				return result;
			}
		}

		public static string MobileOS
		{
			get
			{
				HttpContext current = HttpContext.Current;
				string input = current.Request.ServerVariables["HTTP_USER_AGENT"];
				Regex regex = new Regex("android|avantgo|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\\\\/|plucker|pocket|psp|symbian|treo|up\\\\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
				MatchCollection matchCollection = regex.Matches(input);
				string result;
				if (matchCollection.Count < 1)
				{
					result = Browser.OS;
				}
				else
				{
					result = matchCollection[0].Value;
				}
				return result;
			}
		}

		public static string PhoneNumber
		{
			get
			{
				string text = "";
				HttpContext current = HttpContext.Current;
				if (current.Request.ServerVariables["DEVICEID"] != null)
				{
					text = current.Request.ServerVariables["DEVICEID"].ToString();
				}
				if (current.Request.ServerVariables["HTTP_X_UP_subno"] != null)
				{
					text = current.Request.ServerVariables["HTTP_X_UP_subno"].ToString();
					text = text.Substring(3, 11);
				}
				if (current.Request.ServerVariables["HTTP_X_NETWORK_INFO"] != null)
				{
					text = current.Request.ServerVariables["HTTP_X_NETWORK_INFO"].ToString();
				}
				if (current.Request.ServerVariables["HTTP_X_UP_CALLING_LINE_ID"] != null)
				{
					text = current.Request.ServerVariables["HTTP_X_UP_CALLING_LINE_ID"].ToString();
				}
				return text;
			}
		}

		private static bool IsIPAddress(string str1)
		{
			bool result;
			if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15)
			{
				result = false;
			}
			else
			{
				string pattern = "^\\d{1,3}[\\.]\\d{1,3}[\\.]\\d{1,3}[\\.]\\d{1,3}$";
				Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
				result = regex.IsMatch(str1);
			}
			return result;
		}
	}
}
