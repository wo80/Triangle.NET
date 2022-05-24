// -----------------------------------------------------------------------
// <copyright file="Interpolation.cs">
// Triangle Copyright (c) 1993, 1995, 1997, 1998, 2002, 2005 Jonathan Richard Shewchuk
// Triangle.NET code by Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Tools
{
    using TriangleNet.Geometry;

    /// <summary>
    /// Interpolation helper.
    /// </summary>
    public static class Interpolation
    {
        /// <summary>
        /// Linear interpolation of a point.
        /// </summary>
        /// <param name="tri">The triangle containing the point <paramref name="p"/></param>
        /// <param name="p">The point to interpolate.</param>
        /// <param name="data">The vertex data (z values).</param>
        /// <returns>The linear interpolation value.</returns>
        /// <remarks>
        /// IMPORTANT: this method assumes the mesh vertex ids correspond to the data array indices.
        /// </remarks>
        public static double InterpolatePoint(ITriangle tri, Point p, double[] data)
        {
            var org = tri.GetVertex(0);
            var dest = tri.GetVertex(1);
            var apex = tri.GetVertex(2);

            double xdo = dest.x - org.x;
            double ydo = dest.y - org.y;
            double xao = apex.x - org.x;
            double yao = apex.y - org.y;

            double denominator = 0.5 / (xdo * yao - xao * ydo);

            double dx = p.x - org.x;
            double dy = p.y - org.y;

            // To interpolate z value for the given point inserted, define a
            // coordinate system with a xi-axis, directed from the triangle's
            // origin to its destination, and an eta-axis, directed from its
            // origin to its apex.
            double xi = (yao * dx - xao * dy) * (2.0 * denominator);
            double eta = (xdo * dy - ydo * dx) * (2.0 * denominator);

            double orgz = data[org.id];

            return orgz + xi * (data[dest.id] - orgz) + eta * (data[apex.id] - orgz);
        }

#if USE_ATTRIBS
        /// <summary>
        /// Linear interpolation of vertex attributes.
        /// </summary>
        /// <param name="vertex">The interpolation vertex.</param>
        /// <param name="triangle">The triangle containing the vertex.</param>
        /// <param name="n">The number of vertex attributes.</param>
        /// <remarks>
        /// The vertex is expected to lie inside the triangle.
        /// </remarks>
        public static void InterpolateAttributes(Vertex vertex, ITriangle triangle, int n)
        {
            var org = triangle.GetVertex(0);
            var dest = triangle.GetVertex(1);
            var apex = triangle.GetVertex(2);

            double xdo = dest.x - org.x;
            double ydo = dest.y - org.y;
            double xao = apex.x - org.x;
            double yao = apex.y - org.y;

            double denominator = 0.5 / (xdo * yao - xao * ydo);

            double dx = vertex.x - org.x;
            double dy = vertex.y - org.y;

            // To interpolate vertex attributes for the new vertex, define a
            // coordinate system with a xi-axis directed from the triangle's
            // origin to its destination, and an eta-axis, directed from its
            // origin to its apex.
            double xi = (yao * dx - xao * dy) * (2.0 * denominator);
            double eta = (xdo * dy - ydo * dx) * (2.0 * denominator);
        
            for (int i = 0; i < n; i++)
            {
                // Interpolate the vertex attributes.
                vertex.attributes[i] = org.attributes[i]
                    + xi * (dest.attributes[i] - org.attributes[i])
                    + eta * (apex.attributes[i] - org.attributes[i]);
            }
        }
#endif

#if USE_Z
        /// <summary>
        /// Linear interpolation of a scalar value.
        /// </summary>
        /// <param name="p">The interpolation point.</param>
        /// <param name="triangle">The triangle containing the point.</param>
        /// <remarks>
        /// The point is expected to lie inside the triangle.
        /// </remarks>
        public static void InterpolateZ(Point p, ITriangle triangle)
        {
            var org = triangle.GetVertex(0);
            var dest = triangle.GetVertex(1);
            var apex = triangle.GetVertex(2);

            // Compute the circumcenter of the triangle.
            double xdo = dest.x - org.x;
            double ydo = dest.y - org.y;
            double xao = apex.x - org.x;
            double yao = apex.y - org.y;

            double denominator = 0.5 / (xdo * yao - xao * ydo);

            double dx = p.x - org.x;
            double dy = p.y - org.y;

            // To interpolate z value for the given point inserted, define a
            // coordinate system with a xi-axis, directed from the triangle's
            // origin to its destination, and an eta-axis, directed from its
            // origin to its apex.
            double xi = (yao * dx - xao * dy) * (2.0 * denominator);
            double eta = (xdo * dy - ydo * dx) * (2.0 * denominator);

            p.z = org.z + xi * (dest.z - org.z) + eta * (apex.z - org.z);
        }
#endif
    }
}
