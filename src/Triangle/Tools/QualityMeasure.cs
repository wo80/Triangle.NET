// -----------------------------------------------------------------------
// <copyright file="QualityMeasure.cs" company="">
// Triangle.NET code by Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Tools
{
    using System;
    using System.Collections.Generic;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;

    /// <summary>
    /// Base class for quality measures.
    /// </summary>
    public abstract class Measure
    {
        protected double min, max, avg, are;

        /// <summary>
        /// Gets the minimum value of the measure.
        /// </summary>
        public double Minimum => min;

        /// <summary>
        /// Gets the maximum value of the measure.
        /// </summary>
        public double Maximum => max;

        /// <summary>
        /// Gets the average value of the measure.
        /// </summary>
        public double Average => avg;

        /// <summary>
        /// Gets the value averaged over all triangles and weighted by area.
        /// </summary>
        public double Area => are;

        /// <summary>
        /// Initialize the measure.
        /// </summary>
        public virtual void Initialize()
        {
            min = double.MaxValue;
            max = -double.MaxValue;
            avg = 0d;
            are = 0d;
        }

        /// <summary>
        /// Compute measure of given triangle.
        /// </summary>
        /// <param name="ab">Side length ab.</param>
        /// <param name="bc">Side length bc.</param>
        /// <param name="ca">Side length ca.</param>
        /// <param name="area">Triangle area.</param>
        public abstract double Update(double ab, double bc, double ca, double area);

        /// <summary>
        /// Finalize the measure.
        /// </summary>
        /// <param name="n">Total number of triangles measured.</param>
        /// <param name="totalArea">Total area of triangles measured.</param>
        public virtual void Finalize(int n, double totalArea)
        {
            avg = n > 0 ? avg / n : avg;

            are = totalArea > 0.0 ? are / totalArea : are;
        }
    }

    /// <summary>
    /// Provides mesh quality information.
    /// </summary>
    /// <remarks>
    /// Given a triangle abc with points A (ax, ay), B (bx, by), C (cx, cy).
    /// 
    /// The side lengths are given as
    ///   a = sqrt((cx - bx)^2 + (cy - by)^2) -- side BC opposite of A
    ///   b = sqrt((cx - ax)^2 + (cy - ay)^2) -- side CA opposite of B
    ///   c = sqrt((ax - bx)^2 + (ay - by)^2) -- side AB opposite of C
    ///   
    /// The angles are given as
    ///   ang_a = acos((b^2 + c^2 - a^2)  / (2 * b * c)) -- angle at A
    ///   ang_b = acos((c^2 + a^2 - b^2)  / (2 * c * a)) -- angle at B
    ///   ang_c = acos((a^2 + b^2 - c^2)  / (2 * a * b)) -- angle at C
    ///   
    /// The semiperimeter is given as
    ///   s = (a + b + c) / 2
    ///   
    /// The area is given as
    ///   D = abs(ax * (by - cy) + bx * (cy - ay) + cx * (ay - by)) / 2
    ///     = sqrt(s * (s - a) * (s - b) * (s - c))
    ///      
    /// The inradius is given as
    ///   r = D / s
    ///   
    /// The circumradius is given as
    ///   R = a * b * c / (4 * D)
    /// 
    /// The altitudes are given as
    ///   alt_a = 2 * D / a -- altitude above side a
    ///   alt_b = 2 * D / b -- altitude above side b
    ///   alt_c = 2 * D / c -- altitude above side c
    /// 
    /// The aspect ratio may be given as the ratio of the longest to the
    /// shortest edge or, more commonly as the ratio of the circumradius 
    /// to twice the inradius
    ///   ar = R / (2 * r)
    ///      = a * b * c / (8 * (s - a) * (s - b) * (s - c))
    ///      = a * b * c / ((b + c - a) * (c + a - b) * (a + b - c))
    /// </remarks>
    public class QualityMeasure
    {
        MeasureArea _area;
        MeasureAlpha _alpha;
        MeasureEta _eta;
        MeasureQ _q;

        #region Public properties

        /// <summary>
        /// Gets the area measure.
        /// </summary>
        public Measure Area => _area;

        /// <summary>
        /// Gets the alpha measure.
        /// </summary>
        /// <remarks>
        /// The alpha measure computes the minimum angle among all triangles.
        /// The best possible value is 1, and the worst 0.
        /// </remarks>
        public Measure Alpha => _alpha;

        /// <summary>
        /// Gets the eta measure.
        /// </summary>
        /// <remarks>
        /// The eta measure relates the area of a triangle a to its edge lengths.
        /// The best possible value is 1, and the worst 0.
        /// </remarks>
        public Measure Eta => _eta;

        /// <summary>
        /// Gets the Q measure, also knows as normalized shape ratio (NSR).
        /// </summary>
        /// <remarks>
        /// The Q measure relates the incircle to the circumcircle radius.
        /// In an ideally regular mesh, all triangles would have the same
        /// equilateral shape, for which Q = 1.
        /// </remarks>
        public Measure Q => _q;

        /// <summary>
        /// Gets the total triangulation area.
        /// </summary>
        public double AreaTotal => _area.Area;

        #endregion

        List<Measure> measures;

        /// <summary>
        /// Initializes a new instance of the <see cref="QualityMeasure" /> class.
        /// </summary>
        public QualityMeasure()
        {
            _area = new MeasureArea();
            _alpha = new MeasureAlpha();
            _eta = new MeasureEta();
            _q = new MeasureQ();

            measures = new List<Measure>()
            {
                _area, _alpha, _eta, _q
            };
        }

        /// <summary>
        /// Add a custom measure.
        /// </summary>
        /// <param name="measure"></param>
        public void Add(Measure measure)
        {
            measures.Add(measure);
        }

        /// <summary>
        /// Update all measures for the given mesh.
        /// </summary>
        /// <param name="mesh">The mesh.</param>
        public void Update(IMesh mesh)
        {
            Update(mesh.Triangles);
        }

        /// <summary>
        /// Update all measures for the given triangles.
        /// </summary>
        /// <param name="triangles">The triangles.</param>
        public void Update(IEnumerable<ITriangle> triangles)
        {
            Point a, b, c;
            double ab, bc, ca;
            double lx, ly;
            double area;

            int n = 0;

            foreach (var m in measures)
            {
                m.Initialize();
            }

            foreach (var tri in triangles)
            {
                n++;

                a = tri.GetVertex(0);
                b = tri.GetVertex(1);
                c = tri.GetVertex(2);

                lx = a.x - b.x;
                ly = a.y - b.y;
                ab = Math.Sqrt(lx * lx + ly * ly);
                lx = b.x - c.x;
                ly = b.y - c.y;
                bc = Math.Sqrt(lx * lx + ly * ly);
                lx = c.x - a.x;
                ly = c.y - a.y;
                ca = Math.Sqrt(lx * lx + ly * ly);

                area = 0.5 * Math.Abs(a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y));

                foreach (var m in measures)
                {
                    m.Update(ab, bc, ca, area);
                }
            }

            var totalArea = _area.Area;

            foreach (var m in measures)
            {
                m.Finalize(n, totalArea);
            }
        }

        /// <summary>
        /// Determines the bandwidth of the coefficient matrix.
        /// </summary>
        /// <returns>Bandwidth of the coefficient matrix.</returns>
        /// <remarks>
        /// The quantity computed here is the "geometric" bandwidth determined
        /// by the finite element mesh alone.
        ///
        /// If a single finite element variable is associated with each node
        /// of the mesh, and if the nodes and variables are numbered in the
        /// same way, then the geometric bandwidth is the same as the bandwidth
        /// of a typical finite element matrix.
        ///
        /// The bandwidth M is defined in terms of the lower and upper bandwidths:
        ///
        ///   M = ML + 1 + MU
        ///
        /// where 
        ///
        ///   ML = maximum distance from any diagonal entry to a nonzero
        ///   entry in the same row, but earlier column,
        ///
        ///   MU = maximum distance from any diagonal entry to a nonzero
        ///   entry in the same row, but later column.
        ///
        /// Because the finite element node adjacency relationship is symmetric,
        /// we are guaranteed that ML = MU.
        /// 
        /// Based on Matlab code by John Burkardt, Florida State University.
        /// </remarks>
        public static int Bandwidth(IMesh mesh)
        {
            if (mesh == null) return 0;

            // Lower and upper bandwidth of the matrix
            int ml = 0, mu = 0;

            int gi, gj;

            foreach (var tri in mesh.Triangles)
            {
                for (int j = 0; j < 3; j++)
                {
                    gi = tri.GetVertexID(j);

                    for (int k = 0; k < 3; k++)
                    {
                        gj = tri.GetVertexID(k);

                        mu = Math.Max(mu, gj - gi);
                        ml = Math.Max(ml, gi - gj);
                    }
                }
            }

            return ml + 1 + mu;
        }

        class MeasureArea : Measure
        {
            private readonly double EPS = 0.0;

            // Number of triangles with zero area
            public int zero;

            /// <inheritdoc />
            public override void Initialize()
            {
                zero = 0;

                base.Initialize();
            }

            /// <inheritdoc />
            public override double Update(double ab, double bc, double ca, double area)
            {
                min = Math.Min(min, area);
                max = Math.Max(max, area);

                are += area;

                if (area <= EPS)
                {
                    zero++;
                }

                return area;
            }

            /// <inheritdoc />
            public override void Finalize(int n, double area_total)
            {
                avg = n > 0 ? are / n : are;
            }
        }

        /// <summary>
        /// The alpha measure determines the triangulated point set quality.
        /// </summary>
        /// <remarks>
        /// The alpha measure evaluates the uniformity of the shapes of the triangles
        /// defined by a triangulated point set.
        ///
        /// We compute the minimum angle among all the triangles in the triangulated
        /// dataset and divide by the maximum possible value (which, in degrees,
        /// is 60). The best possible value is 1, and the worst 0. A good
        /// triangulation should have an alpha score close to 1.
        /// </remarks>
        class MeasureAlpha : Measure
        {
            /// <inheritdoc />
            public override double Update(double ab, double bc, double ca, double area)
            {
                double alpha = double.MaxValue;

                double ab2 = ab * ab;
                double bc2 = bc * bc;
                double ca2 = ca * ca;

                double a_angle;
                double b_angle;
                double c_angle;

                // Take care of a ridiculous special case.
                if (ab == 0.0 && bc == 0.0 && ca == 0.0)
                {
                    a_angle = 2.0 * Math.PI / 3.0;
                    b_angle = 2.0 * Math.PI / 3.0;
                    c_angle = 2.0 * Math.PI / 3.0;
                }
                else
                {
                    if (ca == 0.0 || ab == 0.0)
                    {
                        a_angle = Math.PI;
                    }
                    else
                    {
                        a_angle = acos((ca2 + ab2 - bc2) / (2.0 * ca * ab));
                    }

                    if (ab == 0.0 || bc == 0.0)
                    {
                        b_angle = Math.PI;
                    }
                    else
                    {
                        b_angle = acos((ab2 + bc2 - ca2) / (2.0 * ab * bc));
                    }

                    if (bc == 0.0 || ca == 0.0)
                    {
                        c_angle = Math.PI;
                    }
                    else
                    {
                        c_angle = acos((bc2 + ca2 - ab2) / (2.0 * bc * ca));
                    }
                }

                alpha = Math.Min(alpha, a_angle);
                alpha = Math.Min(alpha, b_angle);
                alpha = Math.Min(alpha, c_angle);

                // Normalize angle from [0,pi/3] radians into qualities in [0,1].
                alpha = alpha * 3.0 / Math.PI;

                avg += alpha;
                are += area * alpha;

                min = Math.Min(alpha, min);
                max = Math.Max(alpha, max);

                return alpha;

                double acos(double x) => (x <= -1.0) ? Math.PI : ((1.0 <= x) ? 0.0 : Math.Acos(x));
            }
        }

        /// <summary>
        /// The eta measure determines the triangulated point set quality.
        /// </summary>
        /// <remarks>
        /// The eta measure evaluates the uniformity of the shapes of the triangles
        /// defined by a triangulated point set.
        ///
        /// The measure relates the area of a triangle a to its edge lengths. The best
        /// possible value is 1, and the worst 0. A good triangulation should have an
        /// eta score close to 1.
        /// </remarks>
        class MeasureEta : Measure
        {
            // Normalization factor to ensure that a perfect triangle
            // (equal edges) has a quality of 1.
            private readonly static double Factor = 4d * Math.Sqrt(3);

            /// <inheritdoc />
            public override double Update(double ab, double bc, double ca, double area)
            {
                double ab2 = ab * ab;
                double bc2 = bc * bc;
                double ca2 = ca * ca;

                double eta = Factor * area / (ab2 + bc2 + ca2);

                avg += eta;
                are += area * eta;

                min = Math.Min(eta, min);
                max = Math.Max(eta, max);

                return eta;
            }
        }

        /// <summary>
        /// The Q measure determines the triangulated point set quality.
        /// </summary>
        /// <remarks>
        /// The Q measure evaluates the uniformity of the shapes of the triangles
        /// defined by a triangulated point set. It uses the aspect ratio
        ///
        ///    2 * (incircle radius) / (circumcircle radius)
        ///
        /// In an ideally regular mesh, all triangles would have the same
        /// equilateral shape, for which Q = 1. A good mesh would have
        /// 0.5 &lt; Q.
        /// </remarks>
        class MeasureQ : Measure
        {
            /// <inheritdoc />
            public override double Update(double ab, double bc, double ca, double area)
            {
                double q = (bc + ca - ab) * (ca + ab - bc) * (ab + bc - ca) / (ab * bc * ca);

                min = Math.Min(min, q);
                max = Math.Max(max, q);

                avg += q;
                are += q * area;

                return q;
            }
        }
    }
}
