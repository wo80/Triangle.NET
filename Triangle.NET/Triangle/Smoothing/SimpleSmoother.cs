// -----------------------------------------------------------------------
// <copyright file="SimpleSmoother.cs" company="">
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Smoothing
{
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Topology.DCEL;
    using TriangleNet.Voronoi;

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

        private void Step(Mesh mesh)
        {
            var voronoi = new BoundedVoronoi(mesh);

            double x, y;

            foreach (var face in voronoi.Faces)
            {
                if (face.generator.mark == 0)
                {
                    Centroid(face, out x, out y);

                    face.generator.x = x;
                    face.generator.y = y;
                }
            }
        }

        /// <summary>
        /// Calculate the centroid of a polygon.
        /// </summary>
        private void Centroid(Face face, out double x, out double y)
        {
            double ai, atmp = 0, xtmp = 0, ytmp = 0;

            var edge = face.Edge;
            var first = edge.Next.ID;

            Point p, q;

            do
            {
                p = edge.Origin;
                q = edge.Twin.Origin;

                ai = p.X * q.Y - q.X * p.Y;
                atmp += ai;
                xtmp += (q.X + p.X) * ai;
                ytmp += (q.Y + p.Y) * ai;

                edge = edge.Next;

            } while (edge.Next.ID != first);

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
