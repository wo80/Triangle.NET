
namespace TriangleNet.Meshing
{
    public class ConstraintOptions
    {
        public static ConstraintOptions Empty
        {
            get { return new ConstraintOptions(); }
        }

        #region Public properties

        /// <summary>
        /// Gets or sets a value indicating wether to use regions.
        /// </summary>
        public bool UseRegions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating wether to create a Conforming
        /// Delaunay triangulation.
        /// </summary>
        public bool ConformingDelaunay { get; set; }

        /// <summary>
        /// Enclose the convex hull with segments.
        /// </summary>
        public bool Convex { get; set; }

        #endregion
    }
}
