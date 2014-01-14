using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MeshExplorer.Topology;
using TriangleNet;
using TriangleNet.Geometry;
using TriangleNet.Tools;

namespace MeshExplorer
{
    public partial class FormTopology : Form
    {
        Mesh mesh;
        QuadTree tree;
        OrientedTriangle current;

        public FormTopology()
        {
            InitializeComponent();
        }

        private void FormTopology_Load(object sender, EventArgs e)
        {
            var geometry = RectanglePolygon.Generate(4);

            mesh = new Mesh();
            mesh.Triangulate(geometry);

            renderControl.Initialize(mesh);

            topoControlView.PrimitiveCommandInvoked += PrimitiveCommandHandler;

            current = new OrientedTriangle();
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

            current.Triangle = tri;
            current.Orientation = 0;

            renderControl.Update(current);

            topoControlView.SetTriangle(current);
        }

        private ITriangle FindTriangleAt(float x, float y)
        {
            // Get mesh coordinates
            var p = renderControl.Zoom.ScreenToWorld(x, y);

            topoControlView.SetPosition(p);

            if (tree == null)
            {
                tree = new QuadTree(mesh, 5, 2);
            }

            return tree.Query(p.X, p.Y);
        }

        private bool TriangleContainsPoint(ITriangle triangle, float x, float y)
        {
            bool t1, t2, t3;

            t1 = Sign(x, y, triangle.GetVertex(0), triangle.GetVertex(1)) < 0.0;
            t2 = Sign(x, y, triangle.GetVertex(1), triangle.GetVertex(2)) < 0.0;
            t3 = Sign(x, y, triangle.GetVertex(2), triangle.GetVertex(0)) < 0.0;

            return (t1 == t2) && (t2 == t3);
        }

        private double Sign(double x, double y, Point p, Point q)
        {
            return (x - q.X) * (p.Y - q.Y) - (p.X - q.X) * (y - q.Y);
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
            topoControlView.SetTriangle(current);
        }
    }
}
