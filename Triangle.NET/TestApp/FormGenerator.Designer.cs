namespace MeshExplorer
{
    partial class FormGenerator
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
            this.lbParam1 = new System.Windows.Forms.Label();
            this.lbParam2 = new System.Windows.Forms.Label();
            this.lbParam3 = new System.Windows.Forms.Label();
            this.lbParam1Val = new System.Windows.Forms.Label();
            this.lbParam2Val = new System.Windows.Forms.Label();
            this.lbParam3Val = new System.Windows.Forms.Label();
            this.lbDescription = new System.Windows.Forms.Label();
            this.sliderParam3 = new MeshExplorer.Controls.DarkSlider();
            this.sliderParam2 = new MeshExplorer.Controls.DarkSlider();
            this.sliderParam1 = new MeshExplorer.Controls.DarkSlider();
            this.darkListBox1 = new MeshExplorer.Controls.DarkListBox();
            this.btnClose = new MeshExplorer.Controls.DarkButton();
            this.btnGenerate = new MeshExplorer.Controls.DarkButton();
            this.SuspendLayout();
            // 
            // lbParam1
            // 
            this.lbParam1.BackColor = System.Drawing.Color.DimGray;
            this.lbParam1.ForeColor = System.Drawing.Color.White;
            this.lbParam1.Location = new System.Drawing.Point(171, 24);
            this.lbParam1.Name = "lbParam1";
            this.lbParam1.Size = new System.Drawing.Size(114, 13);
            this.lbParam1.TabIndex = 4;
            this.lbParam1.Text = "Param 1:";
            // 
            // lbParam2
            // 
            this.lbParam2.BackColor = System.Drawing.Color.DimGray;
            this.lbParam2.ForeColor = System.Drawing.Color.White;
            this.lbParam2.Location = new System.Drawing.Point(171, 47);
            this.lbParam2.Name = "lbParam2";
            this.lbParam2.Size = new System.Drawing.Size(114, 13);
            this.lbParam2.TabIndex = 4;
            this.lbParam2.Text = "Param 2:";
            // 
            // lbParam3
            // 
            this.lbParam3.BackColor = System.Drawing.Color.DimGray;
            this.lbParam3.ForeColor = System.Drawing.Color.White;
            this.lbParam3.Location = new System.Drawing.Point(171, 70);
            this.lbParam3.Name = "lbParam3";
            this.lbParam3.Size = new System.Drawing.Size(114, 13);
            this.lbParam3.TabIndex = 4;
            this.lbParam3.Text = "Param 3:";
            // 
            // lbParam1Val
            // 
            this.lbParam1Val.BackColor = System.Drawing.Color.DimGray;
            this.lbParam1Val.ForeColor = System.Drawing.Color.White;
            this.lbParam1Val.Location = new System.Drawing.Point(436, 24);
            this.lbParam1Val.Name = "lbParam1Val";
            this.lbParam1Val.Size = new System.Drawing.Size(40, 13);
            this.lbParam1Val.TabIndex = 4;
            this.lbParam1Val.Text = "-";
            // 
            // lbParam2Val
            // 
            this.lbParam2Val.BackColor = System.Drawing.Color.DimGray;
            this.lbParam2Val.ForeColor = System.Drawing.Color.White;
            this.lbParam2Val.Location = new System.Drawing.Point(436, 47);
            this.lbParam2Val.Name = "lbParam2Val";
            this.lbParam2Val.Size = new System.Drawing.Size(40, 13);
            this.lbParam2Val.TabIndex = 4;
            this.lbParam2Val.Text = "-";
            // 
            // lbParam3Val
            // 
            this.lbParam3Val.BackColor = System.Drawing.Color.DimGray;
            this.lbParam3Val.ForeColor = System.Drawing.Color.White;
            this.lbParam3Val.Location = new System.Drawing.Point(436, 70);
            this.lbParam3Val.Name = "lbParam3Val";
            this.lbParam3Val.Size = new System.Drawing.Size(40, 13);
            this.lbParam3Val.TabIndex = 4;
            this.lbParam3Val.Text = "-";
            // 
            // lbDescription
            // 
            this.lbDescription.BackColor = System.Drawing.Color.DimGray;
            this.lbDescription.ForeColor = System.Drawing.Color.White;
            this.lbDescription.Location = new System.Drawing.Point(171, 104);
            this.lbDescription.Name = "lbDescription";
            this.lbDescription.Size = new System.Drawing.Size(94, 13);
            this.lbDescription.TabIndex = 4;
            this.lbDescription.Text = "Description";
            // 
            // sliderParam3
            // 
            this.sliderParam3.BackColor = System.Drawing.Color.Transparent;
            this.sliderParam3.CriticalPercent = ((uint)(0u));
            this.sliderParam3.Enabled = false;
            this.sliderParam3.Location = new System.Drawing.Point(291, 64);
            this.sliderParam3.Maximum = 100;
            this.sliderParam3.Minimum = 0;
            this.sliderParam3.Name = "sliderParam3";
            this.sliderParam3.Size = new System.Drawing.Size(138, 23);
            this.sliderParam3.TabIndex = 3;
            this.sliderParam3.Text = "sliderParam3";
            this.sliderParam3.Value = 50;
            this.sliderParam3.ValueChanging += new System.EventHandler(this.sliderParam3_ValueChanging);
            // 
            // sliderParam2
            // 
            this.sliderParam2.BackColor = System.Drawing.Color.Transparent;
            this.sliderParam2.CriticalPercent = ((uint)(0u));
            this.sliderParam2.Enabled = false;
            this.sliderParam2.Location = new System.Drawing.Point(291, 41);
            this.sliderParam2.Maximum = 100;
            this.sliderParam2.Minimum = 0;
            this.sliderParam2.Name = "sliderParam2";
            this.sliderParam2.Size = new System.Drawing.Size(138, 23);
            this.sliderParam2.TabIndex = 3;
            this.sliderParam2.Text = "sliderParam2";
            this.sliderParam2.Value = 50;
            this.sliderParam2.ValueChanging += new System.EventHandler(this.sliderParam2_ValueChanging);
            // 
            // sliderParam1
            // 
            this.sliderParam1.BackColor = System.Drawing.Color.Transparent;
            this.sliderParam1.CriticalPercent = ((uint)(0u));
            this.sliderParam1.Enabled = false;
            this.sliderParam1.Location = new System.Drawing.Point(291, 18);
            this.sliderParam1.Maximum = 100;
            this.sliderParam1.Minimum = 0;
            this.sliderParam1.Name = "sliderParam1";
            this.sliderParam1.Size = new System.Drawing.Size(138, 23);
            this.sliderParam1.TabIndex = 3;
            this.sliderParam1.Text = "sliderParam1";
            this.sliderParam1.Value = 50;
            this.sliderParam1.ValueChanging += new System.EventHandler(this.sliderParam1_ValueChanging);
            // 
            // darkListBox1
            // 
            this.darkListBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(96)))), ((int)(((byte)(96)))));
            this.darkListBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.darkListBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.darkListBox1.FormattingEnabled = true;
            this.darkListBox1.ItemHeight = 22;
            this.darkListBox1.Location = new System.Drawing.Point(5, 5);
            this.darkListBox1.Name = "darkListBox1";
            this.darkListBox1.Size = new System.Drawing.Size(160, 228);
            this.darkListBox1.TabIndex = 2;
            this.darkListBox1.SelectedIndexChanged += new System.EventHandler(this.darkListBox1_SelectedIndexChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(291, 247);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(94, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(391, 247);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(94, 23);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // FormGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.ClientSize = new System.Drawing.Size(495, 278);
            this.Controls.Add(this.lbParam3Val);
            this.Controls.Add(this.lbDescription);
            this.Controls.Add(this.lbParam3);
            this.Controls.Add(this.lbParam2Val);
            this.Controls.Add(this.lbParam2);
            this.Controls.Add(this.lbParam1Val);
            this.Controls.Add(this.lbParam1);
            this.Controls.Add(this.sliderParam3);
            this.Controls.Add(this.sliderParam2);
            this.Controls.Add(this.sliderParam1);
            this.Controls.Add(this.darkListBox1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnGenerate);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormGenerator";
            this.ShowInTaskbar = false;
            this.Text = "Input Generator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGenerator_FormClosing);
            this.Load += new System.EventHandler(this.FormGenerator_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.DarkButton btnGenerate;
        private Controls.DarkButton btnClose;
        private Controls.DarkListBox darkListBox1;
        private Controls.DarkSlider sliderParam1;
        private Controls.DarkSlider sliderParam2;
        private Controls.DarkSlider sliderParam3;
        private System.Windows.Forms.Label lbParam1;
        private System.Windows.Forms.Label lbParam2;
        private System.Windows.Forms.Label lbParam3;
        private System.Windows.Forms.Label lbParam1Val;
        private System.Windows.Forms.Label lbParam2Val;
        private System.Windows.Forms.Label lbParam3Val;
        private System.Windows.Forms.Label lbDescription;
    }
}