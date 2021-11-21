namespace CDweigh
{
    partial class ChangePaswd
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel3 = new System.Windows.Forms.Panel();
            this.CloseBtn = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.IsSee = new System.Windows.Forms.PictureBox();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.Password_Old = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.IsSee2 = new System.Windows.Forms.PictureBox();
            this.Password_New = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.username_combo = new System.Windows.Forms.ComboBox();
            this.MsgText = new System.Windows.Forms.Label();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IsSee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsSee2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel3.Controls.Add(this.CloseBtn);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(398, 59);
            this.panel3.TabIndex = 10;
            this.panel3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel3_MouseDown);
            // 
            // CloseBtn
            // 
            this.CloseBtn.BackColor = System.Drawing.Color.Transparent;
            this.CloseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseBtn.ForeColor = System.Drawing.Color.Transparent;
            this.CloseBtn.Image = global::CDweigh.Properties.Resources.close_White;
            this.CloseBtn.Location = new System.Drawing.Point(354, 8);
            this.CloseBtn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(40, 40);
            this.CloseBtn.TabIndex = 1;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(4, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(259, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "修改密码 ChangePas";
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            // 
            // IsSee
            // 
            this.IsSee.BackgroundImage = global::CDweigh.Properties.Resources.UnSee;
            this.IsSee.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.IsSee.Location = new System.Drawing.Point(336, 175);
            this.IsSee.Name = "IsSee";
            this.IsSee.Size = new System.Drawing.Size(24, 24);
            this.IsSee.TabIndex = 19;
            this.IsSee.TabStop = false;
            this.IsSee.Click += new System.EventHandler(this.IsSee_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.BackColor = System.Drawing.Color.MediumAquamarine;
            this.SaveBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveBtn.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SaveBtn.ForeColor = System.Drawing.SystemColors.Control;
            this.SaveBtn.Location = new System.Drawing.Point(64, 307);
            this.SaveBtn.Margin = new System.Windows.Forms.Padding(4);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(266, 61);
            this.SaveBtn.TabIndex = 18;
            this.SaveBtn.Text = "登   录";
            this.SaveBtn.UseVisualStyleBackColor = false;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // Password_Old
            // 
            this.Password_Old.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Password_Old.BackColor = System.Drawing.Color.AliceBlue;
            this.Password_Old.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Password_Old.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Password_Old.Location = new System.Drawing.Point(110, 168);
            this.Password_Old.Margin = new System.Windows.Forms.Padding(4);
            this.Password_Old.Name = "Password_Old";
            this.Password_Old.PasswordChar = '*';
            this.Password_Old.Size = new System.Drawing.Size(219, 31);
            this.Password_Old.TabIndex = 17;
            this.Password_Old.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Password_Old.WordWrap = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Location = new System.Drawing.Point(111, 202);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(218, 1);
            this.panel2.TabIndex = 15;
            // 
            // IsSee2
            // 
            this.IsSee2.BackgroundImage = global::CDweigh.Properties.Resources.UnSee;
            this.IsSee2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.IsSee2.Location = new System.Drawing.Point(337, 233);
            this.IsSee2.Name = "IsSee2";
            this.IsSee2.Size = new System.Drawing.Size(24, 24);
            this.IsSee2.TabIndex = 23;
            this.IsSee2.TabStop = false;
            this.IsSee2.Click += new System.EventHandler(this.IsSee2_Click);
            // 
            // Password_New
            // 
            this.Password_New.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Password_New.BackColor = System.Drawing.Color.AliceBlue;
            this.Password_New.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Password_New.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Password_New.Location = new System.Drawing.Point(111, 226);
            this.Password_New.Margin = new System.Windows.Forms.Padding(4);
            this.Password_New.Name = "Password_New";
            this.Password_New.PasswordChar = '*';
            this.Password_New.Size = new System.Drawing.Size(219, 31);
            this.Password_New.TabIndex = 22;
            this.Password_New.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Password_New.WordWrap = false;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Black;
            this.panel4.Location = new System.Drawing.Point(110, 261);
            this.panel4.Margin = new System.Windows.Forms.Padding(4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(218, 1);
            this.panel4.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 13.8F);
            this.label2.Location = new System.Drawing.Point(17, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 31);
            this.label2.TabIndex = 24;
            this.label2.Text = "登录名";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 13.8F);
            this.label3.Location = new System.Drawing.Point(17, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 31);
            this.label3.TabIndex = 24;
            this.label3.Text = "新密码";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 13.8F);
            this.label4.Location = new System.Drawing.Point(18, 226);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 31);
            this.label4.TabIndex = 24;
            this.label4.Text = "再确认";
            // 
            // username_combo
            // 
            this.username_combo.BackColor = System.Drawing.Color.AliceBlue;
            this.username_combo.Font = new System.Drawing.Font("微软雅黑", 13.8F);
            this.username_combo.FormattingEnabled = true;
            this.username_combo.Location = new System.Drawing.Point(109, 95);
            this.username_combo.Name = "username_combo";
            this.username_combo.Size = new System.Drawing.Size(219, 38);
            this.username_combo.TabIndex = 25;
            // 
            // MsgText
            // 
            this.MsgText.Font = new System.Drawing.Font("黑体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MsgText.Location = new System.Drawing.Point(0, 377);
            this.MsgText.Name = "MsgText";
            this.MsgText.Size = new System.Drawing.Size(398, 40);
            this.MsgText.TabIndex = 26;
            this.MsgText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ChangePaswd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(398, 426);
            this.ControlBox = false;
            this.Controls.Add(this.MsgText);
            this.Controls.Add(this.username_combo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.IsSee2);
            this.Controls.Add(this.Password_New);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.IsSee);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.Password_Old);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ChangePaswd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ChangePaswd";
            this.Load += new System.EventHandler(this.ChangePaswd_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IsSee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IsSee2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label CloseBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox IsSee;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.TextBox Password_Old;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox IsSee2;
        private System.Windows.Forms.TextBox Password_New;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox username_combo;
        private System.Windows.Forms.Label MsgText;
    }
}