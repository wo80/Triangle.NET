// -----------------------------------------------------------------------
// <copyright file="SimpleSmoother.cs" company="">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace TriangleNet.Smoothing
{
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Topology.DCEL;
    using TriangleNet.Voronoi;

    /// <summary>
    /// Simple mesh smoother implementation (Lloyd's relaxation algorithm).
    /// </summary>
    /// <remarks>
    /// Vertices which should not move (e.g. segment vertices) MUST have a
    /// boundary mark greater than 0.
    /// </remarks>
    public class SimpleSmoother
    {

        TrianglePool pool;
        Configuration config;

        IVoronoiFactory factory;

        ConstraintOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleSmoother" /> class.
        /// </summary>
        public SimpleSmoother()
            : this(new VoronoiFactory())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleSmoother" /> class.
        /// </summary>
        public SimpleSmoother(IVoronoiFactory factory)
        {
            this.factory = factory;
            this.pool = new TrianglePool();

            this.config = new Configuration(
                () => RobustPredicates.Default,
                () => pool.Restart());

            this.options = new ConstraintOptions() { ConformingDelaunay = true };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleSmoother" /> class.
        /// </summary>
        /// <param name="factory">Voronoi object factory.</param>
        /// <param name="config">Configuration.</param>
        public SimpleSmoother(IVoronoiFactory factory, Configuration config)
        {
            this.factory = factory;
            this.config = config;

            this.options = new ConstraintOptions() { ConformingDelaunay = true };
        }

        /// <summary>
        /// Smooth mesh with a maximum given number of rounds of Voronoi
        /// iteration.
        /// </summary>
        /// <param name="mesh">The mesh.</param>
        /// <param name="limit">The maximum number of iterations. If
        /// non-positive, no iteration is applied at all.</param>
        /// <param name="tol">The desired tolerance on the result. At each
        /// iteration, the maximum movement by any side is considered, both for
        /// the previous and the current solutions. If their relative difference
        /// is not greater than the tolerance, the current solution is
        /// considered good enough already.</param>
        /// <returns>The number of actual iterations performed. It is 0 if a
        /// non-positive limit is passed. Otherwise, it is always a value
        /// between 1 and the limit (inclusive).
        /// </returns>
        public int Smooth(IMesh mesh, int limit = 10, double tol = .01)
        {
            if (limit <= 0)
              return 0;

            var smoothedMesh = (Mesh)mesh;

            var mesher = new GenericMesher(config);
            var predicates = config.Predicates();

            // The smoother should respect the mesh segment splitting behavior.
            this.options.SegmentSplitting = smoothedMesh.behavior.NoBisect;

            // The maximum distances moved from any site at the previous and
            // current iterations.
            double
                prevMax = Double.PositiveInfinity,
                currMax = Step(smoothedMesh, factory, predicates);
            // Take a few smoothing rounds (Lloyd's algorithm). The stop
            // criteria are the maximum number of iterations and the convergence
            // criterion.
            int i = 1;
            while (i < limit && Math.Abs(currMax - prevMax) > tol * currMax)
            {
                prevMax = currMax;
                currMax = Step(smoothedMesh, factory, predicates);

                // Actually, we only want to rebuild, if the mesh is no longer
                // Delaunay. Flipping edges could be the right choice instead 
                // of re-triangulating...
                smoothedMesh = (Mesh)mesher.Triangulate(Rebuild(smoothedMesh), options);

                factory.Reset();

                i++;
            }

            smoothedMesh.CopyTo((Mesh)mesh);

            return i;
        }

        private double Step(Mesh mesh, IVoronoiFactory factory, IPredicates predicates)
        {
            var voronoi = new BoundedVoronoi(mesh, factory, predicates);

            double x, y, maxDistanceMoved = 0;

            foreach (var face in voronoi.Faces)
            {
                if (face.generator.label == 0)
                {
                    Centroid(face, out x, out y);

                    double
                        xShift = face.generator.x - x,
                        yShift = face.generator.y - y,
                        distanceMoved = Math.Sqrt(xShift * xShift + yShift * yShift);
                    if (distanceMoved > maxDistanceMoved)
                        maxDistanceMoved = distanceMoved;

                    face.generator.x = x;
                    face.generator.y = y;
                }
            }

            // The maximum distance moved from any site.
            return maxDistanceMoved;
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

                ai = p.x * q.y - q.x * p.y;
                atmp += ai;
                xtmp += (q.x + p.x) * ai;
                ytmp += (q.y + p.y) * ai;

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
