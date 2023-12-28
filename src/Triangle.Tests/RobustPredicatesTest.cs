using NUnit.Framework;
using TriangleNet.Geometry;

namespace TriangleNet.Tests
{
    public class RobustPredicatesTest
    {
        [Test]
        public void TestCounterClockwise()
        {
            var robust = RobustPredicates.Default;

            var a = new Point(-1d, 0d);
            var b = new Point( 0d, 1d);

            Assert.That(robust.CounterClockwise(a, b,  new Point(1d, 0d)), Is.LessThan(0d));
            Assert.That(robust.CounterClockwise(a, b,  new Point(0d, 2d)), Is.GreaterThan(0d));
            Assert.That(robust.CounterClockwise(a, b,  new Point(1d, 2d)), Is.EqualTo(0d));
        }

        [Test]
        public void TestInCircle()
        {
            var robust = RobustPredicates.Default;

            var a = new Point(-1d, 0d);
            var b = new Point(0d, 1d);
            var c = new Point(1d, 0d);

            Assert.That(robust.InCircle(a, b, c, new Point(0d, 0.5)), Is.LessThan(0d));
            Assert.That(robust.InCircle(a, b, c, new Point(0d, 1.5)), Is.GreaterThan(0d));
            Assert.That(robust.InCircle(a, b, c, new Point(0d, 1.0)), Is.EqualTo(0d));
        }

        [Test]
        public void TestFindCircumcenter()
        {
            var robust = RobustPredicates.Default;

            var a = new Point(-1d, 0d);
            var b = new Point(0d, 1d);
            var c = new Point(1d, 0d);

            double xi = 0d, eta = 0d;

            var actual = robust.FindCircumcenter(a, b, c, ref xi, ref eta);
            var expected = new Point(0d, 0d);

            Assert.That(actual.X, Is.EqualTo(expected.X));
            Assert.That(actual.Y, Is.EqualTo(expected.Y));
            Assert.That(xi, Is.EqualTo(0.0));
            Assert.That(eta, Is.EqualTo(0.5));
        }
    }
}
