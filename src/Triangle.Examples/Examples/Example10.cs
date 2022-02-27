
namespace TriangleNet.Examples
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using TriangleNet;
    using TriangleNet.IO;
    using TriangleNet.Meshing;

    /// <summary>
    /// Processing meshes in parallel.
    /// </summary>
    public class Example10
    {
        /// <summary>
        /// Reads all .poly files from given directory and processes them in parallel.
        /// </summary>
        public static void Run(string dir)
        {
            var files = Directory.EnumerateFiles(dir, "*.poly", SearchOption.AllDirectories);

            var queue = new ConcurrentQueue<string>(files);

            int concurrencyLevel = Environment.ProcessorCount / 2;

            var tasks = new Task<MeshResult>[concurrencyLevel];

            for (int i = 0; i < concurrencyLevel; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    // Each task has it's own triangle pool and predicates instance.
                    var pool = new TrianglePool();
                    var predicates = new RobustPredicates();

                    // The factory methods return the above instances.
                    var config = new Configuration()
                    {
                        Predicates = () => predicates,
                        TrianglePool = () => pool.Restart()
                    };

                    var mesher = new GenericMesher(config);
                    var result = new MeshResult();

                    while (queue.Count > 0)
                    {
                        if (queue.TryDequeue(out var file))
                        {
                            var poly = FileProcessor.Read(file);

                            var mesh = mesher.Triangulate(poly);

                            ProcessMesh(mesh, result);
                        }
                    }

                    pool.Clear();

                    return result;
                });
            }

            Task.WaitAll(tasks);

            int numberOfTriangles = tasks.Sum(t => t.Result.NumberOfTriangles);
            int invalid = tasks.Sum(t => t.Result.Invalid);

            Console.WriteLine("Total number of triangles processed: {0}", numberOfTriangles);

            if (invalid > 0)
            {
                Console.WriteLine("   Number of invalid triangulations: {0}", invalid);
            }
        }

        private static void ProcessMesh(IMesh mesh, MeshResult result)
        {
            result.NumberOfTriangles += mesh.Triangles.Count;

            if (!MeshValidator.IsConsistent((Mesh)mesh))
            {
                result.Invalid += 1;
            }
        }

        class MeshResult
        {
            public int NumberOfTriangles;
            public int Invalid;
        }
    }
}
