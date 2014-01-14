
namespace MeshExplorer.Topology
{
    using TriangleNet.Data;
    using TriangleNet.Geometry;

    public class OrientedTriangle
    {
        /// <summary>
        /// 
        /// </summary>
        public ITriangle Triangle { get; set; }

        /// <summary>
        /// Ranges from 0 to 2.
        /// </summary>
        public int Orientation { get; set; }

        #region Oriented triangle primitives

        // For fast access
        static readonly int[] plus1Mod3 = { 1, 2, 0 };
        static readonly int[] minus1Mod3 = { 2, 0, 1 };

        /// <summary>
        /// Find the abutting triangle; same edge. [sym(abc) -> ba*]
        /// </summary>
        public void Sym()
        {
            //this = tri.triangles[Orientation];
            // decode(ptr, otri);

            var org = this.Org();
            Triangle = Triangle.GetNeighbor(Orientation);
            Orientation = GetOrientation(Triangle, org.ID);
        }

        /// <summary>
        /// Find the next edge (counterclockwise) of a triangle. [lnext(abc) -> bca]
        /// </summary>
        public void Lnext()
        {
            Orientation = plus1Mod3[Orientation];
        }

        /// <summary>
        /// Find the previous edge (clockwise) of a triangle. [lprev(abc) -> cab]
        /// </summary>
        public void Lprev()
        {
            Orientation = minus1Mod3[Orientation];
        }

        /// <summary>
        /// Find the next edge counterclockwise with the same origin. [onext(abc) -> ac*]
        /// </summary>
        public void Onext()
        {
            Lprev();
            Sym();
        }

        /// <summary>
        /// Find the next edge clockwise with the same origin. [oprev(abc) -> a*b]
        /// </summary>
        public void Oprev()
        {
            Sym();
            Lnext();
        }

        /// <summary>
        /// Find the next edge counterclockwise with the same destination. [dnext(abc) -> *ba]
        /// </summary>
        public void Dnext()
        {
            Sym();
            Lprev();
        }

        /// <summary>
        /// Find the next edge clockwise with the same destination. [dprev(abc) -> cb*]
        /// </summary>
        public void Dprev()
        {
            Lnext();
            Sym();
        }

        /// <summary>
        /// Find the next edge (counterclockwise) of the adjacent triangle. [rnext(abc) -> *a*]
        /// </summary>
        public void Rnext()
        {
            Sym();
            Lnext();
            Sym();
        }

        /// <summary>
        /// Find the previous edge (clockwise) of the adjacent triangle. [rprev(abc) -> b**]
        /// </summary>
        public void Rprev()
        {
            Sym();
            Lprev();
            Sym();
        }

        /// <summary>
        /// Origin [org(abc) -> a]
        /// </summary>
        public Vertex Org()
        {
            return Triangle.GetVertex(plus1Mod3[Orientation]);
        }

        /// <summary>
        /// Destination [dest(abc) -> b]
        /// </summary>
        public Vertex Dest()
        {
            return Triangle.GetVertex(minus1Mod3[Orientation]);
        }

        /// <summary>
        /// Apex [apex(abc) -> c]
        /// </summary>
        public Vertex Apex()
        {
            return Triangle.GetVertex(Orientation);
        }

        #endregion

        private int GetOrientation(ITriangle tri, int org)
        {
            if (tri == null)
            {
                return 0;
            }

            if (tri.P0 == org)
            {
                return 1;
            }

            if (tri.P1 == org)
            {
                return 2;
            }

            return 0;
        }
    }
}
