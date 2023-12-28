using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TriangleNet.Geometry;
using TriangleNet.Meshing;
using TriangleNet.Smoothing;

namespace TriangleNet.Tests.Smoothing
{
    public class SimpleSmootherTest
    {
        [Test]
        public void TestSmoothWithDuplicate()
        {
            var poly = GetPolygon();

            var options = new ConstraintOptions() { ConformingDelaunay = true };

            var quality = new QualityOptions()
            {
                MinimumAngle = 30.0
            };

            var mesh = poly.Triangulate(options, quality);

            Assert.That(mesh.Vertices
                .Where(v => v.Type == VertexType.UndeadVertex)
                .Count(), Is.EqualTo(1));

            quality.MaximumArea = 0.2;

            mesh.Refine(quality, true);

            Assert.That(mesh.Vertices
                .Where(v => v.Type == VertexType.UndeadVertex)
                .Count(), Is.EqualTo(1));

            var smoother = new SimpleSmoother();

            // Smooth mesh.
            Assert.That(smoother.Smooth(mesh, 25), Is.GreaterThan(0));
        }

        private Polygon GetPolygon()
        {
            var poly = new Polygon(7);

            var p = new List<Vertex>()
            {
                new Vertex(0.0, 0.0, 1),
                new Vertex(2.0, 0.0, 1),
                new Vertex(2.0, 1.0, 1),
                new Vertex(2.0, 2.0, 1),
                new Vertex(0.0, 2.0, 1),
                new Vertex(1.5, 1.6, 1),
                new Vertex(2.0, 0.0, 1) // duplicate
            };

            poly.Points.AddRange(p);

            poly.Add(new Segment(p[0], p[1], 1));
            poly.Add(new Segment(p[1], p[2], 1));
            poly.Add(new Segment(p[2], p[3], 1));
            poly.Add(new Segment(p[3], p[4], 1));
            poly.Add(new Segment(p[4], p[0], 1));
            poly.Add(new Segment(p[2], p[5], 2));

            return poly;
        }
    }
}
