// -----------------------------------------------------------------------
// <copyright file="Sampler.cs">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet
{
    using System;
    using System.Collections.Generic;
    using TriangleNet.Topology;

    /// <summary>
    /// Used for triangle sampling in the <see cref="TriangleLocator"/> class.
    /// </summary>
    class Sampler : IEnumerable<Triangle>
    {
        // Empirically chosen factor.
        private const int samplefactor = 11;

        private Random rand;
        private Mesh mesh;

        // Number of random samples for point location (at least 1).
        private int samples = 1;

        // Number of triangles in mesh.
        private int triangleCount = 0;

        public Sampler(Mesh mesh)
        {
            this.mesh = mesh;
            this.rand = new Random(110503);
        }

        /// <summary>
        /// Reset the sampler.
        /// </summary>
        public void Reset()
        {
            this.samples = 1;
            this.triangleCount = 0;
        }

        /// <summary>
        /// Update sampling parameters if mesh changed.
        /// </summary>
        /// <param name="mesh">Current mesh.</param>
        public void Update(bool forceUpdate = false)
        {
            int count = mesh.triangles.Count;

            // TODO: Is checking the triangle count a good way to monitor mesh changes?
            if (triangleCount != count || forceUpdate)
            {
                triangleCount = count;

                // The number of random samples taken is proportional to the cube root of
                // the number of triangles in the mesh.  The next bit of code assumes
                // that the number of triangles increases monotonically (or at least
                // doesn't decrease enough to matter).
                while (samplefactor * samples * samples * samples < count)
                {
                    samples++;
                }
            }
        }

        public IEnumerator<Triangle> GetEnumerator()
        {
            return mesh.triangles.Sample(samples, rand).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
