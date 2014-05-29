
namespace MeshExplorer.Topology
{
    using TriangleNet.Geometry;

    internal static class RectanglePolygon
    {
        public static IPolygon Generate(int n, double bounds = 10.0)
        {
            var geometry = new Polygon((n + 1) * (n + 1));

            double x, y, d = 2 * bounds / n;

            int mark = 0;

            for (int i = 0; i <= n; i++)
            {
                y = -bounds + i * d;

                for (int j = 0; j <= n; j++)
                {
                    x = -bounds + j * d;

                    geometry.Add(new Vertex(x, y, mark));
                }
            }

            // Add boundary segments
            for (int i = 0; i < n; i++)
            {
                // Bottom
                geometry.Add(new Edge(i, i + 1));

                // Right
                geometry.Add(new Edge(i * (n + 1) + n, (i + 1) * (n + 1) + n));

                // Top
                geometry.Add(new Edge(n * (n + 1) + i, n * (n + 1) + (i + 1)));

                // Left
                geometry.Add(new Edge(i * (n + 1), (i + 1) * (n + 1)));
            }

            return geometry;
        }
    }
}
