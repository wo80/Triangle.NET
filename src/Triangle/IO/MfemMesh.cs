
namespace TriangleNet.IO
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using TriangleNet.Meshing;

    /// <summary>
    /// A simple helper class to write a mesh using MFEM mesh format and send it to GLVis.
    /// </summary>
    public static class MfemMesh
    {
        /// <summary>
        /// Send the to default GLVis socket (127.0.0.1:19916).
        /// </summary>
        /// <param name="mesh">The mesh to send.</param>
        /// <param name="port">The port number (default = 19916).</param>
        public static async Task Send(IMesh mesh, int port = 19916)
        {
            await Send(mesh, IPAddress.Parse("127.0.0.1"), port);
        }

        /// <summary>
        /// Number the vertices and write them to a .node file.
        /// </summary>
        /// <param name="mesh">The mesh to send.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="port">The port number.</param>
        public static async Task Send(IMesh mesh, IPAddress ip, int port)
        {
            var client = new TcpClient();

            try
            {
                await client.ConnectAsync(ip, port);

                var stream = client.GetStream();

                using (var sw = new StreamWriter(stream) { NewLine = "\n" })
                {
                    sw.WriteLine("mesh"); // fem2d_gf_data
                    sw.WriteLine();

                    Write(mesh, sw);
                }

                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Write the mesh to given file.
        /// </summary>
        /// <param name="mesh">The mesh to write.</param>
        /// <param name="filename">The target file.</param>
        public static void Write(IMesh mesh, string filename)
        {
            using var file = File.Open(filename, FileMode.Create);
            using var sw = new StreamWriter(file);

            Write(mesh, sw);
        }

        /// <summary>
        /// Write the mesh to given stream.
        /// </summary>
        /// <param name="mesh">The mesh to write.</param>
        /// <param name="stream">The target stream.</param>
        public static void Write(IMesh mesh, Stream stream)
        {
            using var sw = new StreamWriter(stream);

            Write(mesh, sw);
        }

        private static void Write(IMesh mesh, StreamWriter sw)
        {
            sw.NewLine = "\n";

            mesh.Renumber();

            // Header

            sw.WriteLine("MFEM mesh v1.0");
            sw.WriteLine();
            sw.WriteLine("dimension");
            sw.WriteLine("2");
            sw.WriteLine();

            // Elements

            // POINT = 0
            // SEGMENT = 1
            // TRIANGLE = 2
            // SQUARE = 3
            // TETRAHEDRON = 4
            // CUBE = 5
            // PRISM = 6

            sw.WriteLine("elements");
            sw.WriteLine(mesh.Triangles.Count());

            foreach (var t in mesh.Triangles)
            {
                int label = t.Label;

                if (label < 0)
                {
                    throw new FormatException("MFEM element attributes must be positive.");
                }

                // We add 1 to the label since MFEM expects positive attributes.
                sw.WriteLine("{0} 2 {1} {2} {3}", label + 1,
                    t.GetVertexID(0),
                    t.GetVertexID(1),
                    t.GetVertexID(2));
            }

            sw.WriteLine();

            // Boundary

            sw.WriteLine("boundary");
            sw.WriteLine(mesh.Segments.Count());

            foreach (var s in mesh.Segments)
            {
                int label = s.Label;

                if (label < 0)
                {
                    throw new FormatException("MFEM boundary attributes must be positive.");
                }

                // We add 1 to the label since MFEM expects positive attributes.
                sw.WriteLine("{0} 1 {1} {2}", label + 1, s.P0, s.P1);
            }

            sw.WriteLine();

            // Vertices

            sw.WriteLine("vertices");
            sw.WriteLine(mesh.Vertices.Count);
            sw.WriteLine("2");

            foreach (var v in mesh.Vertices)
            {
                sw.WriteLine("{0} {1}",
                    v.x.ToString(NumberFormatInfo.InvariantInfo),
                    v.y.ToString(NumberFormatInfo.InvariantInfo));
            }

            sw.WriteLine();
        }
    }
}
