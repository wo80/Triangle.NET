using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MeshExplorer.Controls;
using MeshExplorer.IO;
using TriangleNet;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using TriangleNet.Meshing.Algorithm;
using TriangleNet.Rendering;
using TriangleNet.Smoothing;
using TriangleNet.Voronoi;

namespace MeshExplorer
{
    public partial class FormMain : Form
    {
        Settings settings;

        Mesh mesh;
        IPolygon input;
        VoronoiBase voronoi;

        FormLog frmLog;
        FormGenerator frmGenerator;

        RenderManager renderManager;

        public FormMain()
        {
            InitializeComponent();

            ToolStripManager.Renderer = new DarkToolStripRenderer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            oldClientSize = this.ClientSize;

            settings = new Settings();

            renderManager = new RenderManager();

            IRenderControl control = new TriangleNet.Rendering.GDI.RenderControl();

            /*
            if (!renderManager.TryCreateControl("Triangle.Rendering.SharpGL.dll",
                new string[] { "SharpGL.dll" }, out control))
            {
                control = new TriangleNet.Rendering.GDI.RenderControl();

                if (frmLog == null)
                {
                    frmLog = new FormLog();
                }

                frmLog.AddItem("Failed to initialize OpenGL.", true);
            }
            //*/

            if (control != null)
            {
                InitializeRenderControl((Control)control);
                renderManager.Initialize(control);
            }
            else
            {
                DarkMessageBox.Show("Ooops ...", "Failed to initialize renderer.");
            }
        }

        private void InitializeRenderControl(Control control)
        {
            this.splitContainer.SuspendLayout();
            this.splitContainer.Panel2.Controls.Add(control);

            var size = this.splitContainer.Panel2.ClientRectangle;

            // Initialize control
            control.BackColor = Color.Black;
            control.Dock = DockStyle.Fill;
            control.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            control.Location = new System.Drawing.Point(0, 0);
            control.Name = "renderControl1";
            control.Size = new Size(size.Width, size.Height);
            control.TabIndex = 0;
            control.Text = "renderControl1";

            this.splitContainer.ResumeLayout();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (splitContainer.Panel2.Bounds.Contains(e.Location))
            {
                var control = renderManager.Control as Control;

                // Set focus on the render control.
                if (control != null && !control.Focused)
                {
                    control.Focus();
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F3:
                    OpenWithDialog();
                    break;
                case Keys.F4:
                    Save();
                    break;
                case Keys.F5:
                    Reload();
                    break;
                case Keys.F8:
                    TriangulateOrRefine();
                    break;
                case Keys.F9:
                    Smooth();
                    break;
                case Keys.F12:
                    ShowLog();
                    break;
            }
        }

        void frmGenerator_InputGenerated(object sender, EventArgs e)
        {
            this.input = sender as IPolygon;

            if (input != null)
            {
                settings.CurrentFile = "tmp-" + DateTime.Now.ToString("HH-mm-ss");
                HandleNewInput();
            }
        }

        private void btnMesh_Click(object sender, EventArgs e)
        {
            TriangulateOrRefine();
        }

        private void btnSmooth_Click(object sender, EventArgs e)
        {
            Smooth();
        }

        #region Drag and drop

        private void frmDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] args = (string[])e.Data.GetData(DataFormats.FileDrop);

                Open(args[0]);
            }
        }

        private void frmDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] args = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (args.Length > 1)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                string file = args[0].ToLower();

                // Check if file extension is known
                if (FileProcessor.CanHandleFile(file))
                {
                    e.Effect = DragDropEffects.Copy;
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        #endregion

        #region Resize event handler

        bool isResizing = false;
        Size oldClientSize;

        private void ResizeHandler(object sender, EventArgs e)
        {
            // Handle window minimize and maximize
            if (!isResizing)
            {
                renderManager.Resize();
            }
        }

        private void ResizeEndHandler(object sender, EventArgs e)
        {
            isResizing = false;

            if (this.ClientSize != this.oldClientSize)
            {
                this.oldClientSize = this.ClientSize;
                renderManager.Resize();
            }
        }

        private void ResizeBeginHandler(object sender, EventArgs e)
        {
            isResizing = true;
        }

        #endregion

        #region State changes

        private void LockOnException()
        {
            btnMesh.Enabled = false;
            btnSmooth.Enabled = false;

            //menuFileSave.Enabled = false;
            //menuFileExport.Enabled = false;
            menuViewVoronoi.Enabled = false;
            menuToolsCheck.Enabled = false;
            menuToolsRcm.Enabled = false;

            settings.ExceptionThrown = true;
        }

        private void HandleNewInput()
        {
            // Reset mesh
            mesh = null;
            voronoi = null;

            // Reset state
            settings.RefineMode = false;
            settings.ExceptionThrown = false;

            // Reset buttons
            btnMesh.Enabled = true;
            btnMesh.Text = "Triangulate";
            btnSmooth.Enabled = false;

            // Update Statistic view
            statisticView.HandleNewInput(input);

            // Clear voronoi
            menuViewVoronoi.Checked = false;

            // Disable menu items
            menuFileSave.Enabled = false;
            menuFileExport.Enabled = false;
            menuViewVoronoi.Enabled = false;
            menuToolsCheck.Enabled = false;
            menuToolsRcm.Enabled = false;

            // Render input
            renderManager.Set(input);

            // Update window caption
            this.Text = "Triangle.NET - Mesh Explorer - " + settings.CurrentFile;
        }

        private void HandleMeshImport()
        {
            voronoi = null;

            // Render mesh
            renderManager.Set(mesh, true);

            // Update window caption
            this.Text = "Triangle.NET - Mesh Explorer - " + settings.CurrentFile;

            // Update Statistic view
            statisticView.HandleMeshImport(input, mesh);

            // Set refine mode
            btnMesh.Enabled = true;
            btnMesh.Text = "Refine";

            settings.RefineMode = true;

            HandleMeshChange();
        }

        private void HandleMeshUpdate()
        {
            // Render mesh
            renderManager.Set(mesh, false);

            // Update Statistic view
            statisticView.HandleMeshUpdate(mesh);

            HandleMeshChange();
        }

        private void HandleMeshChange()
        {
            // Update Statistic view
            statisticView.HandleMeshChange(mesh);

            // TODO: Should the Voronoi diagram automatically update?
            voronoi = null;
            menuViewVoronoi.Checked = false;

            // Enable menu items
            menuFileSave.Enabled = true;
            menuFileExport.Enabled = true;
            menuViewVoronoi.Enabled = true;
            menuToolsCheck.Enabled = true;
            menuToolsRcm.Enabled = true;
        }

        #endregion

        #region Commands

        private void OpenWithDialog()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = settings.OfdFilter;
            ofd.FilterIndex = settings.OfdFilterIndex;
            ofd.InitialDirectory = settings.OfdDirectory;
            ofd.FileName = "";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (Open(ofd.FileName))
                {
                    // Update folder settings
                    settings.OfdFilterIndex = ofd.FilterIndex;
                    settings.OfdDirectory = Path.GetDirectoryName(ofd.FileName);
                }
            }
        }

        private bool Open(string filename)
        {
            if (!FileProcessor.CanHandleFile(filename))
            {
                // TODO: show message.
            }
            else
            {
                if (FileProcessor.ContainsMeshData(filename))
                {
                    if (filename.EndsWith(".ele") || DarkMessageBox.Show("Import mesh", Settings.ImportString,
                        "Do you want to import the mesh?", MessageBoxButtons.YesNo) == DialogResult.OK)
                    {
                        input = null;

                        try
                        {
                            mesh = FileProcessor.Import(filename);
                        }
                        catch (Exception e)
                        {
                            DarkMessageBox.Show("Import mesh error", e.Message, MessageBoxButtons.OK);
                            return false;
                        }

                        if (mesh != null)
                        {
                            statisticView.UpdateStatistic(mesh);

                            // Update settings
                            settings.CurrentFile = Path.GetFileName(filename);

                            HandleMeshImport();
                            btnSmooth.Enabled = true; // TODO: Remove
                        }
                        // else Message

                        return true;
                    }
                }

                input = FileProcessor.Read(filename);
            }

            if (input != null)
            {
                // Update settings
                settings.CurrentFile = Path.GetFileName(filename);

                HandleNewInput();
            }
            // else Message

            return true;
        }

        private void Save()
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = settings.SfdFilter;
            sfd.FilterIndex = settings.SfdFilterIndex;
            sfd.InitialDirectory = settings.SfdDirectory;
            sfd.FileName = "";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileProcessor.Save(sfd.FileName, mesh);
            }
        }

        private void Reload()
        {
            if (input != null)
            {
                mesh = null;
                settings.RefineMode = false;
                settings.ExceptionThrown = false;

                HandleNewInput();
            }
        }

        private void TriangulateOrRefine()
        {
            if ((input == null && !settings.RefineMode) || settings.ExceptionThrown)
            {
                return;
            }

            if (settings.RefineMode == false)
            {
                Triangulate();

                if (meshControlView.ParamQualityChecked)
                {
                    btnMesh.Text = "Refine";
                    btnSmooth.Enabled = mesh.IsPolygon;
                }
            }
            else if (meshControlView.ParamQualityChecked)
            {
                Refine();
            }
        }

        private void Triangulate()
        {
            if (input == null) return;

            var options = new ConstraintOptions();
            var quality = new QualityOptions();

            if (meshControlView.ParamConformDelChecked)
            {
                options.ConformingDelaunay = true;
            }

            if (meshControlView.ParamQualityChecked)
            {
                quality.MinimumAngle = meshControlView.ParamMinAngleValue;

                double maxAngle = meshControlView.ParamMaxAngleValue;

                if (maxAngle < 180)
                {
                    quality.MaximumAngle = maxAngle;
                }

                // Ignore area constraints on initial triangulation.

                //double area = slMaxArea.Value * 0.01;
                //if (area > 0 && area < 1)
                //{
                //    var size = input.Bounds;
                //    double min = Math.Min(size.Width, size.Height);
                //    mesh.SetOption(Options.MaxArea, area * min);
                //}
            }

            if (meshControlView.ParamConvexChecked)
            {
                options.Convex = true;
            }

            try
            {
                if (meshControlView.ParamSweeplineChecked)
                {
                    mesh = (Mesh)input.Triangulate(options, quality, new SweepLine());
                }
                else
                {
                    mesh = (Mesh)input.Triangulate(options, quality);
                }

                statisticView.UpdateStatistic(mesh);

                HandleMeshUpdate();

                if (meshControlView.ParamQualityChecked)
                {
                    settings.RefineMode = true;
                }
            }
            catch (Exception ex)
            {
                LockOnException();
                DarkMessageBox.Show("Exception - Triangulate", ex.Message, MessageBoxButtons.OK);
            }

            UpdateLog();
        }

        private void Refine()
        {
            if (mesh == null) return;

            double area = meshControlView.ParamMaxAreaValue;

            var quality = new QualityOptions();

            if (area > 0 && area < 1)
            {
                quality.MaximumArea = area * statisticView.Statistic.LargestArea;
            }

            quality.MinimumAngle = meshControlView.ParamMinAngleValue;

            double maxAngle = meshControlView.ParamMaxAngleValue;

            if (maxAngle < 180)
            {
                quality.MaximumAngle = maxAngle;
            }

            try
            {
                mesh.Refine(quality, meshControlView.ParamConformDelChecked);

                statisticView.UpdateStatistic(mesh);

                HandleMeshUpdate();
            }
            catch (Exception ex)
            {
                LockOnException();
                DarkMessageBox.Show("Exception - Refine", ex.Message, MessageBoxButtons.OK);
            }

            UpdateLog();
        }

        private void Renumber()
        {
            if (mesh == null || settings.ExceptionThrown) return;

            bool tmp = Log.Verbose;
            Log.Verbose = true;

            mesh.Renumber(NodeNumbering.CuthillMcKee);
            ShowLog();

            Log.Verbose = tmp;
        }

        private void Smooth()
        {
            if (mesh == null || settings.ExceptionThrown) return;

            if (!mesh.IsPolygon)
            {
                return;
            }

            var smoother = new SimpleSmoother();

            try
            {
                smoother.Smooth(this.mesh);

                statisticView.UpdateStatistic(mesh);

                HandleMeshUpdate();
            }
            catch (Exception ex)
            {
                LockOnException();
                DarkMessageBox.Show("Exception - Smooth", ex.Message, MessageBoxButtons.OK);
            }

            UpdateLog();
        }

        private bool CreateVoronoi()
        {
            if (mesh == null)
            {
                return false;
            }

            if (mesh.IsPolygon)
            {
                try
                {
                    this.voronoi = new BoundedVoronoi(mesh);
                }
                catch (Exception ex)
                {
                    if (!meshControlView.ParamConformDelChecked)
                    {
                        DarkMessageBox.Show("Exception - Bounded Voronoi", Settings.VoronoiString, MessageBoxButtons.OK);
                    }
                    else
                    {
                        DarkMessageBox.Show("Exception - Bounded Voronoi", ex.Message, MessageBoxButtons.OK);
                    }

                    return false;
                }
            }
            else
            {
                this.voronoi = new StandardVoronoi(mesh);
            }

            // HACK: List<Vertex> -> ICollection<Point> ? Nope, no way.
            //           Vertex[] -> ICollection<Point> ? Well, ok.
            renderManager.Set(voronoi.Vertices.ToArray(), voronoi.Edges, false);

            return true;
        }

        private void ShowLog()
        {
            if (frmLog == null)
            {
                frmLog = new FormLog();
            }

            UpdateLog();

            if (!frmLog.Visible)
            {
                frmLog.Show(this);
            }
        }

        private void UpdateLog()
        {
            if (frmLog != null)
            {
                frmLog.UpdateItems();
            }
        }

        #endregion

        #region Menu Handler

        private void menuFileOpen_Click(object sender, EventArgs e)
        {
            OpenWithDialog();
        }

        private void menuFileSave_Click(object sender, EventArgs ev)
        {
            if (mesh != null)
            {
                Save();
            }
        }

        private void menuFileExport_Click(object sender, EventArgs e)
        {
            if (mesh != null)
            {
                FormExport export = new FormExport();

                string file = settings.OfdDirectory;

                if (!file.EndsWith("\\"))
                {
                    file += "\\";
                }

                file += settings.CurrentFile;

                export.ImageName = Path.ChangeExtension(file, ".png");

                if (export.ShowDialog() == DialogResult.OK)
                {
                    int format = export.ImageFormat;
                    int size = export.ImageSize;
                    bool compress = export.UseCompression;

                    var writer = new ImageWriter();

                    writer.Export(this.mesh, export.ImageName, format, size, compress);
                }
            }
        }

        private void menuFileQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuViewVoronoi_Click(object sender, EventArgs e)
        {
            if (this.voronoi == null)
            {
                menuViewVoronoi.Checked = CreateVoronoi();
            }
            else
            {
                bool visible = menuViewVoronoi.Checked;

                renderManager.Enable(4, !visible);
                menuViewVoronoi.Checked = !visible;
            }
        }

        private void menuViewLog_Click(object sender, EventArgs e)
        {
            ShowLog();
        }

        private void menuToolsGenerator_Click(object sender, EventArgs e)
        {
            if (frmGenerator == null || frmGenerator.IsDisposed)
            {
                frmGenerator = new FormGenerator();
                frmGenerator.InputGenerated += new EventHandler(frmGenerator_InputGenerated);
            }

            if (!frmGenerator.Visible)
            {
                frmGenerator.Show();
            }
            else
            {
                frmGenerator.Activate();
            }
        }

        private void menuToolsCheck_Click(object sender, EventArgs e)
        {
            if (mesh != null)
            {
                bool save = Log.Verbose;

                Log.Verbose = true;

                bool isConsistent = MeshValidator.IsConsistent(mesh);
                bool isDelaunay = MeshValidator.IsDelaunay(mesh);

                Log.Verbose = save;

                if (isConsistent)
                {
                    Log.Instance.Info("Mesh topology appears to be consistent.");
                }

                if (isDelaunay)
                {
                    Log.Instance.Info("Mesh is (conforming) Delaunay.");
                }

                ShowLog();
            }
        }

        private void menuToolsTopology_Click(object sender, EventArgs e)
        {
            (new FormTopology()).ShowDialog(this);
        }

        private void menuToolsRcm_Click(object sender, EventArgs e)
        {
            Renumber();
        }

        #endregion
    }
}
