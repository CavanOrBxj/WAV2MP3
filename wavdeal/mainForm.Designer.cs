namespace wavdeal
{
    partial class mainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.button1 = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtWavForlder = new System.Windows.Forms.TextBox();
            this.txtMp3Forlder = new System.Windows.Forms.TextBox();
            this.btnSelectWav = new System.Windows.Forms.Button();
            this.btnSelectMp3 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(178, 220);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(85, 32);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(336, 234);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(107, 43);
            this.btnConvert.TabIndex = 1;
            this.btnConvert.Text = "启动转换";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSelectMp3);
            this.groupBox1.Controls.Add(this.btnSelectWav);
            this.groupBox1.Controls.Add(this.txtMp3Forlder);
            this.groupBox1.Controls.Add(this.txtWavForlder);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(463, 202);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "音频文件夹路径：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "MP3存放路径：";
            // 
            // txtWavForlder
            // 
            this.txtWavForlder.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtWavForlder.Location = new System.Drawing.Point(106, 62);
            this.txtWavForlder.Name = "txtWavForlder";
            this.txtWavForlder.ReadOnly = true;
            this.txtWavForlder.Size = new System.Drawing.Size(287, 21);
            this.txtWavForlder.TabIndex = 2;
            // 
            // txtMp3Forlder
            // 
            this.txtMp3Forlder.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtMp3Forlder.Location = new System.Drawing.Point(106, 125);
            this.txtMp3Forlder.Name = "txtMp3Forlder";
            this.txtMp3Forlder.ReadOnly = true;
            this.txtMp3Forlder.Size = new System.Drawing.Size(287, 21);
            this.txtMp3Forlder.TabIndex = 3;
            // 
            // btnSelectWav
            // 
            this.btnSelectWav.Location = new System.Drawing.Point(399, 61);
            this.btnSelectWav.Name = "btnSelectWav";
            this.btnSelectWav.Size = new System.Drawing.Size(41, 23);
            this.btnSelectWav.TabIndex = 4;
            this.btnSelectWav.Text = "选择";
            this.btnSelectWav.UseVisualStyleBackColor = true;
            this.btnSelectWav.Click += new System.EventHandler(this.btnSelectWav_Click);
            // 
            // btnSelectMp3
            // 
            this.btnSelectMp3.Location = new System.Drawing.Point(400, 124);
            this.btnSelectMp3.Name = "btnSelectMp3";
            this.btnSelectMp3.Size = new System.Drawing.Size(40, 23);
            this.btnSelectMp3.TabIndex = 5;
            this.btnSelectMp3.Text = "选择";
            this.btnSelectMp3.UseVisualStyleBackColor = true;
            this.btnSelectMp3.Click += new System.EventHandler(this.btnSelectMp3_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 293);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "mainForm";
            this.Text = "音频转换服务端";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMp3Forlder;
        private System.Windows.Forms.TextBox txtWavForlder;
        private System.Windows.Forms.Button btnSelectMp3;
        private System.Windows.Forms.Button btnSelectWav;
    }
}

