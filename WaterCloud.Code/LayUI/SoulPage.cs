using Serenity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WaterCloud.Code
{
	/// <summary>
	///封装table查询数据
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class SoulPage<T>
	{
		/**
     * layui表格必须参数⬇⬇⬇⬇⬇⬇
     */
		public int state { get; set; }
		public string message { get; set; }
		/**
         * 总记录
         */
		public int count { get; set; }
		/**
         * 显示的记录
         */
		public List<T> data { get; set; }

		/**
         * 请求条件
         */
		public object obj { get; set; }
		/**
         * 查询条件
         */
		public Dictionary<string, object> condition = new Dictionary<string, object>();
		/**
         * 请求参数⬇⬇⬇⬇⬇⬇
         */
		/**
         * 当前页 从1开始
         */
		public int page { get; set; }
		/**
         * 页大小
         */
		public int rows { get; set; }
		/**
         * 查询列数据
         */
		public string columns { get; set; }

		/**
         * 表格列类型
         */
		public string tableFilterType { get; set; }
		/**
         * 筛选信息
         */
		public string filterSos { get; set; }

		/**
         * 排序信息
         */
		public string field { get; set; }

		public string order { get; set; }

		public SoulPage()
		{
			this.state = 0;
			this.message = "";
			this.page = 1;
			this.rows = 100000000;
			this.order = "asc";
		}

		public SoulPage(int page, int limit)
		{
			this.state = 0;
			this.message = "";
			this.page = 1;
			this.rows = 100000000;
			this.order = "asc";
			this.page = page;
			this.rows = limit;
		}

		public List<FilterSo> getFilterSos()
		{
			if (string.IsNullOrEmpty(filterSos))
			{
				return new List<FilterSo>();
			}
			return filterSos.ToObject<List<FilterSo>>();
		}

		public bool isColumn()
		{
			return getColumns().Count > 0;
		}

		public int getOffset()
		{
			return (page - 1) * rows;
		}

		public List<string> getColumns()
		{
			return !string.IsNullOrEmpty(columns) ? columns.ToObject<List<string>>() : new List<string>();
		}

		private string dateFormat(DateTime date, string format)
		{
			if (string.IsNullOrEmpty(format))
			{
				return date.ToString("yyyy-MM-dd HH:mm:ss");
			}
			else
			{
				return date.ToString(format);
			}
		}

		public Dictionary<string, Dictionary<string, string>> getTypeMap()
		{
			Dictionary<string, Dictionary<string, string>> typeMap = new Dictionary<string, Dictionary<string, string>>();
			if (!string.IsNullOrEmpty(tableFilterType))
			{
				Dictionary<string, string> filterType = tableFilterType.ToObject<Dictionary<string, string>>();
				foreach (var item in filterType)
				{
					Dictionary<string, string> map = new Dictionary<string, string>();
					map.Add("type", item.Value.Substring(0, item.Value.IndexOf("[")));
					int IndexofA = item.Value.IndexOf('['); //字符串的话总以第一位为指定位置
					int IndexofB = item.Value.IndexOf(']');
					map.Add("value", item.Value.Substring(IndexofA + 1, IndexofB - IndexofA - 1));
					typeMap.Add(item.Key, map);
				};
			}
			return typeMap;
		}

		public string getFormatValue(Dictionary<string, Dictionary<string, string>> typeMap, string column, object columnObject)
		{
			string columnValue;
			if (typeMap.ContainsKey(column))
			{
				if ("date".Equals(typeMap.Get(column).Get("type")) && columnObject is DateTime)
				{
					columnValue = dateFormat((DateTime)columnObject, typeMap.Get(column).Get("value"));
				}
				else
				{
					columnValue = columnObject.ToString();
				}
			}
			else
			{
				if (columnObject is DateTime || columnObject is Nullable<DateTime>)
				{
					columnValue = dateFormat((DateTime)columnObject, null);
				}
				else if (columnObject is bool || columnObject is Nullable<bool>)
				{
					columnValue = (bool)columnObject == true ? "1" : "0";
				}
				else
				{
					columnValue = columnObject.ToString();
				}
			}
			return columnValue;
		}

		public Object setData(List<T> data)
		{
			if (isColumn())
			{
				Dictionary<string, Dictionary<string, string>> typeMap = getTypeMap();
				Dictionary<string, HashSet<string>> columnMap = new Dictionary<string, HashSet<string>>();
				foreach (T datum in data)
				{
					foreach (string column in getColumns())
					{
						if (!columnMap.ContainsKey(column))
						{
							columnMap.Add(column, new HashSet<string>());
						}
						var columnObject = ReflectionHelper.GetObjectPropertyValue(datum, column);
						if (columnObject != null)
						{ //空值不展示
							columnMap.Get(column).Add(getFormatValue(typeMap, column, columnObject));
						}
					}
				}
				Dictionary<string, List<string>> columnSortMap = new Dictionary<string, List<string>>();
				foreach (var item in columnMap)
				{
					columnSortMap.Add(item.Key, item.Value.ToList());
				}
				return columnSortMap;
			}
			else
			{
				this.data = data;
				return this;
			}
		}
	}
}