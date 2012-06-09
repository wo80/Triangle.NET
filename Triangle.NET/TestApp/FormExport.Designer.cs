namespace MeshExplorer
{
    partial class FormExport
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
            this.label1 = new System.Windows.Forms.Label();
            this.darkButton1 = new MeshExplorer.Controls.DarkButton();
            this.btnExport = new MeshExplorer.Controls.DarkButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(302, 39);
            this.label1.TabIndex = 2;
            this.label1.Text = "No options available at the moment.\r\n\r\nYou will find the saved image in the appli" +
    "cation directory.";
            // 
            // darkButton1
            // 
            this.darkButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.darkButton1.Location = new System.Drawing.Point(162, 141);
            this.darkButton1.Name = "darkButton1";
            this.darkButton1.Size = new System.Drawing.Size(82, 23);
            this.darkButton1.TabIndex = 1;
            this.darkButton1.Text = "Cancel";
            this.darkButton1.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnExport.Location = new System.Drawing.Point(250, 141);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(82, 23);
            this.btnExport.TabIndex = 0;
            this.btnExport.Text = "Save";
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // FormExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.ClientSize = new System.Drawing.Size(344, 172);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.darkButton1);
            this.Controls.Add(this.btnExport);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormExport";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export Image";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.DarkButton btnExport;
        private Controls.DarkButton darkButton1;
        private System.Windows.Forms.Label label1;
    }
}