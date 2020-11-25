using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace EntityInfo
{
    [Serializable]
    public class EntityClassInfo
    {
        public EntityClassInfo(DataTable dt,string strNameSpace)
        {
            string name= dt.TableName.PadLeft(4).Remove(0, 4);
            this.TableName = dt.TableName;
            this.ClassName = name;
            this.EntityName = name;
            this.EntityNameSpace = strNameSpace;

            List<EntityClassPropertyInfo> ropertyListTemp = new List<EntityClassPropertyInfo>();
           
            foreach (DataColumn dcol in dt.Columns)
            {
                ropertyListTemp.Add(new EntityClassPropertyInfo(dcol));
            }
            this.RopertyList = ropertyListTemp;

            List<EntityClassPropertyInfo> primaryKeyListTemp = new List<EntityClassPropertyInfo>();
            List<EntityClassPropertyInfo> notPrimaryKeyListTemp = new List<EntityClassPropertyInfo>(ropertyListTemp);
            foreach (DataColumn dcol in dt.PrimaryKey)
            {
                primaryKeyListTemp.Add(new EntityClassPropertyInfo(dcol));
                notPrimaryKeyListTemp.Remove(new EntityClassPropertyInfo(dcol));
            }
            this.PrimaryKeyList = primaryKeyListTemp;
            this.NotPrimaryKeyList = notPrimaryKeyListTemp;
        }
        public string ClassName
        {
            get;
            private set;
        }
        public string EntityName
        {
            get;
            private set;
        }
        public string TableName
        {
            get;
            private set;
        }
        public string EntityNameSpace
        {
            get;
            private set;
        }
        public List<EntityClassPropertyInfo> RopertyList
        {
            get;
            private set;
        }
        public List<EntityClassPropertyInfo> PrimaryKeyList
        {
            get;
            private set;
        }
        public List<EntityClassPropertyInfo> NotPrimaryKeyList
        {
            get;
            private set;
        }
    }
}