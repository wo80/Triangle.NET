using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MeshExplorer.Controls;
using MeshExplorer.IO;
using MeshRenderer.Core;
using TriangleNet;
using TriangleNet.Geometry;
using TriangleNet.Tools;

namespace MeshExplorer
{
    public partial class FormMain : Form
    {
        Settings settings;
        InputGeometry input;
        Mesh mesh;

        FormLog frmLog;
        FormGenerator frmGenerator;

        RenderManager renderManager;
        RenderData renderData;

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
            renderManager.CreateDefaultControl();

            /*
            if (!renderManager.CreateControl("MeshRenderer.SharpGL2.dll", new string[] { "SharpGL.dll" }))
            {
                renderManager.CreateDefaultControl();

                if (frmLog == null)
                {
                    frmLog = new FormLog();
                }

                frmLog.AddItem("Failed to initialize OpenGL.", true);
            }
            */

            var control = renderManager.RenderControl;

            if (control != null)
            {
                this.splitContainer1.Panel2.Controls.Add(control);

                // Initialize control
                control.BackColor = Color.Black;
                control.Dock = DockStyle.Fill;
                control.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                control.Location = new System.Drawing.Point(0, 0);
                control.Name = "renderControl1";
                control.Size = new Size(703, 612);
                control.TabIndex = 0;
                control.Text = "meshRenderer1";

                renderManager.Initialize();
            }
            else
            {
                DarkMessageBox.Show("Ooops ...", "Failed to initialize renderer.");
            }

            renderData = new RenderData();
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
            this.input = sender as InputGeometry;

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

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            var container = this.splitContainer1.Panel2.ClientRectangle;

            System.Drawing.Point pt = e.Location;
            pt.Offset(-splitContainer1.SplitterDistance, 0);

            if (container.Contains(pt))
            {
                renderManager.Zoom(((float)pt.X) / container.Width,
                    ((float)pt.Y) / container.Height, e.Delta);
            }
            base.OnMouseWheel(e);
        }

        #region Resize event handler

        bool isResizing = false;
        Size oldClientSize;

        private void ResizeHandler(object sender, EventArgs e)
        {
            // Handle window minimize and maximize
            if (!isResizing)
            {
                renderManager.HandleResize();
            }
        }

        private void ResizeEndHandler(object sender, EventArgs e)
        {
            isResizing = false;

            if (this.ClientSize != this.oldClientSize)
            {
                this.oldClientSize = this.ClientSize;
                renderManager.HandleResize();
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
            renderData.SetInputGeometry(input);
            renderManager.SetData(renderData);

            // Update window caption
            this.Text = "Triangle.NET - Mesh Explorer - " + settings.CurrentFile;
        }

        private void HandleMeshImport()
        {
            // Render mesh
            renderData.SetMesh(mesh);
            renderManager.SetData(renderData);
            //renderManager.Initialize();

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
            renderData.SetMesh(mesh);
            renderManager.SetData(renderData);

            // Update Statistic view
            statisticView.HandleMeshUpdate(mesh);

            HandleMeshChange();
        }

        private void HandleMeshChange()
        {
            // Update Statistic view
            statisticView.HandleMeshChange(mesh);

            // TODO: Should the Voronoi diagram automatically update?
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
            if (FileProcessor.ContainsMeshData(filename))
            {
                if (DarkMessageBox.Show("Import mesh", Settings.ImportString,
                    "Do you want to import the mesh?", MessageBoxButtons.YesNo) == DialogResult.OK)
                {
                    input = null;
                    mesh = FileProcessor.Import(filename);

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

            //Stopwatch sw = new Stopwatch();

            mesh = new Mesh();

            if (meshControlView.ParamConformDelChecked)
            {
                mesh.Behavior.ConformingDelaunay = true;
            }

            if (meshControlView.ParamSweeplineChecked)
            {
                mesh.Behavior.Algorithm = TriangulationAlgorithm.SweepLine;
            }

            if (meshControlView.ParamQualityChecked)
            {
                mesh.Behavior.Quality = true;

                mesh.Behavior.MinAngle = meshControlView.ParamMinAngleValue;

                // Ignore area constraints on initial triangulation.

                //double area = slMaxArea.Value * 0.01;
                //if (area > 0 && area < 1)
                //{
                //    var size = input.Bounds;
                //    double min = Math.Min(size.Width, size.Height);
                //    mesh.Behavior.MaxArea, area * min);
                //}
            }

            if (meshControlView.ParamConvexChecked)
            {
                mesh.Behavior.Convex = true;
            }

            try
            {
                //sw.Start();
                mesh.Triangulate(input);
                //sw.Stop();

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

            Stopwatch sw = new Stopwatch();

            double area = meshControlView.ParamMaxAreaValue;

            if (area > 0 && area < 1)
            {
                mesh.Behavior.MaxArea = area * statisticView.Statistic.LargestArea;
            }

            mesh.Behavior.MinAngle = meshControlView.ParamMinAngleValue;

            try
            {
                sw.Start();
                mesh.Refine();
                sw.Stop();

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

            bool tmp = Behavior.Verbose;
            Behavior.Verbose = true;

            mesh.Renumber(NodeNumbering.CuthillMcKee);
            ShowLog();

            Behavior.Verbose = tmp;
        }

        private void Smooth()
        {
            if (mesh == null || settings.ExceptionThrown) return;

            if (!mesh.IsPolygon)
            {
                return;
            }

            Stopwatch sw = new Stopwatch();

            try
            {
                sw.Start();
                mesh.Smooth();
                sw.Stop();

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

                    if (format == 1)
                    {
                        EpsImage eps = new EpsImage();
                        eps.Export(this.mesh, export.ImageName, size);
                    }
                    else if (format == 2)
                    {
                        SvgImage svg = new SvgImage();
                        svg.Export(this.mesh, export.ImageName, size);
                    }
                    else
                    {
                        RasterImage img = new RasterImage();
                        img.ColorScheme = ColorManager.LightScheme();
                        img.Export(this.mesh, export.ImageName, size);
                    }
                }
            }
        }

        private void menuFileQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuViewVoronoi_Click(object sender, EventArgs e)
        {
            if (mesh == null)
            {
                return;
            }

            if (menuViewVoronoi.Checked)
            {
                renderManager.ShowVoronoi = false;
                menuViewVoronoi.Checked = false;
                return;
            }

            IVoronoi voronoi;

            if (mesh.IsPolygon)
            {
                voronoi = new BoundedVoronoi(mesh);
            }
            else
            {
                voronoi = new Voronoi(mesh);
            }

            renderData.SetVoronoi(voronoi);
            renderManager.SetData(renderData);

            menuViewVoronoi.Checked = true;
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
                bool isConsistent, isDelaunay;
                mesh.Check(out isConsistent, out isDelaunay);

                ShowLog();
            }
        }

        private void menuToolsRcm_Click(object sender, EventArgs e)
        {
            Renumber();
        }

        #endregion
    }
}
