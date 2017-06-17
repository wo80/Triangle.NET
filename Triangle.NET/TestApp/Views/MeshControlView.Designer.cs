namespace MeshExplorer.Views
{
    partial class MeshControlView
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
            this.lbMaxArea = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbMinAngle = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbMaxAngle = new System.Windows.Forms.Label();
            this.cbSweepline = new MeshExplorer.Controls.DarkCheckBox();
            this.slMaxAngle = new MeshExplorer.Controls.DarkSlider();
            this.slMaxArea = new MeshExplorer.Controls.DarkSlider();
            this.slMinAngle = new MeshExplorer.Controls.DarkSlider();
            this.cbConformDel = new MeshExplorer.Controls.DarkCheckBox();
            this.cbConvex = new MeshExplorer.Controls.DarkCheckBox();
            this.cbQuality = new MeshExplorer.Controls.DarkCheckBox();
            this.SuspendLayout();
            // 
            // lbMaxArea
            // 
            this.lbMaxArea.AutoSize = true;
            this.lbMaxArea.ForeColor = System.Drawing.Color.White;
            this.lbMaxArea.Location = new System.Drawing.Point(227, 87);
            this.lbMaxArea.Name = "lbMaxArea";
            this.lbMaxArea.Size = new System.Drawing.Size(13, 13);
            this.lbMaxArea.TabIndex = 20;
            this.lbMaxArea.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(8, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Maximum area";
            // 
            // lbMinAngle
            // 
            this.lbMinAngle.AutoSize = true;
            this.lbMinAngle.ForeColor = System.Drawing.Color.White;
            this.lbMinAngle.Location = new System.Drawing.Point(227, 43);
            this.lbMinAngle.Name = "lbMinAngle";
            this.lbMinAngle.Size = new System.Drawing.Size(19, 13);
            this.lbMinAngle.TabIndex = 22;
            this.lbMinAngle.Text = "20";
            // 
            // label23
            // 
            this.label23.BackColor = System.Drawing.Color.DimGray;
            this.label23.ForeColor = System.Drawing.Color.DarkGray;
            this.label23.Location = new System.Drawing.Point(8, 173);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(251, 33);
            this.label23.TabIndex = 24;
            this.label23.Text = "Ensure that all triangles in the mesh are truly Delaunay, and not just constraine" +
    "d Delaunay.";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.DimGray;
            this.label9.ForeColor = System.Drawing.Color.DarkGray;
            this.label9.Location = new System.Drawing.Point(8, 237);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(258, 33);
            this.label9.TabIndex = 26;
            this.label9.Text = "Use the convex mesh option, if the convex hull should be included in the output.";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.DimGray;
            this.label8.ForeColor = System.Drawing.Color.DarkGray;
            this.label8.Location = new System.Drawing.Point(8, 110);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(258, 33);
            this.label8.TabIndex = 25;
            this.label8.Text = "Hint: maximum area values of 0 or 1 will be irgnored (no area constraints are set" +
    ").";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(8, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Minimum angle";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.DimGray;
            this.label1.ForeColor = System.Drawing.Color.DarkGray;
            this.label1.Location = new System.Drawing.Point(8, 304);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(258, 33);
            this.label1.TabIndex = 26;
            this.label1.Text = "Use Sweepline algorithm for triangulation instead of default Divide && Conquer.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(8, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Maximum angle";
            // 
            // lbMaxAngle
            // 
            this.lbMaxAngle.AutoSize = true;
            this.lbMaxAngle.ForeColor = System.Drawing.Color.White;
            this.lbMaxAngle.Location = new System.Drawing.Point(227, 65);
            this.lbMaxAngle.Name = "lbMaxAngle";
            this.lbMaxAngle.Size = new System.Drawing.Size(25, 13);
            this.lbMaxAngle.TabIndex = 22;
            this.lbMaxAngle.Text = "180";
            // 
            // cbSweepline
            // 
            this.cbSweepline.BackColor = System.Drawing.Color.DimGray;
            this.cbSweepline.Checked = false;
            this.cbSweepline.Location = new System.Drawing.Point(11, 278);
            this.cbSweepline.Name = "cbSweepline";
            this.cbSweepline.Size = new System.Drawing.Size(181, 23);
            this.cbSweepline.TabIndex = 27;
            this.cbSweepline.Text = "Use Sweepline algorithm";
            this.cbSweepline.UseVisualStyleBackColor = false;
            // 
            // slMaxAngle
            // 
            this.slMaxAngle.BackColor = System.Drawing.Color.Transparent;
            this.slMaxAngle.CriticalPercent = ((uint)(89u));
            this.slMaxAngle.Location = new System.Drawing.Point(102, 61);
            this.slMaxAngle.Maximum = 100;
            this.slMaxAngle.Minimum = 0;
            this.slMaxAngle.Name = "slMaxAngle";
            this.slMaxAngle.Size = new System.Drawing.Size(119, 18);
            this.slMaxAngle.TabIndex = 18;
            this.slMaxAngle.Text = "darkSlider1";
            this.slMaxAngle.Value = 0;
            this.slMaxAngle.ValueChanging += new System.EventHandler(this.slMaxAngle_ValueChanging);
            // 
            // slMaxArea
            // 
            this.slMaxArea.BackColor = System.Drawing.Color.Transparent;
            this.slMaxArea.CriticalPercent = ((uint)(0u));
            this.slMaxArea.Location = new System.Drawing.Point(102, 83);
            this.slMaxArea.Maximum = 100;
            this.slMaxArea.Minimum = 0;
            this.slMaxArea.Name = "slMaxArea";
            this.slMaxArea.Size = new System.Drawing.Size(119, 18);
            this.slMaxArea.TabIndex = 19;
            this.slMaxArea.Text = "darkSlider1";
            this.slMaxArea.Value = 0;
            this.slMaxArea.ValueChanging += new System.EventHandler(this.slMaxArea_ValueChanging);
            // 
            // slMinAngle
            // 
            this.slMinAngle.BackColor = System.Drawing.Color.Transparent;
            this.slMinAngle.CriticalPercent = ((uint)(89u));
            this.slMinAngle.Location = new System.Drawing.Point(102, 39);
            this.slMinAngle.Maximum = 100;
            this.slMinAngle.Minimum = 0;
            this.slMinAngle.Name = "slMinAngle";
            this.slMinAngle.Size = new System.Drawing.Size(119, 18);
            this.slMinAngle.TabIndex = 18;
            this.slMinAngle.Text = "darkSlider1";
            this.slMinAngle.Value = 50;
            this.slMinAngle.ValueChanging += new System.EventHandler(this.slMinAngle_ValueChanging);
            // 
            // cbConformDel
            // 
            this.cbConformDel.BackColor = System.Drawing.Color.DimGray;
            this.cbConformDel.Checked = false;
            this.cbConformDel.Location = new System.Drawing.Point(11, 153);
            this.cbConformDel.Name = "cbConformDel";
            this.cbConformDel.Size = new System.Drawing.Size(142, 17);
            this.cbConformDel.TabIndex = 16;
            this.cbConformDel.Text = "Conforming Delaunay";
            this.cbConformDel.UseVisualStyleBackColor = false;
            // 
            // cbConvex
            // 
            this.cbConvex.BackColor = System.Drawing.Color.DimGray;
            this.cbConvex.Checked = false;
            this.cbConvex.Location = new System.Drawing.Point(11, 217);
            this.cbConvex.Name = "cbConvex";
            this.cbConvex.Size = new System.Drawing.Size(115, 17);
            this.cbConvex.TabIndex = 15;
            this.cbConvex.Text = "Convex mesh";
            this.cbConvex.UseVisualStyleBackColor = false;
            // 
            // cbQuality
            // 
            this.cbQuality.BackColor = System.Drawing.Color.DimGray;
            this.cbQuality.Checked = false;
            this.cbQuality.Location = new System.Drawing.Point(11, 16);
            this.cbQuality.Name = "cbQuality";
            this.cbQuality.Size = new System.Drawing.Size(115, 17);
            this.cbQuality.TabIndex = 17;
            this.cbQuality.Text = "Quality mesh";
            this.cbQuality.UseVisualStyleBackColor = false;
            // 
            // MeshControlView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.Controls.Add(this.cbSweepline);
            this.Controls.Add(this.lbMaxArea);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lbMaxAngle);
            this.Controls.Add(this.lbMinAngle);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.slMaxAngle);
            this.Controls.Add(this.slMaxArea);
            this.Controls.Add(this.slMinAngle);
            this.Controls.Add(this.cbConformDel);
            this.Controls.Add(this.cbConvex);
            this.Controls.Add(this.cbQuality);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.DarkGray;
            this.Name = "MeshControlView";
            this.Size = new System.Drawing.Size(272, 509);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbMaxArea;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbMinAngle;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private Controls.DarkSlider slMaxArea;
        private Controls.DarkSlider slMinAngle;
        private Controls.DarkCheckBox cbConformDel;
        private Controls.DarkCheckBox cbConvex;
        private Controls.DarkCheckBox cbQuality;
        private Controls.DarkCheckBox cbSweepline;
        private System.Windows.Forms.Label label1;
        private Controls.DarkSlider slMaxAngle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbMaxAngle;
    }
}
