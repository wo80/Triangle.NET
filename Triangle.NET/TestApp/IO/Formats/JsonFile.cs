// -----------------------------------------------------------------------
// <copyright file="JsonFile.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.IO.Formats
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using TriangleNet;
    using TriangleNet.Topology;
    using TriangleNet.Geometry;
    using TriangleNet.IO;
    using TriangleNet.Meshing;

    /// <summary>
    /// Read and write JSON files.
    /// </summary>
    /// <remarks>
    /// The JSON format:
    /// {
    ///   "config": {
    ///     "ver": 1,
    ///     "type": "mesh" | "poly" | "points",
    ///     "dim": 2
    ///   },
    ///   "points": {
    ///     "data": [ p0.x, p0.y ... pn.x, pn.y ],
    ///     "markers": [ ... ],
    ///     "attributes": [ ... ]
    ///   },
    ///   "segments": {
    ///     "data": [ s0(1), s0(2) ... sn(1), sn(2) ],
    ///     "markers": [ ... ]
    ///   },
    ///   "holes": [ h0.x, h0.y ... hn.x, hn.y ],
    ///   "triangles":  {
    ///     "data": [ t0(1), t0(2), t0(3) ... tn(1), tn(2), tn(3) ],
    ///     "neighbors": [ t0.n1, t0.n2, t0.n3 ... tn.n1, tn.n2, tn.n3 ],
    ///     "attributes": [ ... ]
    ///   }
    /// }
    /// </remarks>
    public class JsonFile : IMeshFile
    {
        string file;
        Dictionary<string, object> json;

        /// <summary>
        /// Gets the supported file extensions.
        /// </summary>
        public string[] Extensions
        {
            get { return new string[] { ".json" }; }
        }

        public bool ContainsMeshData(string filename)
        {
            ParseJson(filename);

            if (this.json.ContainsKey("config"))
            {
                var config = this.json["config"] as Dictionary<string, object>;

                if (config != null && config.ContainsKey("type"))
                {
                    return config["type"].ToString() == "mesh";
                }
            }

            return false;
        }

        public bool IsSupported(string file)
        {
            throw new NotImplementedException();
        }

        public IMesh Import(string filename)
        {
            var geometry = (Polygon)this.Read(filename);

            List<ITriangle> triangles = null;

            if (this.json.ContainsKey("triangles"))
            {
                var tri = this.json["triangles"] as Dictionary<string, object>;

                if (tri != null)
                {
                    triangles = ReadTriangles(tri, geometry.Points.Count);
                }
            }

            return Converter.ToMesh(geometry, triangles);
        }

        public void Write(IMesh mesh, string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                int nv = mesh.Vertices.Count;
                int ns = mesh.Segments.Count;
                int nh = mesh.Holes.Count;
                int ne = mesh.Triangles.Count;

                writer.Write("{");

                // Config header
                writer.Write("\"config\":{");
                writer.Write("\"ver\":1,");
                writer.Write("\"type\":\"{0}\",", ne > 0 ? "mesh" : (ns > 0 ? "poly" : "points"));
                writer.Write("\"dim\":2");
                writer.Write("}");

                if (((Mesh)mesh).CurrentNumbering == NodeNumbering.None)
                {
                    ((Mesh)mesh).Renumber(NodeNumbering.Linear);
                }

                // Write the coordinates
                if (nv > 0)
                {
                    writer.Write(",");
                    WritePoints((Mesh)mesh, writer, nv);
                }

                // Write the segments
                if (ns > 0)
                {
                    writer.Write(",");
                    WriteSegments(mesh.Segments, writer, ns);
                }

                // Write the holes
                if (nh > 0)
                {
                    writer.Write(",");
                    WriteHoles(mesh.Holes, writer, nh);
                }

                // Write the elements
                if (ne > 0)
                {
                    writer.Write(",");
                    WriteTriangles(mesh.Triangles, writer, ne);
                }

                writer.Write("}");
            }
        }

        public void Write(IMesh mesh, Stream stream)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public IPolygon Read(string filename)
        {
            ParseJson(filename);

            var data = new Polygon();

            if (json == null)
            {
                // TODO: Exception?
                return data;
            }

            if (json.ContainsKey("config"))
            {

            }

            int count = 0;

            if (json.ContainsKey("points"))
            {
                var points = json["points"] as Dictionary<string, object>;

                if (points != null)
                {
                    ReadPoints(data, points, ref count);
                }
                else
                {
                    // TODO: Exception?
                    return data;
                }
            }

            if (json.ContainsKey("segments"))
            {
                var segments = json["segments"] as Dictionary<string, object>;

                if (segments != null)
                {
                    ReadSegments(data, segments, count);
                }
            }

            if (json.ContainsKey("holes"))
            {
                var holes = json["holes"] as ArrayList;

                if (holes != null)
                {
                    ReadHoles(data, holes);
                }
            }

            return data;
        }

        public void Write(IPolygon polygon, string filename)
        {
            throw new NotImplementedException();
        }

        public void Write(IPolygon polygon, Stream stream)
        {
            throw new NotImplementedException();
        }

        private void ParseJson(string filename)
        {
            if (this.json == null || this.file != filename)
            {
                this.file = filename;

                string content = File.ReadAllText(filename);

                JsonParser parser = new JsonParser(content);
                this.json = parser.Decode() as Dictionary<string, object>;
            }
        }

        #region Read helpers

        private void ReadPoints(Polygon geometry, Dictionary<string, object> points, ref int count)
        {
            ArrayList data = points["data"] as ArrayList;

            ArrayList markers = null;
            ArrayList attributes = null;

            if (points.ContainsKey("markers"))
            {
                markers = points["markers"] as ArrayList;
            }

            if (points.ContainsKey("attributes"))
            {
                attributes = points["attributes"] as ArrayList;
            }

            if (data != null)
            {
                int mark, n = data.Count;

                if (n % 2 != 0)
                {
                    throw new Exception("JSON format error (points).");
                }

                // Number of points
                count = n / 2;

                for (int i = 0; i < n; i += 2)
                {
                    mark = 0;

                    if (markers != null && markers.Count == count)
                    {
                        mark = int.Parse(markers[i / 2].ToString());
                    }

                    geometry.Add(new Vertex(
                        double.Parse(data[i].ToString(), Util.Nfi),
                        double.Parse(data[i + 1].ToString(), Util.Nfi),
                        mark
                    ));
                }
            }
        }

        private void ReadSegments(Polygon geometry, Dictionary<string, object> segments, int count)
        {
            ArrayList data = segments["data"] as ArrayList;

            ArrayList markers = null;

            if (segments.ContainsKey("markers"))
            {
                markers = segments["markers"] as ArrayList;
            }

            if (data != null)
            {
                int mark, n = data.Count;

                if (n % 2 != 0)
                {
                    throw new Exception("JSON format error (segments).");
                }

                int p0, p1;

                throw new NotImplementedException();
                // TODO: Fix JSON format

                for (int i = 0; i < n; i += 2)
                {
                    mark = 0;

                    if (markers != null && markers.Count == n)
                    {
                        mark = int.Parse(markers[i / 2].ToString());
                    }

                    p0 = int.Parse(data[i].ToString());
                    p1 = int.Parse(data[i + 1].ToString());

                    if (p0 < 0 || p0 >= count || p1 < 0 || p1 >= count)
                    {
                        throw new Exception("JSON format error (segment index).");
                    }

                    //geometry.Add(new Edge(p0, p1, mark));
                }
            }
        }

        private void ReadHoles(Polygon geometry, ArrayList holes)
        {
            int n = holes.Count;

            if (n % 2 != 0)
            {
                throw new Exception("JSON format error (holes).");
            }

            for (int i = 0; i < n; i += 2)
            {
                geometry.Holes.Add(new Point(
                    double.Parse(holes[i].ToString(), Util.Nfi),
                    double.Parse(holes[i + 1].ToString(), Util.Nfi)
                ));
            }
        }

        private List<ITriangle> ReadTriangles(Dictionary<string, object> triangles, int points)
        {
            ArrayList data = triangles["data"] as ArrayList;

            ArrayList neighbors = null;
            ArrayList attributes = null;

            if (triangles.ContainsKey("neighbors"))
            {
                neighbors = triangles["neighbors"] as ArrayList;
            }

            if (triangles.ContainsKey("attributes"))
            {
                attributes = triangles["attributes"] as ArrayList;
            }

            List<ITriangle> output = null;

            if (data != null)
            {
                int n = data.Count;

                if (n % 3 != 0)
                {
                    throw new Exception("JSON format error (triangles).");
                }

                output = new List<ITriangle>(n / 3);

                int p0, p1, p2, n0, n1, n2;

                for (int i = 0; i < n; i += 3)
                {
                    p0 = int.Parse(data[i].ToString());
                    p1 = int.Parse(data[i + 1].ToString());
                    p2 = int.Parse(data[i + 2].ToString());

                    n0 = n1 = n2 = -1;

                    if (p0 < 0 || p0 >= points || p1 < 0 || p1 >= points || p2 < 0 || p2 >= points)
                    {
                        throw new Exception("JSON format error (triangle index).");
                    }

                    if (neighbors.Count == n)
                    {
                        n0 = int.Parse(neighbors[i].ToString());
                        n1 = int.Parse(neighbors[i + 1].ToString());
                        n2 = int.Parse(neighbors[i + 2].ToString());
                    }

                    // TODO: Set neighbors
                    output.Add(new InputTriangle(p0, p1, p2));
                }
            }

            return output;
        }

        #endregion

        #region Write helpers

        private void WritePoints(Mesh mesh, StreamWriter writer, int nv)
        {
            bool useMarkers = false;

            StringBuilder markers;

            writer.Write("\"points\":{\"data\":[");

            if (mesh.CurrentNumbering == NodeNumbering.Linear)
            {
                markers = WritePoints(mesh.Vertices, writer, nv, useMarkers);
            }
            else
            {
                Vertex[] nodes = new Vertex[mesh.Vertices.Count];

                foreach (var node in mesh.Vertices)
                {
                    nodes[node.ID] = node;
                }

                markers = WritePoints(nodes, writer, nv, useMarkers);
            }

            writer.Write("]");
            if (useMarkers)
            {
                writer.Write(",\"markers\":[" + markers.ToString() + "]");
            }

            // TODO: writer.Write(",\"attributes\":[]");
            writer.Write("}");
        }

        private static StringBuilder WritePoints(IEnumerable<Point> data, StreamWriter writer, int nv, bool useMarkers)
        {
            StringBuilder markers = new StringBuilder();

            int i = 0;
            string seperator;
            foreach (var item in data)
            {
                seperator = (i == nv - 1) ? String.Empty : ", ";

                writer.Write("{0},{1}{2}",
                    item.X.ToString(Util.Nfi),
                    item.Y.ToString(Util.Nfi), seperator);

                if (item.Label > 0)
                {
                    useMarkers = true;
                }

                markers.AppendFormat("{0}{1}", item.Label, seperator);

                i++;
            }

            return markers;
        }

        private void WriteHoles(IEnumerable<Point> data, StreamWriter writer, int nh)
        {
            int i = 0;

            writer.Write("\"holes\":[");
            foreach (var item in data)
            {
                writer.Write("{0},{1}{2}",
                    item.X.ToString(Util.Nfi),
                    item.Y.ToString(Util.Nfi), (i == nh - 1) ? String.Empty : ", ");

                i++;
            }

            writer.Write("]");
        }

        private void WriteSegments(IEnumerable<SubSegment> data, StreamWriter writer, int ns)
        {
            int i = 0;

            StringBuilder markers = new StringBuilder();
            bool useMarkers = false;

            string seperator;

            writer.Write("\"segments\":{\"data\":[");
            foreach (var item in data)
            {
                seperator = (i == ns - 1) ? String.Empty : ", ";

                writer.Write("{0},{1}{2}",
                    item.P0, item.P1, seperator);

                if (item.Label > 0)
                {
                    useMarkers = true;
                }

                markers.AppendFormat("{0}{1}", item.Label, seperator);

                i++;
            }

            writer.Write("]");

            if (useMarkers)
            {
                writer.Write(",\"markers\":[" + markers.ToString() + "]");
            }

            writer.Write("}");
        }

        private void WriteTriangles(IEnumerable<Triangle> data, StreamWriter writer, int ne)
        {
            int i = 0;

            StringBuilder neighbors = new StringBuilder();

            string seperator;

            writer.Write("\"triangles\":{\"data\":[");
            foreach (var item in data)
            {
                seperator = (i == ne - 1) ? String.Empty : ", ";

                writer.Write("{0},{1},{2}{3}",
                    item.GetVertexID(0),
                    item.GetVertexID(1),
                    item.GetVertexID(2),
                    seperator);

                neighbors.AppendFormat("{0},{1},{2}{3}",
                    item.GetNeighborID(0),
                    item.GetNeighborID(1),
                    item.GetNeighborID(2),
                    seperator);

                i++;
            }
            writer.Write("]");
            writer.Write(",\"neighbors\":[" + neighbors.ToString() + "]");
            writer.Write("}");
        }

        #endregion
    }
}