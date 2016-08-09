// -----------------------------------------------------------------------
// <copyright file="RasterImage.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.IO
{
    using System.IO;
    using System.IO.Compression;
    using TriangleNet;
    using TriangleNet.Rendering.GDI;
    using TriangleNet.Rendering.Text;

    /// <summary>
    /// Writes an image of the mesh to disk.
    /// </summary>
    public class ImageWriter
    {
        /// <summary>
        /// Export the mesh to PNG format.
        /// </summary>
        /// <param name="mesh">The current mesh.</param>
        /// <param name="filename">The PNG filename.</param>
        /// <param name="type">Image type (0 = png, 1 = eps, 2 = svg).</param>
        /// <param name="width">The desired width of the image.</param>
        /// <param name="compress">Use GZip compression (only eps or svg).</param>
        public void Export(Mesh mesh, string filename, int type, int width, bool compress)
        {
            if (type == 1)
            {
                ExportEps(mesh, filename, width, compress);
            }
            else if (type == 2)
            {
                ExportSvg(mesh, filename, width, compress);
            }
            else
            {
                ImageRenderer.Save(mesh, filename, width);
            }
        }

        private void ExportEps(Mesh mesh, string filename, int width, bool compress)
        {
            var eps = new EpsImage();

            eps.Export(mesh, filename, width);

            if (compress)
            {
                CompressFile(filename, true);
            }
        }

        private void ExportSvg(Mesh mesh, string filename, int width, bool compress)
        {
            var svg = new SvgImage();

            svg.Export(mesh, filename, width);

            if (compress)
            {
                CompressFile(filename, true);
            }
        }

        private void CompressFile(string filename, bool cleanup)
        {
            if (!File.Exists(filename))
            {
                return;
            }

            using (var input = File.OpenRead(filename))
            using (var output = File.Create(filename + ".gz"))
            using (var gzip = new GZipStream(output, CompressionMode.Compress))
            {
                input.CopyTo(gzip);
            }

            if (cleanup)
            {
                File.Delete(filename);
            }
        }
    }
}
