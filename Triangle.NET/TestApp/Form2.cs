using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TriangleNet;
using System.Globalization;

namespace TestApp
{
    public partial class Form2 : Form
    {
        static NumberFormatInfo nfi = CultureInfo.InvariantCulture.NumberFormat;

        public Form2()
        {
            InitializeComponent();

        }

        public void UpdateSatistic(Statistic stat, long calcTime, long renderTime)
        {
            UpdateMesh(stat);
            UpdateQuality(stat);
            UpdateTime(calcTime, renderTime);

            histogram1.SetData(stat.AngleHistogram);
        }

        private void UpdateMesh(Statistic stat)
        {
            lbNumInput.Text = stat.InputVertices.ToString();
            lbNumOutput.Text = stat.Vertices.ToString();
            lbNumTri.Text = stat.Triangles.ToString();
            lbNumEdge.Text = stat.Edges.ToString();
            lbNumBoundary.Text = stat.BoundaryEdges.ToString();
        }

        private void UpdateQuality(Statistic stat)
        {
            lbAreaMin.Text = stat.SmallestArea.ToString("0.00000", nfi);
            lbAreaMax.Text = stat.LargestArea.ToString("0.00000", nfi);
            lbEdgeMin.Text = stat.ShortestEdge.ToString("0.00000", nfi);
            lbEdgeMax.Text = stat.LongestEdge.ToString("0.00000", nfi);
            lbRatioMin.Text = stat.ShortestAltitude.ToString("0.00000", nfi);
            lbRatioMax.Text = stat.LargestAspectRatio.ToString("0.00000", nfi);
            lbAngleMin.Text = stat.SmallestAngle.ToString("0.00000", nfi);
            lbAngleMax.Text = stat.LargestAngle.ToString("0.00000", nfi);
        }

        private void UpdateTime(long calcTime, long renderTime)
        {
            if (calcTime > 0)
            {
                lbCalcTime.Text = calcTime + " ms";
            }
            else
            {
                lbCalcTime.Text = "-";
            }

            if (renderTime > 0)
            {
                lbRenderTime.Text = renderTime + " ms";
            }
            else
            {
                lbRenderTime.Text = "-";
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;
            this.Hide();
        }
    }
}
