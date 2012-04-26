// -----------------------------------------------------------------------
// <copyright file="io.cs" company="">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/triangle/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.IO
{
    using System;
    using System.IO;
    using System.Globalization;
    using TriangleNet.Data;
    using TriangleNet.Log;

    /// <summary>
    /// Helper for reading Triangle files.
    /// </summary>
    public static class FileReader
    {
        static NumberFormatInfo nfi = CultureInfo.InvariantCulture.NumberFormat;
        static int startIndex = 0;

        /// <summary>
        /// Read the input data from a file, which may be a .node or .poly file.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <remarks>Will NOT read associated files by default.</remarks>
        public static MeshData ReadFile(string filename)
        {
            return ReadFile(filename, false);
        }

        /// <summary>
        /// Read the input data from a file, which may be a .node or .poly file.
        /// </summary>
        /// <param name="filename">The file to read.</param>
        /// <param name="readsupp">Read associated files (ele, area, neigh).</param>
        public static MeshData ReadFile(string filename, bool readsupp)
        {
            string ext = Path.GetExtension(filename);

            if (ext == ".node")
            {
                return ReadNodeFile(filename, readsupp);
            }
            else if (ext == ".poly")
            {
                return ReadPolyFile(filename, readsupp, readsupp);
            }

            throw new NotSupportedException("File format '" + ext + "' not supported.");
        }

        static bool TryReadLine(StreamReader reader, out string[] token)
        {
            token = null;

            if (reader.EndOfStream)
            {
                return false;
            }

            string line = reader.ReadLine().Trim();

            while (String.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
            {
                if (reader.EndOfStream)
                {
                    return false;
                }

                line = reader.ReadLine().Trim();
            }

            token = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            return true;
        }

        static void ReadVertex(MeshData data, int index, string[] line)
        {
            int n = data.PointAttributes == null ? 0 : data.PointAttributes.Length;

            data.Points[index] = new double[] {
                double.Parse(line[1], nfi),
                double.Parse(line[2], nfi) };

            // Read the vertex attributes.
            for (int j = 0; j < n; j++)
            {
                data.PointAttributes[index] = new double[n];

                if (line.Length > 3 + j)
                {
                    data.PointAttributes[index][j] = double.Parse(line[3 + j]);
                }
            }

            if (data.PointMarkers != null)
            {
                // Read a vertex marker.
                if (line.Length > 3 + n)
                {
                    data.PointMarkers[index] = int.Parse(line[3 + n]);
                }
            }
        }

        /// <summary>
        /// Read the vertices from a file, which may be a .node or .poly file.
        /// </summary>
        /// <param name="nodefilename"></param>
        /// <remarks>Will NOT read associated .ele by default.</remarks>
        public static MeshData ReadNodeFile(string nodefilename)
        {
            return ReadNodeFile(nodefilename, false);
        }

        /// <summary>
        /// Read the vertices from a file, which may be a .node or .poly file.
        /// </summary>
        /// <param name="nodefilename"></param>
        /// <param name="readElements"></param>
        public static MeshData ReadNodeFile(string nodefilename, bool readElements)
        {
            MeshData data = new MeshData();

            startIndex = 0;

            string[] line;
            int invertices = 0, attributes = 0, nodemarkers = 0;

            using (StreamReader reader = new StreamReader(nodefilename))
            {
                if (!TryReadLine(reader, out line))
                {
                    throw new Exception("Can't read input file.");
                }

                // Read number of vertices, number of dimensions, number of vertex
                // attributes, and number of boundary markers.
                invertices = int.Parse(line[0]);

                if (invertices < 3)
                {
                    throw new Exception("Input must have at least three input vertices.");
                }

                if (line.Length > 1)
                {
                    if (int.Parse(line[1]) != 2)
                    {
                        throw new Exception("Triangle only works with two-dimensional meshes.");
                    }
                }

                if (line.Length > 2)
                {
                    attributes = int.Parse(line[2]);
                }

                if (line.Length > 3)
                {
                    nodemarkers = int.Parse(line[3]);
                }

                // Read the vertices.
                if (invertices > 0)
                {
                    data.Points = new double[invertices][];

                    if (attributes > 0)
                    {
                        data.PointAttributes = new double[invertices][];
                    }

                    if (nodemarkers > 0)
                    {
                        data.PointMarkers = new int[invertices];
                    }

                    for (int i = 0; i < invertices; i++)
                    {
                        if (!TryReadLine(reader, out line))
                        {
                            throw new Exception("Can't read input file (vertices).");
                        }

                        if (line.Length < 3)
                        {
                            throw new Exception("Invalid vertex.");
                        }

                        if (i == 0)
                        {
                            startIndex = int.Parse(line[0], nfi);
                        }

                        ReadVertex(data, i, line);
                    }
                }
            }

            if (readElements)
            {
                // Read area file
                string elefile = Path.ChangeExtension(nodefilename, ".ele");
                if (File.Exists(elefile))
                {
                    ReadEleFile(elefile, data, true);
                }
            }

            return data;
        }

        /// <summary>
        /// Read the vertices and segments from a .poly file.
        /// </summary>
        /// <param name="polyfilename"></param>
        /// <remarks>Will NOT read associated .ele by default.</remarks>
        public static MeshData ReadPolyFile(string polyfilename)
        {
            return ReadPolyFile(polyfilename, false, false);
        }

        /// <summary>
        /// Read the vertices and segments from a .poly file.
        /// </summary>
        /// <param name="polyfilename"></param>
        /// <param name="readElements">If true, look for an associated .ele file.</param>
        /// <remarks>Will NOT read associated .area by default.</remarks>
        public static MeshData ReadPolyFile(string polyfilename, bool readElements)
        {
            return ReadPolyFile(polyfilename, readElements, false);
        }

        /// <summary>
        /// Read the vertices and segments from a .poly file.
        /// </summary>
        /// <param name="polyfilename"></param>
        /// <param name="readElements">If true, look for an associated .ele file.</param>
        /// <param name="readElements">If true, look for an associated .area file.</param>
        public static MeshData ReadPolyFile(string polyfilename, bool readElements, bool readArea)
        {
            // Read poly file
            MeshData data;

            startIndex = 0;

            string[] line;
            int invertices = 0, attributes = 0, nodemarkers = 0;

            using (StreamReader reader = new StreamReader(polyfilename))
            {
                if (!TryReadLine(reader, out line))
                {
                    throw new Exception("Can't read input file.");
                }

                // Read number of vertices, number of dimensions, number of vertex
                // attributes, and number of boundary markers.
                invertices = int.Parse(line[0]);

                if (line.Length > 1)
                {
                    if (int.Parse(line[1]) != 2)
                    {
                        throw new Exception("Triangle only works with two-dimensional meshes.");
                    }
                }

                if (line.Length > 2)
                {
                    attributes = int.Parse(line[2]);
                }

                if (line.Length > 3)
                {
                    nodemarkers = int.Parse(line[3]);
                }

                // Read the vertices.
                if (invertices > 0)
                {
                    data = new MeshData();

                    data.Points = new double[invertices][];

                    if (attributes > 0)
                    {
                        data.PointAttributes = new double[invertices][];
                    }

                    if (nodemarkers > 0)
                    {
                        data.PointMarkers = new int[invertices];
                    }

                    for (int i = 0; i < invertices; i++)
                    {
                        if (!TryReadLine(reader, out line))
                        {
                            throw new Exception("Can't read input file (vertices).");
                        }

                        if (line.Length < 3)
                        {
                            throw new Exception("Invalid vertex.");
                        }

                        if (i == 0)
                        {
                            // Set the start index!
                            startIndex = int.Parse(line[0], nfi);
                        }

                        ReadVertex(data, i, line);
                    }
                }
                else
                {
                    // If the .poly file claims there are zero vertices, that means that
                    // the vertices should be read from a separate .node file.
                    string nodefile = Path.ChangeExtension(polyfilename, ".node");
                    data = ReadNodeFile(nodefile);
                    invertices = data.Points.Length;
                }

                if (data.Points == null)
                {
                    throw new Exception("No nodes available.");
                }

                // Read the segments from a .poly file.

                // Read number of segments and number of boundary markers.
                if (!TryReadLine(reader, out line))
                {
                    throw new Exception("Can't read input file (segments).");
                }

                int insegments = int.Parse(line[0]);

                int segmentmarkers = 0;
                if (line.Length > 1)
                {
                    segmentmarkers = int.Parse(line[1]);
                }

                if (insegments > 0)
                {
                    data.Segments = new int[insegments][];
                }

                if (segmentmarkers > 0)
                {
                    data.SegmentMarkers = new int[insegments];
                }

                int end1, end2;
                // Read and insert the segments.
                for (int i = 0; i < insegments; i++)
                {
                    if (!TryReadLine(reader, out line))
                    {
                        throw new Exception("Can't read input file (segments).");
                    }

                    if (line.Length < 3)
                    {
                        throw new Exception("Segment has no endpoints.");
                    }

                    // TODO: startIndex ok?
                    end1 = int.Parse(line[1]) - startIndex;
                    end2 = int.Parse(line[2]) - startIndex;

                    if (segmentmarkers > 0)
                    {
                        if (line.Length > 3)
                        {
                            data.SegmentMarkers[i] = int.Parse(line[3]);
                        }
                        else
                        {
                            data.SegmentMarkers[i] = 0;
                        }
                    }

                    if ((end1 < 0) || (end1 >= invertices))
                    {
                        if (Behavior.Verbose)
                        {
                            SimpleLog.Instance.Warning("Invalid first endpoint of segment.", 
                                "MeshReader.ReadPolyfile()");
                        }
                    }
                    else if ((end2 < 0) || (end2 >= invertices))
                    {
                        if (Behavior.Verbose)
                        {
                            SimpleLog.Instance.Warning("Invalid second endpoint of segment.", 
                                "MeshReader.ReadPolyfile()");
                        }
                    }
                    else
                    {
                        data.Segments[i] = new int[] { end1, end2 };
                    }
                }

                // Read holes from a .poly file.

                // Read the holes.
                if (!TryReadLine(reader, out line))
                {
                    throw new Exception("Can't read input file (holes).");
                }

                int holes = int.Parse(line[0]);
                if (holes > 0)
                {
                    data.Holes = new double[holes][];

                    for (int i = 0; i < holes; i++)
                    {
                        if (!TryReadLine(reader, out line))
                        {
                            throw new Exception("Can't read input file (holes).");
                        }

                        if (line.Length < 3)
                        {
                            throw new Exception("Invalid hole.");
                        }

                        data.Holes[i] = new double[] {
                            double.Parse(line[1], nfi),
                            double.Parse(line[2], nfi) };
                    }
                }

                // Read area constraints (optional).
                if (TryReadLine(reader, out line))
                {
                    int regions = int.Parse(line[0]);

                    if (regions > 0)
                    {
                        data.Regions = new double[regions][];

                        for (int i = 0; i < regions; i++)
                        {
                            if (!TryReadLine(reader, out line))
                            {
                                throw new Exception("Can't read input file (region).");
                            }

                            if (line.Length < 5)
                            {
                                throw new Exception("Invalid region.");
                            }

                            data.Regions[i] = new double[] {
                                // Region x and y
                                double.Parse(line[1]),
                                double.Parse(line[2]),
                                // Region attribute
                                double.Parse(line[3]),
                                // Region area constraint
                                double.Parse(line[4]) };
                        }
                    }
                }
            }

            // Read ele file
            if (readElements)
            {
                string elefile = Path.ChangeExtension(polyfilename, ".ele");
                if (File.Exists(elefile))
                {
                    ReadEleFile(elefile, data, readArea);
                }
            }

            return data;
        }

        public static MeshData ReadEleFile(string elefilename)
        {
            MeshData data = new MeshData();

            ReadEleFile(elefilename, data, false);

            return data;
        }

        /// <summary>
        /// Read the elements from an .ele file.
        /// </summary>
        /// <param name="elefilename"></param>
        /// <param name="data"></param>
        /// <param name="readArea"></param>
        private static void ReadEleFile(string elefilename, MeshData data, bool readArea)
        {
            int intriangles = 0, attributes = 0;

            using (StreamReader reader = new StreamReader(elefilename))
            {
                // Read number of elements and number of attributes.
                string[] line;

                if (!TryReadLine(reader, out line))
                {
                    throw new Exception("Can't read input file (elements).");
                }

                intriangles = int.Parse(line[0]);

                // We irgnore index 1 (number of nodes per triangle)
                attributes = 0;
                if (line.Length > 2)
                {
                    attributes = int.Parse(line[2]);
                }

                data.Triangles = new int[intriangles][];

                if (attributes > 0)
                {
                    data.TriangleAttributes = new double[intriangles][];
                }

                // Read triangles.
                for (int i = 0; i < intriangles; i++)
                {
                    if (!TryReadLine(reader, out line))
                    {
                        throw new Exception("Can't read input file (elements).");
                    }

                    if (line.Length < 4)
                    {
                        throw new Exception("Triangle has no nodes.");
                    }

                    // TODO: startIndex ok?
                    data.Triangles[i] = new int[] {
                        int.Parse(line[1]) - startIndex,
                        int.Parse(line[2]) - startIndex,
                        int.Parse(line[3]) - startIndex };

                    // Read triangle attributes
                    if (attributes > 0)
                    {
                        for (int j = 0; j < attributes; j++)
                        {
                            data.TriangleAttributes[i] = new double[attributes];

                            if (line.Length > 4 + j)
                            {
                                data.TriangleAttributes[i][j] = double.Parse(line[4 + j]);
                            }
                        }
                    }
                }
            }

            // Read area file
            if (readArea)
            {
                string areafile = Path.ChangeExtension(elefilename, ".area");
                if (File.Exists(areafile))
                {
                    ReadAreaFile(areafile, intriangles, data);
                }
            }
        }

        /// <summary>
        /// Read the area constraints from an .area file.
        /// </summary>
        /// <param name="areafilename"></param>
        /// <param name="intriangles"></param>
        /// <param name="data"></param>
        private static void ReadAreaFile(string areafilename, int intriangles, MeshData data)
        {
            using (StreamReader reader = new StreamReader(areafilename))
            {
                string[] line;

                if (!TryReadLine(reader, out line))
                {
                    throw new Exception("Can't read input file (area).");
                }

                if (int.Parse(line[0]) != intriangles)
                {
                    SimpleLog.Instance.Warning("Number of area constraints doesn't match number of triangles.", 
                        "ReadAreaFile()");
                    return;
                }

                data.TriangleAreas = new double[intriangles];

                // Read area constraints.
                for (int i = 0; i < intriangles; i++)
                {
                    if (!TryReadLine(reader, out line))
                    {
                        throw new Exception("Can't read input file (area).");
                    }

                    if (line.Length != 2)
                    {
                        throw new Exception("Triangle has no nodes.");
                    }

                    data.TriangleAreas[i] = double.Parse(line[1], nfi);
                }
            }
        }

        public static MeshData ReadEdgeFile(string edgeFile)
        {
            // Read poly file
            MeshData data = new MeshData();

            startIndex = 0;

            string[] line;

            using (StreamReader reader = new StreamReader(edgeFile))
            {
                // Read the edges from a .edge file.

                // Read number of segments and number of boundary markers.
                if (!TryReadLine(reader, out line))
                {
                    throw new Exception("Can't read input file (segments).");
                }

                int inedges = int.Parse(line[0]);

                int edgemarkers = 0;
                if (line.Length > 1)
                {
                    edgemarkers = int.Parse(line[1]);
                }

                if (inedges > 0)
                {
                    data.Edges = new int[inedges][];
                }

                if (edgemarkers > 0)
                {
                    data.EdgeMarkers = new int[inedges];
                }

                int end1, end2;
                // Read and insert the segments.
                for (int i = 0; i < inedges; i++)
                {
                    if (!TryReadLine(reader, out line))
                    {
                        throw new Exception("Can't read input file (segments).");
                    }

                    if (line.Length < 3)
                    {
                        throw new Exception("Segment has no endpoints.");
                    }

                    // TODO: startIndex ok?
                    end1 = int.Parse(line[1]) - startIndex;
                    end2 = int.Parse(line[2]) - startIndex;

                    if (edgemarkers > 0)
                    {
                        if (line.Length > 3)
                        {
                            data.SegmentMarkers[i] = int.Parse(line[3]);
                        }
                        else
                        {
                            data.SegmentMarkers[i] = 0;
                        }
                    }

                    data.Segments[i] = new int[] { end1, end2 };

                    //if ((end1 < 0) || (end1 >= invertices))
                    //{
                    //    if (Behavior.Verbose)
                    //    {
                    //        SimpleLogger.Instance.Warning("Invalid first endpoint of segment.",
                    //            "MeshReader.ReadPolyfile()");
                    //    }
                    //}
                    //else if ((end2 < 0) || (end2 >= invertices))
                    //{
                    //    if (Behavior.Verbose)
                    //    {
                    //        SimpleLogger.Instance.Warning("Invalid second endpoint of segment.",
                    //            "MeshReader.ReadPolyfile()");
                    //    }
                    //}
                    //else
                    //{
                    //    data.Segments[i] = new int[] { end1, end2 };
                    //}
                }
            }

            return data;
        }
    }
}
