// -----------------------------------------------------------------------
// <copyright file="UcdFile.cs" company="">
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
    public class JsonFile : MeshExplorer.IO.IMeshFormat
    {
        /// <summary>
        /// Gets the supported file extensions.
        /// </summary>
        public string[] Extensions
        {
            get { return new string[] { ".json" }; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public InputGeometry Read(string filename)
        {
            string json = File.ReadAllText(filename);

            JsonParser parser = new JsonParser("json");
            object rawData = parser.Decode();

            InputGeometry data = new InputGeometry();

            List<ITriangle> triangles = new List<ITriangle>();

            return data;
        }

        public void Write(string filename, Mesh data)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                int nv = data.NumberOfVertices;
                int ns = data.NumberOfSegments;
                int ne = data.NumberOfTriangles;
                int nn = data.NumberOfTriangles;
                int i = 0;

                string sep = String.Empty;

                // Header
                writer.Write("{{\"format\":\"{0}\",\"dim\":2", ne > 0 ? "mesh" : (ns > 0 ? "poly" : "nodes"));

                // Write the coordinates
                writer.Write(",\"points\":[");
                foreach (var item in data.Vertices)
                {
                    sep = (i == nv - 1) ? String.Empty : ", ";

                    writer.Write("{0},{1}{2}", 
                        item.X.ToString(Util.Nfi),
                        item.Y.ToString(Util.Nfi), sep);

                    i++;
                }
                writer.Write("]");

                // Write the segments
                if (ns > 0)
                {
                    writer.Write(",\"segments\":[");
                    foreach (var item in data.Segments)
                    {
                        sep = (i == ns - 1) ? String.Empty : ", ";

                        writer.Write("{0},{1}{2}",
                            item.P0, item.P1, sep);

                        i++;
                    }

                    writer.Write("]");
                }
                
                // Write the elements
                if (ne > 0)
                {
                    writer.Write(",\"triangles\":[");
                    foreach (var item in data.Triangles)
                    {
                        sep = (i == ne - 1) ? String.Empty : ", ";

                        writer.Write("{0},{1},{2}{3}",
                            item.P0, item.P1, item.P2, sep);

                        i++;
                    }
                    writer.Write("]");
                }

                // Write neighbors
                if (nn > 0)
                {
                    writer.Write(",\"neighbors\":[");
                    //for (int i = 0; i < nn; i++)
                    //{
                    //    sep = (i == nn - 1) ? String.Empty : ", ";

                    //    writer.Write("{0},{1},{2}{3}",
                    //        data.Neighbors[i][0],
                    //        data.Neighbors[i][1],
                    //        data.Neighbors[i][2], sep);

                    //}
                    writer.Write("]");
                }

                writer.Write("}");
            }
        }
    }
}