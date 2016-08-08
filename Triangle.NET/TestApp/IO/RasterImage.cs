// -----------------------------------------------------------------------
// <copyright file="RasterImage.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.IO
{
    using System;
    using TriangleNet;
    using TriangleNet.Rendering.GDI;

    /// <summary>
    /// Writes an image of the mesh to disk.
    /// </summary>
    public class RasterImage
    {
        /// <summary>
        /// Export the mesh to PNG format.
        /// </summary>
        /// <param name="mesh">The current mesh.</param>
        /// <param name="filename">The PNG filename.</param>
        /// <param name="width">The desired width (pixel) of the image.</param>
        public void Export(Mesh mesh, string filename, int width)
        {
            // Check file name
            if (String.IsNullOrWhiteSpace(filename))
            {
                filename = String.Format("mesh-{0}.png", DateTime.Now.ToString("yyyy-M-d-hh-mm-ss"));
            }

            ImageRenderer.Save(mesh, filename, width);
        }
    }
}
