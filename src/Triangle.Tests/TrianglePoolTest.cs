using NUnit.Framework;
using System.Linq;

namespace TriangleNet.Tests
{
    class TrianglePoolTest
    {
        [Test]
        public void TestGetRelease()
        {
            var pool = new TrianglePool();

            var t0 = pool.Get();
            var t1 = pool.Get();
            var t2 = pool.Get();

            Assert.That(t0.ID, Is.EqualTo(0));
            Assert.That(t1.ID, Is.EqualTo(1));
            Assert.That(t2.ID, Is.EqualTo(2));

            Assert.That(pool.Count, Is.EqualTo(3));

            pool.Release(t0);

            Assert.That(pool.Count, Is.EqualTo(2));
            Assert.That(t0.GetHashCode(), Is.LessThan(0));

            pool.Release(t1);

            Assert.That(pool.Count, Is.EqualTo(1));
            Assert.That(t1.GetHashCode(), Is.LessThan(0));

            var t4 = pool.Get();

            Assert.That(pool.Count, Is.EqualTo(2));
            Assert.That(t4.ID, Is.EqualTo(t1.ID));

            var t5 = pool.Get();

            Assert.That(pool.Count, Is.EqualTo(3));
            Assert.That(t5.ID, Is.EqualTo(t0.ID));

            var t6 = pool.Get();

            Assert.That(pool.Count, Is.EqualTo(4));
            Assert.That(t6.ID, Is.EqualTo(3));
        }

        [Test]
        public void TestToArray()
        {
            var pool = new TrianglePool();

            // Create 4 triangles.
            pool.Get();
            pool.Get();
            pool.Get();
            pool.Get();

            var a = pool.ToArray();

            Assert.That(a.Length, Is.EqualTo(4));
            Assert.That(a[0].ID, Is.EqualTo(0));
            Assert.That(a[1].ID, Is.EqualTo(1));
            Assert.That(a[2].ID, Is.EqualTo(2));
            Assert.That(a[3].ID, Is.EqualTo(3));

            pool.Release(a[1]);

            a = pool.ToArray();

            Assert.That(a.Length, Is.EqualTo(3));
            Assert.That(a[0].ID, Is.EqualTo(0));
            Assert.That(a[1].ID, Is.EqualTo(2));
            Assert.That(a[2].ID, Is.EqualTo(3));

            pool.Release(a[1]);

            a = pool.ToArray();

            Assert.That(a.Length, Is.EqualTo(2));
            Assert.That(a[0].ID, Is.EqualTo(0));
            Assert.That(a[1].ID, Is.EqualTo(3));

            var t2 = pool.Get();

            a = pool.ToArray();

            Assert.That(a.Length, Is.EqualTo(3));
            Assert.That(a[0].ID, Is.EqualTo(0));
            Assert.That(a[1].ID, Is.EqualTo(2));
            Assert.That(t2.ID, Is.EqualTo(2));
            Assert.That(a[2].ID, Is.EqualTo(3));

            var t1 = pool.Get();

            a = pool.ToArray();

            Assert.That(a.Length, Is.EqualTo(4));
            Assert.That(a[0].ID, Is.EqualTo(0));
            Assert.That(a[1].ID, Is.EqualTo(1));
            Assert.That(t1.ID, Is.EqualTo(1));
            Assert.That(a[2].ID, Is.EqualTo(2));
            Assert.That(a[3].ID, Is.EqualTo(3));
        }

        [Test]
        public void TestRestart()
        {
            var pool = new TrianglePool();

            Assert.That(pool.Count, Is.EqualTo(0));
            Assert.That(pool.Capacity, Is.EqualTo(0));

            int n = 10;

            for (int i = 0; i < n; i++)
            {
                Assert.That(pool.Get().ID, Is.EqualTo(i));
            }

            Assert.That(pool.Count, Is.EqualTo(n));

            pool.Restart();

            Assert.That(pool.Count, Is.EqualTo(0));
            Assert.That(pool.Capacity, Is.EqualTo(10));
        }
    }
}
