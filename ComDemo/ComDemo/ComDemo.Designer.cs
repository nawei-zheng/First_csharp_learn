namespace ComDemo
{
    partial class ComDemo
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel_U = new System.Windows.Forms.Panel();
            this.pb_Upd = new System.Windows.Forms.ProgressBar();
            this.bt_UpdOpt = new System.Windows.Forms.Button();
            this.bt_ComCheck = new System.Windows.Forms.Button();
            this.tb_FilePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.bt_LoadFile = new System.Windows.Forms.Button();
            this.bt_ComOpen = new System.Windows.Forms.Button();
            this.cb_ComBandList = new System.Windows.Forms.ComboBox();
            this.cb_ComNumList = new System.Windows.Forms.ComboBox();
            this.panel_D = new System.Windows.Forms.Panel();
            this.rb_Log = new System.Windows.Forms.RichTextBox();
            this.panel_U.SuspendLayout();
            this.panel_D.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_U
            // 
            this.panel_U.Controls.Add(this.pb_Upd);
            this.panel_U.Controls.Add(this.bt_UpdOpt);
            this.panel_U.Controls.Add(this.bt_ComCheck);
            this.panel_U.Controls.Add(this.tb_FilePath);
            this.panel_U.Controls.Add(this.label2);
            this.panel_U.Controls.Add(this.label1);
            this.panel_U.Controls.Add(this.bt_LoadFile);
            this.panel_U.Controls.Add(this.bt_ComOpen);
            this.panel_U.Controls.Add(this.cb_ComBandList);
            this.panel_U.Controls.Add(this.cb_ComNumList);
            this.panel_U.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_U.Location = new System.Drawing.Point(0, 0);
            this.panel_U.Name = "panel_U";
            this.panel_U.Size = new System.Drawing.Size(552, 142);
            this.panel_U.TabIndex = 0;
            // 
            // pb_Upd
            // 
            this.pb_Upd.Location = new System.Drawing.Point(20, 105);
            this.pb_Upd.Name = "pb_Upd";
            this.pb_Upd.Size = new System.Drawing.Size(394, 23);
            this.pb_Upd.TabIndex = 10;
            // 
            // bt_UpdOpt
            // 
            this.bt_UpdOpt.Enabled = false;
            this.bt_UpdOpt.Location = new System.Drawing.Point(441, 107);
            this.bt_UpdOpt.Name = "bt_UpdOpt";
            this.bt_UpdOpt.Size = new System.Drawing.Size(75, 23);
            this.bt_UpdOpt.TabIndex = 9;
            this.bt_UpdOpt.Text = "固件升级";
            this.bt_UpdOpt.UseVisualStyleBackColor = true;
            this.bt_UpdOpt.Click += new System.EventHandler(this.bt_UpdOpt_Click);
            // 
            // bt_ComCheck
            // 
            this.bt_ComCheck.Location = new System.Drawing.Point(339, 21);
            this.bt_ComCheck.Name = "bt_ComCheck";
            this.bt_ComCheck.Size = new System.Drawing.Size(75, 23);
            this.bt_ComCheck.TabIndex = 8;
            this.bt_ComCheck.Text = "识别串口";
            this.bt_ComCheck.UseVisualStyleBackColor = true;
            this.bt_ComCheck.Click += new System.EventHandler(this.bt_ComCheck_Click);
            // 
            // tb_FilePath
            // 
            this.tb_FilePath.Location = new System.Drawing.Point(19, 64);
            this.tb_FilePath.Name = "tb_FilePath";
            this.tb_FilePath.Size = new System.Drawing.Size(395, 21);
            this.tb_FilePath.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(181, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "波特率：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "串口号：";
            // 
            // bt_LoadFile
            // 
            this.bt_LoadFile.Location = new System.Drawing.Point(441, 63);
            this.bt_LoadFile.Name = "bt_LoadFile";
            this.bt_LoadFile.Size = new System.Drawing.Size(75, 23);
            this.bt_LoadFile.TabIndex = 3;
            this.bt_LoadFile.Text = "加载文件";
            this.bt_LoadFile.UseVisualStyleBackColor = true;
            this.bt_LoadFile.Click += new System.EventHandler(this.bt_LoadFile_Click);
            // 
            // bt_ComOpen
            // 
            this.bt_ComOpen.Location = new System.Drawing.Point(441, 19);
            this.bt_ComOpen.Name = "bt_ComOpen";
            this.bt_ComOpen.Size = new System.Drawing.Size(75, 23);
            this.bt_ComOpen.TabIndex = 2;
            this.bt_ComOpen.Text = "打开串口";
            this.bt_ComOpen.UseVisualStyleBackColor = true;
            this.bt_ComOpen.Click += new System.EventHandler(this.bt_ComOpen_Click);
            // 
            // cb_ComBandList
            // 
            this.cb_ComBandList.FormattingEnabled = true;
            this.cb_ComBandList.Location = new System.Drawing.Point(239, 22);
            this.cb_ComBandList.Name = "cb_ComBandList";
            this.cb_ComBandList.Size = new System.Drawing.Size(74, 20);
            this.cb_ComBandList.TabIndex = 1;
            // 
            // cb_ComNumList
            // 
            this.cb_ComNumList.FormattingEnabled = true;
            this.cb_ComNumList.Location = new System.Drawing.Point(77, 22);
            this.cb_ComNumList.Name = "cb_ComNumList";
            this.cb_ComNumList.Size = new System.Drawing.Size(74, 20);
            this.cb_ComNumList.TabIndex = 0;
            // 
            // panel_D
            // 
            this.panel_D.Controls.Add(this.rb_Log);
            this.panel_D.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_D.Location = new System.Drawing.Point(0, 142);
            this.panel_D.Name = "panel_D";
            this.panel_D.Size = new System.Drawing.Size(552, 151);
            this.panel_D.TabIndex = 1;
            // 
            // rb_Log
            // 
            this.rb_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rb_Log.Location = new System.Drawing.Point(0, 0);
            this.rb_Log.Name = "rb_Log";
            this.rb_Log.Size = new System.Drawing.Size(552, 151);
            this.rb_Log.TabIndex = 0;
            this.rb_Log.Text = "";
            // 
            // ComDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 293);
            this.Controls.Add(this.panel_D);
            this.Controls.Add(this.panel_U);
            this.Name = "ComDemo";
            this.Text = "串口工具Demo";
            this.Load += new System.EventHandler(this.ComDemo_Load);
            this.panel_U.ResumeLayout(false);
            this.panel_U.PerformLayout();
            this.panel_D.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_U;
        private System.Windows.Forms.Panel panel_D;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bt_LoadFile;
        private System.Windows.Forms.Button bt_ComOpen;
        private System.Windows.Forms.ComboBox cb_ComBandList;
        private System.Windows.Forms.ComboBox cb_ComNumList;
        private System.Windows.Forms.RichTextBox rb_Log;
        private System.Windows.Forms.TextBox tb_FilePath;
        private System.Windows.Forms.Button bt_ComCheck;
        private System.Windows.Forms.Button bt_UpdOpt;
        private System.Windows.Forms.ProgressBar pb_Upd;
    }
}

