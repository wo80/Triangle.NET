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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
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
            this.btnSmooth = new MeshExplorer.Controls.DarkButton();
            this.flatTabControl1 = new MeshExplorer.Controls.DarkTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lbMaxArea = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbMinAngle = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.slMaxArea = new MeshExplorer.Controls.DarkSlider();
            this.slMinAngle = new MeshExplorer.Controls.DarkSlider();
            this.cbConformDel = new MeshExplorer.Controls.DarkCheckBox();
            this.cbConvex = new MeshExplorer.Controls.DarkCheckBox();
            this.cbQuality = new MeshExplorer.Controls.DarkCheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label20 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbNumSeg = new System.Windows.Forms.Label();
            this.lbNumSeg2 = new System.Windows.Forms.Label();
            this.lbNumTri = new System.Windows.Forms.Label();
            this.lbNumTri2 = new System.Windows.Forms.Label();
            this.lbNumVert = new System.Windows.Forms.Label();
            this.lbNumVert2 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lbAngleMax = new System.Windows.Forms.Label();
            this.lbQualAspectAve = new System.Windows.Forms.Label();
            this.lbEdgeMax = new System.Windows.Forms.Label();
            this.lbQualAlphaAve = new System.Windows.Forms.Label();
            this.lbAreaMax = new System.Windows.Forms.Label();
            this.lbAngleMin = new System.Windows.Forms.Label();
            this.lbQualAspectMin = new System.Windows.Forms.Label();
            this.lbEdgeMin = new System.Windows.Forms.Label();
            this.lbQualAlphaMin = new System.Windows.Forms.Label();
            this.lbAreaMin = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.angleHistogram1 = new MeshExplorer.Controls.AngleHistogram();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lbCodeplex = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbShortcuts = new System.Windows.Forms.Label();
            this.btnMesh = new MeshExplorer.Controls.DarkButton();
            this.renderControl1 = new MeshExplorer.Controls.RendererControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.flatTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.splitContainer1.Panel1.Controls.Add(this.btnSmooth);
            this.splitContainer1.Panel1.Controls.Add(this.flatTabControl1);
            this.splitContainer1.Panel1.Controls.Add(this.menuStrip1);
            this.splitContainer1.Panel1.Controls.Add(this.btnMesh);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Black;
            this.splitContainer1.Panel2.Controls.Add(this.renderControl1);
            this.splitContainer1.Size = new System.Drawing.Size(984, 612);
            this.splitContainer1.SplitterDistance = 280;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
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
            this.menuToolsCheck});
            this.menuTools.Name = "menuTools";
            this.menuTools.Size = new System.Drawing.Size(46, 24);
            this.menuTools.Text = "Tools";
            // 
            // menuToolsGen
            // 
            this.menuToolsGen.Name = "menuToolsGen";
            this.menuToolsGen.Size = new System.Drawing.Size(157, 22);
            this.menuToolsGen.Text = "Input Generator";
            this.menuToolsGen.Click += new System.EventHandler(this.menuToolsGenerator_Click);
            // 
            // menuToolsCheck
            // 
            this.menuToolsCheck.Enabled = false;
            this.menuToolsCheck.Name = "menuToolsCheck";
            this.menuToolsCheck.Size = new System.Drawing.Size(157, 22);
            this.menuToolsCheck.Text = "Check Mesh";
            this.menuToolsCheck.Click += new System.EventHandler(this.menuToolsCheck_Click);
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
            this.tabPage1.Controls.Add(this.lbMaxArea);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.lbMinAngle);
            this.tabPage1.Controls.Add(this.label23);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.slMaxArea);
            this.tabPage1.Controls.Add(this.slMinAngle);
            this.tabPage1.Controls.Add(this.cbConformDel);
            this.tabPage1.Controls.Add(this.cbConvex);
            this.tabPage1.Controls.Add(this.cbQuality);
            this.tabPage1.ForeColor = System.Drawing.Color.DarkGray;
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(272, 509);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Mesh Control";
            // 
            // lbMaxArea
            // 
            this.lbMaxArea.AutoSize = true;
            this.lbMaxArea.ForeColor = System.Drawing.Color.White;
            this.lbMaxArea.Location = new System.Drawing.Point(227, 66);
            this.lbMaxArea.Name = "lbMaxArea";
            this.lbMaxArea.Size = new System.Drawing.Size(13, 13);
            this.lbMaxArea.TabIndex = 14;
            this.lbMaxArea.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(8, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Maximum area";
            // 
            // lbMinAngle
            // 
            this.lbMinAngle.AutoSize = true;
            this.lbMinAngle.ForeColor = System.Drawing.Color.White;
            this.lbMinAngle.Location = new System.Drawing.Point(227, 44);
            this.lbMinAngle.Name = "lbMinAngle";
            this.lbMinAngle.Size = new System.Drawing.Size(19, 13);
            this.lbMinAngle.TabIndex = 14;
            this.lbMinAngle.Text = "20";
            // 
            // label23
            // 
            this.label23.BackColor = System.Drawing.Color.DimGray;
            this.label23.ForeColor = System.Drawing.Color.DarkGray;
            this.label23.Location = new System.Drawing.Point(8, 152);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(251, 33);
            this.label23.TabIndex = 14;
            this.label23.Text = "Ensure that all triangles in the mesh are truly Delaunay, and not just constraine" +
    "d Delaunay.";
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.DimGray;
            this.label9.ForeColor = System.Drawing.Color.DarkGray;
            this.label9.Location = new System.Drawing.Point(8, 216);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(258, 33);
            this.label9.TabIndex = 14;
            this.label9.Text = "Use the convex mesh option, if the convex hull should be included in the output.";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.DimGray;
            this.label8.ForeColor = System.Drawing.Color.DarkGray;
            this.label8.Location = new System.Drawing.Point(8, 89);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(258, 33);
            this.label8.TabIndex = 14;
            this.label8.Text = "Hint: maximum area values of 0 or 1 will be irgnored (no area constraints are set" +
    ").";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(8, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Minimum angle";
            // 
            // slMaxArea
            // 
            this.slMaxArea.BackColor = System.Drawing.Color.Transparent;
            this.slMaxArea.CriticalPercent = ((uint)(0u));
            this.slMaxArea.Location = new System.Drawing.Point(102, 62);
            this.slMaxArea.Maximum = 100;
            this.slMaxArea.Minimum = 0;
            this.slMaxArea.Name = "slMaxArea";
            this.slMaxArea.Size = new System.Drawing.Size(119, 18);
            this.slMaxArea.TabIndex = 13;
            this.slMaxArea.Text = "darkSlider1";
            this.slMaxArea.Value = 0;
            this.slMaxArea.ValueChanging += new System.EventHandler(this.slMaxArea_ValueChanging);
            // 
            // slMinAngle
            // 
            this.slMinAngle.BackColor = System.Drawing.Color.Transparent;
            this.slMinAngle.CriticalPercent = ((uint)(89u));
            this.slMinAngle.Location = new System.Drawing.Point(102, 40);
            this.slMinAngle.Maximum = 100;
            this.slMinAngle.Minimum = 0;
            this.slMinAngle.Name = "slMinAngle";
            this.slMinAngle.Size = new System.Drawing.Size(119, 18);
            this.slMinAngle.TabIndex = 13;
            this.slMinAngle.Text = "darkSlider1";
            this.slMinAngle.Value = 50;
            this.slMinAngle.ValueChanging += new System.EventHandler(this.slMinAngle_ValueChanging);
            // 
            // cbConformDel
            // 
            this.cbConformDel.BackColor = System.Drawing.Color.DimGray;
            this.cbConformDel.Checked = false;
            this.cbConformDel.Location = new System.Drawing.Point(11, 132);
            this.cbConformDel.Name = "cbConformDel";
            this.cbConformDel.Size = new System.Drawing.Size(142, 17);
            this.cbConformDel.TabIndex = 0;
            this.cbConformDel.Text = "Conforming Delaunay";
            this.cbConformDel.UseVisualStyleBackColor = false;
            // 
            // cbConvex
            // 
            this.cbConvex.BackColor = System.Drawing.Color.DimGray;
            this.cbConvex.Checked = false;
            this.cbConvex.Location = new System.Drawing.Point(11, 196);
            this.cbConvex.Name = "cbConvex";
            this.cbConvex.Size = new System.Drawing.Size(115, 17);
            this.cbConvex.TabIndex = 0;
            this.cbConvex.Text = "Convex mesh";
            this.cbConvex.UseVisualStyleBackColor = false;
            // 
            // cbQuality
            // 
            this.cbQuality.BackColor = System.Drawing.Color.DimGray;
            this.cbQuality.Checked = false;
            this.cbQuality.Location = new System.Drawing.Point(11, 17);
            this.cbQuality.Name = "cbQuality";
            this.cbQuality.Size = new System.Drawing.Size(115, 17);
            this.cbQuality.TabIndex = 0;
            this.cbQuality.Text = "Quality mesh";
            this.cbQuality.UseVisualStyleBackColor = false;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.DimGray;
            this.tabPage2.Controls.Add(this.label20);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.lbNumSeg);
            this.tabPage2.Controls.Add(this.lbNumSeg2);
            this.tabPage2.Controls.Add(this.lbNumTri);
            this.tabPage2.Controls.Add(this.lbNumTri2);
            this.tabPage2.Controls.Add(this.lbNumVert);
            this.tabPage2.Controls.Add(this.lbNumVert2);
            this.tabPage2.Controls.Add(this.label32);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.label31);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.label16);
            this.tabPage2.Controls.Add(this.label29);
            this.tabPage2.Controls.Add(this.label14);
            this.tabPage2.Controls.Add(this.lbAngleMax);
            this.tabPage2.Controls.Add(this.lbQualAspectAve);
            this.tabPage2.Controls.Add(this.lbEdgeMax);
            this.tabPage2.Controls.Add(this.lbQualAlphaAve);
            this.tabPage2.Controls.Add(this.lbAreaMax);
            this.tabPage2.Controls.Add(this.lbAngleMin);
            this.tabPage2.Controls.Add(this.lbQualAspectMin);
            this.tabPage2.Controls.Add(this.lbEdgeMin);
            this.tabPage2.Controls.Add(this.lbQualAlphaMin);
            this.tabPage2.Controls.Add(this.lbAreaMin);
            this.tabPage2.Controls.Add(this.label22);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.label17);
            this.tabPage2.Controls.Add(this.label21);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.angleHistogram1);
            this.tabPage2.ForeColor = System.Drawing.Color.White;
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(272, 509);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Statistic";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.White;
            this.label20.Location = new System.Drawing.Point(8, 17);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(39, 13);
            this.label20.TabIndex = 26;
            this.label20.Text = "Mesh:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.DarkGray;
            this.label4.Location = new System.Drawing.Point(8, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Segments:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.DarkGray;
            this.label3.Location = new System.Drawing.Point(8, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Triangles:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.DarkGray;
            this.label2.Location = new System.Drawing.Point(8, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Vertices:";
            // 
            // lbNumSeg
            // 
            this.lbNumSeg.ForeColor = System.Drawing.Color.White;
            this.lbNumSeg.Location = new System.Drawing.Point(98, 57);
            this.lbNumSeg.Name = "lbNumSeg";
            this.lbNumSeg.Size = new System.Drawing.Size(70, 13);
            this.lbNumSeg.TabIndex = 20;
            this.lbNumSeg.Text = "-";
            this.lbNumSeg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbNumSeg2
            // 
            this.lbNumSeg2.ForeColor = System.Drawing.Color.DarkGray;
            this.lbNumSeg2.Location = new System.Drawing.Point(188, 57);
            this.lbNumSeg2.Name = "lbNumSeg2";
            this.lbNumSeg2.Size = new System.Drawing.Size(70, 13);
            this.lbNumSeg2.TabIndex = 19;
            this.lbNumSeg2.Text = "-";
            this.lbNumSeg2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbNumTri
            // 
            this.lbNumTri.ForeColor = System.Drawing.Color.White;
            this.lbNumTri.Location = new System.Drawing.Point(98, 76);
            this.lbNumTri.Name = "lbNumTri";
            this.lbNumTri.Size = new System.Drawing.Size(70, 13);
            this.lbNumTri.TabIndex = 17;
            this.lbNumTri.Text = "-";
            this.lbNumTri.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbNumTri2
            // 
            this.lbNumTri2.ForeColor = System.Drawing.Color.DarkGray;
            this.lbNumTri2.Location = new System.Drawing.Point(188, 76);
            this.lbNumTri2.Name = "lbNumTri2";
            this.lbNumTri2.Size = new System.Drawing.Size(70, 13);
            this.lbNumTri2.TabIndex = 18;
            this.lbNumTri2.Text = "-";
            this.lbNumTri2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbNumVert
            // 
            this.lbNumVert.ForeColor = System.Drawing.Color.White;
            this.lbNumVert.Location = new System.Drawing.Point(98, 38);
            this.lbNumVert.Name = "lbNumVert";
            this.lbNumVert.Size = new System.Drawing.Size(70, 13);
            this.lbNumVert.TabIndex = 21;
            this.lbNumVert.Text = "-";
            this.lbNumVert.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbNumVert2
            // 
            this.lbNumVert2.ForeColor = System.Drawing.Color.DarkGray;
            this.lbNumVert2.Location = new System.Drawing.Point(188, 38);
            this.lbNumVert2.Name = "lbNumVert2";
            this.lbNumVert2.Size = new System.Drawing.Size(70, 13);
            this.lbNumVert2.TabIndex = 22;
            this.lbNumVert2.Text = "-";
            this.lbNumVert2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.ForeColor = System.Drawing.Color.DarkGray;
            this.label32.Location = new System.Drawing.Point(210, 213);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(48, 13);
            this.label32.TabIndex = 11;
            this.label32.Text = "Average";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.DarkGray;
            this.label13.Location = new System.Drawing.Point(202, 114);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 13);
            this.label13.TabIndex = 11;
            this.label13.Text = "Maximum";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.ForeColor = System.Drawing.Color.DarkGray;
            this.label31.Location = new System.Drawing.Point(112, 213);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(55, 13);
            this.label31.TabIndex = 12;
            this.label31.Text = "Minimum";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.DarkGray;
            this.label12.Location = new System.Drawing.Point(113, 114);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(55, 13);
            this.label12.TabIndex = 12;
            this.label12.Text = "Minimum";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.DarkGray;
            this.label16.Location = new System.Drawing.Point(8, 173);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(40, 13);
            this.label16.TabIndex = 9;
            this.label16.Text = "Angle:";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.ForeColor = System.Drawing.Color.DarkGray;
            this.label29.Location = new System.Drawing.Point(8, 253);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(71, 13);
            this.label29.TabIndex = 15;
            this.label29.Text = "Aspect ratio:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.DarkGray;
            this.label14.Location = new System.Drawing.Point(8, 154);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(73, 13);
            this.label14.TabIndex = 15;
            this.label14.Text = "Edge length:";
            // 
            // lbAngleMax
            // 
            this.lbAngleMax.ForeColor = System.Drawing.Color.White;
            this.lbAngleMax.Location = new System.Drawing.Point(182, 173);
            this.lbAngleMax.Name = "lbAngleMax";
            this.lbAngleMax.Size = new System.Drawing.Size(76, 13);
            this.lbAngleMax.TabIndex = 16;
            this.lbAngleMax.Text = "-";
            this.lbAngleMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbQualAspectAve
            // 
            this.lbQualAspectAve.ForeColor = System.Drawing.Color.White;
            this.lbQualAspectAve.Location = new System.Drawing.Point(182, 253);
            this.lbQualAspectAve.Name = "lbQualAspectAve";
            this.lbQualAspectAve.Size = new System.Drawing.Size(76, 13);
            this.lbQualAspectAve.TabIndex = 14;
            this.lbQualAspectAve.Text = "-";
            this.lbQualAspectAve.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbEdgeMax
            // 
            this.lbEdgeMax.ForeColor = System.Drawing.Color.White;
            this.lbEdgeMax.Location = new System.Drawing.Point(182, 154);
            this.lbEdgeMax.Name = "lbEdgeMax";
            this.lbEdgeMax.Size = new System.Drawing.Size(76, 13);
            this.lbEdgeMax.TabIndex = 14;
            this.lbEdgeMax.Text = "-";
            this.lbEdgeMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbQualAlphaAve
            // 
            this.lbQualAlphaAve.ForeColor = System.Drawing.Color.White;
            this.lbQualAlphaAve.Location = new System.Drawing.Point(182, 234);
            this.lbQualAlphaAve.Name = "lbQualAlphaAve";
            this.lbQualAlphaAve.Size = new System.Drawing.Size(76, 13);
            this.lbQualAlphaAve.TabIndex = 3;
            this.lbQualAlphaAve.Text = "-";
            this.lbQualAlphaAve.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbAreaMax
            // 
            this.lbAreaMax.ForeColor = System.Drawing.Color.White;
            this.lbAreaMax.Location = new System.Drawing.Point(182, 135);
            this.lbAreaMax.Name = "lbAreaMax";
            this.lbAreaMax.Size = new System.Drawing.Size(76, 13);
            this.lbAreaMax.TabIndex = 3;
            this.lbAreaMax.Text = "-";
            this.lbAreaMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbAngleMin
            // 
            this.lbAngleMin.ForeColor = System.Drawing.Color.White;
            this.lbAngleMin.Location = new System.Drawing.Point(100, 173);
            this.lbAngleMin.Name = "lbAngleMin";
            this.lbAngleMin.Size = new System.Drawing.Size(68, 13);
            this.lbAngleMin.TabIndex = 4;
            this.lbAngleMin.Text = "-";
            this.lbAngleMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbQualAspectMin
            // 
            this.lbQualAspectMin.ForeColor = System.Drawing.Color.White;
            this.lbQualAspectMin.Location = new System.Drawing.Point(100, 253);
            this.lbQualAspectMin.Name = "lbQualAspectMin";
            this.lbQualAspectMin.Size = new System.Drawing.Size(68, 13);
            this.lbQualAspectMin.TabIndex = 2;
            this.lbQualAspectMin.Text = "-";
            this.lbQualAspectMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbEdgeMin
            // 
            this.lbEdgeMin.ForeColor = System.Drawing.Color.White;
            this.lbEdgeMin.Location = new System.Drawing.Point(100, 154);
            this.lbEdgeMin.Name = "lbEdgeMin";
            this.lbEdgeMin.Size = new System.Drawing.Size(68, 13);
            this.lbEdgeMin.TabIndex = 2;
            this.lbEdgeMin.Text = "-";
            this.lbEdgeMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbQualAlphaMin
            // 
            this.lbQualAlphaMin.ForeColor = System.Drawing.Color.White;
            this.lbQualAlphaMin.Location = new System.Drawing.Point(100, 234);
            this.lbQualAlphaMin.Name = "lbQualAlphaMin";
            this.lbQualAlphaMin.Size = new System.Drawing.Size(68, 13);
            this.lbQualAlphaMin.TabIndex = 7;
            this.lbQualAlphaMin.Text = "-";
            this.lbQualAlphaMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbAreaMin
            // 
            this.lbAreaMin.ForeColor = System.Drawing.Color.White;
            this.lbAreaMin.Location = new System.Drawing.Point(100, 135);
            this.lbAreaMin.Name = "lbAreaMin";
            this.lbAreaMin.Size = new System.Drawing.Size(68, 13);
            this.lbAreaMin.TabIndex = 7;
            this.lbAreaMin.Text = "-";
            this.lbAreaMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.ForeColor = System.Drawing.Color.DarkGray;
            this.label22.Location = new System.Drawing.Point(8, 234);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(65, 13);
            this.label22.TabIndex = 8;
            this.label22.Text = "Min. angle:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.DarkGray;
            this.label10.Location = new System.Drawing.Point(8, 135);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "Triangle area:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(8, 290);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(97, 13);
            this.label17.TabIndex = 5;
            this.label17.Text = "Angle histogram:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(8, 213);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(47, 13);
            this.label21.TabIndex = 6;
            this.label21.Text = "Quality:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(8, 114);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(50, 13);
            this.label11.TabIndex = 6;
            this.label11.Text = "Statistic:";
            // 
            // angleHistogram1
            // 
            this.angleHistogram1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.angleHistogram1.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.angleHistogram1.Location = new System.Drawing.Point(6, 308);
            this.angleHistogram1.Name = "angleHistogram1";
            this.angleHistogram1.Size = new System.Drawing.Size(260, 195);
            this.angleHistogram1.TabIndex = 0;
            this.angleHistogram1.Text = "angleHistogram1";
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.DimGray;
            this.tabPage3.Controls.Add(this.lbCodeplex);
            this.tabPage3.Controls.Add(this.label15);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.label19);
            this.tabPage3.Controls.Add(this.label18);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.lbShortcuts);
            this.tabPage3.Location = new System.Drawing.Point(4, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(272, 509);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "About";
            // 
            // lbCodeplex
            // 
            this.lbCodeplex.AutoSize = true;
            this.lbCodeplex.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCodeplex.ForeColor = System.Drawing.Color.White;
            this.lbCodeplex.Location = new System.Drawing.Point(70, 82);
            this.lbCodeplex.Name = "lbCodeplex";
            this.lbCodeplex.Size = new System.Drawing.Size(153, 13);
            this.lbCodeplex.TabIndex = 2;
            this.lbCodeplex.Text = "http://triangle.codeplex.com";
            this.lbCodeplex.Click += new System.EventHandler(this.lbCodeplex_Clicked);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(8, 17);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(73, 13);
            this.label15.TabIndex = 1;
            this.label15.Text = "Triangle.NET";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(8, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Keyboard shortcuts";
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.White;
            this.label19.Location = new System.Drawing.Point(70, 42);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(134, 40);
            this.label19.TabIndex = 0;
            this.label19.Text = "Beta 2 (2012-06-11)\r\nChristian Woltering\r\nMIT";
            // 
            // label18
            // 
            this.label18.ForeColor = System.Drawing.Color.White;
            this.label18.Location = new System.Drawing.Point(13, 42);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(51, 40);
            this.label18.TabIndex = 0;
            this.label18.Text = "Version:\r\nAuthor:\r\nLicense:";
            this.label18.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(70, 155);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(134, 108);
            this.label7.TabIndex = 0;
            this.label7.Text = "File Open\r\nFile Save\r\nReload Input\r\n\r\nTriangulate / Refine\r\nSmooth\r\n\r\nShow Log";
            // 
            // lbShortcuts
            // 
            this.lbShortcuts.ForeColor = System.Drawing.Color.White;
            this.lbShortcuts.Location = new System.Drawing.Point(22, 155);
            this.lbShortcuts.Name = "lbShortcuts";
            this.lbShortcuts.Size = new System.Drawing.Size(36, 108);
            this.lbShortcuts.TabIndex = 0;
            this.lbShortcuts.Text = "F3\r\nF4\r\nF5\r\n\r\nF8\r\nF9\r\n\r\nF12";
            this.lbShortcuts.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            // renderControl1
            // 
            this.renderControl1.BackColor = System.Drawing.Color.Black;
            this.renderControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.renderControl1.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.renderControl1.Location = new System.Drawing.Point(0, 0);
            this.renderControl1.Name = "renderControl1";
            this.renderControl1.ShowVoronoi = false;
            this.renderControl1.Size = new System.Drawing.Size(703, 612);
            this.renderControl1.TabIndex = 0;
            this.renderControl1.Text = "meshRenderer1";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.ClientSize = new System.Drawing.Size(984, 612);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1000, 650);
            this.Name = "FormMain";
            this.Text = "Triangle.NET - Mesh Explorer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResizeBegin += new System.EventHandler(this.ResizeBeginHandler);
            this.ResizeEnd += new System.EventHandler(this.ResizeEndHandler);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.Resize += new System.EventHandler(this.ResizeHandler);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.flatTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Controls.RendererControl renderControl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuFileOpen;
        private System.Windows.Forms.ToolStripMenuItem menuFileSave;
        private Controls.DarkTabControl flatTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Controls.DarkButton btnSmooth;
        private Controls.DarkButton btnMesh;
        private Controls.DarkCheckBox cbQuality;
        private Controls.AngleHistogram angleHistogram1;
        private Controls.DarkSlider slMinAngle;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label lbShortcuts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private Controls.DarkCheckBox cbConvex;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private Controls.DarkSlider slMaxArea;
        private System.Windows.Forms.Label lbMaxArea;
        private System.Windows.Forms.Label lbMinAngle;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lbAngleMax;
        private System.Windows.Forms.Label lbEdgeMax;
        private System.Windows.Forms.Label lbAreaMax;
        private System.Windows.Forms.Label lbAngleMin;
        private System.Windows.Forms.Label lbEdgeMin;
        private System.Windows.Forms.Label lbAreaMin;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ToolStripMenuItem menuView;
        private System.Windows.Forms.ToolStripMenuItem menuViewVoronoi;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuViewLog;
        private System.Windows.Forms.ToolStripMenuItem menuTools;
        private System.Windows.Forms.ToolStripMenuItem menuToolsGen;
        private System.Windows.Forms.ToolStripMenuItem menuToolsCheck;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuFileQuit;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lbCodeplex;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuFileExport;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label lbQualAspectAve;
        private System.Windows.Forms.Label lbQualAlphaAve;
        private System.Windows.Forms.Label lbQualAspectMin;
        private System.Windows.Forms.Label lbQualAlphaMin;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbNumSeg;
        private System.Windows.Forms.Label lbNumSeg2;
        private System.Windows.Forms.Label lbNumTri;
        private System.Windows.Forms.Label lbNumTri2;
        private System.Windows.Forms.Label lbNumVert;
        private System.Windows.Forms.Label lbNumVert2;
        private System.Windows.Forms.Label label23;
        private Controls.DarkCheckBox cbConformDel;

    }
}

