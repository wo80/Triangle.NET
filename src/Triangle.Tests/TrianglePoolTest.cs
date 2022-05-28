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

            Assert.AreEqual(0, t0.ID);
            Assert.AreEqual(1, t1.ID);
            Assert.AreEqual(2, t2.ID);

            Assert.AreEqual(3, pool.Count);

            pool.Release(t0);

            Assert.AreEqual(2, pool.Count);
            Assert.Less(t0.GetHashCode(), 0);

            pool.Release(t1);

            Assert.AreEqual(1, pool.Count);
            Assert.Less(t1.GetHashCode(), 0);

            var t4 = pool.Get();

            Assert.AreEqual(2, pool.Count);
            Assert.AreEqual(t1.ID, t4.ID);

            var t5 = pool.Get();

            Assert.AreEqual(3, pool.Count);
            Assert.AreEqual(t0.ID, t5.ID);

            var t6 = pool.Get();

            Assert.AreEqual(4, pool.Count);
            Assert.AreEqual(3, t6.ID);
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

            Assert.AreEqual(4, a.Length);
            Assert.AreEqual(0, a[0].ID);
            Assert.AreEqual(1, a[1].ID);
            Assert.AreEqual(2, a[2].ID);
            Assert.AreEqual(3, a[3].ID);

            pool.Release(a[1]);

            a = pool.ToArray();

            Assert.AreEqual(3, a.Length);
            Assert.AreEqual(0, a[0].ID);
            Assert.AreEqual(2, a[1].ID);
            Assert.AreEqual(3, a[2].ID);

            pool.Release(a[1]);

            a = pool.ToArray();

            Assert.AreEqual(2, a.Length);
            Assert.AreEqual(0, a[0].ID);
            Assert.AreEqual(3, a[1].ID);

            var t2 = pool.Get();

            a = pool.ToArray();

            Assert.AreEqual(3, a.Length);
            Assert.AreEqual(0, a[0].ID);
            Assert.AreEqual(2, a[1].ID);
            Assert.AreEqual(2, t2.ID);
            Assert.AreEqual(3, a[2].ID);

            var t1 = pool.Get();

            a = pool.ToArray();

            Assert.AreEqual(4, a.Length);
            Assert.AreEqual(0, a[0].ID);
            Assert.AreEqual(1, a[1].ID);
            Assert.AreEqual(1, t1.ID);
            Assert.AreEqual(2, a[2].ID);
            Assert.AreEqual(3, a[3].ID);
        }

        [Test]
        public void TestRestart()
        {
            var pool = new TrianglePool();

            Assert.AreEqual(0, pool.Count);
            Assert.AreEqual(0, pool.Capacity);

            int n = 10;

            for (int i = 0; i < n; i++)
            {
                Assert.AreEqual(i, pool.Get().ID);
            }

            Assert.AreEqual(n, pool.Count);

            pool.Restart();

            Assert.AreEqual(0, pool.Count);
            Assert.AreEqual(10, pool.Capacity);
        }
    }
}
