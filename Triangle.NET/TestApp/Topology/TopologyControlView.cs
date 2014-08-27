using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using TriangleNet.Geometry;

namespace MeshExplorer.Topology
{
    public partial class TopologyControlView : UserControl
    {
        public event EventHandler<GenericEventArgs<string>> PrimitiveCommandInvoked;

        public TopologyControlView()
        {
            InitializeComponent();
        }

        public void SetPosition(PointF p)
        {
            var nfi = NumberFormatInfo.InvariantInfo;

            lbPosition.Text = String.Format(nfi, "X: {0:0.0}, Y: {1:0.0}", p.X, p.Y);
        }

        public void SetTriangle(ITriangle tri)
        {
            if (tri != null)
            {
                lbTriangle.Text = tri.ID.ToString();

                lbV0.Text = tri.P0.ToString();
                lbV1.Text = tri.P1.ToString();
                lbV2.Text = tri.P2.ToString();

                lbN0.Text = tri.N0.ToString();
                lbN1.Text = tri.N1.ToString();
                lbN2.Text = tri.N2.ToString();

                lbS0.Text = GetSegmentString(tri.GetSegment(0));
                lbS1.Text = GetSegmentString(tri.GetSegment(1));
                lbS2.Text = GetSegmentString(tri.GetSegment(2));
            }
            else
            {
                lbTriangle.Text = "-";

                lbV0.Text = "-";
                lbV1.Text = "-";
                lbV2.Text = "-";

                lbN0.Text = "-";
                lbN1.Text = "-";
                lbN2.Text = "-";

                lbS0.Text = "-";
                lbS1.Text = "-";
                lbS2.Text = "-";
            }
        }

        private string GetSegmentString(ISegment seg)
        {
            return seg == null ? "-" : "[" + seg.P0 + " - " + seg.P1 + "]";
        }

        private void btnPrimitive_Click(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button != null)
            {
                var name = button.Text.ToLowerInvariant();

                var handler = PrimitiveCommandInvoked;

                if (handler != null)
                {
                    handler(this, new GenericEventArgs<string>(name));
                }
            }
        }
    }
}
