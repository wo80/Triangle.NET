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
            int boundaryLabel = 3;

            var p = Helper.SplitRectangle(-1, 1, 1, -1, boundaryLabel);

            Assert.That(p.Regions.Count, Is.EqualTo(2));

            int regionLabel1 = p.Regions[0].Label;
            int regionLabel2 = p.Regions[1].Label;

            var mesher = new GenericMesher();
            var mesh = (Mesh)mesher.Triangulate(p);

            var voronoi = new BoundedVoronoi(mesh);

            // The "split rectangle" polygon has two region pointer set.
            Assert.That(voronoi.Vertices.Count(v => v.Label == regionLabel1), Is.EqualTo(2));
            Assert.That(voronoi.Vertices.Count(v => v.Label == regionLabel2), Is.EqualTo(2));

            // The polygon has 6 boundary segments, so the Voronoi diagram
            // should have 6 infinite edges (which are projected onto the
            // boundary). Additionally, the 6 boundary vertices are part of
            // the Voronoi diagram.
            Assert.That(voronoi.Vertices.Count(v => v.Label == 0), Is.EqualTo(12));
            Assert.That(voronoi.Vertices.Count(v => v.ID >= mesh.Triangles.Count), Is.EqualTo(12));

            // All Voronoi cells should have a generator vertex.
            Assert.That(voronoi.Faces.All(f => f.Generator is not null));

            // All Voronoi cells should have the same label as the dual vertex.
            Assert.That(voronoi.Faces.All(f => f.Label == boundaryLabel));

            // Check DCEL topology (all Voronoi cells should be closed).
            Assert.That(voronoi.IsConsistent());
        }
    }
}
