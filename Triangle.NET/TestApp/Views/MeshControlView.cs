using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
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

        private void slMaxArea_ValueChanging(object sender, EventArgs e)
        {
            // Between 0 and 1 (step 0.01)
            double area = slMaxArea.Value * 0.01;
            lbMaxArea.Text = area.ToString(Util.Nfi);
        }

        #region IView

        public void HandleNewInput(InputGeometry geometry)
        {
        }

        public void HandleMeshImport(InputGeometry geometry, Mesh mesh)
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
