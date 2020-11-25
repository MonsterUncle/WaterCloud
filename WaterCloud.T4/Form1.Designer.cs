namespace JRT4
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCreateCode = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtConnStr = new System.Windows.Forms.TextBox();
            this.btnConnection = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.txtTableTag = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnFiltrate = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSelectPathFloder = new System.Windows.Forms.FolderBrowserDialog();
            this.btnSelectPath = new System.Windows.Forms.Button();
            this.cbbTableName = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbDatabaseType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtModuleDic = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCreateCode
            // 
            this.btnCreateCode.Location = new System.Drawing.Point(1089, 234);
            this.btnCreateCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCreateCode.Name = "btnCreateCode";
            this.btnCreateCode.Size = new System.Drawing.Size(112, 34);
            this.btnCreateCode.TabIndex = 0;
            this.btnCreateCode.Text = "生成代码";
            this.btnCreateCode.UseVisualStyleBackColor = true;
            this.btnCreateCode.Click += new System.EventHandler(this.btnCreateCode_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(80, 159);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "连接字符串：";
            // 
            // txtConnStr
            // 
            this.txtConnStr.Location = new System.Drawing.Point(204, 154);
            this.txtConnStr.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtConnStr.Name = "txtConnStr";
            this.txtConnStr.Size = new System.Drawing.Size(331, 28);
            this.txtConnStr.TabIndex = 2;
            // 
            // btnConnection
            // 
            this.btnConnection.Location = new System.Drawing.Point(548, 150);
            this.btnConnection.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnConnection.Name = "btnConnection";
            this.btnConnection.Size = new System.Drawing.Size(112, 34);
            this.btnConnection.TabIndex = 3;
            this.btnConnection.Text = "连接";
            this.btnConnection.UseVisualStyleBackColor = true;
            this.btnConnection.Click += new System.EventHandler(this.btnConnection_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(714, 158);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "目标路径：";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(804, 153);
            this.txtPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(272, 28);
            this.txtPath.TabIndex = 5;
            // 
            // txtTableTag
            // 
            this.txtTableTag.Location = new System.Drawing.Point(204, 238);
            this.txtTableTag.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTableTag.Name = "txtTableTag";
            this.txtTableTag.Size = new System.Drawing.Size(331, 28);
            this.txtTableTag.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(82, 250);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 18);
            this.label3.TabIndex = 7;
            this.label3.Text = "数据表标识：";
            // 
            // btnFiltrate
            // 
            this.btnFiltrate.Location = new System.Drawing.Point(548, 238);
            this.btnFiltrate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnFiltrate.Name = "btnFiltrate";
            this.btnFiltrate.Size = new System.Drawing.Size(112, 34);
            this.btnFiltrate.TabIndex = 8;
            this.btnFiltrate.Text = "筛选";
            this.btnFiltrate.UseVisualStyleBackColor = true;
            this.btnFiltrate.Click += new System.EventHandler(this.btnFiltrate_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(717, 244);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 18);
            this.label4.TabIndex = 9;
            this.label4.Text = "数据表名：";
            // 
            // btnSelectPath
            // 
            this.btnSelectPath.Location = new System.Drawing.Point(1089, 150);
            this.btnSelectPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSelectPath.Name = "btnSelectPath";
            this.btnSelectPath.Size = new System.Drawing.Size(112, 34);
            this.btnSelectPath.TabIndex = 11;
            this.btnSelectPath.Text = "选择";
            this.btnSelectPath.UseVisualStyleBackColor = true;
            this.btnSelectPath.Click += new System.EventHandler(this.btnSelectPath_Click);
            // 
            // cbbTableName
            // 
            this.cbbTableName.FormattingEnabled = true;
            this.cbbTableName.Location = new System.Drawing.Point(804, 238);
            this.cbbTableName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbbTableName.Name = "cbbTableName";
            this.cbbTableName.Size = new System.Drawing.Size(272, 26);
            this.cbbTableName.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(82, 63);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 18);
            this.label5.TabIndex = 13;
            this.label5.Text = "数据库类型：";
            // 
            // cbDatabaseType
            // 
            this.cbDatabaseType.FormattingEnabled = true;
            this.cbDatabaseType.Items.AddRange(new object[] {
            "MSSQL",
            "MySQL"});
            this.cbDatabaseType.Location = new System.Drawing.Point(204, 62);
            this.cbDatabaseType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbDatabaseType.Name = "cbDatabaseType";
            this.cbDatabaseType.Size = new System.Drawing.Size(331, 26);
            this.cbDatabaseType.TabIndex = 14;
            this.cbDatabaseType.SelectedIndexChanged += new System.EventHandler(this.cbDatabaseType_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(714, 62);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 18);
            this.label6.TabIndex = 15;
            this.label6.Text = "命名空间：";
            // 
            // txtModuleDic
            // 
            this.txtModuleDic.Location = new System.Drawing.Point(804, 58);
            this.txtModuleDic.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtModuleDic.Name = "txtModuleDic";
            this.txtModuleDic.Size = new System.Drawing.Size(272, 28);
            this.txtModuleDic.TabIndex = 16;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1292, 402);
            this.Controls.Add(this.txtModuleDic);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbDatabaseType);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbbTableName);
            this.Controls.Add(this.btnSelectPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnFiltrate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTableTag);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnConnection);
            this.Controls.Add(this.txtConnStr);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCreateCode);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "JR代码生成器";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreateCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtConnStr;
        private System.Windows.Forms.Button btnConnection;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.TextBox txtTableTag;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnFiltrate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FolderBrowserDialog btnSelectPathFloder;
        private System.Windows.Forms.Button btnSelectPath;
        private System.Windows.Forms.ComboBox cbbTableName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbDatabaseType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtModuleDic;
    }
}

