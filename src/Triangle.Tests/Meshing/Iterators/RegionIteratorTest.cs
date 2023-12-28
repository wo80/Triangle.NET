using NUnit.Framework;
using TriangleNet.Geometry;
using TriangleNet.Meshing.Iterators;
using TriangleNet.Tools;
using TriangleNet.Topology;

namespace TriangleNet.Tests.Meshing.Iterators
{
    public class RegionIteratorTest
    {
        public void TestProcessRegionProtected()
        {
            var poly = new Polygon();

            // Outer region.
            poly.Add(Helper.Rectangle(-2d, 2d, 2d, -2d, 2));

            // Inner region.
            poly.Add(Helper.Rectangle(-1d, 1d, 1d, -1d, 1));

            poly.Regions.Add(new RegionPointer(0d, 0d, 1));

            var mesh = (Mesh)poly.Triangulate();

            var iterator = new RegionIterator();

            var qtree = new TriangleQuadTree(mesh);

            // Find a seeding triangle in region 1.
            var seed = (Triangle)qtree.Query(0.0, 0.0);

            iterator.Process(seed, t => Assert.That(t.Label, Is.EqualTo(1)));
        }
    }
}
