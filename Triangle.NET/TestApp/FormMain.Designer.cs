namespace MeshExplorer
{
    partial class FormMain
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
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.btnSmooth = new MeshExplorer.Controls.DarkButton();
            this.flatTabControl1 = new MeshExplorer.Controls.DarkTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.meshControlView = new MeshExplorer.Views.MeshControlView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.statisticView = new MeshExplorer.Views.StatisticView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.aboutView = new MeshExplorer.Views.AboutView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileExport = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuFileQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuViewVoronoi = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuViewLog = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTools = new System.Windows.Forms.ToolStripMenuItem();
            this.menuToolsGen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuToolsCheck = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuToolsTopology = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuToolsRcm = new System.Windows.Forms.ToolStripMenuItem();
            this.btnMesh = new MeshExplorer.Controls.DarkButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.flatTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.IsSplitterFixed = true;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.splitContainer.Panel1.Controls.Add(this.btnSmooth);
            this.splitContainer.Panel1.Controls.Add(this.flatTabControl1);
            this.splitContainer.Panel1.Controls.Add(this.menuStrip1);
            this.splitContainer.Panel1.Controls.Add(this.btnMesh);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.BackColor = System.Drawing.Color.Black;
            this.splitContainer.Size = new System.Drawing.Size(992, 623);
            this.splitContainer.SplitterDistance = 280;
            this.splitContainer.SplitterWidth = 1;
            this.splitContainer.TabIndex = 0;
            // 
            // btnSmooth
            // 
            this.btnSmooth.Enabled = false;
            this.btnSmooth.Location = new System.Drawing.Point(146, 44);
            this.btnSmooth.Name = "btnSmooth";
            this.btnSmooth.Size = new System.Drawing.Size(130, 23);
            this.btnSmooth.TabIndex = 12;
            this.btnSmooth.Text = "Smooth";
            this.btnSmooth.UseVisualStyleBackColor = true;
            this.btnSmooth.Click += new System.EventHandler(this.btnSmooth_Click);
            // 
            // flatTabControl1
            // 
            this.flatTabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.flatTabControl1.Controls.Add(this.tabPage1);
            this.flatTabControl1.Controls.Add(this.tabPage2);
            this.flatTabControl1.Controls.Add(this.tabPage3);
            this.flatTabControl1.Location = new System.Drawing.Point(0, 73);
            this.flatTabControl1.Name = "flatTabControl1";
            this.flatTabControl1.SelectedIndex = 0;
            this.flatTabControl1.Size = new System.Drawing.Size(280, 538);
            this.flatTabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.DimGray;
            this.tabPage1.Controls.Add(this.meshControlView);
            this.tabPage1.ForeColor = System.Drawing.Color.DarkGray;
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(272, 509);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Mesh Control";
            // 
            // meshControlView
            // 
            this.meshControlView.BackColor = System.Drawing.Color.DimGray;
            this.meshControlView.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.meshControlView.ForeColor = System.Drawing.Color.DarkGray;
            this.meshControlView.Location = new System.Drawing.Point(0, 0);
            this.meshControlView.Name = "meshControlView";
            this.meshControlView.Size = new System.Drawing.Size(272, 509);
            this.meshControlView.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.DimGray;
            this.tabPage2.Controls.Add(this.statisticView);
            this.tabPage2.ForeColor = System.Drawing.Color.White;
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(272, 509);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Statistic";
            // 
            // statisticView
            // 
            this.statisticView.BackColor = System.Drawing.Color.DimGray;
            this.statisticView.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statisticView.ForeColor = System.Drawing.Color.DarkGray;
            this.statisticView.Location = new System.Drawing.Point(0, 0);
            this.statisticView.Name = "statisticView";
            this.statisticView.Size = new System.Drawing.Size(272, 509);
            this.statisticView.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.DimGray;
            this.tabPage3.Controls.Add(this.aboutView);
            this.tabPage3.Location = new System.Drawing.Point(4, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(272, 509);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "About";
            // 
            // aboutView
            // 
            this.aboutView.BackColor = System.Drawing.Color.DimGray;
            this.aboutView.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutView.ForeColor = System.Drawing.Color.DarkGray;
            this.aboutView.Location = new System.Drawing.Point(0, 0);
            this.aboutView.Name = "aboutView";
            this.aboutView.Size = new System.Drawing.Size(272, 509);
            this.aboutView.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(0);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuView,
            this.menuTools});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.menuStrip1.Size = new System.Drawing.Size(280, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.menuFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileOpen,
            this.menuFileSave,
            this.toolStripSeparator3,
            this.menuFileExport,
            this.toolStripSeparator2,
            this.menuFileQuit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(37, 24);
            this.menuFile.Text = "File";
            // 
            // menuFileOpen
            // 
            this.menuFileOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuFileOpen.Name = "menuFileOpen";
            this.menuFileOpen.Size = new System.Drawing.Size(141, 22);
            this.menuFileOpen.Text = "Open";
            this.menuFileOpen.Click += new System.EventHandler(this.menuFileOpen_Click);
            // 
            // menuFileSave
            // 
            this.menuFileSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuFileSave.Enabled = false;
            this.menuFileSave.Name = "menuFileSave";
            this.menuFileSave.Size = new System.Drawing.Size(141, 22);
            this.menuFileSave.Text = "Save";
            this.menuFileSave.Click += new System.EventHandler(this.menuFileSave_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(138, 6);
            // 
            // menuFileExport
            // 
            this.menuFileExport.Enabled = false;
            this.menuFileExport.Name = "menuFileExport";
            this.menuFileExport.Size = new System.Drawing.Size(141, 22);
            this.menuFileExport.Text = "Export Image";
            this.menuFileExport.Click += new System.EventHandler(this.menuFileExport_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(138, 6);
            // 
            // menuFileQuit
            // 
            this.menuFileQuit.Name = "menuFileQuit";
            this.menuFileQuit.Size = new System.Drawing.Size(141, 22);
            this.menuFileQuit.Text = "Quit";
            this.menuFileQuit.Click += new System.EventHandler(this.menuFileQuit_Click);
            // 
            // menuView
            // 
            this.menuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuViewVoronoi,
            this.toolStripSeparator1,
            this.menuViewLog});
            this.menuView.Name = "menuView";
            this.menuView.Size = new System.Drawing.Size(44, 24);
            this.menuView.Text = "View";
            // 
            // menuViewVoronoi
            // 
            this.menuViewVoronoi.Enabled = false;
            this.menuViewVoronoi.Name = "menuViewVoronoi";
            this.menuViewVoronoi.Size = new System.Drawing.Size(162, 22);
            this.menuViewVoronoi.Text = "Voronoi Diagram";
            this.menuViewVoronoi.Click += new System.EventHandler(this.menuViewVoronoi_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(159, 6);
            // 
            // menuViewLog
            // 
            this.menuViewLog.Name = "menuViewLog";
            this.menuViewLog.Size = new System.Drawing.Size(162, 22);
            this.menuViewLog.Text = "Show Log";
            this.menuViewLog.Click += new System.EventHandler(this.menuViewLog_Click);
            // 
            // menuTools
            // 
            this.menuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolsGen,
            this.menuToolsCheck,
            this.toolStripSeparator5,
            this.menuToolsTopology,
            this.toolStripSeparator4,
            this.menuToolsRcm});
            this.menuTools.Name = "menuTools";
            this.menuTools.Size = new System.Drawing.Size(46, 24);
            this.menuTools.Text = "Tools";
            // 
            // menuToolsGen
            // 
            this.menuToolsGen.Name = "menuToolsGen";
            this.menuToolsGen.Size = new System.Drawing.Size(195, 22);
            this.menuToolsGen.Text = "Input Generator";
            this.menuToolsGen.Click += new System.EventHandler(this.menuToolsGenerator_Click);
            // 
            // menuToolsCheck
            // 
            this.menuToolsCheck.Enabled = false;
            this.menuToolsCheck.Name = "menuToolsCheck";
            this.menuToolsCheck.Size = new System.Drawing.Size(195, 22);
            this.menuToolsCheck.Text = "Check Mesh";
            this.menuToolsCheck.Click += new System.EventHandler(this.menuToolsCheck_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(192, 6);
            // 
            // menuToolsTopology
            // 
            this.menuToolsTopology.Name = "menuToolsTopology";
            this.menuToolsTopology.Size = new System.Drawing.Size(195, 22);
            this.menuToolsTopology.Text = "Topology Explorer";
            this.menuToolsTopology.Click += new System.EventHandler(this.menuToolsTopology_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(192, 6);
            // 
            // menuToolsRcm
            // 
            this.menuToolsRcm.Enabled = false;
            this.menuToolsRcm.Name = "menuToolsRcm";
            this.menuToolsRcm.Size = new System.Drawing.Size(195, 22);
            this.menuToolsRcm.Text = "Renumber nodes (RCM)";
            this.menuToolsRcm.Click += new System.EventHandler(this.menuToolsRcm_Click);
            // 
            // btnMesh
            // 
            this.btnMesh.Enabled = false;
            this.btnMesh.Location = new System.Drawing.Point(4, 44);
            this.btnMesh.Name = "btnMesh";
            this.btnMesh.Size = new System.Drawing.Size(130, 23);
            this.btnMesh.TabIndex = 12;
            this.btnMesh.Text = "Triangulate";
            this.btnMesh.UseVisualStyleBackColor = true;
            this.btnMesh.Click += new System.EventHandler(this.btnMesh_Click);
            // 
            // FormMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.ClientSize = new System.Drawing.Size(992, 623);
            this.Controls.Add(this.splitContainer);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1000, 650);
            this.Name = "FormMain";
            this.Text = "Triangle.NET - Mesh Explorer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResizeBegin += new System.EventHandler(this.ResizeBeginHandler);
            this.ResizeEnd += new System.EventHandler(this.ResizeEndHandler);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmDragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.frmDragOver);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.Resize += new System.EventHandler(this.ResizeHandler);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.flatTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuFileOpen;
        private System.Windows.Forms.ToolStripMenuItem menuFileSave;
        private Controls.DarkTabControl flatTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Controls.DarkButton btnSmooth;
        private Controls.DarkButton btnMesh;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ToolStripMenuItem menuView;
        private System.Windows.Forms.ToolStripMenuItem menuViewVoronoi;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuViewLog;
        private System.Windows.Forms.ToolStripMenuItem menuTools;
        private System.Windows.Forms.ToolStripMenuItem menuToolsGen;
        private System.Windows.Forms.ToolStripMenuItem menuToolsCheck;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuFileQuit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuFileExport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuToolsRcm;
        private Views.MeshControlView meshControlView;
        private Views.StatisticView statisticView;
        private Views.AboutView aboutView;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem menuToolsTopology;

    }
}

