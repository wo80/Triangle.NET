// -----------------------------------------------------------------------
// <copyright file="SimpleSmoother.cs" company="">
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Smoothing
{
    using System.Collections.Generic;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Voronoi.Legacy;

    /// <summary>
    /// Simple mesh smoother implementation.
    /// </summary>
    /// <remarks>
    /// Vertices wich should not move (e.g. segment vertices) MUST have a
    /// boundary mark greater than 0.
    /// </remarks>
    public class SimpleSmoother : ISmoother
    {
        ConstraintOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleSmoother" /> class.
        /// </summary>
        public SimpleSmoother()
        {
            this.options = new ConstraintOptions() { ConformingDelaunay = true };
        }

        public void Smooth(IMesh mesh)
        {
            Smooth(mesh, 10);
        }

        public void Smooth(IMesh mesh, int limit)
        {
            var smoothedMesh = (Mesh)mesh;

            // The smoother should respect the mesh segment splitting behavior.
            this.options.SegmentSplitting = smoothedMesh.behavior.NoBisect;

            // Take a few smoothing rounds (Lloyd's algorithm).
            for (int i = 0; i < limit; i++)
            {
                Step(smoothedMesh);

                // Actually, we only want to rebuild, if mesh is no longer
                // Delaunay. Flipping edges could be the right choice instead 
                // of re-triangulating...
                smoothedMesh = (Mesh)Rebuild(smoothedMesh).Triangulate(options);
            }

            smoothedMesh.CopyTo((Mesh)mesh);
        }

        /// <summary>
        /// Smooth all free nodes.
        /// </summary>
        private void Step(Mesh mesh)
        {
            var voronoi = new BoundedVoronoiLegacy(mesh, false);

            var cells = voronoi.Regions;

            double x, y;

            foreach (var cell in cells)
            {
                Centroid((List<Point>)cell.Vertices, out x, out y);

                cell.Generator.x = x;
                cell.Generator.y = y;
            }
        }

        /// <summary>
        /// Calculate the centroid of a polygon.
        /// </summary>
        /// <param name="points">Points of the polygon.</param>
        /// <param name="x">Centroid x coordinate.</param>
        /// <param name="y">Centroid y coordinate.</param>
        /// <remarks>
        /// Based on ANSI C code from the article "Centroid of a Polygon" by Gerard Bashein
        /// and Paul R. Detmer in "Graphics Gems IV", Academic Press, 1994
        /// </remarks>
        private void Centroid(List<Point> points, out double x, out double y)
        {
            int i, j, n = points.Count;
            double ai, atmp = 0, xtmp = 0, ytmp = 0;

            for (i = n - 1, j = 0; j < n; i = j, j++)
            {
                ai = points[i].X * points[j].Y - points[j].X * points[i].Y;
                atmp += ai;
                xtmp += (points[j].X + points[i].X) * ai;
                ytmp += (points[j].Y + points[i].Y) * ai;
            }

            x = xtmp / (3 * atmp);
            y = ytmp / (3 * atmp);

            //area = atmp / 2;
        }

        /// <summary>
        /// Rebuild the input geometry.
        /// </summary>
        private Polygon Rebuild(Mesh mesh)
        {
            var data = new Polygon(mesh.vertices.Count);

            foreach (var v in mesh.vertices.Values)
            {
                // Reset to input vertex.
                v.type = VertexType.InputVertex;

                data.Points.Add(v);
            }

            data.Segments.AddRange(mesh.subsegs.Values);

            data.Holes.AddRange(mesh.holes);
            data.Regions.AddRange(mesh.regions);

            return data;
        }
    }
}
