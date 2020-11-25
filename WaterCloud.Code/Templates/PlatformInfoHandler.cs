using System;
using System.Configuration;
using System.Xml;

namespace WaterCloud.Code
{
    public class PlatformInfoHandler : IConfigurationSectionHandler
    {
        public class SiteInfoHandlerParaNode
        {
            private XmlNodeList _list;

            private XmlNode _node;

            private string _key;

            private string _value;

            public string Key
            {
                get
                {
                    return this._key;
                }
            }

            public ConvertToAnyValue Value
            {
                get
                {
                    return new ConvertToAnyValue(this._value);
                }
            }

            public PlatformInfoHandler.SiteInfoHandlerParaNode FirstChildren
            {
                get
                {
                    PlatformInfoHandler.SiteInfoHandlerParaNode result;
                    if (this._list.Count < 1)
                    {
                        result = null;
                    }
                    else
                    {
                        result = new PlatformInfoHandler.SiteInfoHandlerParaNode(this._list[0]);
                    }
                    return result;
                }
            }

            public PlatformInfoHandler.SiteInfoHandlerParaNode[] Children
            {
                get
                {
                    PlatformInfoHandler.SiteInfoHandlerParaNode[] result;
                    if (this._list.Count < 1)
                    {
                        result = null;
                    }
                    else
                    {
                        PlatformInfoHandler.SiteInfoHandlerParaNode[] array = new PlatformInfoHandler.SiteInfoHandlerParaNode[this._list.Count];
                        for (int i = 0; i < this._list.Count; i++)
                        {
                            array[i] = new PlatformInfoHandler.SiteInfoHandlerParaNode(this._list[0]);
                        }
                        result = array;
                    }
                    return result;
                }
            }

            public PlatformInfoHandler.SiteInfoHandlerParaNode this[string key]
            {
                get
                {
                    XmlNode node = null;
                    foreach (XmlNode xmlNode in this._list)
                    {
                        if (xmlNode.Attributes["key"].Value.ToLower() == key.Trim().ToLower())
                        {
                            node = xmlNode;
                            break;
                        }
                    }
                    return new PlatformInfoHandler.SiteInfoHandlerParaNode(node);
                }
            }

            public SiteInfoHandlerParaNode(XmlNode node)
            {
                XmlNodeList elementsByTagName = ((XmlElement)node).GetElementsByTagName("item");
                this._list = elementsByTagName;
                this._node = node;
                this._key = ((node.Attributes["key"] != null) ? node.Attributes["key"].Value : "");
                this._value = ((node.Attributes["value"] != null) ? node.Attributes["value"].Value : "");
            }

            public string GetAttr(string attr)
            {
                return (this._node.Attributes[attr] != null) ? this._node.Attributes[attr].Value : "";
            }

            public string ItemValue(string key)
            {
                string result = "";
                foreach (XmlNode xmlNode in this._list)
                {
                    if (xmlNode.Attributes["key"].Value.ToLower() == key.Trim().ToLower())
                    {
                        result = xmlNode.Attributes["value"].Value;
                        break;
                    }
                }
                return result;
            }
        }

        object IConfigurationSectionHandler.Create(object parent, object configContext, XmlNode section)
        {
            return section;
        }

        public static XmlNode GetParaNode(string xpath)
        {
            object section = ConfigurationManager.GetSection("Platform");
            XmlElement xmlElement = section as XmlElement;
            return xmlElement.SelectSingleNode(xpath);
        }

        public static PlatformInfoHandler.SiteInfoHandlerParaNode Node(string xpath)
        {
            object section = ConfigurationManager.GetSection("Platform");
            XmlElement xmlElement = section as XmlElement;
            PlatformInfoHandler.SiteInfoHandlerParaNode result;
            if (xmlElement == null)
            {
                result = null;
            }
            else
            {
                XmlNode node = xmlElement.SelectSingleNode(xpath);
                PlatformInfoHandler.SiteInfoHandlerParaNode siteInfoHandlerParaNode = new PlatformInfoHandler.SiteInfoHandlerParaNode(node);
                result = siteInfoHandlerParaNode;
            }
            return result;
        }
    }
}
