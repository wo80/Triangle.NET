using NUnit.Framework;
using System;
using TriangleNet.Geometry;
using TriangleNet.Tools;

namespace TriangleNet.Tests.Tools
{
    public class StatisticTest
    {
        [Test, DefaultFloatingPointTolerance(1e-1)]
        public void TestComputeAngles()
        {
            // Angles: 90, 45, 45
            var t = Helper.CreateTriangle(1 ,new Vertex(0.0, 0.0), new Vertex(1.0, 0.0), new Vertex(0.0, 1.0));
            var a = ComputeAngles(t);

            Assert.That(a.min, Is.EqualTo(45.0));
            Assert.That(a.max, Is.EqualTo(90.0));

            // Angles: 135, 14, 31
            t = Helper.CreateTriangle(1 ,new Vertex(0.0, 0.0), new Vertex(3.0, 0.0), new Vertex(-1.0, 1.0));
            a = ComputeAngles(t);

            Assert.That(a.min, Is.EqualTo(14.0));
            Assert.That(a.max, Is.EqualTo(135.0));

            // Angles: 60, 60, 60
            t = Helper.CreateTriangle(1 ,new Vertex(0.0, 0.0), new Vertex(2.0, 0.0), new Vertex(1.0, 1.73));
            a = ComputeAngles(t);

            Assert.That(a.min, Is.EqualTo(60.0));
            Assert.That(a.max, Is.EqualTo(60.0));

            // Angles: 180
            t = Helper.CreateTriangle(1 ,new Vertex(0.0, 0.0), new Vertex(10000.0, 1.0), new Vertex(-10000.0, 1.0));
            a = ComputeAngles(t);

            Assert.That(a.min, Is.EqualTo(0.0));
            Assert.That(a.max, Is.EqualTo(180.0));
        }

        /// <summary>
        /// Returns the minimum and maximum angle of given triangle (in degrees).
        /// </summary>
        private (double min, double max) ComputeAngles(ITriangle triangle)
        {
            var data = new double[6];

            Statistic.ComputeAngles(triangle, data);

            bool acute = data[2] > 0;

            double min = Math.Acos(Math.Sqrt(data[0]));
            double max = Math.Acos(Math.Sqrt(data[1]));

            const double deg = 180.0 / Math.PI;

            return (deg * min, acute ? deg * max : deg * (Math.PI - max));
        }
    }
}
