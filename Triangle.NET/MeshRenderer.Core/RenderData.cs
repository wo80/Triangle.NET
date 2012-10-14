// -----------------------------------------------------------------------
// <copyright file="RenderData.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshRenderer.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using TriangleNet;
    using TriangleNet.Geometry;
    using TriangleNet.Tools;

    /// <summary>
    /// Stores the current mesh in a rendering friendly data structure.
    /// </summary>
    public class RenderData
    {
        public float[] Points;
        public uint[] Segments;
        public uint[] Triangles;
        public uint[] MeshEdges;
        public float[] VoronoiPoints;
        public uint[] VoronoiEdges;

        public int NumberOfInputPoints;
        public BoundingBox Bounds;

        /// <summary>
        /// Copy input geometry data.
        /// </summary>
        public void SetInputGeometry(InputGeometry data)
        {
            // Clear unused buffers
            this.Segments = null;
            this.Triangles = null;
            this.MeshEdges = null;
            this.VoronoiPoints = null;
            this.VoronoiEdges = null;

            int n = data.Count;
            int i = 0;

            this.NumberOfInputPoints = n;

            // Copy points
            this.Points = new float[2 * n];
            foreach (var pt in data.Points)
            {
                this.Points[2 * i] = (float)pt.X;
                this.Points[2 * i + 1] = (float)pt.Y;
                i++;
            }

            // Copy segments
            n = data.Segments.Count;
            if (n > 0)
            {
                var segments = new List<uint>(2 * n);
                foreach (var seg in data.Segments)
                {
                    segments.Add((uint)seg.P0);
                    segments.Add((uint)seg.P1);
                }
                this.Segments = segments.ToArray();
            }

            this.Bounds = new BoundingBox(
                (float)data.Bounds.Xmin,
                (float)data.Bounds.Xmax,
                (float)data.Bounds.Ymin,
                (float)data.Bounds.Ymax);
        }

        /// <summary>
        /// Copy mesh data.
        /// </summary>
        public void SetMesh(Mesh mesh)
        {
            // Clear unused buffers
            this.Segments = null;
            this.VoronoiPoints = null;
            this.VoronoiEdges = null;

            int n = mesh.Vertices.Count;
            int i = 0;

            this.NumberOfInputPoints = mesh.NumberOfInputPoints;

            // Linear numbering of mesh
            mesh.Renumber();

            // Copy points
            this.Points = new float[2 * n];
            foreach (var pt in mesh.Vertices)
            {
                this.Points[2 * i] = (float)pt.X;
                this.Points[2 * i + 1] = (float)pt.Y;
                i++;
            }

            // Copy segments
            n = mesh.Segments.Count;
            if (n > 0)
            {
                var segments = new List<uint>(2 * n);
                foreach (var seg in mesh.Segments)
                {
                    segments.Add((uint)seg.P0);
                    segments.Add((uint)seg.P1);
                }
                this.Segments = segments.ToArray();
            }

            // Copy edges
            var edges = new List<uint>(2 * mesh.NumberOfEdges);

            EdgeEnumerator e = new EdgeEnumerator(mesh);
            while (e.MoveNext())
            {
                edges.Add((uint)e.Current.P0);
                edges.Add((uint)e.Current.P1);
            }
            this.MeshEdges = edges.ToArray();

            // Copy Triangles
            var triangles = new List<uint>(3 * mesh.Triangles.Count);
            foreach (var tri in mesh.Triangles)
            {
                triangles.Add((uint)tri.P0);
                triangles.Add((uint)tri.P1);
                triangles.Add((uint)tri.P2);
            }
            this.Triangles = triangles.ToArray();

            this.Bounds = new BoundingBox(
                (float)mesh.Bounds.Xmin,
                (float)mesh.Bounds.Xmax,
                (float)mesh.Bounds.Ymin,
                (float)mesh.Bounds.Ymax);
        }

        /// <summary>
        /// Copy voronoi data.
        /// </summary>
        public void SetVoronoi(IVoronoi voro)
        {
            SetVoronoi(voro, 0);
        }

        /// <summary>
        /// Copy voronoi data.
        /// </summary>
        public void SetVoronoi(IVoronoi voro, int infCount)
        {

            int i, n = voro.Points.Length;

            // Copy points
            this.VoronoiPoints = new float[2 * n + infCount];
            foreach (var v in voro.Points)
            {
                if (v == null)
                {
                    continue;
                }

                i = v.ID;
                this.VoronoiPoints[2 * i] = (float)v.X;
                this.VoronoiPoints[2 * i + 1] = (float)v.Y;
            }

            // Copy edges
            Point first, last;
            var edges = new List<uint>(voro.Regions.Count * 4);
            foreach (var region in voro.Regions)
            {
                first = null;
                last = null;

                foreach (var pt in region.Vertices)
                {
                    if (first == null)
                    {
                        first = pt;
                        last = pt;
                    }
                    else
                    {
                        edges.Add((uint)last.ID);
                        edges.Add((uint)pt.ID);

                        last = pt;
                    }
                }

                if (region.Bounded && first != null)
                {
                    edges.Add((uint)last.ID);
                    edges.Add((uint)first.ID);
                }
            }
            this.VoronoiEdges = edges.ToArray();

            /*
            int i, n = voro.VertexList.Count;

            // Copy points
            this.VoronoiPoints = new float[2 * n + infCount];
            foreach (var v in voro.VertexList)
            {
                i = v.Id;
                this.VoronoiPoints[2 * i] = (float)v.X;
                this.VoronoiPoints[2 * i + 1] = (float)v.Y;
            }

            // Copy edges
            var edges = new List<uint>(voro.HalfEdgeList.Count);
            foreach (var edge in voro.Edges)
            {
                if (edge.P0 >= 0 && edge.P1 >= 0)
                {
                    edges.Add((uint)edge.P0);
                    edges.Add((uint)edge.P1);
                }
            }
            this.VoronoiEdges = edges.ToArray();
             * */
        }
    }
}
