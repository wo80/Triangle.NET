
namespace TriangleNet.Tests
{
    using NUnit.Framework;
    using TriangleNet.Geometry;
    using TriangleNet.Topology;

    // The Otri (orientent triangle) struct is the heart of Triangle's mesh
    // datastructure. It basically represents one of the three edges of the
    // triangle by associating an orientation to the triangle object.
    //
    // For testing, a minimal example mesh is considered. It consists of
    // 6 vertices and 4 triangles.
    //
    // Vertices:
    //    X [-2] [0] [2] [-1] [1] [0]
    //    Y [ 0] [0] [0] [ 1] [1] [2]
    //
    // Triangles and neighbors and neighbor orientations:
    //      [3 0 1]   [-1  1 -1]   [- 2 -]
    //      [3 1 4]   [ 2  3  0]   [2 0 1]
    //      [4 1 2]   [-1 -1  1]   [- - 0]
    //      [5 3 4]   [ 1 -1 -1]   [1 - -]
    //
    // The shape is one large triangle, split into four smaller ones.

    public class OtriTest
    {
        // The vertices of the mesh.
        Vertex[] vertices;

        private Triangle[] CreateExampleMesh()
        {
            var triangles = new Triangle[4];

            // Outer space triangle.
            var dummy = new Triangle() { id = -1 };

            // Setup triangles. Keep in mind the ordering:
            //
            // The CreateTriangle method takes vertex arguments in range org-dest-apex,
            // but internally the vertices get stored in an array with ordering
            //
            //    [0] = apex
            //    [1] = org
            //    [2] = dest
            //
            // This is because, for example, and oriented triange will return as its
            // origin (Org() method) the element at index [(orientation + 1) % 3].
            var t0 = triangles[0] = Helper.CreateTriangle(0, vertices[0], vertices[1], vertices[3]);
            var t1 = triangles[1] = Helper.CreateTriangle(1, vertices[1], vertices[4], vertices[3]);
            var t2 = triangles[2] = Helper.CreateTriangle(2, vertices[1], vertices[2], vertices[4]);
            var t3 = triangles[3] = Helper.CreateTriangle(3, vertices[3], vertices[4], vertices[5]);

            // Setup connectivity of triangle 0.
            t0.neighbors[0].tri = dummy;
            t0.neighbors[1].tri = t1;
            t0.neighbors[1].orient = 2;
            t0.neighbors[2].tri = dummy;

            // Setup connectivity of triangle 1.
            t1.neighbors[0].tri = t2;
            t1.neighbors[0].orient = 2;
            t1.neighbors[1].tri = t3;
            t1.neighbors[1].orient = 0;
            t1.neighbors[2].tri = t0;
            t1.neighbors[2].orient = 1;

            // Setup connectivity of triangle 2.
            t2.neighbors[0].tri = dummy;
            t2.neighbors[1].tri = dummy;
            t2.neighbors[2].tri = t1;
            t2.neighbors[2].orient = 0;

            // Setup connectivity of triangle 3.
            t3.neighbors[0].tri = t1;
            t3.neighbors[0].orient = 1;
            t3.neighbors[1].tri = dummy;
            t3.neighbors[2].tri = dummy;

            return triangles;
        }

        [SetUp]
        public void Initialize()
        {
            vertices = new Vertex[6];

            vertices[0] = new Vertex(-2.0, 0.0) { id = 0 };
            vertices[1] = new Vertex( 0.0, 0.0) { id = 1 };
            vertices[2] = new Vertex( 2.0, 0.0) { id = 2 };
            vertices[3] = new Vertex(-1.0, 1.0) { id = 3 };
            vertices[4] = new Vertex( 1.0, 1.0) { id = 4 };
            vertices[5] = new Vertex( 0.0, 2.0) { id = 5 };
        }

        [Test]
        public void TestOrg()
        {
            Otri t = default;

            t.tri = Helper.CreateTriangle(0, vertices[1], vertices[4], vertices[3]);

            t.orient = 0;
            Assert.AreEqual(1, t.Org().ID);

            t.orient = 1;
            Assert.AreEqual(4, t.Org().ID);

            t.orient = 2;
            Assert.AreEqual(3, t.Org().ID);
        }

        [Test]
        public void TestDest()
        {
            Otri t = default;

            t.tri = Helper.CreateTriangle(0, vertices[1], vertices[4], vertices[3]);

            t.orient = 0;
            Assert.AreEqual(4, t.Dest().ID);

            t.orient = 1;
            Assert.AreEqual(3, t.Dest().ID);

            t.orient = 2;
            Assert.AreEqual(1, t.Dest().ID);
        }

        [Test]
        public void TestApex()
        {
            Otri t = default;

            t.tri = Helper.CreateTriangle(0, vertices[1], vertices[4], vertices[3]);

            t.orient = 0;
            Assert.AreEqual(3, t.Apex().ID);

            t.orient = 1;
            Assert.AreEqual(1, t.Apex().ID);

            t.orient = 2;
            Assert.AreEqual(4, t.Apex().ID);
        }

        [Test]
        public void TestSym()
        {
            var triangles = CreateExampleMesh();

            Otri t = default;
            Otri s = default;

            // The center triangle.
            t.tri = triangles[1];

            t.orient = 0;
            t.Sym(ref s);
            Assert.AreEqual(2, s.tri.ID);

            t.orient = 1;
            t.Sym(ref s);
            Assert.AreEqual(3, s.tri.ID);

            t.orient = 2;
            t.Sym(ref s);
            Assert.AreEqual(0, s.tri.ID);
        }

        [Test]
        public void TestLnext()
        {
            var triangles = CreateExampleMesh();

            Otri t = default;

            // The center triangle.
            t.tri = triangles[1];
            t.orient = 0;

            t.Lnext();
            Assert.AreEqual(4, t.Org().ID);

            t.Lnext();
            Assert.AreEqual(3, t.Org().ID);

            t.Lnext();
            Assert.AreEqual(1, t.Org().ID);
        }

        [Test]
        public void TestLprev()
        {
            var triangles = CreateExampleMesh();

            Otri t = default;

            // The center triangle.
            t.tri = triangles[1];
            t.orient = 0;

            t.Lprev();
            Assert.AreEqual(3, t.Org().ID);

            t.Lprev();
            Assert.AreEqual(4, t.Org().ID);

            t.Lprev();
            Assert.AreEqual(1, t.Org().ID);
        }

        [Test]
        public void TestOnext()
        {
            var triangles = CreateExampleMesh();

            Otri t = default;

            // Start with the bottom right triangle.
            t.tri = triangles[2];

            // Start with edge  1 -> 2.
            t.orient = 0;

            // Make sure we're on the correct edge.
            Assert.AreEqual(1, t.Org().ID);
            Assert.AreEqual(2, t.Dest().ID);

            t.Onext();
            Assert.AreEqual(1, t.Org().ID);
            Assert.AreEqual(4, t.Dest().ID);
            Assert.AreEqual(1, t.tri.ID);

            t.Onext();
            Assert.AreEqual(1, t.Org().ID);
            Assert.AreEqual(3, t.Dest().ID);
            Assert.AreEqual(0, t.tri.ID);

            // Out of mesh.
            t.Onext();
            Assert.AreEqual(-1, t.tri.ID);
        }

        [Test]
        public void TestOprev()
        {
            var triangles = CreateExampleMesh();

            Otri t = default;

            // Start with the bottom left triangle.
            t.tri = triangles[0];

            // Start with edge  1 -> 3.
            t.orient = 1;

            // Make sure we're on the correct edge.
            Assert.AreEqual(1, t.Org().ID);
            Assert.AreEqual(3, t.Dest().ID);

            t.Oprev();
            Assert.AreEqual(1, t.Org().ID);
            Assert.AreEqual(4, t.Dest().ID);
            Assert.AreEqual(1, t.tri.ID);

            t.Oprev();
            Assert.AreEqual(1, t.Org().ID);
            Assert.AreEqual(2, t.Dest().ID);
            Assert.AreEqual(2, t.tri.ID);

            // Out of mesh.
            t.Oprev();
            Assert.AreEqual(-1, t.tri.ID);
        }

        [Test]
        public void TestDnext()
        {
            var triangles = CreateExampleMesh();

            Otri t = default;

            // Start with the bottom left triangle.
            t.tri = triangles[0];

            // Start with edge  1 -> 3.
            t.orient = 1;

            // Make sure we're on the correct edge.
            Assert.AreEqual(1, t.Org().ID);
            Assert.AreEqual(3, t.Dest().ID);

            t.Dnext();
            Assert.AreEqual(4, t.Org().ID);
            Assert.AreEqual(3, t.Dest().ID);
            Assert.AreEqual(1, t.tri.ID);

            t.Dnext();
            Assert.AreEqual(5, t.Org().ID);
            Assert.AreEqual(3, t.Dest().ID);
            Assert.AreEqual(3, t.tri.ID);

            // Out of mesh.
            t.Dnext();
            Assert.AreEqual(-1, t.tri.ID);
        }

        [Test]
        public void TestDprev()
        {
            var triangles = CreateExampleMesh();

            Otri t = default;

            // Start with the top triangle.
            t.tri = triangles[3];

            // Start with edge  5 -> 3.
            t.orient = 2;

            // Make sure we're on the correct edge.
            Assert.AreEqual(5, t.Org().ID);
            Assert.AreEqual(3, t.Dest().ID);

            t.Dprev();
            Assert.AreEqual(4, t.Org().ID);
            Assert.AreEqual(3, t.Dest().ID);
            Assert.AreEqual(1, t.tri.ID);

            t.Dprev();
            Assert.AreEqual(1, t.Org().ID);
            Assert.AreEqual(3, t.Dest().ID);
            Assert.AreEqual(0, t.tri.ID);

            // Out of mesh.
            t.Dprev();
            Assert.AreEqual(-1, t.tri.ID);
        }

        [Test]
        public void TestRnext()
        {
            var triangles = CreateExampleMesh();

            Otri t = default;

            // Start with the bottom left triangle.
            t.tri = triangles[0];

            // Start with edge  1 -> 3.
            t.orient = 1;

            // Make sure we're on the correct edge.
            Assert.AreEqual(1, t.Org().ID);
            Assert.AreEqual(3, t.Dest().ID);

            t.Rnext();
            Assert.AreEqual(4, t.Org().ID);
            Assert.AreEqual(1, t.Dest().ID);
            Assert.AreEqual(2, t.tri.ID);

            t.Rnext();
            Assert.AreEqual(3, t.Org().ID);
            Assert.AreEqual(4, t.Dest().ID);
            Assert.AreEqual(3, t.tri.ID);

            // Back where we started.
            t.Rnext();
            Assert.AreEqual(0, t.tri.ID);
        }

        [Test]
        public void TestRprev()
        {
            var triangles = CreateExampleMesh();

            Otri t = default;

            // Start with the top triangle.
            t.tri = triangles[3];

            // Start with edge  3 -> 4.
            t.orient = 0;

            // Make sure we're on the correct edge.
            Assert.AreEqual(3, t.Org().ID);
            Assert.AreEqual(4, t.Dest().ID);

            t.Rprev();
            Assert.AreEqual(4, t.Org().ID);
            Assert.AreEqual(1, t.Dest().ID);
            Assert.AreEqual(2, t.tri.ID);

            t.Rprev();
            Assert.AreEqual(1, t.Org().ID);
            Assert.AreEqual(3, t.Dest().ID);
            Assert.AreEqual(0, t.tri.ID);

            // Back where we started.
            t.Rprev();
            Assert.AreEqual(3, t.tri.ID);
        }

        [Test]
        public void TestBond()
        {
            Otri s = default;
            Otri t = default;

            Otri tmp = default;

            s.tri = Helper.CreateTriangle(0, vertices[0], vertices[1], vertices[3]);
            t.tri = Helper.CreateTriangle(1, vertices[1], vertices[4], vertices[3]);

            s.orient = 1; // Edge  1 -> 3.
            t.orient = 2; // Edge  3 -> 1.

            // Make sure we're on the correct edges.
            Assert.AreEqual(1, s.Org().ID);
            Assert.AreEqual(3, s.Dest().ID);
            Assert.AreEqual(3, t.Org().ID);
            Assert.AreEqual(1, t.Dest().ID);

            // Check that the triangles don't have neighbors.
            s.Sym(ref tmp);
            //Assert.AreEqual(-1, tmp.tri.ID);
            Assert.IsNull(tmp.tri);
            t.Sym(ref tmp);
            //Assert.AreEqual(-1, tmp.tri.ID);
            Assert.IsNull(tmp.tri);

            // Bond the two triangles.
            s.Bond(ref t);

            // Check that neighbors are properly set.
            s.Sym(ref tmp);
            Assert.AreEqual(1, tmp.tri.ID);
            t.Sym(ref tmp);
            Assert.AreEqual(0, tmp.tri.ID);
        }

        [Test]
        public void TestDissolve()
        {
            // Outer space triangle.
            var dummy = new Triangle() { id = -1 };

            var triangles = CreateExampleMesh();

            Otri s = default;
            Otri t = default;

            Otri tmp = default;

            // The bottom left triangle with edge 1 -> 3.
            s.tri = triangles[0];
            s.orient = 1;

            // The center triangle with edge 3 -> 1.
            t.tri = triangles[1];
            t.orient = 2;

            // Make sure we're on the correct edges.
            Assert.AreEqual(1, s.Org().ID);
            Assert.AreEqual(3, s.Dest().ID);
            Assert.AreEqual(3, t.Org().ID);
            Assert.AreEqual(1, t.Dest().ID);

            // Check that neighbors are properly set.
            s.Sym(ref tmp);
            Assert.AreEqual(1, tmp.tri.ID);
            t.Sym(ref tmp);
            Assert.AreEqual(0, tmp.tri.ID);

            // Now dissolve the bond from one side.
            s.Dissolve(dummy);

            // Check neighbors.
            s.Sym(ref tmp);
            Assert.AreEqual(-1, tmp.tri.ID);
            t.Sym(ref tmp);
            Assert.AreEqual(0, tmp.tri.ID);

            // And dissolve the bond from the other side.
            t.Dissolve(dummy);

            // Check neighbors.
            s.Sym(ref tmp);
            Assert.AreEqual(-1, tmp.tri.ID);
            t.Sym(ref tmp);
            Assert.AreEqual(-1, tmp.tri.ID);
        }
    }
}
