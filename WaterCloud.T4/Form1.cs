using AdoNetBase;
using EntityInfo;
using System.Data.SqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace JRT4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            StatusOfConnection(false);
            //sqlserver
            txtConnStr.Text = MSSQLHelper.GetConnectionString();
            cbDatabaseType.SelectedIndex = 1;
            ////mysql
            //txtConnStr.Text = MySQLHelper.GetConnectionString();
        }
        

        private void btnConnection_Click(object sender, EventArgs e)
        {
            if ("连接" == btnConnection.Text.ToString())
            {
                if (string.IsNullOrEmpty(txtConnStr.Text))
                {
                    MessageBox.Show("请填写连接字符串!", "提示");
                    return;
                }
                if (cbDatabaseType.Text == "MSSQL")
                {
                    MSSQLHelper.UpdateConnectionString(txtConnStr.Text);
                    CbbTableNameBind("MSSQL");
                }
                else
                {
                    MySQLHelper.UpdateConnectionString(txtConnStr.Text);
                    CbbTableNameBind("MySQL");
                }

            }
            else
            {
                StatusOfConnection(false);
            }
        }

        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtPath.Text = folderBrowser.SelectedPath + "\\";
            }
        }
        private void CbbTableNameBind(string sqltype)
        {
            //string sql = @"Select Name From SysObjects Where XType='U' AND Name LIKE concat(@Server,'%') order By Name;";
            string sql = @"Select Name From SysObjects Where XType='U' AND Name LIKE @TableTage+'%' order By Name;";
            if (sqltype=="MySQL")
            {
                sql = @"select table_name from information_schema.tables where table_schema=@DataBase"; 
            }

            DataTable dt = new DataTable();
            try
            {
                if (sqltype == "MSSQL")
                {
                    dt = MSSQLHelper.ExecuteDataTable(txtConnStr.Text, sql,
    new SqlParameter("@TableTage", txtTableTag.Text),
    new SqlParameter("@DataBase", MSSQLHelper.GetDataBaseString(MSSQLHelper.GetConnectionString())));
                }
                else
                {
                    dt = MySQLHelper.ExecuteDataTable(txtConnStr.Text, sql,
new MySqlParameter("@TableTage", txtTableTag.Text),
new MySqlParameter("@DataBase", MySQLHelper.GetDataBaseString(MySQLHelper.GetConnectionString())));
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show("连接数据库失败！错误信息：" + ex.Message, "错误");
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("不是有效的连接字符串！错误信息：" + ex.Message, "错误");
                return;
            }
            StatusOfConnection(true);
            //cbbTableName.ItemsSource = dt.DefaultView;
            //cbbTableName.DisplayMemberPath = "TABLE_NAME";
            //cbbTableName.SelectedValuePath = "TABLE_NAME";
            cbbTableName.DataSource = dt.DefaultView;
            if (sqltype == "MSSQL")
            {
                cbbTableName.DisplayMember = "Name";
                cbbTableName.ValueMember = "Name";
            }
            else
            {
                cbbTableName.DisplayMember = "table_name";
                cbbTableName.ValueMember = "table_name";
            }
            //cbbTableName.SelectedIndex = 0;
        }
        private void StatusOfConnection(bool isEnable)
        {
            txtTableTag.Enabled = isEnable;
            btnFiltrate.Enabled = isEnable;
            cbbTableName.Enabled = isEnable;
            btnCreateCode.Enabled = isEnable;
            txtConnStr.Enabled = !isEnable;
            if (isEnable)
            {
                btnConnection.Text = "断开";
            }
            else
            {
                btnConnection.Text = "连接";
                cbbTableName.DataSource = null;
                //rtxtModel.Document.Blocks.Clear();
                //rtxtDAL.Document.Blocks.Clear();
            }
        }

        private void btnCreateCode_Click(object sender, EventArgs e)
        {
            //rtxtModel.Document.Blocks.Clear();
            //rtxtDAL.Document.Blocks.Clear();
            if (string.IsNullOrWhiteSpace(txtPath.Text))
            {
                MessageBox.Show("请选择目标路径！");
                return;
            }
            DataTable dt = new DataTable();

            try
            {
                if (cbDatabaseType.Text == "MSSQL")
                {
                    dt = MSSQLHelper.ExecuteDataTable(MSSQLHelper.GetConnectionString(), string.Format("SELECT top 0 * FROM {0}", cbbTableName.SelectedValue.ToString()));
                }
                else
                {
                    dt = MySQLHelper.ExecuteDataTable(MySQLHelper.GetConnectionString(), string.Format("select  *  from  {0}  limit 0 ", cbbTableName.SelectedValue.ToString()));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取数据库失败！" + ex.Message);
                return;
            }
            EntityClassInfo entityInfo = new EntityClassInfo(dt, txtModuleDic.Text);
            string str = CreateCode.CreatT4Class(entityInfo, txtPath.Text, txtModuleDic.Text);
            //string codeEntity = CreateCode.CreateEntityClass(entityInfo);
            //string codeDataAccess = CreateCode.CreateDataAccessClass(entityInfo);
            //rtxtDAL.AppendText(codeDataAccess);
            //if (!string.IsNullOrEmpty(txtPath.Text))
            //{
            //    File.WriteAllText(txtPath.Text + entityInfo.ClassName + ".cs",
            //        codeEntity);
            //    File.WriteAllText(txtPath.Text + entityInfo.ClassName + "DAL.cs",
            //        codeDataAccess);
            //}
        }

        private void btnFiltrate_Click(object sender, EventArgs e)
        {
            if ("连接" == btnConnection.Text.ToString())
            {
                MessageBox.Show("未连接数据库！", "提示");
                return;
            }
            CbbTableNameBind(cbDatabaseType.Text);
        }

        private void cbDatabaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbDatabaseType.Text=="MSSQL")
            {
                txtConnStr.Text = MSSQLHelper.GetConnectionString();
            }
            else
            {
                txtConnStr.Text = MySQLHelper.GetConnectionString();
            }
        }
    }
}
