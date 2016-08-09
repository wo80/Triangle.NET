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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbSize = new System.Windows.Forms.Label();
            this.cbUseCompression = new MeshExplorer.Controls.DarkCheckBox();
            this.darkSlider1 = new MeshExplorer.Controls.DarkSlider();
            this.darkTextBox1 = new MeshExplorer.Controls.DarkTextBox();
            this.darkListBox1 = new MeshExplorer.Controls.DarkListBox();
            this.darkButton1 = new MeshExplorer.Controls.DarkButton();
            this.btnExport = new MeshExplorer.Controls.DarkButton();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "File fromat:\r\n";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(12, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "File name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(12, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Image width:";
            // 
            // lbSize
            // 
            this.lbSize.AutoSize = true;
            this.lbSize.BackColor = System.Drawing.Color.Transparent;
            this.lbSize.ForeColor = System.Drawing.Color.Gray;
            this.lbSize.Location = new System.Drawing.Point(272, 148);
            this.lbSize.Name = "lbSize";
            this.lbSize.Size = new System.Drawing.Size(40, 13);
            this.lbSize.TabIndex = 2;
            this.lbSize.Text = "800 px";
            // 
            // cbUseCompression
            // 
            this.cbUseCompression.BackColor = System.Drawing.Color.DimGray;
            this.cbUseCompression.Checked = false;
            this.cbUseCompression.Enabled = false;
            this.cbUseCompression.Location = new System.Drawing.Point(15, 231);
            this.cbUseCompression.Name = "cbUseCompression";
            this.cbUseCompression.Size = new System.Drawing.Size(297, 23);
            this.cbUseCompression.TabIndex = 6;
            this.cbUseCompression.Text = "Use GZip compression";
            this.cbUseCompression.UseVisualStyleBackColor = false;
            // 
            // darkSlider1
            // 
            this.darkSlider1.BackColor = System.Drawing.Color.Transparent;
            this.darkSlider1.CriticalPercent = ((uint)(0u));
            this.darkSlider1.Location = new System.Drawing.Point(15, 161);
            this.darkSlider1.Maximum = 100;
            this.darkSlider1.Minimum = 0;
            this.darkSlider1.Name = "darkSlider1";
            this.darkSlider1.Size = new System.Drawing.Size(297, 17);
            this.darkSlider1.TabIndex = 5;
            this.darkSlider1.Text = "darkSlider1";
            this.darkSlider1.Value = 35;
            this.darkSlider1.ValueChanging += new System.EventHandler(this.darkSlider1_ValueChanging);
            // 
            // darkTextBox1
            // 
            this.darkTextBox1.BackColor = System.Drawing.Color.White;
            this.darkTextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.darkTextBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.darkTextBox1.ForeColor = System.Drawing.Color.Black;
            this.darkTextBox1.Location = new System.Drawing.Point(12, 206);
            this.darkTextBox1.Name = "darkTextBox1";
            this.darkTextBox1.Size = new System.Drawing.Size(300, 21);
            this.darkTextBox1.TabIndex = 4;
            this.darkTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            // 
            // darkListBox1
            // 
            this.darkListBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.darkListBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.darkListBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.darkListBox1.FormattingEnabled = true;
            this.darkListBox1.ItemHeight = 22;
            this.darkListBox1.Items.AddRange(new object[] {
            "Portable Network Graphics (*.png)",
            "Encapsulated PostScript (*.eps)",
            "Scalable Vector Graphics (*.svg)"});
            this.darkListBox1.Location = new System.Drawing.Point(12, 25);
            this.darkListBox1.Name = "darkListBox1";
            this.darkListBox1.Size = new System.Drawing.Size(302, 110);
            this.darkListBox1.TabIndex = 3;
            this.darkListBox1.SelectedIndexChanged += new System.EventHandler(this.darkListBox1_SelectedIndexChanged);
            // 
            // darkButton1
            // 
            this.darkButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.darkButton1.Location = new System.Drawing.Point(144, 270);
            this.darkButton1.Name = "darkButton1";
            this.darkButton1.Size = new System.Drawing.Size(82, 23);
            this.darkButton1.TabIndex = 1;
            this.darkButton1.Text = "Cancel";
            this.darkButton1.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnExport.Location = new System.Drawing.Point(232, 270);
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
            this.ClientSize = new System.Drawing.Size(324, 299);
            this.Controls.Add(this.cbUseCompression);
            this.Controls.Add(this.darkSlider1);
            this.Controls.Add(this.darkTextBox1);
            this.Controls.Add(this.darkListBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
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
        private Controls.DarkListBox darkListBox1;
        private System.Windows.Forms.Label label2;
        private Controls.DarkTextBox darkTextBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private Controls.DarkSlider darkSlider1;
        private System.Windows.Forms.Label lbSize;
        private Controls.DarkCheckBox cbUseCompression;
    }
}