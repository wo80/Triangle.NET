namespace TestApp
{
    partial class FormQuality
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.lbNumInput = new System.Windows.Forms.Label();
            this.lbNumOutput = new System.Windows.Forms.Label();
            this.lbNumTri = new System.Windows.Forms.Label();
            this.lbNumEdge = new System.Windows.Forms.Label();
            this.lbNumBoundary = new System.Windows.Forms.Label();
            this.lbCalcTime = new System.Windows.Forms.Label();
            this.lbRenderTime = new System.Windows.Forms.Label();
            this.lbAreaMin = new System.Windows.Forms.Label();
            this.lbAreaMax = new System.Windows.Forms.Label();
            this.lbEdgeMin = new System.Windows.Forms.Label();
            this.lbEdgeMax = new System.Windows.Forms.Label();
            this.lbRatioMin = new System.Windows.Forms.Label();
            this.lbRatioMax = new System.Windows.Forms.Label();
            this.lbAngleMin = new System.Windows.Forms.Label();
            this.lbAngleMax = new System.Windows.Forms.Label();
            this.histogram1 = new TestApp.Controls.AngleHistogram();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(11, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Input vertices:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(11, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Mesh vertices:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Gray;
            this.label3.Location = new System.Drawing.Point(11, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Mesh triangles:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Gray;
            this.label4.Location = new System.Drawing.Point(11, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Mesh edges:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Gray;
            this.label5.Location = new System.Drawing.Point(11, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Exterior boundary edges:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(12, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Mesh:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Gray;
            this.label7.Location = new System.Drawing.Point(12, 191);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Triangulation:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Gray;
            this.label8.Location = new System.Drawing.Point(12, 212);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Rendering:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(11, 266);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Quality:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Gray;
            this.label10.Location = new System.Drawing.Point(12, 296);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Triangle area:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(12, 163);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Stopwatch:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.Gray;
            this.label12.Location = new System.Drawing.Point(109, 266);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(55, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Minimum";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Gray;
            this.label13.Location = new System.Drawing.Point(199, 266);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Maximum";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.Gray;
            this.label14.Location = new System.Drawing.Point(12, 320);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(73, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "Edge length:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.Gray;
            this.label15.Location = new System.Drawing.Point(11, 343);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(71, 13);
            this.label15.TabIndex = 0;
            this.label15.Text = "Aspect ratio:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.Gray;
            this.label16.Location = new System.Drawing.Point(11, 366);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(40, 13);
            this.label16.TabIndex = 0;
            this.label16.Text = "Angle:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(11, 412);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(97, 13);
            this.label17.TabIndex = 0;
            this.label17.Text = "Angle histogram:";
            // 
            // lbNumInput
            // 
            this.lbNumInput.AutoSize = true;
            this.lbNumInput.ForeColor = System.Drawing.Color.White;
            this.lbNumInput.Location = new System.Drawing.Point(171, 36);
            this.lbNumInput.Name = "lbNumInput";
            this.lbNumInput.Size = new System.Drawing.Size(11, 13);
            this.lbNumInput.TabIndex = 0;
            this.lbNumInput.Text = "-";
            // 
            // lbNumOutput
            // 
            this.lbNumOutput.AutoSize = true;
            this.lbNumOutput.ForeColor = System.Drawing.Color.White;
            this.lbNumOutput.Location = new System.Drawing.Point(171, 59);
            this.lbNumOutput.Name = "lbNumOutput";
            this.lbNumOutput.Size = new System.Drawing.Size(11, 13);
            this.lbNumOutput.TabIndex = 0;
            this.lbNumOutput.Text = "-";
            // 
            // lbNumTri
            // 
            this.lbNumTri.AutoSize = true;
            this.lbNumTri.ForeColor = System.Drawing.Color.White;
            this.lbNumTri.Location = new System.Drawing.Point(171, 81);
            this.lbNumTri.Name = "lbNumTri";
            this.lbNumTri.Size = new System.Drawing.Size(11, 13);
            this.lbNumTri.TabIndex = 0;
            this.lbNumTri.Text = "-";
            // 
            // lbNumEdge
            // 
            this.lbNumEdge.AutoSize = true;
            this.lbNumEdge.ForeColor = System.Drawing.Color.White;
            this.lbNumEdge.Location = new System.Drawing.Point(171, 103);
            this.lbNumEdge.Name = "lbNumEdge";
            this.lbNumEdge.Size = new System.Drawing.Size(11, 13);
            this.lbNumEdge.TabIndex = 0;
            this.lbNumEdge.Text = "-";
            // 
            // lbNumBoundary
            // 
            this.lbNumBoundary.AutoSize = true;
            this.lbNumBoundary.ForeColor = System.Drawing.Color.White;
            this.lbNumBoundary.Location = new System.Drawing.Point(171, 125);
            this.lbNumBoundary.Name = "lbNumBoundary";
            this.lbNumBoundary.Size = new System.Drawing.Size(11, 13);
            this.lbNumBoundary.TabIndex = 0;
            this.lbNumBoundary.Text = "-";
            // 
            // lbCalcTime
            // 
            this.lbCalcTime.AutoSize = true;
            this.lbCalcTime.ForeColor = System.Drawing.Color.White;
            this.lbCalcTime.Location = new System.Drawing.Point(171, 191);
            this.lbCalcTime.Name = "lbCalcTime";
            this.lbCalcTime.Size = new System.Drawing.Size(11, 13);
            this.lbCalcTime.TabIndex = 0;
            this.lbCalcTime.Text = "-";
            // 
            // lbRenderTime
            // 
            this.lbRenderTime.AutoSize = true;
            this.lbRenderTime.ForeColor = System.Drawing.Color.White;
            this.lbRenderTime.Location = new System.Drawing.Point(171, 212);
            this.lbRenderTime.Name = "lbRenderTime";
            this.lbRenderTime.Size = new System.Drawing.Size(11, 13);
            this.lbRenderTime.TabIndex = 0;
            this.lbRenderTime.Text = "-";
            // 
            // lbAreaMin
            // 
            this.lbAreaMin.ForeColor = System.Drawing.Color.White;
            this.lbAreaMin.Location = new System.Drawing.Point(97, 296);
            this.lbAreaMin.Name = "lbAreaMin";
            this.lbAreaMin.Size = new System.Drawing.Size(68, 13);
            this.lbAreaMin.TabIndex = 0;
            this.lbAreaMin.Text = "-";
            this.lbAreaMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbAreaMax
            // 
            this.lbAreaMax.ForeColor = System.Drawing.Color.White;
            this.lbAreaMax.Location = new System.Drawing.Point(179, 296);
            this.lbAreaMax.Name = "lbAreaMax";
            this.lbAreaMax.Size = new System.Drawing.Size(76, 13);
            this.lbAreaMax.TabIndex = 0;
            this.lbAreaMax.Text = "-";
            this.lbAreaMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbEdgeMin
            // 
            this.lbEdgeMin.ForeColor = System.Drawing.Color.White;
            this.lbEdgeMin.Location = new System.Drawing.Point(97, 320);
            this.lbEdgeMin.Name = "lbEdgeMin";
            this.lbEdgeMin.Size = new System.Drawing.Size(68, 13);
            this.lbEdgeMin.TabIndex = 0;
            this.lbEdgeMin.Text = "-";
            this.lbEdgeMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbEdgeMax
            // 
            this.lbEdgeMax.ForeColor = System.Drawing.Color.White;
            this.lbEdgeMax.Location = new System.Drawing.Point(179, 320);
            this.lbEdgeMax.Name = "lbEdgeMax";
            this.lbEdgeMax.Size = new System.Drawing.Size(76, 13);
            this.lbEdgeMax.TabIndex = 0;
            this.lbEdgeMax.Text = "-";
            this.lbEdgeMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbRatioMin
            // 
            this.lbRatioMin.ForeColor = System.Drawing.Color.White;
            this.lbRatioMin.Location = new System.Drawing.Point(97, 343);
            this.lbRatioMin.Name = "lbRatioMin";
            this.lbRatioMin.Size = new System.Drawing.Size(68, 13);
            this.lbRatioMin.TabIndex = 0;
            this.lbRatioMin.Text = "-";
            this.lbRatioMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbRatioMax
            // 
            this.lbRatioMax.ForeColor = System.Drawing.Color.White;
            this.lbRatioMax.Location = new System.Drawing.Point(179, 343);
            this.lbRatioMax.Name = "lbRatioMax";
            this.lbRatioMax.Size = new System.Drawing.Size(76, 13);
            this.lbRatioMax.TabIndex = 0;
            this.lbRatioMax.Text = "-";
            this.lbRatioMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbAngleMin
            // 
            this.lbAngleMin.ForeColor = System.Drawing.Color.White;
            this.lbAngleMin.Location = new System.Drawing.Point(97, 366);
            this.lbAngleMin.Name = "lbAngleMin";
            this.lbAngleMin.Size = new System.Drawing.Size(68, 13);
            this.lbAngleMin.TabIndex = 0;
            this.lbAngleMin.Text = "-";
            this.lbAngleMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbAngleMax
            // 
            this.lbAngleMax.ForeColor = System.Drawing.Color.White;
            this.lbAngleMax.Location = new System.Drawing.Point(179, 366);
            this.lbAngleMax.Name = "lbAngleMax";
            this.lbAngleMax.Size = new System.Drawing.Size(76, 13);
            this.lbAngleMax.TabIndex = 0;
            this.lbAngleMax.Text = "-";
            this.lbAngleMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // histogram1
            // 
            this.histogram1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.histogram1.Location = new System.Drawing.Point(15, 428);
            this.histogram1.Name = "histogram1";
            this.histogram1.Size = new System.Drawing.Size(257, 212);
            this.histogram1.TabIndex = 1;
            this.histogram1.Text = "histogram1";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.ClientSize = new System.Drawing.Size(284, 652);
            this.Controls.Add(this.histogram1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.lbAngleMax);
            this.Controls.Add(this.lbRatioMax);
            this.Controls.Add(this.lbEdgeMax);
            this.Controls.Add(this.lbAreaMax);
            this.Controls.Add(this.lbAngleMin);
            this.Controls.Add(this.lbRatioMin);
            this.Controls.Add(this.lbEdgeMin);
            this.Controls.Add(this.lbAreaMin);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbRenderTime);
            this.Controls.Add(this.lbCalcTime);
            this.Controls.Add(this.lbNumBoundary);
            this.Controls.Add(this.lbNumEdge);
            this.Controls.Add(this.lbNumTri);
            this.Controls.Add(this.lbNumOutput);
            this.Controls.Add(this.lbNumInput);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Gray;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Mesh Statistic";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label lbNumInput;
        private System.Windows.Forms.Label lbNumOutput;
        private System.Windows.Forms.Label lbNumTri;
        private System.Windows.Forms.Label lbNumEdge;
        private System.Windows.Forms.Label lbNumBoundary;
        private System.Windows.Forms.Label lbCalcTime;
        private System.Windows.Forms.Label lbRenderTime;
        private System.Windows.Forms.Label lbAreaMin;
        private System.Windows.Forms.Label lbAreaMax;
        private System.Windows.Forms.Label lbEdgeMin;
        private System.Windows.Forms.Label lbEdgeMax;
        private System.Windows.Forms.Label lbRatioMin;
        private System.Windows.Forms.Label lbRatioMax;
        private System.Windows.Forms.Label lbAngleMin;
        private System.Windows.Forms.Label lbAngleMax;
        private Controls.AngleHistogram histogram1;
    }
}