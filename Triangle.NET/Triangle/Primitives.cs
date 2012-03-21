// -----------------------------------------------------------------------
// <copyright file="Primitives.cs">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/triangle/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet
{
    using System;
    using TriangleNet.Data;

    /// <summary>
    /// Provides some primitives regularly used in computational geometry.
    /// </summary>
    public static class Primitives
    {
        static double splitter;       // Used to split double factors for exact multiplication.
        static double epsilon;        // Floating-point machine epsilon.
        static double resulterrbound;
        static double ccwerrboundA, ccwerrboundB, ccwerrboundC;
        static double iccerrboundA, iccerrboundB, iccerrboundC;
        static double o3derrboundA, o3derrboundB, o3derrboundC;

        /// <summary>
        /// Initialize the variables used for exact arithmetic.
        /// </summary>
        /// <remarks>
        /// 'epsilon' is the largest power of two such that 1.0 + epsilon = 1.0 in
        /// floating-point arithmetic. 'epsilon' bounds the relative roundoff
        /// error. It is used for floating-point error analysis.
        ///
        /// 'splitter' is used to split floating-point numbers into two half-
        /// length significands for exact multiplication.
        ///
        /// I imagine that a highly optimizing compiler might be too smart for its
        /// own good, and somehow cause this routine to fail, if it pretends that
        /// floating-point arithmetic is too much like real arithmetic.
        ///
        /// Don't change this routine unless you fully understand it.
        /// </remarks>
        public static void ExactInit()
        {
            double half;
            double check, lastcheck;
            bool every_other;

            every_other = true;
            half = 0.5;
            epsilon = 1.0;
            splitter = 1.0;
            check = 1.0;
            // Repeatedly divide 'epsilon' by two until it is too small to add to
            // one without causing roundoff. (Also check if the sum is equal to
            // the previous sum, for machines that round up instead of using exact
            // rounding.  Not that these routines will work on such machines.)
            do
            {
                lastcheck = check;
                epsilon *= half;
                if (every_other)
                {
                    splitter *= 2.0;
                }
                every_other = !every_other;
                check = 1.0 + epsilon;
            } while ((check != 1.0) && (check != lastcheck));
            splitter += 1.0;
            // Error bounds for orientation and incircle tests.
            resulterrbound = (3.0 + 8.0 * epsilon) * epsilon;
            ccwerrboundA = (3.0 + 16.0 * epsilon) * epsilon;
            ccwerrboundB = (2.0 + 12.0 * epsilon) * epsilon;
            ccwerrboundC = (9.0 + 64.0 * epsilon) * epsilon * epsilon;
            iccerrboundA = (10.0 + 96.0 * epsilon) * epsilon;
            iccerrboundB = (4.0 + 48.0 * epsilon) * epsilon;
            iccerrboundC = (44.0 + 576.0 * epsilon) * epsilon * epsilon;
            o3derrboundA = (7.0 + 56.0 * epsilon) * epsilon;
            o3derrboundB = (3.0 + 28.0 * epsilon) * epsilon;
            o3derrboundC = (26.0 + 288.0 * epsilon) * epsilon * epsilon;
        }

        /// <summary>
        /// Check, if the three points appear in counterclockwise order. The result is 
        /// also a rough approximation of twice the signed area of the triangle defined 
        /// by the three points.
        /// </summary>
        /// <param name="pa"></param>
        /// <param name="pb"></param>
        /// <param name="pc"></param>
        /// <returns>Return a positive value if the points pa, pb, and pc occur in 
        /// counterclockwise order; a negative value if they occur in clockwise order; 
        /// and zero if they are collinear.</returns>
        /// <remarks>
        /// Uses exact arithmetic if necessary to ensure a correct answer.  The
        /// result returned is the determinant of a matrix. This determinant is
        /// computed adaptively, in the sense that exact arithmetic is used only to
        /// the degree it is needed to ensure that the returned value has the
        /// correct sign. Hence, this function is usually quite fast, but will run
        /// more slowly when the input points are collinear or nearly so.
        ///
        /// See Robust Predicates paper for details.
        /// </remarks>
        public static double CounterClockwise(Point2 pa, Point2 pb, Point2 pc)
        {
            double detleft, detright, det;
            double detsum, errbound;

            Statistic.CounterClockwiseCount++;

            detleft = (pa.X - pc.X) * (pb.Y - pc.Y);
            detright = (pa.Y - pc.Y) * (pb.X - pc.X);
            det = detleft - detright;

            if (Behavior.NoExact)
            {
                return det;
            }

            if (detleft > 0.0)
            {
                if (detright <= 0.0)
                {
                    return det;
                }
                else
                {
                    detsum = detleft + detright;
                }
            }
            else if (detleft < 0.0)
            {
                if (detright >= 0.0)
                {
                    return det;
                }
                else
                {
                    detsum = -detleft - detright;
                }
            }
            else
            {
                return det;
            }

            errbound = ccwerrboundA * detsum;
            if ((det >= errbound) || (-det >= errbound))
            {
                return det;
            }

            return det;

            // TODO: throw new Exception();
            // return counterclockwiseadapt(pa, pb, pc, detsum);
        }


        /// <summary>
        /// Check if the point pd lies inside the circle passing through pa, pb, and pc. The 
        /// points pa, pb, and pc must be in counterclockwise order, or the sign of the result 
        /// will be reversed.
        /// </summary>
        /// <param name="pa"></param>
        /// <param name="pb"></param>
        /// <param name="pc"></param>
        /// <param name="pd"></param>
        /// <returns>Return a positive value if the point pd lies inside the circle passing through 
        /// pa, pb, and pc; a negative value if it lies outside; and zero if the four points 
        /// are cocircular.</returns>
        /// <remarks>
        /// Uses exact arithmetic if necessary to ensure a correct answer.  The
        /// result returned is the determinant of a matrix.  This determinant is
        /// computed adaptively, in the sense that exact arithmetic is used only to
        /// the degree it is needed to ensure that the returned value has the
        /// correct sign.  Hence, this function is usually quite fast, but will run
        /// more slowly when the input points are cocircular or nearly so.
        ///
        /// See Robust Predicates paper for details.
        /// </remarks>
        public static double InCircle(Point2 pa, Point2 pb, Point2 pc, Point2 pd)
        {
            double adx, bdx, cdx, ady, bdy, cdy;
            double bdxcdy, cdxbdy, cdxady, adxcdy, adxbdy, bdxady;
            double alift, blift, clift;
            double det;
            double permanent, errbound;

            Statistic.InCircleCount++;

            adx = pa.X - pd.X;
            bdx = pb.X - pd.X;
            cdx = pc.X - pd.X;
            ady = pa.Y - pd.Y;
            bdy = pb.Y - pd.Y;
            cdy = pc.Y - pd.Y;

            bdxcdy = bdx * cdy;
            cdxbdy = cdx * bdy;
            alift = adx * adx + ady * ady;

            cdxady = cdx * ady;
            adxcdy = adx * cdy;
            blift = bdx * bdx + bdy * bdy;

            adxbdy = adx * bdy;
            bdxady = bdx * ady;
            clift = cdx * cdx + cdy * cdy;

            det = alift * (bdxcdy - cdxbdy)
                + blift * (cdxady - adxcdy)
                + clift * (adxbdy - bdxady);

            if (Behavior.NoExact)
            {
                return det;
            }

            permanent = (Math.Abs(bdxcdy) + Math.Abs(cdxbdy)) * alift
                      + (Math.Abs(cdxady) + Math.Abs(adxcdy)) * blift
                      + (Math.Abs(adxbdy) + Math.Abs(bdxady)) * clift;
            errbound = iccerrboundA * permanent;
            if ((det > errbound) || (-det > errbound))
            {
                return det;
            }

            return det;

            // TODO: throw new Exception();
            //return incircleadapt(pa, pb, pc, pd, permanent);
        }

        /// <summary>
        /// Return a positive value if the point pd is incompatible with the circle 
        /// or plane passing through pa, pb, and pc (meaning that pd is inside the 
        /// circle or below the plane); a negative value if it is compatible; and 
        /// zero if the four points are cocircular/coplanar. The points pa, pb, and 
        /// pc must be in counterclockwise order, or the sign of the result will be 
        /// reversed.
        /// </summary>
        /// <param name="pa"></param>
        /// <param name="pb"></param>
        /// <param name="pc"></param>
        /// <param name="pd"></param>
        /// <returns></returns>
        /// <remarks>
        /// If the -w switch is used, the points are lifted onto the parabolic
        /// lifting map, then they are dropped according to their weights, then the
        /// 3D orientation test is applied. If the -W switch is used, the points'
        /// heights are already provided, so the 3D orientation test is applied
        /// directly. If neither switch is used, the incircle test is applied.
        /// </remarks>
        public static double NonRegular(Point2 pa, Point2 pb, Point2 pc, Point2 pd)
        {
            return InCircle(pa, pb, pc, pd);
        }

        /// <summary>
        /// Find the circumcenter of a triangle.
        /// </summary>
        /// <param name="torg"></param>
        /// <param name="tdest"></param>
        /// <param name="tapex"></param>
        /// <param name="circumcenter"></param>
        /// <param name="xi"></param>
        /// <param name="eta"></param>
        /// <param name="offcenter"></param>
        /// <remarks>
        /// The result is returned both in terms of x-y coordinates and xi-eta
        /// (barycentric) coordinates. The xi-eta coordinate system is defined in
        /// terms of the triangle: the origin of the triangle is the origin of the
        /// coordinate system; the destination of the triangle is one unit along the
        /// xi axis; and the apex of the triangle is one unit along the eta axis.
        /// This procedure also returns the square of the length of the triangle's
        /// shortest edge.
        /// </remarks>
        public static Point2 FindCircumcenter(Point2 torg, Point2 tdest, Point2 tapex,
                              ref double xi, ref double eta, bool offcenter)
        {
            double xdo, ydo, xao, yao;
            double dodist, aodist, dadist;
            double denominator;
            double dx, dy, dxoff, dyoff;

            Statistic.CircumcenterCount++;

            // Compute the circumcenter of the triangle.
            xdo = tdest.X - torg.X;
            ydo = tdest.Y - torg.Y;
            xao = tapex.X - torg.X;
            yao = tapex.Y - torg.Y;
            dodist = xdo * xdo + ydo * ydo;
            aodist = xao * xao + yao * yao;
            dadist = (tdest.X - tapex.X) * (tdest.X - tapex.X) +
                     (tdest.Y - tapex.Y) * (tdest.Y - tapex.Y);
            if (Behavior.NoExact)
            {
                denominator = 0.5 / (xdo * yao - xao * ydo);
            }
            else
            {
                // Use the counterclockwise() routine to ensure a positive (and
                // reasonably accurate) result, avoiding any possibility of
                // division by zero.
                denominator = 0.5 / CounterClockwise(tdest, tapex, torg);
                // Don't count the above as an orientation test.
                Statistic.CounterClockwiseCount--;
            }
            dx = (yao * dodist - ydo * aodist) * denominator;
            dy = (xdo * aodist - xao * dodist) * denominator;

            // Find the (squared) length of the triangle's shortest edge.  This
            // serves as a conservative estimate of the insertion radius of the
            // circumcenter's parent. The estimate is used to ensure that
            // the algorithm terminates even if very small angles appear in
            // the input PSLG.
            if ((dodist < aodist) && (dodist < dadist))
            {
                if (offcenter && (Behavior.Offconstant > 0.0))
                {
                    // Find the position of the off-center, as described by Alper Ungor.
                    dxoff = 0.5 * xdo - Behavior.Offconstant * ydo;
                    dyoff = 0.5 * ydo + Behavior.Offconstant * xdo;
                    // If the off-center is closer to the origin than the
                    // circumcenter, use the off-center instead.
                    if (dxoff * dxoff + dyoff * dyoff < dx * dx + dy * dy)
                    {
                        dx = dxoff;
                        dy = dyoff;
                    }
                }
            }
            else if (aodist < dadist)
            {
                if (offcenter && (Behavior.Offconstant > 0.0))
                {
                    dxoff = 0.5 * xao + Behavior.Offconstant * yao;
                    dyoff = 0.5 * yao - Behavior.Offconstant * xao;
                    // If the off-center is closer to the origin than the
                    // circumcenter, use the off-center instead.
                    if (dxoff * dxoff + dyoff * dyoff < dx * dx + dy * dy)
                    {
                        dx = dxoff;
                        dy = dyoff;
                    }
                }
            }
            else
            {
                if (offcenter && (Behavior.Offconstant > 0.0))
                {
                    dxoff = 0.5 * (tapex.X - tdest.X) -
                          Behavior.Offconstant * (tapex.Y - tdest.Y);
                    dyoff = 0.5 * (tapex.Y - tdest.Y) +
                          Behavior.Offconstant * (tapex.X - tdest.X);
                    // If the off-center is closer to the destination than the
                    // circumcenter, use the off-center instead.
                    if (dxoff * dxoff + dyoff * dyoff <
                        (dx - xdo) * (dx - xdo) + (dy - ydo) * (dy - ydo))
                    {
                        dx = xdo + dxoff;
                        dy = ydo + dyoff;
                    }
                }
            }

            Point2 circumcenter = new Point2(torg.X + dx, torg.Y + dy);

            // To interpolate vertex attributes for the new vertex inserted at
            // the circumcenter, define a coordinate system with a xi-axis,
            // directed from the triangle's origin to its destination, and
            // an eta-axis, directed from its origin to its apex.
            // Calculate the xi and eta coordinates of the circumcenter.
            xi = (yao * dx - xao * dy) * (2.0 * denominator);
            eta = (xdo * dy - ydo * dx) * (2.0 * denominator);

            return circumcenter;
        }


        /*
        /// <summary>
        /// Return a positive value if the point pd lies below the plane passing 
        /// through pa, pb, and pc; "below" is defined so that pa, pb, and pc appear 
        /// in counterclockwise order when viewed from above the plane. The result is 
        /// also a rough approximation of six times the signed volume of the 
        /// tetrahedron defined by the four points.
        /// </summary>
        /// <param name="pa"></param>
        /// <param name="pb"></param>
        /// <param name="pc"></param>
        /// <param name="pd"></param>
        /// <param name="aheight"></param>
        /// <param name="bheight"></param>
        /// <param name="cheight"></param>
        /// <param name="dheight"></param>
        /// <returns>Return a positive value if the point pd lies below the plane 
        /// passing through pa, pb, and pc. Returns a negative value if pd lies above 
        /// the plane. Returns zero if the points are coplanar.</returns>
        /// <remarks>
        /// Uses exact arithmetic if necessary to ensure a correct answer.  The
        /// result returned is the determinant of a matrix.  This determinant is
        /// computed adaptively, in the sense that exact arithmetic is used only to
        /// the degree it is needed to ensure that the returned value has the
        /// correct sign.  Hence, this function is usually quite fast, but will run
        /// more slowly when the input points are coplanar or nearly so.
        ///
        /// See my Robust Predicates paper for details.
        /// </remarks>
        public static double Orient3d2(Point2 pa, Point2 pb, Point2 pc, Point2 pd,
                      double aheight, double bheight, double cheight, double dheight)
        {
            double adx, bdx, cdx, ady, bdy, cdy, adheight, bdheight, cdheight;
            double bdxcdy, cdxbdy, cdxady, adxcdy, adxbdy, bdxady;
            double det;
            double permanent, errbound;

            Statistic.Orient3dCount++;

            adx = pa.X - pd.X;
            bdx = pb.X - pd.X;
            cdx = pc.X - pd.X;
            ady = pa.Y - pd.Y;
            bdy = pb.Y - pd.Y;
            cdy = pc.Y - pd.Y;
            adheight = aheight - dheight;
            bdheight = bheight - dheight;
            cdheight = cheight - dheight;

            bdxcdy = bdx * cdy;
            cdxbdy = cdx * bdy;

            cdxady = cdx * ady;
            adxcdy = adx * cdy;

            adxbdy = adx * bdy;
            bdxady = bdx * ady;

            det = adheight * (bdxcdy - cdxbdy)
                + bdheight * (cdxady - adxcdy)
                + cdheight * (adxbdy - bdxady);

            if (Behavior.NoExact)
            {
                return det;
            }

            permanent = (Math.Abs(bdxcdy) + Math.Abs(cdxbdy)) * Math.Abs(adheight)
                      + (Math.Abs(cdxady) + Math.Abs(adxcdy)) * Math.Abs(bdheight)
                      + (Math.Abs(adxbdy) + Math.Abs(bdxady)) * Math.Abs(cdheight);
            errbound = o3derrboundA * permanent;
            if ((det > errbound) || (-det > errbound))
            {
                return det;
            }

            throw new Exception();
            //return orient3dadapt(pa, pb, pc, pd, aheight, bheight, cheight, dheight, permanent);
        }
        */
    }
}
