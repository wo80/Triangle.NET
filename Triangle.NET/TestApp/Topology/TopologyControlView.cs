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

        public void SetTriangle(OrientedTriangle tri)
        {
            var t = tri.Triangle;

            if (t != null)
            {
                lbTriangle.Text = t.ID.ToString();

                lbV0.Text = t.P0.ToString();
                lbV1.Text = t.P1.ToString();
                lbV2.Text = t.P2.ToString();

                lbN0.Text = t.N0.ToString();
                lbN1.Text = t.N1.ToString();
                lbN2.Text = t.N2.ToString();

                lbS0.Text = GetSegmentString(t.GetSegment(0));
                lbS1.Text = GetSegmentString(t.GetSegment(1));
                lbS2.Text = GetSegmentString(t.GetSegment(2));
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
