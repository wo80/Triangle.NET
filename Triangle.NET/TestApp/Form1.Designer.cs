namespace TestApp
{
    partial class Form1
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
            this.btnStatistic = new TestApp.Controls.ButtonDark();
            this.tbNumPoints = new TestApp.Controls.TextBoxDark();
            this.cbConvex = new TestApp.Controls.CheckBoxDark();
            this.cbQuality = new TestApp.Controls.CheckBoxDark();
            this.btnRun = new TestApp.Controls.ButtonDark();
            this.btnOpen = new TestApp.Controls.ButtonDark();
            this.btnRandPts = new TestApp.Controls.ButtonDark();
            this.meshRenderer1 = new TestApp.MeshRenderer();
            this.lbTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStatistic
            // 
            this.btnStatistic.Location = new System.Drawing.Point(847, 12);
            this.btnStatistic.Name = "btnStatistic";
            this.btnStatistic.Size = new System.Drawing.Size(75, 23);
            this.btnStatistic.TabIndex = 11;
            this.btnStatistic.Text = "Statistic";
            this.btnStatistic.UseVisualStyleBackColor = true;
            this.btnStatistic.Click += new System.EventHandler(this.btnStatistic_Click);
            // 
            // tbNumPoints
            // 
            this.tbNumPoints.BackColor = System.Drawing.Color.DimGray;
            this.tbNumPoints.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tbNumPoints.ForeColor = System.Drawing.Color.LightGray;
            this.tbNumPoints.Location = new System.Drawing.Point(244, 13);
            this.tbNumPoints.Name = "tbNumPoints";
            this.tbNumPoints.Size = new System.Drawing.Size(60, 21);
            this.tbNumPoints.TabIndex = 10;
            this.tbNumPoints.Text = "1000";
            this.tbNumPoints.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cbConvex
            // 
            this.cbConvex.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.cbConvex.Checked = false;
            this.cbConvex.Location = new System.Drawing.Point(506, 12);
            this.cbConvex.Name = "cbConvex";
            this.cbConvex.Size = new System.Drawing.Size(110, 23);
            this.cbConvex.TabIndex = 9;
            this.cbConvex.Text = "Convex polygon";
            this.cbConvex.UseVisualStyleBackColor = false;
            // 
            // cbQuality
            // 
            this.cbQuality.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.cbQuality.Checked = false;
            this.cbQuality.Location = new System.Drawing.Point(622, 12);
            this.cbQuality.Name = "cbQuality";
            this.cbQuality.Size = new System.Drawing.Size(138, 23);
            this.cbQuality.TabIndex = 9;
            this.cbQuality.Text = "Produce quality mesh";
            this.cbQuality.UseVisualStyleBackColor = false;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(766, 12);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 8;
            this.btnRun.Text = "Triangulate";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(12, 12);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(110, 23);
            this.btnOpen.TabIndex = 7;
            this.btnOpen.Text = "Open File";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnRandPts
            // 
            this.btnRandPts.Location = new System.Drawing.Point(128, 12);
            this.btnRandPts.Name = "btnRandPts";
            this.btnRandPts.Size = new System.Drawing.Size(110, 23);
            this.btnRandPts.TabIndex = 6;
            this.btnRandPts.Text = "Random Points";
            this.btnRandPts.UseVisualStyleBackColor = true;
            this.btnRandPts.Click += new System.EventHandler(this.btnRandPts_Click);
            // 
            // meshRenderer1
            // 
            this.meshRenderer1.BackColor = System.Drawing.Color.Black;
            this.meshRenderer1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.meshRenderer1.Location = new System.Drawing.Point(0, 52);
            this.meshRenderer1.Name = "meshRenderer1";
            this.meshRenderer1.Size = new System.Drawing.Size(934, 600);
            this.meshRenderer1.TabIndex = 5;
            this.meshRenderer1.Text = "meshRenderer1";
            // 
            // lbTime
            // 
            this.lbTime.AutoSize = true;
            this.lbTime.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lbTime.Location = new System.Drawing.Point(323, 17);
            this.lbTime.Name = "lbTime";
            this.lbTime.Size = new System.Drawing.Size(0, 13);
            this.lbTime.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.ClientSize = new System.Drawing.Size(934, 652);
            this.Controls.Add(this.btnStatistic);
            this.Controls.Add(this.tbNumPoints);
            this.Controls.Add(this.cbConvex);
            this.Controls.Add(this.cbQuality);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnRandPts);
            this.Controls.Add(this.meshRenderer1);
            this.Controls.Add(this.lbTime);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Delaunay Triangulation";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MeshRenderer meshRenderer1;
        private Controls.ButtonDark btnRandPts;
        private Controls.ButtonDark btnOpen;
        private Controls.ButtonDark btnRun;
        private Controls.CheckBoxDark cbQuality;
        private Controls.TextBoxDark tbNumPoints;
        private Controls.ButtonDark btnStatistic;
        private Controls.CheckBoxDark cbConvex;
        private System.Windows.Forms.Label lbTime;

    }
}

