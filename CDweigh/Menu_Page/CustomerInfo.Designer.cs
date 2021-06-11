namespace CDweigh.Menu_Page
{
    partial class CustomerInfo
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
            this.Select_But = new System.Windows.Forms.Button();
            this.SecurityNum_text = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // Select_But
            // 
            this.Select_But.BackColor = System.Drawing.Color.DarkOrchid;
            this.Select_But.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Select_But.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Select_But.ForeColor = System.Drawing.Color.Transparent;
            this.Select_But.Location = new System.Drawing.Point(846, 252);
            this.Select_But.Name = "Select_But";
            this.Select_But.Size = new System.Drawing.Size(159, 39);
            this.Select_But.TabIndex = 18;
            this.Select_But.Text = "查     询";
            this.Select_But.UseVisualStyleBackColor = false;
            // 
            // SecurityNum_text
            // 
            this.SecurityNum_text.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.SecurityNum_text.Location = new System.Drawing.Point(830, 42);
            this.SecurityNum_text.Name = "SecurityNum_text";
            this.SecurityNum_text.Size = new System.Drawing.Size(175, 34);
            this.SecurityNum_text.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(913, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 27);
            this.label2.TabIndex = 16;
            this.label2.Text = "单位名称";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Azure;
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(812, 291);
            this.panel1.TabIndex = 15;
            // 
            // CustomerInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1280, 678);
            this.Controls.Add(this.Select_But);
            this.Controls.Add(this.SecurityNum_text);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CustomerInfo";
            this.Text = "CustomerInfo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Select_But;
        private System.Windows.Forms.TextBox SecurityNum_text;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
    }
}