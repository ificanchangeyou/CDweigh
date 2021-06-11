namespace CDweigh.Menu_Page
{
    partial class OperationLog
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.OperationPage = new System.Windows.Forms.TabPage();
            this.AlarmPage = new System.Windows.Forms.TabPage();
            this.OperationGridView = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.StartPicker_Oper = new System.Windows.Forms.DateTimePicker();
            this.EndPicker_Oper = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.Select_But_Oper = new System.Windows.Forms.Button();
            this.Select_But_Alarm = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.EndPicker_Alarm = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.Start_Picker_Alarm = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.AlarmGridView = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.OperationPage.SuspendLayout();
            this.AlarmPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OperationGridView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AlarmGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.OperationPage);
            this.tabControl1.Controls.Add(this.AlarmPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1496, 582);
            this.tabControl1.TabIndex = 0;
            // 
            // OperationPage
            // 
            this.OperationPage.Controls.Add(this.Select_But_Oper);
            this.OperationPage.Controls.Add(this.groupBox1);
            this.OperationPage.Controls.Add(this.OperationGridView);
            this.OperationPage.Location = new System.Drawing.Point(4, 25);
            this.OperationPage.Name = "OperationPage";
            this.OperationPage.Padding = new System.Windows.Forms.Padding(3);
            this.OperationPage.Size = new System.Drawing.Size(1488, 553);
            this.OperationPage.TabIndex = 0;
            this.OperationPage.Text = "操作日志";
            this.OperationPage.UseVisualStyleBackColor = true;
            // 
            // AlarmPage
            // 
            this.AlarmPage.Controls.Add(this.Select_But_Alarm);
            this.AlarmPage.Controls.Add(this.groupBox2);
            this.AlarmPage.Controls.Add(this.AlarmGridView);
            this.AlarmPage.Location = new System.Drawing.Point(4, 25);
            this.AlarmPage.Name = "AlarmPage";
            this.AlarmPage.Padding = new System.Windows.Forms.Padding(3);
            this.AlarmPage.Size = new System.Drawing.Size(1488, 553);
            this.AlarmPage.TabIndex = 1;
            this.AlarmPage.Text = "报警日志";
            this.AlarmPage.UseVisualStyleBackColor = true;
            // 
            // OperationGridView
            // 
            this.OperationGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.OperationGridView.Dock = System.Windows.Forms.DockStyle.Left;
            this.OperationGridView.Location = new System.Drawing.Point(3, 3);
            this.OperationGridView.Name = "OperationGridView";
            this.OperationGridView.RowTemplate.Height = 27;
            this.OperationGridView.Size = new System.Drawing.Size(1194, 547);
            this.OperationGridView.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.EndPicker_Oper);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.StartPicker_Oper);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(1232, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 185);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询时间段";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(139, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "起始时间";
            // 
            // StartPicker_Oper
            // 
            this.StartPicker_Oper.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.StartPicker_Oper.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.StartPicker_Oper.Location = new System.Drawing.Point(17, 65);
            this.StartPicker_Oper.Name = "StartPicker_Oper";
            this.StartPicker_Oper.Size = new System.Drawing.Size(214, 34);
            this.StartPicker_Oper.TabIndex = 1;
            // 
            // EndPicker_Oper
            // 
            this.EndPicker_Oper.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.EndPicker_Oper.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.EndPicker_Oper.Location = new System.Drawing.Point(17, 134);
            this.EndPicker_Oper.Name = "EndPicker_Oper";
            this.EndPicker_Oper.Size = new System.Drawing.Size(214, 34);
            this.EndPicker_Oper.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(139, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 27);
            this.label2.TabIndex = 2;
            this.label2.Text = "结束时间";
            // 
            // Select_But_Oper
            // 
            this.Select_But_Oper.Font = new System.Drawing.Font("微软雅黑", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Select_But_Oper.Location = new System.Drawing.Point(1253, 477);
            this.Select_But_Oper.Name = "Select_But_Oper";
            this.Select_But_Oper.Size = new System.Drawing.Size(227, 68);
            this.Select_But_Oper.TabIndex = 2;
            this.Select_But_Oper.Text = "查    询";
            this.Select_But_Oper.UseVisualStyleBackColor = true;
            // 
            // Select_But_Alarm
            // 
            this.Select_But_Alarm.Font = new System.Drawing.Font("微软雅黑", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Select_But_Alarm.Location = new System.Drawing.Point(1253, 477);
            this.Select_But_Alarm.Name = "Select_But_Alarm";
            this.Select_But_Alarm.Size = new System.Drawing.Size(227, 68);
            this.Select_But_Alarm.TabIndex = 5;
            this.Select_But_Alarm.Text = "查    询";
            this.Select_But_Alarm.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.EndPicker_Alarm);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.Start_Picker_Alarm);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(1232, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(248, 185);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "查询时间段";
            // 
            // EndPicker_Alarm
            // 
            this.EndPicker_Alarm.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.EndPicker_Alarm.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.EndPicker_Alarm.Location = new System.Drawing.Point(17, 134);
            this.EndPicker_Alarm.Name = "EndPicker_Alarm";
            this.EndPicker_Alarm.Size = new System.Drawing.Size(214, 34);
            this.EndPicker_Alarm.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(139, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 27);
            this.label3.TabIndex = 2;
            this.label3.Text = "结束时间";
            // 
            // Start_Picker_Alarm
            // 
            this.Start_Picker_Alarm.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.Start_Picker_Alarm.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Start_Picker_Alarm.Location = new System.Drawing.Point(17, 65);
            this.Start_Picker_Alarm.Name = "Start_Picker_Alarm";
            this.Start_Picker_Alarm.Size = new System.Drawing.Size(214, 34);
            this.Start_Picker_Alarm.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(139, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 27);
            this.label4.TabIndex = 0;
            this.label4.Text = "起始时间";
            // 
            // AlarmGridView
            // 
            this.AlarmGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AlarmGridView.Dock = System.Windows.Forms.DockStyle.Left;
            this.AlarmGridView.Location = new System.Drawing.Point(3, 3);
            this.AlarmGridView.Name = "AlarmGridView";
            this.AlarmGridView.RowTemplate.Height = 27;
            this.AlarmGridView.Size = new System.Drawing.Size(1194, 547);
            this.AlarmGridView.TabIndex = 3;
            // 
            // OperationLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1496, 582);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OperationLog";
            this.Text = "OperationLog";
            this.tabControl1.ResumeLayout(false);
            this.OperationPage.ResumeLayout(false);
            this.AlarmPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OperationGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AlarmGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage OperationPage;
        private System.Windows.Forms.TabPage AlarmPage;
        private System.Windows.Forms.Button Select_But_Oper;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker EndPicker_Oper;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker StartPicker_Oper;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView OperationGridView;
        private System.Windows.Forms.Button Select_But_Alarm;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker EndPicker_Alarm;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker Start_Picker_Alarm;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView AlarmGridView;
    }
}