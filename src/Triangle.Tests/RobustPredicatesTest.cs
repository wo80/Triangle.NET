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

            Assert.IsTrue(robust.CounterClockwise(a, b,  new Point(1d, 0d)) < 0d);
            Assert.IsTrue(robust.CounterClockwise(a, b,  new Point(0d, 2d)) > 0d);
            Assert.IsTrue(robust.CounterClockwise(a, b,  new Point(1d, 2d)) == 0d);
        }

        [Test]
        public void TestInCircle()
        {
            var robust = RobustPredicates.Default;

            var a = new Point(-1d, 0d);
            var b = new Point(0d, 1d);
            var c = new Point(1d, 0d);

            Assert.IsTrue(robust.InCircle(a, b, c, new Point(0d, 0.5)) < 0d);
            Assert.IsTrue(robust.InCircle(a, b, c, new Point(0d, 1.5)) > 0d);
            Assert.IsTrue(robust.InCircle(a, b, c, new Point(0d, 1d)) == 0d);
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

            Assert.AreEqual(expected.X, actual.X);
            Assert.AreEqual(expected.Y, actual.Y);
            Assert.AreEqual(0.0, xi);
            Assert.AreEqual(0.5, eta);
        }
    }
}
