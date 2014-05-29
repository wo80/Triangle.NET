
namespace TriangleNet.Meshing
{
    using System;
    using TriangleNet.Geometry;

    public class QualityOptions
    {
        public static QualityOptions Empty
        {
            get { return new QualityOptions(); }
        }

        #region Public properties

        /// <summary>
        /// Gets or sets a maximum angle constraint.
        /// </summary>
        public double MaximumAngle { get; set; }

        /// <summary>
        /// Gets or sets a minimum angle constraint.
        /// </summary>
        public double MinimumAngle { get; set; }

        /// <summary>
        /// Gets or sets a maximum triangle area constraint.
        /// </summary>
        public double MaximumArea { get; set; }

        /// <summary>
        /// Apply a user-defined triangle constraint.
        /// </summary>
        public Func<ITriangle, double, bool> UserTest { get; set; }

        #endregion
    }
}
