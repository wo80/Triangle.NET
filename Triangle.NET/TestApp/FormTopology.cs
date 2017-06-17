using System;
using System.Windows.Forms;
using MeshExplorer.Topology;
using TriangleNet;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using TriangleNet.Tools;
using TriangleNet.Topology;

namespace MeshExplorer
{
    public partial class FormTopology : Form
    {
        Mesh mesh;
        TriangleQuadTree tree;
        Otri current;

        public FormTopology()
        {
            InitializeComponent();
        }

        private void FormTopology_Load(object sender, EventArgs e)
        {
            mesh = (Mesh)GenericMesher.StructuredMesh(new Rectangle(0.0, 0.0, 4.0, 4.0), 4, 4);

            renderControl.Initialize(mesh);

            topoControlView.PrimitiveCommandInvoked += PrimitiveCommandHandler;

            current = default(Otri);
        }

        void PrimitiveCommandHandler(object sender, GenericEventArgs<string> e)
        {
            if (current.Triangle != null)
            {
                InvokePrimitive(e.Argument);
            }
        }

        private void renderControl_MouseClick(object sender, MouseEventArgs e)
        {
            var p = e.Location;
            var size = renderControl.Size;

            var tri = FindTriangleAt(((float)p.X) / size.Width, ((float)p.Y) / size.Height);

            current.Triangle = (Triangle)tri;

            renderControl.Update(current);
            topoControlView.SetTriangle(current.Triangle);
        }

        private ITriangle FindTriangleAt(float x, float y)
        {
            // Get mesh coordinates
            var p = new System.Drawing.PointF(x, y);
            renderControl.Zoom.ScreenToWorld(ref p);

            topoControlView.SetPosition(p);

            if (tree == null)
            {
                tree = new TriangleQuadTree(mesh, 5, 2);
            }

            return tree.Query(p.X, p.Y);
        }

        private void InvokePrimitive(string name)
        {
            if (name == "sym")
            {
                current.Sym();
            }
            else if (name == "lnext")
            {
                current.Lnext();
            }
            else if (name == "lprev")
            {
                current.Lprev();
            }
            else if (name == "onext")
            {
                current.Onext();
            }
            else if (name == "oprev")
            {
                current.Oprev();
            }
            else if (name == "dnext")
            {
                current.Dnext();
            }
            else if (name == "dprev")
            {
                current.Dprev();
            }
            else if (name == "rnext")
            {
                current.Rnext();
            }
            else if (name == "rprev")
            {
                current.Rprev();
            }

            renderControl.Update(current);
            topoControlView.SetTriangle(current.Triangle);
        }
    }
}
