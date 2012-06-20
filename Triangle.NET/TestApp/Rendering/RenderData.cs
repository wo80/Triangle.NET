// -----------------------------------------------------------------------
// <copyright file="RenderData.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.Rendering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TriangleNet.IO;
    using System.Drawing;
    using TriangleNet;
    using TriangleNet.Data;
    using TriangleNet.Geometry;

    /// <summary>
    /// Stores the current mesh in GDI friendly data structure.
    /// </summary>
    public class RenderData
    {
        public PointF[] Points;

        public Edge[] Edges;
        public Edge[] Segments;
        public IEnumerable<ITriangle> Triangles;

        public int NumberOfInputPoints;
        public RectangleF Bounds;

        /// <summary>
        /// Update input geometry data.
        /// </summary>
        public void SetData(InputGeometry data)
        {
            int n = data.Count;
            int i = 0;

            this.NumberOfInputPoints = n;

            this.Triangles = null;
            this.Edges = null;

            // Convert points to float
            this.Points = new PointF[n];
            foreach (var pt in data.Points)
            {
                this.Points[i++] = new PointF((float)pt.X, (float)pt.Y);
            }

            this.Bounds = new RectangleF(
                (float)data.Bounds.Xmin,
                (float)data.Bounds.Ymin,
                (float)data.Bounds.Width,
                (float)data.Bounds.Height);

            // Copy segments
            this.Segments = data.Segments.ToArray();
        }

        /// <summary>
        /// Update mesh data.
        /// </summary>
        public void SetData(Mesh mesh)
        {
            mesh.Renumber();

            this.NumberOfInputPoints = mesh.NumberOfInputPoints;

            this.Triangles = mesh.Triangles;

            this.Segments = null;

            int n = mesh.NumberOfVertices;

            // Convert points to float
            this.Points = new PointF[n];

            SetPoints(mesh.Vertices);

            // Get segments
            if (mesh.IsPolygon)
            {
                var segs = mesh.Segments;

                List<Edge> segList = new List<Edge>(mesh.NumberOfSegments);

                foreach (var seg in segs)
                {
                    segList.Add(new Edge(seg.P0, seg.P1));
                }

                this.Segments = segList.ToArray();
            }

            // Get edges (more efficient than rendering triangles)
            EdgeEnumerator e = new EdgeEnumerator(mesh);

            List<Edge> edgeList = new List<Edge>(mesh.NumberOfEdges);

            while (e.MoveNext())
            {
                edgeList.Add(e.Current);
            }

            this.Edges = edgeList.ToArray();
        }

        private void SetPoints(IEnumerable<Vertex> points)
        {
            // Bounds
            float minx = float.MaxValue;
            float maxx = float.MinValue;
            float miny = float.MaxValue;
            float maxy = float.MinValue;

            float x, y;
            int i = 0;

            foreach (var pt in points)
            {
                x = (float)pt.X;
                y = (float)pt.Y;
                // Update bounding box
                if (minx > x) minx = x;
                if (maxx < x) maxx = x;
                if (miny > y) miny = y;
                if (maxy < y) maxy = y;

                this.Points[i] = new PointF(x, y);

                i++;
            }

            this.Bounds = new RectangleF(minx, miny, maxx - minx, maxy - miny);
        }
    }
}
