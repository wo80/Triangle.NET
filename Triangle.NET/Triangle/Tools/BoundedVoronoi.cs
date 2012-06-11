// -----------------------------------------------------------------------
// <copyright file="BoundedVoronoi.cs" company="">
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/index.html
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TriangleNet.Data;
    using TriangleNet.Geometry;

    /// <summary>
    /// The Bounded Voronoi Diagram is the dual of a PSLG triangulation.
    /// </summary>
    /// <remarks>
    /// 2D Centroidal Voronoi Tessellations with Constraints, 2010,
    /// Jane Tournois, Pierre Alliez and Olivier Devillers
    /// </remarks>
    public class BoundedVoronoi
    {
        Mesh mesh;
        Dictionary<int, Segment> subsegMap;
        List<Point[]> voronoi;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedVoronoi" /> class.
        /// </summary>
        /// <param name="mesh">Mesh instance.</param>
        public BoundedVoronoi(Mesh mesh)
        {
            this.mesh = mesh;
            this.voronoi = new List<Point[]>();

            // TODO: introduce isVertexMapValid in mesh and don't call
            // MakeVertexMap() if not necessary.
            this.mesh.MakeVertexMap();
        }

        /// <summary>
        /// Gets the list of Voronoi cells.
        /// </summary>
        public List<Point[]> Cells
        {
            get { return voronoi; }
        }

        /// <summary>
        /// Computes the bounded voronoi diagram.
        /// </summary>
        public void Generate()
        {
            voronoi.Clear();

            TagBlindTriangles();

            foreach (var v in mesh.vertices.Values)
            {
                if (v.type == VertexType.FreeVertex)
                {
                    voronoi.Add(ConstructBvdCell(v));
                }
                else
                {
                    voronoi.Add(ConstructBoundaryBvdCell(v));
                }
            }
        }

        private void TagBlindTriangles()
        {
            int blinded = 0;

            Stack<Triangle> triangles;
            subsegMap = new Dictionary<int, Segment>();

            Otri f = default(Otri);
            Otri f0 = default(Otri);
            Osub e = default(Osub);
            Osub sub1 = default(Osub);

            // Tag all triangles non-blind
            foreach (var t in mesh.triangles.Values)
            {
                // Use the infected flag for 'blinded' attribute.
                t.infected = false;
            }

            // for each constrained edge e of cdt do
            foreach (var ss in mesh.subsegs.Values)
            {
                // Create a stack: triangles
                triangles = new Stack<Triangle>();

                // for both adjacent triangles fe to e tagged non-blind do
                // Push fe into triangles
                e.seg = ss;
                e.orient = 0;
                e.TriPivot(ref f);

                if (f.triangle != Mesh.dummytri && !f.triangle.infected)
                {
                    triangles.Push(f.triangle);
                }

                e.SymSelf();
                e.TriPivot(ref f);

                if (f.triangle != Mesh.dummytri && !f.triangle.infected)
                {
                    triangles.Push(f.triangle);
                }

                // while triangles is non-empty
                while (triangles.Count > 0)
                {
                    // Pop f from stack triangles
                    f.triangle = triangles.Pop();
                    f.orient = 0;

                    // if f is blinded by e (use P) then
                    if (TriangleIsBlinded(ref f, ref e))
                    {
                        // Tag f as blinded by e
                        f.triangle.infected = true;
                        blinded++;

                        // Store association triangle -> subseg
                        subsegMap.Add(f.triangle.hash, e.seg);

                        // for each adjacent triangle f0 to f do
                        for (f.orient = 0; f.orient < 3; f.orient++)
                        {
                            f.Sym(ref f0);

                            f0.SegPivot(ref sub1);

                            // if f0 is finite and tagged non-blind & the common edge 
                            // between f and f0 is unconstrained then
                            if (f0.triangle != Mesh.dummytri && !f0.triangle.infected && sub1.seg == Mesh.dummysub)
                            {
                                // Push f0 into triangles.
                                triangles.Push(f0.triangle);
                            }
                        }
                    }
                }
            }

            blinded = 0;
        }

        private bool TriangleIsBlinded(ref Otri tri, ref Osub seg)
        {
            Point cc, pt = new Point();
            double xi = 0, eta = 0;

            Vertex torg = tri.Org();
            Vertex tdest = tri.Dest();
            Vertex tapex = tri.Apex();

            cc = Primitives.FindCircumcenter(torg, tdest, tapex, ref xi, ref eta);

            if (SegmentsIntersect(ref seg, cc, torg, ref pt, true))
            {
                return true;
            }

            if (SegmentsIntersect(ref seg, cc, tdest, ref pt, true))
            {
                return true;
            }

            if (SegmentsIntersect(ref seg, cc, tapex, ref pt, true))
            {
                return true;
            }

            return false;
        }

        private Point[] ConstructBvdCell(Vertex x)
        {
            Otri f = default(Otri);
            Otri f_init = default(Otri);
            Otri f_next = default(Otri);
            Osub sf = default(Osub);
            Osub sfn = default(Osub);

            Vertex torg, tdest, tapex;
            Point cc_f, cc_f_next, p;
            double xi = 0, eta = 0;

            // Call P the polygon (cell) in construction
            List<Point> P = new List<Point>();

            // Call f_init a triangle incident to x
            x.tri.Copy(ref f_init);

            if (f_init.Org() != x)
            {
                throw new Exception("ConstructBvdCell: inconsistent topology.");
            }

            // Let f be initialized to f_init
            f_init.Copy(ref f);
            // Call f_next the next triangle counterclockwise around x
            f_init.Onext(ref f_next);

            // repeat ... until f = f_init
            do
            {
                // Call Lffnext the line going through the circumcenters of f and f_next
                torg = f.Org();
                tdest = f.Dest();
                tapex = f.Apex();
                cc_f = Primitives.FindCircumcenter(torg, tdest, tapex, ref xi, ref eta);

                torg = f_next.Org();
                tdest = f_next.Dest();
                tapex = f_next.Apex();
                cc_f_next = Primitives.FindCircumcenter(torg, tdest, tapex, ref xi, ref eta);

                // if f is tagged non-blind then
                if (!f.triangle.infected)
                {
                    // Insert the circumcenter of f into P
                    P.Add(cc_f);

                    if (f_next.triangle.infected)
                    {
                        // Call S_fnext the constrained edge blinding f_next
                        sfn.seg = subsegMap[f_next.triangle.hash];

                        p = new Point();
                        // Insert point Lf,f_next /\ Sf_next into P
                        if (SegmentsIntersect(ref sfn, cc_f, cc_f_next, ref p, true))
                        {
                            P.Add(p);
                        }
                    }
                }
                else
                {
                    // Call Sf the constrained edge blinding f
                    sf.seg = subsegMap[f.triangle.hash];

                    // if f_next is tagged non-blind then
                    if (!f_next.triangle.infected)
                    {
                        p = new Point();
                        // Insert point Lf,f_next /\ Sf into P
                        if (SegmentsIntersect(ref sf, cc_f, cc_f_next, ref p, true))
                        {
                            P.Add(p);
                        }
                    }
                    else
                    {
                        // Call Sf_next the constrained edge blinding f_next
                        sfn.seg = subsegMap[f_next.triangle.hash];

                        // if Sf != Sf_next then
                        if (!sf.Equal(sfn))
                        {
                            p = new Point();
                            // Insert Lf,fnext /\ Sf and Lf,fnext /\ Sfnext into P
                            if (SegmentsIntersect(ref sf, cc_f, cc_f_next, ref p, true))
                            {
                                P.Add(p);
                            }

                            p = new Point();
                            if (SegmentsIntersect(ref sfn, cc_f, cc_f_next, ref p, true))
                            {
                                P.Add(p);
                            }
                        }
                    }
                }

                // f <- f_next
                f_next.Copy(ref f);

                // Call f_next the next triangle counterclockwise around x
                f_next.OnextSelf();
            }
            while (!f.Equal(f_init));

            // Output: Bounded Voronoi cell of x in counterclockwise order.
            return P.ToArray();
        }

        private Point[] ConstructBoundaryBvdCell(Vertex x)
        {
            Otri f = default(Otri);
            Otri f_init = default(Otri);
            Otri f_next = default(Otri);
            Otri f_prev = default(Otri);
            Osub sf = default(Osub);
            Osub sfn = default(Osub);

            Vertex torg, tdest, tapex;
            Point cc_f, cc_f_next, p;
            double xi = 0, eta = 0;

            // Call P the polygon (cell) in construction
            List<Point> P = new List<Point>();

            // Call f_init a triangle incident to x
            x.tri.Copy(ref f_init);

            if (f_init.Org() != x)
            {
                throw new Exception("ConstructBoundaryBvdCell: inconsistent topology.");
            }
            // Let f be initialized to f_init
            f_init.Copy(ref f);
            // Call f_next the next triangle counterclockwise around x
            f_init.Onext(ref f_next);

            f_init.Oprev(ref f_prev);

            // Is the border to the left?
            if (f_prev.triangle != Mesh.dummytri)
            {
                // Go clockwise until we reach the border (or the initial triangle)
                while (f_prev.triangle != Mesh.dummytri && !f_prev.Equal(f_init))
                {
                    f_prev.Copy(ref f);
                    f_prev.OprevSelf();
                }

                f.Copy(ref f_init);
                f.Onext(ref f_next);
            }

            // Add vertex on border
            P.Add(x);

            // Add midpoint of start triangles' edge.
            torg = f.Org();
            tdest = f.Dest();
            P.Add(new Point((torg.X + tdest.X) / 2, (torg.Y + tdest.Y) / 2));

            // repeat ... until f = f_init
            do
            {
                // Call Lffnext the line going through the circumcenters of f and f_next
                torg = f.Org();
                tdest = f.Dest();
                tapex = f.Apex();
                cc_f = Primitives.FindCircumcenter(torg, tdest, tapex, ref xi, ref eta);

                if (f_next.triangle == Mesh.dummytri)
                {
                    if (!f.triangle.infected)
                    {
                        // Add last circumcenter
                        P.Add(cc_f);
                    }

                    // Add midpoint of last triangles' edge (chances are it has already
                    // been added, so post process cell to remove duplicates???)
                    torg = f.Org();
                    tapex = f.Apex();
                    P.Add(new Point((torg.X + tapex.X) / 2, (torg.Y + tapex.Y) / 2));

                    break;
                }

                torg = f_next.Org();
                tdest = f_next.Dest();
                tapex = f_next.Apex();
                cc_f_next = Primitives.FindCircumcenter(torg, tdest, tapex, ref xi, ref eta);

                // if f is tagged non-blind then
                if (!f.triangle.infected)
                {
                    // Insert the circumcenter of f into P
                    P.Add(cc_f);

                    if (f_next.triangle.infected)
                    {
                        // Call S_fnext the constrained edge blinding f_next
                        sfn.seg = subsegMap[f_next.triangle.hash];

                        p = new Point();
                        // Insert point Lf,f_next /\ Sf_next into P
                        if (SegmentsIntersect(ref sfn, cc_f, cc_f_next, ref p, true))
                        {
                            P.Add(p);
                        }
                    }
                }
                else
                {
                    // Call Sf the constrained edge blinding f
                    sf.seg = subsegMap[f.triangle.hash];

                    // if f_next is tagged non-blind then
                    if (!f_next.triangle.infected)
                    {
                        tdest = f.Dest();
                        tapex = f.Apex();

                        // Both circumcenters lie on the blinded side, but we
                        // have to add the intersection with the segment.
                        p = new Point();

                        // Center of f edge dest->apex
                        Point bisec = new Point((tdest.X + tapex.X) / 2, (tdest.Y + tapex.Y) / 2);

                        // Find intersection of seg with line through f's bisector and circumcenter
                        if (SegmentsIntersect(ref sf, bisec, cc_f, ref p, false))
                        {
                            P.Add(p);
                        }



                        p = new Point();
                        // Insert point Lf,f_next /\ Sf into P
                        if (SegmentsIntersect(ref sf, cc_f, cc_f_next, ref p, true))
                        {
                            P.Add(p);
                        }
                    }
                    else
                    {
                        // Call Sf_next the constrained edge blinding f_next
                        sfn.seg = subsegMap[f_next.triangle.hash];

                        // if Sf != Sf_next then
                        if (!sf.Equal(sfn))
                        {
                            p = new Point();
                            // Insert Lf,fnext /\ Sf and Lf,fnext /\ Sfnext into P
                            if (SegmentsIntersect(ref sf, cc_f, cc_f_next, ref p, true))
                            {
                                P.Add(p);
                            }

                            p = new Point();
                            if (SegmentsIntersect(ref sfn, cc_f, cc_f_next, ref p, true))
                            {
                                P.Add(p);
                            }
                        }
                        else
                        {
                            // Both circumcenters lie on the blinded side, but we
                            // have to add the intersection with the segment.
                            p = new Point();

                            // Center of f_next edge org->dest
                            Point bisec = new Point((torg.X + tdest.X) / 2, (torg.Y + tdest.Y) / 2);

                            // Find intersection of seg with line through f_next's bisector and circumcenter
                            if (SegmentsIntersect(ref sf, bisec, cc_f_next, ref p, false))
                            {
                                P.Add(p);
                            }
                        }
                    }
                }

                // f <- f_next
                f_next.Copy(ref f);

                // Call f_next the next triangle counterclockwise around x
                f_next.OnextSelf();
            }
            while (!f.Equal(f_init));

            // Output: Bounded Voronoi cell of x in counterclockwise order.
            return P.ToArray();
        }

        /// <summary>
        /// Determines the intersection point of the line segment defined by points A and B with the 
        /// line segment defined by points C and D.
        /// </summary>
        /// <param name="seg">The first segment AB.</param>
        /// <param name="pc">Endpoint C of second segment.</param>
        /// <param name="pd">Endpoint D of second segment.</param>
        /// <param name="p">Reference to the intersection point.</param>
        /// <param name="strictIntersect">If false, pa and pb represent a line.</param>
        /// <returns>Returns true if the intersection point was found, and stores that point in X,Y.
        /// Returns false if there is no determinable intersection point, in which case X,Y will
        /// be unmodified.
        /// </returns>
        private bool SegmentsIntersect(ref Osub seg, Point pc, Point pd, ref Point p, bool strictIntersect)
        {
            Vertex s1 = seg.Org();
            Vertex s2 = seg.Dest();

            double Ax = s1.X, Ay = s1.Y;
            double Bx = s2.X, By = s2.Y;
            double Cx = pc.X, Cy = pc.Y;
            double Dx = pd.X, Dy = pd.Y;

            double distAB, theCos, theSin, newX, ABpos;

            //  Fail if either line segment is zero-length.
            if (Ax == Bx && Ay == By || Cx == Dx && Cy == Dy) return false;

            //  Fail if the segments share an end-point.
            if (Ax == Cx && Ay == Cy || Bx == Cx && By == Cy
            || Ax == Dx && Ay == Dy || Bx == Dx && By == Dy)
            {
                return false;
            }

            //  (1) Translate the system so that point A is on the origin.
            Bx -= Ax; By -= Ay;
            Cx -= Ax; Cy -= Ay;
            Dx -= Ax; Dy -= Ay;

            //  Discover the length of segment A-B.
            distAB = Math.Sqrt(Bx * Bx + By * By);

            //  (2) Rotate the system so that point B is on the positive X axis.
            theCos = Bx / distAB;
            theSin = By / distAB;
            newX = Cx * theCos + Cy * theSin;
            Cy = Cy * theCos - Cx * theSin; Cx = newX;
            newX = Dx * theCos + Dy * theSin;
            Dy = Dy * theCos - Dx * theSin; Dx = newX;

            //  Fail if segment C-D doesn't cross line A-B.
            if (Cy < 0 && Dy < 0 || Cy >= 0 && Dy >= 0 && strictIntersect) return false;

            //  (3) Discover the position of the intersection point along line A-B.
            ABpos = Dx + (Cx - Dx) * Dy / (Dy - Cy);

            //  Fail if segment C-D crosses line A-B outside of segment A-B.
            if (ABpos < 0 || ABpos > distAB && strictIntersect) return false;

            //  (4) Apply the discovered position to line A-B in the original coordinate system.
            p.x = Ax + ABpos * theCos;
            p.y = Ay + ABpos * theSin;

            //  Success.
            return true;
        }
    }
}
