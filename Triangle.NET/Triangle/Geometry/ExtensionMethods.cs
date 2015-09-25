
namespace TriangleNet.Geometry
{
    using TriangleNet.Meshing;

    public static class ExtensionMethods
    {
        #region IPolygon extensions

        /// <summary>
        /// Triangulates a polygon.
        /// </summary>
        public static IMesh Triangulate(this IPolygon polygon)
        {
            return (new GenericMesher()).Triangulate(polygon, null, null);
        }

        /// <summary>
        /// Triangulates a polygon, applying constraint options.
        /// </summary>
        /// <param name="options">Constraint options.</param>
        public static IMesh Triangulate(this IPolygon polygon, ConstraintOptions options)
        {
            return (new GenericMesher()).Triangulate(polygon, options, null);
        }

        /// <summary>
        /// Triangulates a polygon, applying quality options.
        /// </summary>
        /// <param name="quality">Quality options.</param>
        public static IMesh Triangulate(this IPolygon polygon, QualityOptions quality)
        {
            return (new GenericMesher()).Triangulate(polygon, null, quality);
        }

        /// <summary>
        /// Triangulates a polygon, applying quality and constraint options.
        /// </summary>
        /// <param name="options">Constraint options.</param>
        /// <param name="quality">Quality options.</param>
        public static IMesh Triangulate(this IPolygon polygon, ConstraintOptions options, QualityOptions quality)
        {
            return (new GenericMesher()).Triangulate(polygon, options, quality);
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
            return (new GenericMesher(triangulator)).Triangulate(polygon, options, quality);
        }

        #endregion
    }
}
