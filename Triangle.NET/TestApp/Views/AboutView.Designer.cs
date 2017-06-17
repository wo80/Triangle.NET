namespace MeshExplorer.Views
{
    partial class AboutView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbCodeplex = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbShortcuts = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbCodeplex
            // 
            this.lbCodeplex.AutoSize = true;
            this.lbCodeplex.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCodeplex.ForeColor = System.Drawing.Color.White;
            this.lbCodeplex.Location = new System.Drawing.Point(72, 82);
            this.lbCodeplex.Name = "lbCodeplex";
            this.lbCodeplex.Size = new System.Drawing.Size(153, 13);
            this.lbCodeplex.TabIndex = 9;
            this.lbCodeplex.Text = "http://triangle.codeplex.com";
            this.lbCodeplex.Click += new System.EventHandler(this.lbCodeplex_Clicked);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(10, 17);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(73, 13);
            this.label15.TabIndex = 7;
            this.label15.Text = "Triangle.NET";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(10, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Keyboard shortcuts";
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.White;
            this.label19.Location = new System.Drawing.Point(72, 42);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(134, 40);
            this.label19.TabIndex = 6;
            this.label19.Text = "Beta 4 (2014-05-30)\r\nChristian Woltering\r\nMIT";
            // 
            // label18
            // 
            this.label18.ForeColor = System.Drawing.Color.White;
            this.label18.Location = new System.Drawing.Point(15, 42);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(51, 40);
            this.label18.TabIndex = 4;
            this.label18.Text = "Version:\r\nAuthor:\r\nLicense:";
            this.label18.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(72, 155);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(134, 108);
            this.label7.TabIndex = 5;
            this.label7.Text = "File Open\r\nFile Save\r\nReload Input\r\n\r\nTriangulate / Refine\r\nSmooth\r\n\r\nShow Log";
            // 
            // lbShortcuts
            // 
            this.lbShortcuts.ForeColor = System.Drawing.Color.White;
            this.lbShortcuts.Location = new System.Drawing.Point(24, 155);
            this.lbShortcuts.Name = "lbShortcuts";
            this.lbShortcuts.Size = new System.Drawing.Size(36, 108);
            this.lbShortcuts.TabIndex = 3;
            this.lbShortcuts.Text = "F3\r\nF4\r\nF5\r\n\r\nF8\r\nF9\r\n\r\nF12";
            this.lbShortcuts.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // AboutView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.Controls.Add(this.lbCodeplex);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lbShortcuts);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.DarkGray;
            this.Name = "AboutView";
            this.Size = new System.Drawing.Size(272, 509);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbCodeplex;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbShortcuts;
    }
}
