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
            this.components = new System.ComponentModel.Container();
            this.open_btn = new System.Windows.Forms.Button();
            this.serial_port_lbl = new System.Windows.Forms.Label();
            this.serial_port_cbb = new System.Windows.Forms.ComboBox();
            this.baudrate_cbb = new System.Windows.Forms.ComboBox();
            this.filePathBtn = new System.Windows.Forms.Button();
            this.baudrate_lbl = new System.Windows.Forms.Label();
            this.addressOffsetLbl = new System.Windows.Forms.Label();
            this.fileSizeLbl = new System.Windows.Forms.Label();
            this.addressOffsetBox = new System.Windows.Forms.TextBox();
            this.fileSizeBox = new System.Windows.Forms.TextBox();
            this.updateBtn = new System.Windows.Forms.Button();
            this.filePathBox = new System.Windows.Forms.TextBox();
            this.LogBox = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.refreshCom_Btn = new System.Windows.Forms.Button();
            this.baudrateLbl = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.serialPort2 = new System.IO.Ports.SerialPort(this.components);
            this.serialPort3 = new System.IO.Ports.SerialPort(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // open_btn
            // 
            this.open_btn.Location = new System.Drawing.Point(393, 47);
            this.open_btn.Name = "open_btn";
            this.open_btn.Size = new System.Drawing.Size(75, 23);
            this.open_btn.TabIndex = 0;
            this.open_btn.Text = "打开串口";
            this.open_btn.UseVisualStyleBackColor = true;
            this.open_btn.Click += new System.EventHandler(this.open_btn_Click);
            // 
            // serial_port_lbl
            // 
            this.serial_port_lbl.AutoSize = true;
            this.serial_port_lbl.Location = new System.Drawing.Point(20, 70);
            this.serial_port_lbl.Name = "serial_port_lbl";
            this.serial_port_lbl.Size = new System.Drawing.Size(41, 12);
            this.serial_port_lbl.TabIndex = 1;
            this.serial_port_lbl.Text = "串口号";
            // 
            // serial_port_cbb
            // 
            this.serial_port_cbb.FormattingEnabled = true;
            this.serial_port_cbb.Location = new System.Drawing.Point(67, 67);
            this.serial_port_cbb.Name = "serial_port_cbb";
            this.serial_port_cbb.Size = new System.Drawing.Size(121, 20);
            this.serial_port_cbb.Sorted = true;
            this.serial_port_cbb.TabIndex = 3;
            // 
            // baudrate_cbb
            // 
            this.baudrate_cbb.FormattingEnabled = true;
            this.baudrate_cbb.Location = new System.Drawing.Point(245, 50);
            this.baudrate_cbb.Name = "baudrate_cbb";
            this.baudrate_cbb.Size = new System.Drawing.Size(121, 20);
            this.baudrate_cbb.TabIndex = 4;
            // 
            // filePathBtn
            // 
            this.filePathBtn.Location = new System.Drawing.Point(496, 167);
            this.filePathBtn.Name = "filePathBtn";
            this.filePathBtn.Size = new System.Drawing.Size(75, 23);
            this.filePathBtn.TabIndex = 5;
            this.filePathBtn.Text = "加载文件";
            this.filePathBtn.UseVisualStyleBackColor = true;
            this.filePathBtn.Click += new System.EventHandler(this.filePathBtn_Click);
            // 
            // addressOffsetLbl
            // 
            this.addressOffsetLbl.AutoSize = true;
            this.addressOffsetLbl.Location = new System.Drawing.Point(24, 245);
            this.addressOffsetLbl.Name = "addressOffsetLbl";
            this.addressOffsetLbl.Size = new System.Drawing.Size(65, 12);
            this.addressOffsetLbl.TabIndex = 9;
            this.addressOffsetLbl.Text = "起始地址：";
            // 
            // fileSizeLbl
            // 
            this.fileSizeLbl.AutoSize = true;
            this.fileSizeLbl.Location = new System.Drawing.Point(231, 244);
            this.fileSizeLbl.Name = "fileSizeLbl";
            this.fileSizeLbl.Size = new System.Drawing.Size(65, 12);
            this.fileSizeLbl.TabIndex = 10;
            this.fileSizeLbl.Text = "文件长度：";
            // 
            // addressOffsetBox
            // 
            this.addressOffsetBox.Location = new System.Drawing.Point(95, 241);
            this.addressOffsetBox.Name = "addressOffsetBox";
            this.addressOffsetBox.ReadOnly = true;
            this.addressOffsetBox.Size = new System.Drawing.Size(100, 21);
            this.addressOffsetBox.TabIndex = 12;
            // 
            // fileSizeBox
            // 
            this.fileSizeBox.Location = new System.Drawing.Point(299, 241);
            this.fileSizeBox.Name = "fileSizeBox";
            this.fileSizeBox.ReadOnly = true;
            this.fileSizeBox.Size = new System.Drawing.Size(100, 21);
            this.fileSizeBox.TabIndex = 12;
            // 
            // updateBtn
            // 
            this.updateBtn.Enabled = false;
            this.updateBtn.Location = new System.Drawing.Point(427, 117);
            this.updateBtn.Name = "updateBtn";
            this.updateBtn.Size = new System.Drawing.Size(75, 23);
            this.updateBtn.TabIndex = 13;
            this.updateBtn.Text = "固件升级";
            this.updateBtn.UseVisualStyleBackColor = true;
            this.updateBtn.Click += new System.EventHandler(this.updateBtn_Click);
            // 
            // filePathBox
            // 
            this.filePathBox.Location = new System.Drawing.Point(10, 45);
            this.filePathBox.Name = "filePathBox";
            this.filePathBox.ReadOnly = true;
            this.filePathBox.Size = new System.Drawing.Size(447, 21);
            this.filePathBox.TabIndex = 14;
            // 
            // LogBox
            // 
            this.LogBox.Location = new System.Drawing.Point(12, 290);
            this.LogBox.Name = "LogBox";
            this.LogBox.Size = new System.Drawing.Size(568, 126);
            this.LogBox.TabIndex = 15;
            this.LogBox.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.refreshCom_Btn);
            this.groupBox1.Controls.Add(this.baudrateLbl);
            this.groupBox1.Controls.Add(this.open_btn);
            this.groupBox1.Controls.Add(this.baudrate_cbb);
            this.groupBox1.Location = new System.Drawing.Point(12, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(568, 100);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "串口配置";
            // 
            // refreshCom_Btn
            // 
            this.refreshCom_Btn.Location = new System.Drawing.Point(484, 47);
            this.refreshCom_Btn.Name = "refreshCom_Btn";
            this.refreshCom_Btn.Size = new System.Drawing.Size(75, 23);
            this.refreshCom_Btn.TabIndex = 1;
            this.refreshCom_Btn.Text = "刷新";
            this.refreshCom_Btn.UseVisualStyleBackColor = true;
            this.refreshCom_Btn.Click += new System.EventHandler(this.refreshCom_Btn_Click);
            // 
            // baudrateLbl
            // 
            this.baudrateLbl.AutoSize = true;
            this.baudrateLbl.Location = new System.Drawing.Point(198, 53);
            this.baudrateLbl.Name = "baudrateLbl";
            this.baudrateLbl.Size = new System.Drawing.Size(41, 12);
            this.baudrateLbl.TabIndex = 0;
            this.baudrateLbl.Text = "波特率";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.updateBtn);
            this.groupBox2.Controls.Add(this.filePathBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 124);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(568, 160);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "串口升级";
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 115200;
            // 
            // serialPort2
            // 
            this.serialPort2.PortName = "COM2";
            // 
            // serialPort3
            // 
            this.serialPort3.PortName = "COM3";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(588, 420);
            this.Controls.Add(this.fileSizeBox);
            this.Controls.Add(this.fileSizeLbl);
            this.Controls.Add(this.addressOffsetBox);
            this.Controls.Add(this.addressOffsetLbl);
            this.Controls.Add(this.filePathBtn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.serial_port_lbl);
            this.Controls.Add(this.serial_port_cbb);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.LogBox);
            this.Controls.Add(this.baudrate_lbl);
            this.Name = "Form1";
            this.Text = "RDB升级工具";
            this.Load += new System.EventHandler(this.ComDemo_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button open_btn;
        private System.Windows.Forms.Label serial_port_lbl;
        private System.Windows.Forms.ComboBox serial_port_cbb;
        private System.Windows.Forms.ComboBox baudrate_cbb;
        private System.Windows.Forms.Button filePathBtn;
        private System.Windows.Forms.Label baudrate_lbl;
        private System.Windows.Forms.Label addressOffsetLbl;
        private System.Windows.Forms.Label fileSizeLbl;
        private System.Windows.Forms.TextBox addressOffsetBox;
        private System.Windows.Forms.TextBox fileSizeBox;
        private System.Windows.Forms.Button updateBtn;
        private System.Windows.Forms.TextBox filePathBox;
        private System.Windows.Forms.RichTextBox LogBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label baudrateLbl;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.IO.Ports.SerialPort serialPort1;
        private System.IO.Ports.SerialPort serialPort2;
        private System.IO.Ports.SerialPort serialPort3;
        private System.Windows.Forms.Button refreshCom_Btn;
    }
}

