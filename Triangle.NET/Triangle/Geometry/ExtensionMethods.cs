
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

        #endregion

        #region Rectangle extensions

        /// <summary>
        /// Find intersection of a rectangle with a segment.
        /// </summary>
        /// <param name="rect">The rectangle.</param>
        /// <param name="p0">Segment start point.</param>
        /// <param name="p1">Segment end point.</param>
        /// <param name="c0">Intersection associated to start point.</param>
        /// <param name="c1">Intersection associated to end point.</param>
        /// <returns>Returns true, if segment intersects or lies completely in rectangle, otherwise false.</returns>
        /// <remarks>
        /// Liang-Barsky function by Daniel White, http://www.skytopia.com/project/articles/compsci/clipping.html
        /// </remarks>
        public static bool Intersect(this Rectangle rect, Point p0, Point p1, ref Point c0, ref Point c1)
        {
            // Define the x/y clipping values for the border.
            double xmin = rect.Left;
            double xmax = rect.Right;
            double ymin = rect.Bottom;
            double ymax = rect.Top;

            // Define the start and end points of the line.
            double x0 = p0.X;
            double y0 = p0.Y;
            double x1 = p1.X;
            double y1 = p1.Y;

            double t0 = 0.0;
            double t1 = 1.0;

            double dx = x1 - x0;
            double dy = y1 - y0;

            double p = 0.0, q = 0.0, r;

            for (int edge = 0; edge < 4; edge++)
            {
                // Traverse through left, right, bottom, top edges.
                if (edge == 0) { p = -dx; q = -(xmin - x0); }
                if (edge == 1) { p = dx; q = (xmax - x0); }
                if (edge == 2) { p = -dy; q = -(ymin - y0); }
                if (edge == 3) { p = dy; q = (ymax - y0); }
                r = q / p;
                if (p == 0 && q < 0) return false; // Don't draw line at all. (parallel line outside)
                if (p < 0)
                {
                    if (r > t1) return false; // Don't draw line at all.
                    else if (r > t0) t0 = r; // Line is clipped!
                }
                else if (p > 0)
                {
                    if (r < t0) return false; // Don't draw line at all.
                    else if (r < t1) t1 = r; // Line is clipped!
                }
            }

            c0.x = x0 + t0 * dx;
            c0.y = y0 + t0 * dy;
            c1.x = x0 + t1 * dx;
            c1.y = y0 + t1 * dy;

            return true; // (clipped) line is drawn
        }

        #endregion
    }
}
