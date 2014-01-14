namespace MeshExplorer
{
    partial class FormTopology
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.renderControl = new MeshExplorer.Topology.TopologyRenderControl();
            this.topoControlView = new MeshExplorer.Topology.TopologyControlView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.renderControl);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.topoControlView);
            this.splitContainer1.Size = new System.Drawing.Size(674, 455);
            this.splitContainer1.SplitterDistance = 475;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            // 
            // renderControl
            // 
            this.renderControl.BackColor = System.Drawing.Color.Black;
            this.renderControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renderControl.Location = new System.Drawing.Point(0, 0);
            this.renderControl.Name = "renderControl";
            this.renderControl.Size = new System.Drawing.Size(475, 455);
            this.renderControl.TabIndex = 0;
            this.renderControl.Text = "topologyRenderControl";
            this.renderControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.renderControl_MouseClick);
            // 
            // topoControlView
            // 
            this.topoControlView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.topoControlView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topoControlView.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.topoControlView.ForeColor = System.Drawing.Color.White;
            this.topoControlView.Location = new System.Drawing.Point(0, 0);
            this.topoControlView.Name = "topoControlView";
            this.topoControlView.Size = new System.Drawing.Size(198, 455);
            this.topoControlView.TabIndex = 2;
            // 
            // FormTopology
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.ClientSize = new System.Drawing.Size(674, 455);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTopology";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Triangle.NET - Topology Explorer";
            this.Load += new System.EventHandler(this.FormTopology_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Topology.TopologyRenderControl renderControl;
        private Topology.TopologyControlView topoControlView;
    }
}