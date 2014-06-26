// -----------------------------------------------------------------------
// <copyright file="SimpleSmoother.cs" company="">
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Smoothing
{
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

            // Take a few smoothing rounds.
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
            BoundedVoronoiLegacy voronoi = new BoundedVoronoiLegacy(mesh, false);

            var cells = voronoi.Regions;

            double x, y;
            int n;

            foreach (var cell in cells)
            {
                n = 0;
                x = y = 0.0;
                foreach (var p in cell.Vertices)
                {
                    n++;
                    x += p.x;
                    y += p.y;
                }

                cell.Generator.x = x / n;
                cell.Generator.y = y / n;
            }
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
