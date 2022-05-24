
namespace TriangleNet
{
    using TriangleNet.Geometry;

    /// <summary>
    /// Geometric predicates interface.
    /// </summary>
    public interface IPredicates
    {
        /// <summary>
        /// Check, if the three points appear in counterclockwise order. The result is 
        /// also a rough approximation of twice the signed area of the triangle defined 
        /// by the three points.
        /// </summary>
        /// <param name="a">Point a.</param>
        /// <param name="b">Point b.</param>
        /// <param name="c">Point c.</param>
        /// <returns>Return a positive value if the points pa, pb, and pc occur in 
        /// counterclockwise order; a negative value if they occur in clockwise order; 
        /// and zero if they are collinear.</returns>
        double CounterClockwise(Point a, Point b, Point c);

        /// <summary>
        /// Check if the point pd lies inside the circle passing through pa, pb, and pc. The 
        /// points pa, pb, and pc must be in counterclockwise order, or the sign of the result 
        /// will be reversed.
        /// </summary>
        /// <param name="a">Point a.</param>
        /// <param name="b">Point b.</param>
        /// <param name="c">Point c.</param>
        /// <param name="p">Point d.</param>
        /// <returns>Return a positive value if the point pd lies inside the circle passing through 
        /// pa, pb, and pc; a negative value if it lies outside; and zero if the four points 
        /// are cocircular.</returns>
        double InCircle(Point a, Point b, Point c, Point p);

        /// <summary>
        /// Find the circumcenter of a triangle.
        /// </summary>
        /// <param name="org">Triangle point.</param>
        /// <param name="dest">Triangle point.</param>
        /// <param name="apex">Triangle point.</param>
        /// <param name="xi">Relative coordinate of new location.</param>
        /// <param name="eta">Relative coordinate of new location.</param>
        /// <returns>Coordinates of the circumcenter</returns>
        /// <remarks>
        /// The result is returned both in terms of x-y coordinates and xi-eta
        /// (barycentric) coordinates. The xi-eta coordinate system is defined in
        /// terms of the triangle: the origin of the triangle is the origin of the
        /// coordinate system; the destination of the triangle is one unit along the
        /// xi axis; and the apex of the triangle is one unit along the eta axis.
        /// This procedure also returns the square of the length of the triangle's
        /// shortest edge.
        /// </remarks>
        Point FindCircumcenter(Point org, Point dest, Point apex, ref double xi, ref double eta);
    
        /// <summary>
        /// Find the circumcenter of a triangle.
        /// </summary>
        /// <param name="org">Triangle point.</param>
        /// <param name="dest">Triangle point.</param>
        /// <param name="apex">Triangle point.</param>
        /// <param name="xi">Relative coordinate of new location.</param>
        /// <param name="eta">Relative coordinate of new location.</param>
        /// <param name="offconstant">Off-center constant.</param>
        /// <returns>Coordinates of the circumcenter (or off-center)</returns>
        Point FindCircumcenter(Point org, Point dest, Point apex, ref double xi, ref double eta,
            double offconstant);
    }
}
