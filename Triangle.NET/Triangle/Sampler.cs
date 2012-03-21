// -----------------------------------------------------------------------
// <copyright file="Sampler.cs">
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/triangle/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Used for triangle sampling in the Mesh.Locate method.
    /// </summary>
    class Sampler
    {
        static Random rand = new Random(DateTime.Now.Millisecond);

        // Number of random samples for point location (at least 1).
        int samples = 1;

        // Number of triangles in mesh.
        int triangleCount = 0;

        // Empirically chosen factor.
        static int samplefactor = 11;

        // Keys of the triangle dictionary.
        int[] keys;

        /// <summary>
        /// Update sampling parameters if mesh changed.
        /// </summary>
        /// <param name="mesh">Current mesh.</param>
        public void Update(Mesh mesh)
        {
            int count = mesh.triangles.Count;

            // TODO: Is checking the triangle count a good way to monitor mesh changes?
            if (triangleCount != count)
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

                // TODO: Is there a way not calling ToArray()?
                keys = mesh.triangles.Keys.ToArray();
            }
        }

        /// <summary>
        /// Get a random sample set of triangle keys.
        /// </summary>
        /// <returns>Array of triangle keys.</returns>
        public int[] GetSamples()
        {
            int[] randSamples = new int[samples];

            int range = triangleCount / samples;

            for (int i = 0; i < samples; i++)
            {
                // Yeah, rand should be equally distributed, but just to make
                // sure, use a range variable...
                randSamples[i] = keys[rand.Next(i * range, (i + 1) * range - 1)];
            }

            return randSamples;
        }
    }
}
