using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Reflection;

namespace WaterCloud.DBUtility
{
    public class DALHelper<TEntity> where TEntity : class,new()
    {
        private string databasetablename; //数据库表名前缀

        public DALHelper()
        {
            databasetablename = "Buss_";
        }
        public DALHelper(string _databaseprefix)
        {
            databasetablename = _databaseprefix;
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(TEntity entity, string strField, object keyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from " + databasetablename);
            strSql.Append(" where "+ strField + "=@keyValue ");
            SqlParameter[] parameters = {
                 new SqlParameter("@keyValue",keyValue)
            };
            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId(TEntity entity, string strField)
        {
            return DbHelperSQL.GetMaxID(strField, databasetablename );
        }

        public bool Add(TEntity entity)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into "+ databasetablename + "(");

            PropertyInfo[] propertys = entity.GetType().GetProperties();// 获得此模型的公共属性

            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (PropertyInfo pi in propertys)
            {
                if (!pi.CanWrite) continue;
                strSql.Append(pi.Name + ",");
                strSql.Append(" values (");
            }
            strSql.Append(" ) values (");
            foreach (PropertyInfo pi in propertys)
            {
                if (!pi.CanWrite) continue;
                strSql.Append(pi.Name + "@,");
                parameters.Add(new SqlParameter("@" + pi.Name, pi.GetValue(entity)));
            }
            strSql.Append(");select @@IDENTITY");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters.ToArray());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Update(TEntity entity, string strField,object keyValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update "+ databasetablename + " set ");

            PropertyInfo[] propertys = entity.GetType().GetProperties();// 获得此模型的公共属性
            
            List<SqlParameter> parameters =new  List<SqlParameter>();
            foreach (PropertyInfo pi in propertys)
            {
                if (!pi.CanWrite) continue;
                strSql.Append(pi.Name + "=@" + pi.Name + ",");
                parameters.Add( new SqlParameter("@"+ pi.Name, pi.GetValue(entity)));
            }
            strSql.Append(" where "+strField + "=@strValue");
            parameters.Add(new SqlParameter("@strValue", keyValue));
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters.ToArray());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 修改一列数据
        /// </summary>
        public int UpdateField(TEntity entity, string strField, object strValue,string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update "+ databasetablename);
            strSql.Append(" set " + strField + "=@strValue");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" where " + strWhere);
            }
            SqlParameter[] parameters = {
                 new SqlParameter("@strValue",strValue)
            };

            return DbHelperSQL.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(TEntity entity, string strField, object keyValue)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from " + databasetablename );
            strSql.Append(" where " + strField + "=@keyValue ");
            SqlParameter[] parameters = {
                 new SqlParameter("@keyValue",keyValue)
            };

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public TEntity GetModel(TEntity entity, string strField, string keyValue)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 * from " + databasetablename);
            strSql.Append(" where " + strField + "=@keyValue ");
            SqlParameter[] parameters = {
                 new SqlParameter("@keyValue",keyValue)
            };

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(entity,ds.Tables[0]);
            }
            else
            {
                return null;
            }
        }

        public List<TEntity> GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 * from " + databasetablename);
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" where " + strWhere);
            }
            TEntity entity = new TEntity();
            DataSet ds = DbHelperSQL.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModels(entity, ds.Tables[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public TEntity DataRowToModel(TEntity model, DataTable dt)
        {
            if(dt.Rows.Count > 0)
            {
                TEntity t = new TEntity();
                PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性
                foreach (PropertyInfo pi in propertys)
                {
                    if (dt.Columns.Contains(pi.Name))
                    {
                        if (!pi.CanWrite) continue;
                        var value = dt.Rows[0][pi.Name];
                        if (value != DBNull.Value)
                        {
                            switch (pi.PropertyType.FullName)
                            {
                                case "System.Decimal":
                                    pi.SetValue(t, decimal.Parse(value.ToString()), null);
                                    break;
                                case "System.String":
                                    pi.SetValue(t, value.ToString(), null);
                                    break;
                                case "System.Single":
                                    pi.SetValue(t, float.Parse(value.ToString()), null);
                                    break;
                                case "System.Double":
                                    pi.SetValue(t, double.Parse(value.ToString()), null);
                                    break;
                                case "System.Int32":
                                    pi.SetValue(t, int.Parse(value.ToString()), null);
                                    break;
                                case "System.DateTime":
                                    pi.SetValue(t, DateTime.Parse(value.ToString()), null);
                                    break;
                                case "System.Boolean":
                                    pi.SetValue(t, bool.Parse(value.ToString()), null);
                                    break;
                                default:
                                    pi.SetValue(t, value, null);
                                    break;
                            }
                        }
                    }
                }
                return t;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 将DataTable转换得到一个对象实体集合
        /// </summary>
        /// <param name="model"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<TEntity> DataRowToModels(TEntity model, DataTable dt)
        {
            List<TEntity> ts = new List<TEntity>();// 定义集合
            foreach (DataRow dr in dt.Rows)
            {
                TEntity t = new TEntity();
                PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性
                foreach (PropertyInfo pi in propertys)
                {
                    if (dt.Columns.Contains(pi.Name))
                    {
                        if (!pi.CanWrite) continue;
                        var value = dr[pi.Name];
                        if (value != DBNull.Value)
                        {
                            switch (pi.PropertyType.FullName)
                            {
                                case "System.Decimal":
                                    pi.SetValue(t, decimal.Parse(value.ToString()), null);
                                    break;
                                case "System.String":
                                    pi.SetValue(t, value.ToString(), null);
                                    break;
                                case "System.Single":
                                    pi.SetValue(t, float.Parse(value.ToString()), null);
                                    break;
                                case "System.Double":
                                    pi.SetValue(t, double.Parse(value.ToString()), null);
                                    break;
                                case "System.Int32":
                                    pi.SetValue(t, int.Parse(value.ToString()), null);
                                    break;
                                case "System.DateTime":
                                    pi.SetValue(t, DateTime.Parse(value.ToString()), null);
                                    break;
                                case "System.Boolean":
                                    pi.SetValue(t, bool.Parse(value.ToString()), null);
                                    break;
                                default:
                                    pi.SetValue(t, value, null); 
                                    break;
                            }
                        }
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

      
    }
}
