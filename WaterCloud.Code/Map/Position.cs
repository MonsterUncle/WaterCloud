using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace WaterCloud.Code.Method
{
	public class Position : IDisposable
	{

        private string ak = "";//App.Get["BaiduLBS"].String;

		private string _ip;

		private string _latitude;

		private string _longitude;

		private string _province;

		private string _city;

		private string _district;

		private string _street;

		private string _streetNumber;

		private string _address;

		private bool disposed = false;

		public string IPAddress
		{
			get
			{
				return this._ip;
			}
		}

		public string Latitude
		{
			get
			{
				return this._latitude;
			}
		}

		public string Longitude
		{
			get
			{
				return this._longitude;
			}
		}

		public string Province
		{
			get
			{
				return this._province;
			}
		}

		public string City
		{
			get
			{
				return this._city;
			}
		}

		public string District
		{
			get
			{
				return this._district;
			}
		}

		public string Street
		{
			get
			{
				return this._street;
			}
		}

		public string StreetNumber
		{
			get
			{
				return this._streetNumber;
			}
		}

		public string Address
		{
			get
			{
				return this._address;
			}
		}

		public Position(string ip)
		{
			this._ip = ip;
			this._getPosiForIP(this._ip);
		}

		public Position(string lng, string lat)
		{
			this._longitude = lng;
			this._latitude = lat;
			this._ip = Browser.IP;
			this._getPosiForGPS(lng, lat);
		}

		public Position(string address, int type)
		{
			try
			{
				this._getPosiForAddress(address, type);
				this._getPosiForGPS(this._longitude, this._latitude);
			}
			catch
			{
			}
			this._address = address;
		}

		private string _getPosiForIP(string ip)
		{
			string text = "http://api.map.baidu.com/location/ip?ak={0}&ip={1}&coor=bd09ll";
			text = string.Format(text, this.ak, ip);
			WebClient webClient = new WebClient();
			webClient.BaseAddress = text;
			string text2 = "";
			try
			{
				using (Stream stream = webClient.OpenRead(text))
				{
					StreamReader streamReader = new StreamReader(stream, Encoding.Default);
					text2 = streamReader.ReadToEnd();
					streamReader.Close();
				}
			}
			catch
			{
			}
			text2 = this._unicodeToGB(text2);
			this._province = this._getJson(text2, "province");
			this._city = this._getJson(text2, "city");
			this._district = this._getJson(text2, "district");
			this._street = this._getJson(text2, "street");
			this._streetNumber = this._getJson(text2, "street_number");
			this._latitude = this._getJson(text2, "y");
			this._longitude = this._getJson(text2, "x");
			this._address = this._getJson(text2, "address");
			this._address = this._clearNoChinise(this._address);
			return text2;
		}

		private string _clearNoChinise(string text)
		{
			char[] array = text.ToCharArray();
			string text2 = "";
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] >= '一' && array[i] <= '龻')
				{
					text2 += array[i].ToString();
				}
			}
			return text2;
		}

		private string _unicodeToGB(string text)
		{
			MatchCollection matchCollection = Regex.Matches(text, "\\\\u([\\w]{4})");
			if (matchCollection != null && matchCollection.Count > 0)
			{
				foreach (Match match in matchCollection)
				{
					string value = match.Value;
					string text2 = value.Substring(2);
					byte[] array = new byte[2];
					int num = Convert.ToInt32(text2.Substring(0, 2), 16);
					int num2 = Convert.ToInt32(text2.Substring(2), 16);
					array[0] = (byte)num2;
					array[1] = (byte)num;
					text = text.Replace(value, Encoding.Unicode.GetString(array));
				}
			}
			return text;
		}

		private string _getJson(string result, string key)
		{
			string text = "\"{0}\":\"(?<value>[^\"]+)\"";
			text = string.Format(text, key);
			Regex regex = new Regex(text, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
			MatchCollection matchCollection = regex.Matches(result);
			string result2 = "";
			foreach (Match match in matchCollection)
			{
				result2 = match.Groups["value"].Value.Trim();
			}
			return result2;
		}

		private void _getPosiForGPS(string lng, string lat)
		{
			string text = "http://api.map.baidu.com/geocoder/v2/?ak={0}&location={1},{2}&output=xml&pois=0";
			text = string.Format(text, this.ak, lat, lng);
			WebClient webClient = new WebClient();
			webClient.BaseAddress = text;
			string xml = "";
			try
			{
				using (Stream stream = webClient.OpenRead(text))
				{
					StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
					xml = streamReader.ReadToEnd();
					streamReader.Close();
				}
			}
			catch
			{
			}
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);
				XmlNode xmlNode = xmlDocument.SelectSingleNode("GeocoderSearchResponse/status");
				if (xmlNode.InnerText == "0")
				{
					XmlNode xmlNode2 = xmlDocument.SelectSingleNode("GeocoderSearchResponse/result/formatted_address");
					this._address = xmlNode2.InnerText;
					XmlNode xmlNode3 = xmlDocument.SelectSingleNode("GeocoderSearchResponse/result/addressComponent");
					this._province = xmlNode3.SelectSingleNode("province").InnerText;
					this._city = xmlNode3.SelectSingleNode("city").InnerText;
					this._district = xmlNode3.SelectSingleNode("district").InnerText;
					this._street = xmlNode3.SelectSingleNode("street").InnerText;
					this._streetNumber = xmlNode3.SelectSingleNode("streetNumber").InnerText;
				}
			}
			catch
			{
			}
		}

		private void _getPosiForAddress(string address, int type)
		{
			string arg = (type == 1) ? "xml" : "json";
			address = HttpUtility.UrlEncode(address);
			string text = "http://api.map.baidu.com/geocoder/v2/?ak={0}&output={1}&address={2}";
			text = string.Format(text, this.ak, arg, address);
			WebClient webClient = new WebClient();
			webClient.BaseAddress = text;
			string xml = "";
			try
			{
				using (Stream stream = webClient.OpenRead(text))
				{
					StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
					xml = streamReader.ReadToEnd();
					streamReader.Close();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xml);
				XmlNode xmlNode = xmlDocument.SelectSingleNode("GeocoderSearchResponse/status");
				if (xmlNode.InnerText == "0")
				{
					XmlNode xmlNode2 = xmlDocument.SelectSingleNode("GeocoderSearchResponse/result/location");
					this._latitude = xmlNode2.SelectSingleNode("lat").InnerText;
					this._longitude = xmlNode2.SelectSingleNode("lng").InnerText;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
				}
				this.disposed = true;
			}
		}

		~Position()
		{
			this.Dispose(false);
		}
	}
}
