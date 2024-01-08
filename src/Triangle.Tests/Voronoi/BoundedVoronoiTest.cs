using NUnit.Framework;
using System;
using System.Linq;
using TriangleNet.Meshing;
using TriangleNet.Voronoi;

namespace TriangleNet.Tests.Voronoi
{
    public class BoundedVoronoiTest
    {
        [Test]
        public void TestBoundedVoronoi()
        {
            var p = Helper.SplitRectangle(-1, 1, 1, -1, 3);

            var mesher = new GenericMesher();
            var mesh = (Mesh)mesher.Triangulate(p);

            var voronoi = new BoundedVoronoi(mesh);

            // The "split rectangle" polygon has two region pointer set.
            Assert.That(voronoi.Vertices.Count(v => v.Label == 1), Is.EqualTo(2));
            Assert.That(voronoi.Vertices.Count(v => v.Label == 2), Is.EqualTo(2));

            // The polygon has 6 boundary segments, so the Voronoi diagram
            // should have 6 infinite edges (which are projected onto the
            // boundary). Additionally, the 6 boundary vertices are part of
            // the Voronoi diagram.
            Assert.That(voronoi.Vertices.Count(v => v.Label == 0), Is.EqualTo(12));
            Assert.That(voronoi.Vertices.Count(v => v.ID >= mesh.Triangles.Count), Is.EqualTo(12));
        }
    }
}
