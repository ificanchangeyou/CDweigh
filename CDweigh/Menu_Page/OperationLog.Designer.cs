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
            this.OperationGridView = new System.Windows.Forms.DataGridView();
            this.StartPicker_Oper = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.EndPicker_Oper = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Select_But_Oper = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.OperationGridView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OperationGridView
            // 
            this.OperationGridView.AllowUserToAddRows = false;
            this.OperationGridView.AllowUserToDeleteRows = false;
            this.OperationGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.OperationGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.OperationGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.OperationGridView.Dock = System.Windows.Forms.DockStyle.Left;
            this.OperationGridView.Location = new System.Drawing.Point(0, 0);
            this.OperationGridView.Name = "OperationGridView";
            this.OperationGridView.RowTemplate.Height = 27;
            this.OperationGridView.Size = new System.Drawing.Size(1194, 561);
            this.OperationGridView.TabIndex = 3;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.EndPicker_Oper);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.StartPicker_Oper);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(1240, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 185);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询时间段";
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(139, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "起始时间";
            // 
            // Select_But_Oper
            // 
            this.Select_But_Oper.Font = new System.Drawing.Font("微软雅黑", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Select_But_Oper.Location = new System.Drawing.Point(1261, 477);
            this.Select_But_Oper.Name = "Select_But_Oper";
            this.Select_But_Oper.Size = new System.Drawing.Size(227, 68);
            this.Select_But_Oper.TabIndex = 5;
            this.Select_But_Oper.Text = "查    询";
            this.Select_But_Oper.UseVisualStyleBackColor = true;
            // 
            // OperationLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1511, 561);
            this.Controls.Add(this.OperationGridView);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Select_But_Oper);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OperationLog";
            this.Text = "OperationLog";
            this.Load += new System.EventHandler(this.OperationLog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.OperationGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView OperationGridView;
        private System.Windows.Forms.DateTimePicker StartPicker_Oper;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker EndPicker_Oper;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Select_But_Oper;
    }
}