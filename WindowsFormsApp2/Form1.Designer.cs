namespace WindowsFormsApp2
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
            this.打开串口 = new System.Windows.Forms.Button();
            this.串口号 = new System.Windows.Forms.Label();
            this.串口配置 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.加载文件 = new System.Windows.Forms.Button();
            this.波特率 = new System.Windows.Forms.Label();
            this.升级操作 = new System.Windows.Forms.Label();
            this.起始地址 = new System.Windows.Forms.Label();
            this.文件长度 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.固件升级 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // 打开串口
            // 
            this.打开串口.Location = new System.Drawing.Point(479, 71);
            this.打开串口.Name = "打开串口";
            this.打开串口.Size = new System.Drawing.Size(75, 23);
            this.打开串口.TabIndex = 0;
            this.打开串口.Text = "打开串口";
            this.打开串口.UseVisualStyleBackColor = true;
            this.打开串口.Click += new System.EventHandler(this.button1_Click);
            // 
            // 串口号
            // 
            this.串口号.AutoSize = true;
            this.串口号.Location = new System.Drawing.Point(46, 74);
            this.串口号.Name = "串口号";
            this.串口号.Size = new System.Drawing.Size(41, 12);
            this.串口号.TabIndex = 1;
            this.串口号.Text = "串口号";
            // 
            // 串口配置
            // 
            this.串口配置.AutoSize = true;
            this.串口配置.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.串口配置.Location = new System.Drawing.Point(34, 26);
            this.串口配置.Name = "串口配置";
            this.串口配置.Size = new System.Drawing.Size(67, 14);
            this.串口配置.TabIndex = 2;
            this.串口配置.Text = "串口配置";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(93, 71);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 3;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(304, 71);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 20);
            this.comboBox2.TabIndex = 4;
            // 
            // 加载文件
            // 
            this.加载文件.Location = new System.Drawing.Point(522, 192);
            this.加载文件.Name = "加载文件";
            this.加载文件.Size = new System.Drawing.Size(75, 23);
            this.加载文件.TabIndex = 5;
            this.加载文件.Text = "加载文件";
            this.加载文件.UseVisualStyleBackColor = true;
            // 
            // 波特率
            // 
            this.波特率.AutoSize = true;
            this.波特率.Location = new System.Drawing.Point(257, 75);
            this.波特率.Name = "波特率";
            this.波特率.Size = new System.Drawing.Size(41, 12);
            this.波特率.TabIndex = 6;
            this.波特率.Text = "波特率";
            // 
            // 升级操作
            // 
            this.升级操作.AutoSize = true;
            this.升级操作.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.升级操作.Location = new System.Drawing.Point(34, 163);
            this.升级操作.Name = "升级操作";
            this.升级操作.Size = new System.Drawing.Size(67, 14);
            this.升级操作.TabIndex = 8;
            this.升级操作.Text = "升级操作";
            // 
            // 起始地址
            // 
            this.起始地址.AutoSize = true;
            this.起始地址.Location = new System.Drawing.Point(36, 270);
            this.起始地址.Name = "起始地址";
            this.起始地址.Size = new System.Drawing.Size(65, 12);
            this.起始地址.TabIndex = 9;
            this.起始地址.Text = "起始地址：";
            // 
            // 文件长度
            // 
            this.文件长度.AutoSize = true;
            this.文件长度.Location = new System.Drawing.Point(257, 269);
            this.文件长度.Name = "文件长度";
            this.文件长度.Size = new System.Drawing.Size(65, 12);
            this.文件长度.TabIndex = 10;
            this.文件长度.Text = "文件长度：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(107, 266);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 12;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(325, 266);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(100, 21);
            this.textBox2.TabIndex = 12;
            // 
            // 固件升级
            // 
            this.固件升级.Location = new System.Drawing.Point(522, 270);
            this.固件升级.Name = "固件升级";
            this.固件升级.Size = new System.Drawing.Size(75, 23);
            this.固件升级.TabIndex = 13;
            this.固件升级.Text = "固件升级";
            this.固件升级.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(38, 192);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(447, 21);
            this.textBox3.TabIndex = 14;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(38, 315);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(559, 123);
            this.richTextBox1.TabIndex = 15;
            this.richTextBox1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.固件升级);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.文件长度);
            this.Controls.Add(this.起始地址);
            this.Controls.Add(this.升级操作);
            this.Controls.Add(this.波特率);
            this.Controls.Add(this.加载文件);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.串口配置);
            this.Controls.Add(this.串口号);
            this.Controls.Add(this.打开串口);
            this.Name = "Form1";
            this.Text = "RDB升级工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button 打开串口;
        private System.Windows.Forms.Label 串口号;
        private System.Windows.Forms.Label 串口配置;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button 加载文件;
        private System.Windows.Forms.Label 波特率;
        private System.Windows.Forms.Label 升级操作;
        private System.Windows.Forms.Label 起始地址;
        private System.Windows.Forms.Label 文件长度;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button 固件升级;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}

