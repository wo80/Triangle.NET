
namespace TriangleNet.Meshing
{
    using System.Collections.Generic;
    using TriangleNet.Geometry;
    using TriangleNet.IO;
    using TriangleNet.Meshing.Algorithm;

    public class GenericMesher : ITriangulator, IConstraintMesher, IQualityMesher
    {
        ITriangulator triangulator;

        public GenericMesher()
            : this(new Dwyer())
        {
        }

        public GenericMesher(ITriangulator triangulator)
        {
            this.triangulator = triangulator;
        }

        /// <summary>
        /// Triangulates a point set.
        /// </summary>
        /// <param name="points">Collection of points.</param>
        /// <returns>Mesh</returns>
        public IMesh Triangulate(ICollection<Vertex> points)
        {
            return triangulator.Triangulate(points);
        }

        /// <summary>
        /// Triangulates a polygon.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <returns>Mesh</returns>
        public IMesh Triangulate(IPolygon polygon)
        {
            return Triangulate(polygon, null, null);
        }

        /// <summary>
        /// Triangulates a polygon, applying constraint options.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="options">Constraint options.</param>
        /// <returns>Mesh</returns>
        public IMesh Triangulate(IPolygon polygon, ConstraintOptions options)
        {
            return Triangulate(polygon, options, null);
        }

        /// <summary>
        /// Triangulates a polygon, applying quality options.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="quality">Quality options.</param>
        /// <returns>Mesh</returns>
        public IMesh Triangulate(IPolygon polygon, QualityOptions quality)
        {
            return Triangulate(polygon, null, quality);
        }

        /// <summary>
        /// Triangulates a polygon, applying quality and constraint options.
        /// </summary>
        /// <param name="polygon">The polygon.</param>
        /// <param name="options">Constraint options.</param>
        /// <param name="quality">Quality options.</param>
        /// <returns>Mesh</returns>
        public IMesh Triangulate(IPolygon polygon, ConstraintOptions options, QualityOptions quality)
        {
            var mesh = (Mesh)triangulator.Triangulate(polygon.Points);

            mesh.ApplyConstraints(polygon, options, quality);

            return mesh;
        }

        /// <summary>
        /// Generates a structured mesh.
        /// </summary>
        /// <param name="bounds">Bounds of the mesh.</param>
        /// <param name="nx">Number of segments in x direction.</param>
        /// <param name="ny">Number of segments in y direction.</param>
        /// <returns>Mesh</returns>
        public IMesh StructurdMesh(Rectangle bounds, int nx, int ny)
        {
            var polygon = new Polygon((nx + 1) * (ny + 1));

            double x, y, dx, dy, left, bottom;

            dx = bounds.Width / nx;
            dy = bounds.Height / ny;

            left = bounds.Left;
            bottom = bounds.Bottom;

            int i, j, k, l, n;

            // Add vertices.
            var points = polygon.Points;

            for (i = 0; i <= nx; i++)
            {
                x = left + i * dx;

                for (j = 0; j <= ny; j++)
                {
                    y = bottom + j * dy;

                    points.Add(new Vertex(x, y));
                }
            }

            n = 0;

            // Set vertex id and hash.
            foreach (var v in points)
            {
                v.id = v.hash = n++;
            }

            // Add boundary segments.
            var segments = polygon.Segments;

            segments.Capacity = 2 * (nx + ny);

            for (j = 0; j < ny; j++)
            {
                // Left
                segments.Add(new Edge(j, j + 1));

                // Right
                segments.Add(new Edge(nx * (ny + 1) + j, nx * (ny + 1) + (j + 1)));
            }

            for (i = 0; i < nx; i++)
            {
                // Bottom
                segments.Add(new Edge(i * (ny + 1), (i + 1) * (ny + 1)));

                // Top
                segments.Add(new Edge(i * (ny + 1) + nx, (i + 1) * (ny + 1) + nx));
            }

            // Add triangles.
            var triangles = new InputTriangle[2 * nx * ny];

            n = 0;

            for (i = 0; i < nx; i++)
            {
                for (j = 0; j < ny; j++)
                {
                    k = j + (ny + 1) * i;
                    l = j + (ny + 1) * (i + 1);

                    triangles[n++] = new InputTriangle(k, l, l + 1);
                    triangles[n++] = new InputTriangle(k, l + 1, k + 1);
                }
            }

            var converter = new Converter();

            return converter.ToMesh(polygon, triangles);
        }
    }
}
