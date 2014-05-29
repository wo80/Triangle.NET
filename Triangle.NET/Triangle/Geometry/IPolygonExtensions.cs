
namespace TriangleNet.Geometry
{
    using TriangleNet.Meshing;

    public static class IPolygonExtensions
    {
        /// <summary>
        /// Triangulates a polygon.
        /// </summary>
        public static IMesh Triangulate(this IPolygon polygon)
        {
            var mesher = new GenericMesher();

            return mesher.Triangulate(polygon, null, null);
        }

        /// <summary>
        /// Triangulates a polygon, applying constraint options.
        /// </summary>
        /// <param name="options">Constraint options.</param>
        public static IMesh Triangulate(this IPolygon polygon, ConstraintOptions options)
        {
            var mesher = new GenericMesher();

            return mesher.Triangulate(polygon, options, null);
        }

        /// <summary>
        /// Triangulates a polygon, applying quality options.
        /// </summary>
        /// <param name="quality">Quality options.</param>
        public static IMesh Triangulate(this IPolygon polygon, QualityOptions quality)
        {
            var mesher = new GenericMesher();

            return mesher.Triangulate(polygon, null, quality);
        }

        /// <summary>
        /// Triangulates a polygon, applying quality and constraint options.
        /// </summary>
        /// <param name="options">Constraint options.</param>
        /// <param name="quality">Quality options.</param>
        public static IMesh Triangulate(this IPolygon polygon, ConstraintOptions options, QualityOptions quality)
        {
            var mesher = new GenericMesher();

            var mesh = (Mesh)mesher.Triangulate(polygon.Points);

            mesh.ApplyConstraints(polygon, options, quality);

            return mesh;
        }

        /// <summary>
        /// Triangulates a polygon, applying quality and constraint options.
        /// </summary>
        /// <param name="options">Constraint options.</param>
        /// <param name="quality">Quality options.</param>
        /// <param name="triangulator">The triangulation algorithm.</param>
        public static IMesh Triangulate(this IPolygon polygon, ConstraintOptions options, QualityOptions quality,
            ITriangulator triangulator)
        {
            var mesher = new GenericMesher(triangulator);

            var mesh = (Mesh)mesher.Triangulate(polygon.Points);

            mesh.ApplyConstraints(polygon, options, quality);

            return mesh;
        }
    }
}
