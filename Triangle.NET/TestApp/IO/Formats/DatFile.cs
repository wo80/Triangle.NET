// -----------------------------------------------------------------------
// <copyright file="DatFile.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.IO.Formats
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TriangleNet.IO;
    using System.IO;
    using MeshExplorer.Rendering;
    using TriangleNet.Geometry;
    using TriangleNet;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DatFile : IMeshFile
    {
        /// <summary>
        /// Gets the supported file extensions.
        /// </summary>
        public string[] Extensions
        {
            get { return new string[] { ".dat" }; }
        }

        public bool ContainsMeshData(string filename)
        {
            return false;
        }

        public InputGeometry Read(string filename)
        {
            InputGeometry data = new InputGeometry();

            string line;
            string[] split;

            using (TextReader reader = new StreamReader(filename))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    split = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (split.Length == 2)
                    {
                        data.AddPoint(
                            double.Parse(split[0], Util.Nfi),
                            double.Parse(split[1], Util.Nfi));
                    }
                }
            }

            int n = data.Count;

            for (int i = 0; i < n; i++)
            {
                data.AddSegment(i, (i + 1) % n);

            }

            return data;
        }

        public Mesh Import(string filename)
        {
            throw new NotImplementedException();
        }

        public void Write(Mesh mesh, string filename)
        {
            throw new NotImplementedException();
        }
    }
}
