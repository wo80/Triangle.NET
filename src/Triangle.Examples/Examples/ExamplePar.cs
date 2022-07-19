
namespace TriangleNet.Examples
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using TriangleNet;
    using TriangleNet.Geometry;
    using TriangleNet.IO;
    using TriangleNet.Meshing;
    using TriangleNet.Meshing.Algorithm;

    /// <summary>
    /// Processing meshes in parallel.
    /// </summary>
    public class ExamplePar
    {
        #region Point set triangulation

        /// <summary>
        /// Triangulate a given number of random point sets in parallel.
        /// </summary>
        public static bool Run(int n = 1000)
        {
            // Use thread-safe random source.
            var random = Random.Shared;

            // Generate a random set of sizes.
            var sizes = Enumerable.Range(0, n).Select(_ => random.Next(500, 5000));

            var queue = new ConcurrentQueue<int>(sizes);

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
                        TrianglePool = () => pool.Restart(),
                        RandomSource = () => Random.Shared
                    };

                    var triangulator = new Dwyer();
                    var result = new MeshResult();

                    var bounds = new Rectangle(0d, 0d, 1000d, 1000d);

                    while (queue.Count > 0)
                    {
                        if (queue.TryDequeue(out int size))
                        {
                            var points = Generate.RandomPoints(size, bounds);

                            var mesh = triangulator.Triangulate(points, config);

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

            return invalid == 0;
        }

        #endregion

        #region Polygon triangulation

        /// <summary>
        /// Reads all .poly files from given directory and processes them in parallel.
        /// </summary>
        public static bool Run(string dir)
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
                        TrianglePool = () => pool.Restart(),
                        RandomSource = () => Random.Shared
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

            return invalid == 0;
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

        #endregion
    }
}
