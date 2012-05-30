using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using TriangleNet;
using TriangleNet.IO;
using TestApp.Rendering;

namespace TestApp
{
    public partial class FormMain : Form
    {
        Random rand = new Random(DateTime.Now.Millisecond);

        // Triangulation IO
        MeshData input;
        Mesh mesh;

        // Filter index of the "Open file" dialog
        int dlgFilterIndex = 1;

        // Startup directory of the "Open file" dialog
        string dlgDirectory = Application.StartupPath;

        Statistic statistic = new Statistic();
        FormQuality formStats;

        public FormMain()
        {
            InitializeComponent();
        }

        private double[][] CreateRandomPoints(int numPoints)
        {
            bool save = false;

            double[][] points = new double[numPoints][];

            int width = meshRenderer1.Width;
            int height = meshRenderer1.Height;
            if (save)
            {
                using (TextWriter fs = new StreamWriter(numPoints + ".txt"))
                {
                    fs.WriteLine("{0} 2 0 0", numPoints);

                    for (int i = 0; i < numPoints; i++)
                    {
                        points[i] = new double[] {
                        rand.NextDouble() * width,
                        rand.NextDouble() * height };

                        fs.WriteLine(String.Format(CultureInfo.InvariantCulture.NumberFormat,
                            "{0} {1:0.0} {2:0.0}", (i + 1), points[i][0], points[i][1]));
                    }
                }
            }
            else
            {
                for (int i = 0; i < numPoints; i++)
                {
                    points[i] = new double[] {
                        rand.NextDouble() * width,
                        rand.NextDouble() * height };
                }
            }

            return points;
        }

        private void Run()
        {
            if (input == null)
            {
                return;
            }

            Stopwatch sw = new Stopwatch();

            mesh = new Mesh();
            mesh.SetOption(Options.Quality, cbQuality.Checked);
            mesh.SetOption(Options.Convex, cbConvex.Checked);

            try
            {
                sw.Start();
                mesh.Triangulate(input);
                sw.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            meshRenderer1.SetData(mesh, false);

            statistic.Update(mesh, 10);

            if (formStats != null && !formStats.IsDisposed)
            {
                formStats.UpdateSatistic(statistic, sw.ElapsedMilliseconds, meshRenderer1.RenderTime);
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            Point pt = e.Location;
            pt.Offset(0, -meshRenderer1.Top);

            if (meshRenderer1.ClientRectangle.Contains(pt))
            {
                meshRenderer1.Zoom(pt, e.Delta);
            }
            base.OnMouseWheel(e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Directory.Exists(@"..\..\..\Data\"))
            {
                dlgDirectory = Path.GetFullPath(@"..\..\..\Data\");

                //Examples.Example1();
                //Examples.Example2();
                //Examples.Example3();
            }
            else if (Directory.Exists(@"Data\"))
            {
                dlgDirectory = Path.GetFullPath(@"Data\");
            }
        }

        private void btnRandPts_Click(object sender, EventArgs e)
        {
            btnRun.Text = "Triangulate";

            int n = 10;
            int.TryParse(tbNumPoints.Text, out n);

            input = new MeshData();

            input.Points = CreateRandomPoints(n);

            meshRenderer1.SetData(input, true);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = dlgDirectory;
            dlg.Filter = "Triangle polygon (*.node;*.poly)|*.node;*.poly|Polygon data (*.dat)|*.dat";
            dlg.FilterIndex = dlgFilterIndex;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                dlgDirectory = Path.GetDirectoryName(dlg.FileName);

                string file = dlg.FileName;
                string ext = Path.GetExtension(file);

                if (ext == ".dat")
                {
                    dlgFilterIndex = 2;
                    input = new MeshData();
                    input.Points = ParseDatFile(file);

                    int n = input.Points.Length;
                    int[][] segments = new int[n][];

                    for (int i = 0; i < n; i++)
                    {
                        segments[i] = new int[] { i, (i + 1) % n };

                    }
                    input.Segments = segments;
                }
                else
                {
                    dlgFilterIndex = 1;
                    try
                    {
                        input = FileReader.ReadFile(file);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        input = null;
                        return;
                    }
                }

                meshRenderer1.SetData(input, true);

                btnRun.Text = "Triangulate";
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (cbQuality.Checked)
            {
                if (btnRun.Text == "Triangulate")
                {
                    btnRun.Text = "Refine";

                    Run();
                }
                else
                {
                    Stopwatch sw = new Stopwatch();

                    try
                    {
                        sw.Start();
                        mesh.Refine(statistic.LargestArea / 2);
                        sw.Stop();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }

                    meshRenderer1.SetData(mesh, false);

                    statistic.Update(mesh, 10);

                    if (formStats != null && !formStats.IsDisposed)
                    {
                        formStats.UpdateSatistic(statistic, sw.ElapsedMilliseconds, meshRenderer1.RenderTime);
                    }
                }
            }
            else
            {
                btnRun.Text = "Triangulate";
                Run();
            }
        }

        private void btnStatistic_Click(object sender, EventArgs e)
        {
            if (formStats == null)
            {
                formStats = new FormQuality();
            }

            if (!formStats.Visible)
            {
                formStats.UpdateSatistic(this.statistic, -1, -1);
                formStats.Show(this);
            }
            else
            {
                formStats.Hide();
            }
        }

        public static double[][] ParseDatFile(string filename)
        {
            NumberFormatInfo nfi = CultureInfo.InvariantCulture.NumberFormat;

            List<double[]> points = new List<double[]>();

            string line;
            string[] split;

            using (TextReader reader = new StreamReader(filename))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    split = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (split.Length == 2)
                    {
                        points.Add(new double[] { 
                            double.Parse(split[0], nfi), 
                            double.Parse(split[1], nfi)
                        });
                    }
                }
            }

            return points.ToArray();
        }
    }
}
