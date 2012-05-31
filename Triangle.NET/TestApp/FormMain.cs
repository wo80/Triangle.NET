using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using MeshExplorer.Controls;
using MeshExplorer.IO;
using TriangleNet;
using TriangleNet.Geometry;
using TriangleNet.IO;
using TriangleNet.Tools;

namespace MeshExplorer
{
    public partial class FormMain : Form
    {
        Settings settings;
        InputGeometry input;
        Mesh mesh;
        Statistic stats;

        FormLog frmLog;
        FormQuality frmQuality;

        public FormMain()
        {
            InitializeComponent();

            ToolStripManager.Renderer = new DarkToolStripRenderer();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            oldClientSize = this.ClientSize;

            settings = new Settings();

            meshRenderer1.Initialize();

            stats = new Statistic();

            //BatchTest();
        }

        void BatchTest()
        {
            try
            {
                Mesh m = new Mesh();
                m.SetOption(Options.MinAngle, 20);


                for (int j = 0; j < 10; j++)
                {
                    for (int i = 20; i > 0; i--)
                    {
                        var geom = PolygonGenerator.CreateRandomPoints(10 * i, 100, 100);

                        m.Triangulate(geom);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("BLUB\n" + e.Message);
            }
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

        private void btnMesh_Click(object sender, EventArgs e)
        {
            TriangulateOrRefine();
        }

        private void btnSmooth_Click(object sender, EventArgs e)
        {
            //Smooth();
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

            if (meshRenderer1.ClientRectangle.Contains(pt))
            {
                meshRenderer1.Zoom(pt, e.Delta);
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
                meshRenderer1.HandleResize();
            }
        }

        private void ResizeEndHandler(object sender, EventArgs e)
        {
            isResizing = false;

            if (this.ClientSize != this.oldClientSize)
            {
                this.oldClientSize = this.ClientSize;
                meshRenderer1.HandleResize();
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

            // Reset buttons
            btnMesh.Enabled = true;
            btnMesh.Text = "Triangulate";
            btnSmooth.Enabled = false;

            // Render input
            meshRenderer1.SetData(input);

            // Update window caption
            this.Text = "Triangle.NET - Mesh Explorer - " + settings.CurrentFile;

            // Disable menu items
            viewMenuMQuality.Enabled = false;
        }

        private void HandleMeshChange()
        {
            // Render mesh
            meshRenderer1.SetData(mesh);

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
            lbRatioMin.Text = Util.DoubleToString(stats.ShortestAltitude);
            lbRatioMax.Text = Util.DoubleToString(stats.LargestAspectRatio);
            lbAngleMin.Text = Util.AngleToString(stats.SmallestAngle);
            lbAngleMax.Text = Util.AngleToString(stats.LargestAngle);

            // Enable menu items
            viewMenuMQuality.Enabled = true;
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
                input = FileProcessor.Open(ofd.FileName);

                if (input != null)
                {
                    // Update settings
                    settings.CurrentFile = Path.GetFileName(ofd.FileName);

                    HandleNewInput();
                }
                // else Message

                // Update folder settings
                settings.OfdFilterIndex = ofd.FilterIndex;
                settings.OfdDirectory = Path.GetFullPath(ofd.FileName);
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
            if (input == null || settings.ExceptionThrown) return;

            if (settings.RefineMode == false)
            {
                Triangulate();

                if (cbQuality.Checked)
                {
                    btnMesh.Text = "Refine";
                    //btnSmooth.Enabled = true;
                }
            }
            else
            {
                Refine();
            }
        }

        private void Triangulate()
        {
            if (input == null) return;

            //Stopwatch sw = new Stopwatch();

            mesh = new Mesh();

            if (cbQuality.Checked)
            {
                mesh.SetOption(Options.Quality, true);

                int angle = (slMinAngle.Value * 40) / 100;
                mesh.SetOption(Options.MinAngle, angle);

                double area = slMaxArea.Value * 0.01;

                if (area > 0 && area < 1)
                {
                    var size = input.Bounds;

                    double min = Math.Min(size.Width, size.Height);

                    mesh.SetOption(Options.MaxArea, area * min);
                }
            }

            if (cbConvex.Checked)
            {
                mesh.SetOption(Options.Convex, true);
            }

            try
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
            catch (Exception ex)
            {
                settings.ExceptionThrown = true;
                DarkMessageBox.Show("Exception - Triangulate", ex.Message);
            }

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

        private void ShowQuality()
        {
            if (frmQuality == null)
            {
                frmQuality = new FormQuality();
            }

            //UpdateLog();

            if (!frmQuality.Visible)
            {
                frmQuality.Show(this);
            }
        }

        #endregion

        #region Menu Handler

        private void fileMenuOpen_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void fileMenuSave_Click(object sender, EventArgs ev)
        {
            if (mesh != null)
            {
                Save();
            }
        }

        private void viewMenuLog_Click(object sender, EventArgs e)
        {
            ShowLog();
        }

        private void toolsMenuPoly1_Click(object sender, EventArgs e)
        {
            input = PolygonGenerator.StarInBox(20);
            settings.CurrentFile = "star *";
            HandleNewInput();
        }

        private void toolsMenuRandPts_Click(object sender, EventArgs e)
        {
            input = PolygonGenerator.CreateRandomPoints(10, 120, 100);
            settings.CurrentFile = "points *";
            HandleNewInput();
        }

        private void toolsMenuCheck_Click(object sender, EventArgs e)
        {
            if (mesh != null)
            {
                mesh.Check();
                ShowLog();
            }
        }

        private void viewMenuMQuality_Click(object sender, EventArgs e)
        {
            if (mesh != null)
            {
                ShowQuality();
            }

            frmQuality.UpdateQuality(meshRenderer1.Data);
        }

        private void fileMenuQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
