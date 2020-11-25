using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace WaterCloud.Code.Method
{
	public class ConvertToAnyValue
	{
		private object _paravlue;

		private string _parakey = "";

		private string _unit = "";

		public string ParaValue
		{
			get
			{
				return this._paravlue.ToString();
			}
			set
			{
				this._paravlue = value;
			}
		}

		public string ParaKey
		{
			get
			{
				return this._parakey;
			}
			set
			{
				this._parakey = value;
			}
		}

		public string Unit
		{
			get
			{
				return this._unit;
			}
			set
			{
				this._unit = value;
			}
		}

		public int? Int16
		{
			get
			{
				int? result;
				if (this._paravlue == null)
				{
					result = null;
				}
				else
				{
					try
					{
						result = new int?((int)Convert.ToInt16(this._paravlue));
					}
					catch
					{
						result = null;
					}
				}
				return result;
			}
		}

		public int? Int32
		{
			get
			{
				int? result;
				if (this._paravlue == null)
				{
					result = null;
				}
				else
				{
					try
					{
						result = new int?(Convert.ToInt32(this._paravlue));
					}
					catch
					{
						result = null;
					}
				}
				return result;
			}
		}

		public long? Int64
		{
			get
			{
				long? result;
				if (this._paravlue == null)
				{
					result = null;
				}
				else
				{
					try
					{
						result = new long?(Convert.ToInt64(this._paravlue));
					}
					catch
					{
						result = null;
					}
				}
				return result;
			}
		}

		public double? Double
		{
			get
			{
				double? result;
				if (this._paravlue == null)
				{
					result = null;
				}
				else
				{
					try
					{
						result = new double?(Convert.ToDouble(this._paravlue));
					}
					catch
					{
						result = null;
					}
				}
				return result;
			}
		}

		public string String
		{
			get
			{
				return (this._paravlue == null) ? "" : this._paravlue.ToString().Trim();
			}
		}

		public bool? Boolean
		{
			get
			{
				bool? result;
				if (this._paravlue == null)
				{
					result = null;
				}
				else
				{
					try
					{
						result = new bool?(Convert.ToBoolean(this._paravlue));
					}
					catch
					{
						result = null;
					}
				}
				return result;
			}
		}

		public DateTime? DateTime
		{
			get
			{
				DateTime? result;
				if (this._paravlue == null)
				{
					result = null;
				}
				else
				{
					try
					{
						result = new DateTime?(Convert.ToDateTime(this._paravlue));
					}
					catch
					{
						string text = this._paravlue.ToString();
						string text2 = text;
						for (int i = 0; i < text2.Length; i++)
						{
							char c = text2[i];
							text += ((c >= '0' && c <= '9') ? c : '-');
						}
						string text3 = text;
						Regex regex = new Regex("\\-{1,}", RegexOptions.IgnorePatternWhitespace);
						text3 = regex.Replace(text3, "-");
						regex = new Regex("^\\-{1,}(\\w)", RegexOptions.IgnorePatternWhitespace);
						text3 = regex.Replace(text3, "$1");
						regex = new Regex("(\\w)\\-{1,}$", RegexOptions.IgnorePatternWhitespace);
						text3 = regex.Replace(text3, "$1");
						if (text3.IndexOf('-') > -1)
						{
							string text4 = text3.Substring(0, text3.IndexOf('-'));
							text4 = ((text4.Length == 1) ? ("0" + text4) : text4);
							text4 = ((text4.Length == 2) ? ("19" + text4) : text4);
							text4 = ((text4.Length > 4) ? text4.Substring(0, 4) : text4);
							text3 = text4 + "-" + text3.Substring(text3.IndexOf('-') + 1);
						}
						else
						{
							text3 += "-1-1";
						}
						try
						{
							result = new DateTime?(Convert.ToDateTime(text3));
						}
						catch
						{
							result = null;
						}
					}
				}
				return result;
			}
		}

		public string MD5
		{
			get
			{
				string result;
				if (this._paravlue == null)
				{
					result = "";
				}
				else if (string.IsNullOrWhiteSpace(this._paravlue.ToString()))
				{
					result = "";
				}
				else
				{
					result = FormsAuthentication.HashPasswordForStoringInConfigFile(this._paravlue.ToString(), "MD5").ToLower();
				}
				return result;
			}
		}

		public string SHA1
		{
			get
			{
				string result;
				if (this._paravlue == null)
				{
					result = null;
				}
				else
				{
					result = FormsAuthentication.HashPasswordForStoringInConfigFile(this._paravlue.ToString(), "SHA1");
				}
				return result;
			}
		}

		public string UrlDecode
		{
			get
			{
				string result;
				if (this._paravlue == null)
				{
					result = null;
				}
				else
				{
					result = HttpUtility.UrlDecode(this._paravlue.ToString().Trim());
				}
				return result;
			}
		}

		public string UrlEncode
		{
			get
			{
				string result;
				if (this._paravlue == null)
				{
					result = null;
				}
				else
				{
					result = HttpUtility.UrlEncode(this._paravlue.ToString().Trim());
				}
				return result;
			}
		}

		public string HtmlDecode
		{
			get
			{
				string result;
				if (this._paravlue == null)
				{
					result = null;
				}
				else
				{
					HttpContext current = HttpContext.Current;
					result = current.Server.HtmlDecode(this._paravlue.ToString());
				}
				return result;
			}
		}

		public string MapPath
		{
			get
			{
				return Server.MapPath(this._paravlue.ToString());
			}
		}

		public string VirtualPath
		{
			get
			{
				return Server.VirtualPath(this._paravlue.ToString());
			}
		}

		public string JavascriptTime
		{
			get
			{
				string result;
				if (this._paravlue == null)
				{
					result = "";
				}
				else
				{
					try
					{
						DateTime dateTime = System.DateTime.Now;
						dateTime = Convert.ToDateTime(this._paravlue);
						string format = "ddd MMM d HH:mm:ss 'UTC'zz'00' yyyy";
						CultureInfo provider = CultureInfo.CreateSpecificCulture("en-US");
						string text = dateTime.ToString(format, provider);
						result = text;
					}
					catch
					{
						result = "";
					}
				}
				return result;
			}
		}

		public ConvertToAnyValue()
		{
		}

		public ConvertToAnyValue(object para)
		{
			this._paravlue = ((para == null) ? "" : para);
		}

		public ConvertToAnyValue(string para)
		{
			this._paravlue = (string.IsNullOrWhiteSpace(para) ? "" : para);
		}

		public ConvertToAnyValue(string para, string unit)
		{
			this._paravlue = para;
		}

		public string[] Split(char split)
		{
			return this._paravlue.ToString().Split(new char[]
			{
				split
			});
		}
	}
}
