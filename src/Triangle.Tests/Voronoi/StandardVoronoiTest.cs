using NUnit.Framework;
using System;
using System.Linq;
using TriangleNet.Meshing;
using TriangleNet.Voronoi;

namespace TriangleNet.Tests.Voronoi
{
    public class StandardVoronoiTest
    {
        [Test]
        public void TestStandardVoronoi()
        {
            var p = Helper.SplitRectangle(-1, 1, 1, -1, 3);

            var mesher = new GenericMesher();
            var mesh = (Mesh)mesher.Triangulate(p);

            var voronoi = new StandardVoronoi(mesh);

            // The "split rectangle" polygon has two region pointer set.
            Assert.That(voronoi.Vertices.Count(v => v.Label == 1), Is.EqualTo(2));
            Assert.That(voronoi.Vertices.Count(v => v.Label == 2), Is.EqualTo(2));

            // The polygon has 6 boundary segments, so the Voronoi diagram
            // should have 6 infinite edges.
            Assert.That(voronoi.Vertices.Count(v => v.Label == 0), Is.EqualTo(6));
            Assert.That(voronoi.Vertices.Count(v => v.ID >= mesh.Triangles.Count), Is.EqualTo(6));
        }
    }
}
