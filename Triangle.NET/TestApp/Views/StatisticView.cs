using System.Linq;
using System.Windows.Forms;
using TriangleNet;
using TriangleNet.Geometry;
using TriangleNet.Tools;

namespace MeshExplorer.Views
{
    public partial class StatisticView : UserControl, IView
    {
        Statistic statistic = new Statistic();
        QualityMeasure quality;

        public Statistic Statistic
        {
            get { return statistic; }
        }

        public StatisticView()
        {
            InitializeComponent();
        }

        public void UpdateStatistic(Mesh mesh)
        {
            statistic.Update(mesh, 10);
        }

        #region IView

        public void HandleNewInput(IPolygon geometry)
        {
            // Reset labels
            lbNumVert2.Text = "-";
            lbNumTri2.Text = "-";
            lbNumSeg2.Text = "-";

            lbNumVert.Text = geometry.Points.Count.ToString();
            lbNumSeg.Text = geometry.Segments.Count().ToString();
            lbNumTri.Text = "0";

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
        }

        public void HandleMeshImport(IPolygon geometry, Mesh mesh)
        {
            // Previous mesh stats
            lbNumVert2.Text = "-";
            lbNumTri2.Text = "-";
            lbNumSeg2.Text = "-";
        }

        public void HandleMeshUpdate(Mesh mesh)
        {
            // Previous mesh stats
            lbNumVert2.Text = lbNumVert.Text;
            lbNumTri2.Text = lbNumTri.Text;
            lbNumSeg2.Text = lbNumSeg.Text;
        }

        public void HandleMeshChange(Mesh mesh)
        {
            // New mesh stats
            lbNumVert.Text = mesh.Vertices.Count.ToString();
            lbNumSeg.Text = mesh.Segments.Count.ToString();
            lbNumTri.Text = mesh.Triangles.Count.ToString();

            // Update statistics tab
            angleHistogram1.SetData(statistic.MinAngleHistogram, statistic.MaxAngleHistogram);

            lbAreaMin.Text = Util.DoubleToString(statistic.SmallestArea);
            lbAreaMax.Text = Util.DoubleToString(statistic.LargestArea);
            lbEdgeMin.Text = Util.DoubleToString(statistic.ShortestEdge);
            lbEdgeMax.Text = Util.DoubleToString(statistic.LongestEdge);
            lbAngleMin.Text = Util.AngleToString(statistic.SmallestAngle);
            lbAngleMax.Text = Util.AngleToString(statistic.LargestAngle);

            // Update quality
            if (quality == null)
            {
                quality = new QualityMeasure();
            }

            quality.Update(mesh);

            lbQualAlphaMin.Text = Util.DoubleToString(quality.AlphaMinimum);
            lbQualAlphaAve.Text = Util.DoubleToString(quality.AlphaAverage);

            lbQualAspectMin.Text = Util.DoubleToString(quality.Q_Minimum);
            lbQualAspectAve.Text = Util.DoubleToString(quality.Q_Average);
        }

        #endregion
    }
}
