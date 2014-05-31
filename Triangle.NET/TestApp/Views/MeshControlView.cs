using System;
using System.Windows.Forms;
using TriangleNet;
using TriangleNet.Geometry;

namespace MeshExplorer.Views
{
    public partial class MeshControlView : UserControl, IView
    {
        public MeshControlView()
        {
            InitializeComponent();
        }

        public bool ParamQualityChecked
        {
            get { return cbQuality.Checked; }
        }

        public bool ParamConvexChecked
        {
            get { return cbConvex.Checked; }
        }

        public bool ParamConformDelChecked
        {
            get { return cbConformDel.Checked; }
        }

        public bool ParamSweeplineChecked
        {
            get { return cbSweepline.Checked; }
        }

        public int ParamMinAngleValue
        {
            get { return (slMinAngle.Value * 40) / 100; }
        }

        public int ParamMaxAngleValue
        {
            get { return 80 + (100 - slMaxAngle.Value); }
        }

        public double ParamMaxAreaValue
        {
            get { return slMaxArea.Value * 0.01; }
        }

        private void slMinAngle_ValueChanging(object sender, EventArgs e)
        {
            // Between 0 and 40 (step 1)
            int angle = (slMinAngle.Value * 40) / 100;
            lbMinAngle.Text = angle.ToString();
        }

        private void slMaxAngle_ValueChanging(object sender, EventArgs e)
        {
            // Between 180 and 80 (step 1)
            int angle = 80 + (100 - slMaxAngle.Value);
            lbMaxAngle.Text = angle.ToString();
        }

        private void slMaxArea_ValueChanging(object sender, EventArgs e)
        {
            // Between 0 and 1 (step 0.01)
            double area = slMaxArea.Value * 0.01;
            lbMaxArea.Text = area.ToString(Util.Nfi);
        }

        #region IView

        public void HandleNewInput(IPolygon geometry)
        {
        }

        public void HandleMeshImport(IPolygon geometry, Mesh mesh)
        {
        }

        public void HandleMeshUpdate(Mesh mesh)
        {
        }

        public void HandleMeshChange(Mesh mesh)
        {
        }

        #endregion
    }
}
