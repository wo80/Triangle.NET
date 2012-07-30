using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MeshExplorer.Controls;
using MeshExplorer.IO;
using TriangleNet;
using TriangleNet.Geometry;
using TriangleNet.Tools;
using MeshExplorer.Rendering;

namespace MeshExplorer
{
    public partial class FormMain : Form
    {
        Settings settings;
        InputGeometry input;
        Mesh mesh;
        Statistic stats;
        QualityMeasure quality;

        FormLog frmLog;
        FormGenerator frmGenerator;

        public FormMain()
        {
            InitializeComponent();

            ToolStripManager.Renderer = new DarkToolStripRenderer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            oldClientSize = this.ClientSize;

            settings = new Settings();

            renderControl1.Initialize();

            stats = new Statistic();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F3:
                    Open();
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
                settings.CurrentFile = "GEN";
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

        private void lbCodeplex_Clicked(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo sInfo = new ProcessStartInfo("http://triangle.codeplex.com/");
                Process.Start(sInfo);
            }
            catch (Exception)
            { }
        }

        private void slMinAngle_ValueChanging(object sender, EventArgs e)
        {
            // Between 0 and 40 (step 1)
            int angle = (slMinAngle.Value * 40) / 100;
            lbMinAngle.Text = angle.ToString();
        }

        private void slMaxArea_ValueChanging(object sender, EventArgs e)
        {
            // Between 0 and 1 (step 0.01)
            double area = slMaxArea.Value * 0.01;
            lbMaxArea.Text = area.ToString(Util.Nfi);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            System.Drawing.Point pt = e.Location;
            pt.Offset(-splitContainer1.SplitterDistance, 0);

            if (renderControl1.ClientRectangle.Contains(pt))
            {
                renderControl1.Zoom(pt, e.Delta);
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
                renderControl1.HandleResize();
            }
        }

        private void ResizeEndHandler(object sender, EventArgs e)
        {
            isResizing = false;

            if (this.ClientSize != this.oldClientSize)
            {
                this.oldClientSize = this.ClientSize;
                renderControl1.HandleResize();
            }
        }

        private void ResizeBeginHandler(object sender, EventArgs e)
        {
            isResizing = true;
        }

        #endregion

        #region State changes

        private void HandleNewInput()
        {
            // Reset mesh
            mesh = null;

            // Reset state
            settings.RefineMode = false;
            settings.ExceptionThrown = false;

            // Reset labels
            lbNumVert2.Text = "-";
            lbNumTri2.Text = "-";
            lbNumSeg2.Text = "-";

            lbNumVert.Text = input.Count.ToString();
            lbNumSeg.Text = input.Segments.Count().ToString();
            lbNumTri.Text = "0"; //input.Triangles == null ? "0" : input.Triangles.Length.ToString();

            // Statistics labels
            lbAreaMin.Text = "-";
            lbAreaMax.Text = "-";
            lbEdgeMin.Text = "-";
            lbEdgeMax.Text = "-";
            lbAngleMin.Text = "-";
            lbAngleMax.Text = "-";

            // Quality labels
            lbQualAlphaMin.Text = "-";
            lbQualAlphaAve.Text = "-";
            lbQualAspectMin.Text = "-";
            lbQualAspectAve.Text = "-";

            angleHistogram1.SetData(null, null);

            // Reset buttons
            btnMesh.Enabled = true;
            btnMesh.Text = "Triangulate";
            btnSmooth.Enabled = false;

            // Clear voronoi
            menuViewVoronoi.Checked = false;
            renderControl1.ShowVoronoi = false;

            // Disable menu items
            menuFileSave.Enabled = false;
            menuFileExport.Enabled = false;
            menuViewVoronoi.Enabled = false;
            menuToolsCheck.Enabled = false;

            // Render input
            renderControl1.SetData(input);

            // Update window caption
            this.Text = "Triangle.NET - Mesh Explorer - " + settings.CurrentFile;
        }

        private void HandleMeshChange()
        {
            // Render mesh
            renderControl1.SetData(mesh);

            // Previous mesh stats
            lbNumVert2.Text = lbNumVert.Text;
            lbNumTri2.Text = lbNumTri.Text;
            lbNumSeg2.Text = lbNumSeg.Text;

            // New mesh stats
            lbNumVert.Text = stats.Vertices.ToString();
            lbNumSeg.Text = stats.ConstrainedEdges.ToString();
            lbNumTri.Text = stats.Triangles.ToString();

            // Update statistics tab
            angleHistogram1.SetData(stats.MinAngleHistogram, stats.MaxAngleHistogram);

            lbAreaMin.Text = Util.DoubleToString(stats.SmallestArea);
            lbAreaMax.Text = Util.DoubleToString(stats.LargestArea);
            lbEdgeMin.Text = Util.DoubleToString(stats.ShortestEdge);
            lbEdgeMax.Text = Util.DoubleToString(stats.LongestEdge);
            lbAngleMin.Text = Util.AngleToString(stats.SmallestAngle);
            lbAngleMax.Text = Util.AngleToString(stats.LargestAngle);

            // Enable menu items
            menuFileSave.Enabled = true;
            menuFileExport.Enabled = true;
            menuViewVoronoi.Enabled = true;
            menuToolsCheck.Enabled = true;

            // Update quality
            if (quality == null)
            {
                quality = new QualityMeasure();
            }

            quality.Update(this.mesh);

            lbQualAlphaMin.Text = Util.DoubleToString(quality.AlphaMinimum);
            lbQualAlphaAve.Text = Util.DoubleToString(quality.AlphaAverage);

            lbQualAspectMin.Text = Util.DoubleToString(quality.Q_Minimum);
            lbQualAspectAve.Text = Util.DoubleToString(quality.Q_Average);
        }

        private void HandleMeshImport()
        {
            // Render mesh
            renderControl1.SetData(mesh, true);

            // Update window caption
            this.Text = "Triangle.NET - Mesh Explorer - " + settings.CurrentFile;

            // Previous mesh stats
            lbNumVert2.Text = "-";
            lbNumTri2.Text = "-";
            lbNumSeg2.Text = "-";

            // New mesh stats
            lbNumVert.Text = stats.Vertices.ToString();
            lbNumSeg.Text = stats.ConstrainedEdges.ToString();
            lbNumTri.Text = stats.Triangles.ToString();

            // Update statistics tab
            angleHistogram1.SetData(stats.MinAngleHistogram, stats.MaxAngleHistogram);

            lbAreaMin.Text = Util.DoubleToString(stats.SmallestArea);
            lbAreaMax.Text = Util.DoubleToString(stats.LargestArea);
            lbEdgeMin.Text = Util.DoubleToString(stats.ShortestEdge);
            lbEdgeMax.Text = Util.DoubleToString(stats.LongestEdge);
            lbAngleMin.Text = Util.AngleToString(stats.SmallestAngle);
            lbAngleMax.Text = Util.AngleToString(stats.LargestAngle);

            // Enable menu items
            menuFileSave.Enabled = true;
            menuFileExport.Enabled = true;
            menuViewVoronoi.Enabled = true;
            menuToolsCheck.Enabled = true;

            // Set refine mode
            btnMesh.Enabled = true;
            btnMesh.Text = "Refine";

            settings.RefineMode = true;

            // Update quality
            if (quality == null)
            {
                quality = new QualityMeasure();
            }

            quality.Update(this.mesh);

            lbQualAlphaMin.Text = Util.DoubleToString(quality.AlphaMinimum);
            lbQualAlphaAve.Text = Util.DoubleToString(quality.AlphaAverage);

            lbQualAspectMin.Text = Util.DoubleToString(quality.Q_Minimum);
            lbQualAspectAve.Text = Util.DoubleToString(quality.Q_Average);
        }

        #endregion

        #region Commands

        private void Open()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = settings.OfdFilter;
            ofd.FilterIndex = settings.OfdFilterIndex;
            ofd.InitialDirectory = settings.OfdDirectory;
            ofd.FileName = "";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (FileProcessor.ContainsMeshData(ofd.FileName))
                {
                    if (DarkMessageBox.Show("Import mesh", Settings.ImportString,
                        "Do you want to import the mesh?", MessageBoxButtons.YesNo) == DialogResult.OK)
                    {
                        input = null;
                        mesh = FileProcessor.Import(ofd.FileName);

                        if (mesh != null)
                        {
                            stats.Update(mesh, 10);

                            // Update settings
                            settings.CurrentFile = Path.GetFileName(ofd.FileName);

                            HandleMeshImport();
                            btnSmooth.Enabled = true; // TODO: Remove
                        }
                        // else Message

                        // Update folder settings
                        settings.OfdFilterIndex = ofd.FilterIndex;
                        settings.OfdDirectory = Path.GetDirectoryName(ofd.FileName);

                        return;
                    }
                }

                input = FileProcessor.Read(ofd.FileName);

                if (input != null)
                {
                    // Update settings
                    settings.CurrentFile = Path.GetFileName(ofd.FileName);

                    HandleNewInput();
                }
                // else Message

                // Update folder settings
                settings.OfdFilterIndex = ofd.FilterIndex;
                settings.OfdDirectory = Path.GetDirectoryName(ofd.FileName);
            }
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

                if (cbQuality.Checked)
                {
                    btnMesh.Text = "Refine";
                    //btnSmooth.Enabled = true;
                }
            }
            else if (cbQuality.Checked)
            {
                Refine();
            }
        }

        private void Triangulate()
        {
            if (input == null) return;

            //Stopwatch sw = new Stopwatch();

            mesh = new Mesh();

            if (cbConformDel.Checked)
            {
                mesh.SetOption(Options.ConformingDelaunay, true);
            }

            if (cbQuality.Checked)
            {
                mesh.SetOption(Options.Quality, true);

                int angle = (slMinAngle.Value * 40) / 100;
                mesh.SetOption(Options.MinAngle, angle);

                // Ignore area constraints on initial triangulation.

                //double area = slMaxArea.Value * 0.01;
                //if (area > 0 && area < 1)
                //{
                //    var size = input.Bounds;
                //    double min = Math.Min(size.Width, size.Height);
                //    mesh.SetOption(Options.MaxArea, area * min);
                //}
            }

            if (cbConvex.Checked)
            {
                mesh.SetOption(Options.Convex, true);
            }

            //try
            {
                //sw.Start();
                mesh.Triangulate(input);
                //sw.Stop();

                stats.Update(mesh, 10);

                HandleMeshChange();

                if (cbQuality.Checked)
                {
                    settings.RefineMode = true;
                }
            }
            //catch (Exception ex)
            //{
            //    settings.ExceptionThrown = true;
            //    DarkMessageBox.Show("Exception - Triangulate", ex.Message);
            //}

            UpdateLog();
        }

        private void Refine()
        {
            if (mesh == null) return;

            Stopwatch sw = new Stopwatch();

            double area = slMaxArea.Value * 0.01;

            if (area > 0 && area < 1)
            {
                mesh.SetOption(Options.MaxArea, area * stats.LargestArea);
            }

            int angle = (slMinAngle.Value * 40) / 100;
            mesh.SetOption(Options.MinAngle, angle);

            try
            {
                sw.Start();
                mesh.Refine();
                sw.Stop();

                stats.Update(mesh, 10);

                HandleMeshChange();
            }
            catch (Exception ex)
            {
                settings.ExceptionThrown = true;
                DarkMessageBox.Show("Exception - Refine", ex.Message);
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

            Stopwatch sw = new Stopwatch();

            try
            {
                sw.Start();
                mesh.Smooth();
                sw.Stop();

                stats.Update(mesh, 10);

                HandleMeshChange();
            }
            catch (Exception ex)
            {
                settings.ExceptionThrown = true;
                DarkMessageBox.Show("Exception - Smooth", ex.Message);
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
            Open();
        }

        private void menuFileSave_Click(object sender, EventArgs ev)
        {
            if (mesh != null)
            {
                Save();
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
                mesh.Check();
                ShowLog();
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
                        img.ColorScheme = RenderColors.LightScheme();
                        img.Export(this.mesh, export.ImageName, size);
                    }
                }
            }
        }

        private void menuViewVoronoi_Click(object sender, EventArgs e)
        {
            menuViewVoronoi.Checked = !menuViewVoronoi.Checked;
            renderControl1.ShowVoronoi = menuViewVoronoi.Checked;
        }

        #endregion

        private void menuFileQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuToolsRcm_Click(object sender, EventArgs e)
        {
            Renumber();
        }
    }
}
