// -----------------------------------------------------------------------
// <copyright file="Converter.cs" company="">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Meshing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TriangleNet.Data;
    using TriangleNet.Geometry;

    /// <summary>
    /// The Converter class provides methods for mesh reconstruction.
    /// </summary>
    public class Converter
    {
        /// <summary>
        /// Reconstruct a triangulation from its raw data representation.
        /// </summary>
        public Mesh ToMesh(Polygon polygon, IList<ITriangle> triangles)
        {
            return ToMesh(polygon, triangles.ToArray());
        }

        /// <summary>
        /// Reconstruct a triangulation from its raw data representation.
        /// </summary>
        public Mesh ToMesh(Polygon polygon, ITriangle[] triangles)
        {
            Otri tri = default(Otri);
            Osub subseg = default(Osub);
            int i = 0;

            int elements = triangles == null ? 0 : triangles.Length;
            int numberofsegments = polygon.Segments.Count;

            var mesh = new Mesh();

            mesh.TransferNodes(polygon.Points);

            mesh.inelements = elements;
            mesh.regions.AddRange(polygon.Regions);
            mesh.behavior.useRegions = polygon.Regions.Count > 0;

            if (polygon.Segments.Count > 0)
            {
                mesh.behavior.Poly = true;
                mesh.holes.AddRange(polygon.Holes);
            }

            // Create the triangles.
            for (i = 0; i < mesh.inelements; i++)
            {
                mesh.MakeTriangle(ref tri);
            }

            if (mesh.behavior.Poly)
            {
                mesh.insegments = numberofsegments;

                // Create the subsegments.
                for (i = 0; i < mesh.insegments; i++)
                {
                    mesh.MakeSegment(ref subseg);
                }
            }

            var vertexarray = SetNeighbors(mesh, triangles);

            SetSegments(mesh, polygon, vertexarray);

            return mesh;
        }

        /// <summary>
        /// Finds the adjacencies between triangles by forming a stack of triangles for
        /// each vertex. Each triangle is on three different stacks simultaneously.
        /// </summary>
        private static List<Otri>[] SetNeighbors(Mesh mesh, ITriangle[] triangles)
        {
            Otri tri = default(Otri);
            Otri triangleleft = default(Otri);
            Otri checktri = default(Otri);
            Otri checkleft = default(Otri);
            Otri nexttri;
            Vertex tdest, tapex;
            Vertex checkdest, checkapex;
            int[] corner = new int[3];
            int aroundvertex;
            int i;

            // Allocate a temporary array that maps each vertex to some adjacent triangle.
            var vertexarray = new List<Otri>[mesh.vertices.Count];

            // Each vertex is initially unrepresented.
            for (i = 0; i < mesh.vertices.Count; i++)
            {
                Otri tmp = default(Otri);
                tmp.triangle = Mesh.dummytri;
                vertexarray[i] = new List<Otri>(3);
                vertexarray[i].Add(tmp);
            }

            i = 0;

            // Read the triangles from the .ele file, and link
            // together those that share an edge.
            foreach (var item in mesh.triangles.Values)
            {
                tri.triangle = item;

                corner[0] = triangles[i].P0;
                corner[1] = triangles[i].P1;
                corner[2] = triangles[i].P2;

                // Copy the triangle's three corners.
                for (int j = 0; j < 3; j++)
                {
                    if ((corner[j] < 0) || (corner[j] >= mesh.invertices))
                    {
                        Log.Instance.Error("Triangle has an invalid vertex index.", "MeshReader.Reconstruct()");
                        throw new Exception("Triangle has an invalid vertex index.");
                    }
                }

                // Read the triangle's attributes.
                tri.triangle.region = triangles[i].Region;

                // TODO: VarArea
                if (mesh.behavior.VarArea)
                {
                    tri.triangle.area = triangles[i].Area;
                }

                // Set the triangle's vertices.
                tri.orient = 0;
                tri.SetOrg(mesh.vertices[corner[0]]);
                tri.SetDest(mesh.vertices[corner[1]]);
                tri.SetApex(mesh.vertices[corner[2]]);

                // Try linking the triangle to others that share these vertices.
                for (tri.orient = 0; tri.orient < 3; tri.orient++)
                {
                    // Take the number for the origin of triangleloop.
                    aroundvertex = corner[tri.orient];

                    int index = vertexarray[aroundvertex].Count - 1;

                    // Look for other triangles having this vertex.
                    nexttri = vertexarray[aroundvertex][index];

                    // Push the current triangle onto the stack.
                    vertexarray[aroundvertex].Add(tri);

                    checktri = nexttri;

                    if (checktri.triangle != Mesh.dummytri)
                    {
                        tdest = tri.Dest();
                        tapex = tri.Apex();

                        // Look for other triangles that share an edge.
                        do
                        {
                            checkdest = checktri.Dest();
                            checkapex = checktri.Apex();

                            if (tapex == checkdest)
                            {
                                // The two triangles share an edge; bond them together.
                                tri.Lprev(ref triangleleft);
                                triangleleft.Bond(ref checktri);
                            }
                            if (tdest == checkapex)
                            {
                                // The two triangles share an edge; bond them together.
                                checktri.Lprev(ref checkleft);
                                tri.Bond(ref checkleft);
                            }
                            // Find the next triangle in the stack.
                            index--;
                            nexttri = vertexarray[aroundvertex][index];

                            checktri = nexttri;
                        } while (checktri.triangle != Mesh.dummytri);
                    }
                }

                i++;
            }

            return vertexarray;
        }

        /// <summary>
        /// Finds the adjacencies between triangles and subsegments.
        /// </summary>
        private static void SetSegments(Mesh mesh, Polygon polygon, List<Otri>[] vertexarray)
        {
            Otri checktri = default(Otri);
            Otri nexttri; // Triangle
            Vertex checkdest;
            Otri checkneighbor = default(Otri);
            Osub subseg = default(Osub);
            Otri prevlink; // Triangle
            Vertex shorg;
            Vertex segmentorg, segmentdest;
            int[] end = new int[2];
            bool notfound;
            //bool segmentmarkers = false;
            int boundmarker;
            int aroundvertex;
            int i;

            int hullsize = 0;

            // Prepare to count the boundary edges.
            if (mesh.behavior.Poly)
            {
                // Link the segments to their neighboring triangles.
                boundmarker = 0;
                i = 0;
                foreach (var item in mesh.subsegs.Values)
                {
                    subseg.seg = item;

                    end[0] = polygon.Segments[i].P0;
                    end[1] = polygon.Segments[i].P1;
                    boundmarker = polygon.Segments[i].Boundary;

                    for (int j = 0; j < 2; j++)
                    {
                        if ((end[j] < 0) || (end[j] >= mesh.invertices))
                        {
                            Log.Instance.Error("Segment has an invalid vertex index.", "MeshReader.Reconstruct()");
                            throw new Exception("Segment has an invalid vertex index.");
                        }
                    }

                    // set the subsegment's vertices.
                    subseg.orient = 0;
                    segmentorg = mesh.vertices[end[0]];
                    segmentdest = mesh.vertices[end[1]];
                    subseg.SetOrg(segmentorg);
                    subseg.SetDest(segmentdest);
                    subseg.SetSegOrg(segmentorg);
                    subseg.SetSegDest(segmentdest);
                    subseg.seg.boundary = boundmarker;
                    // Try linking the subsegment to triangles that share these vertices.
                    for (subseg.orient = 0; subseg.orient < 2; subseg.orient++)
                    {
                        // Take the number for the destination of subsegloop.
                        aroundvertex = end[1 - subseg.orient];
                        int index = vertexarray[aroundvertex].Count - 1;
                        // Look for triangles having this vertex.
                        prevlink = vertexarray[aroundvertex][index];
                        nexttri = vertexarray[aroundvertex][index];

                        checktri = nexttri;
                        shorg = subseg.Org();
                        notfound = true;
                        // Look for triangles having this edge.  Note that I'm only
                        // comparing each triangle's destination with the subsegment;
                        // each triangle's apex is handled through a different vertex.
                        // Because each triangle appears on three vertices' lists, each
                        // occurrence of a triangle on a list can (and does) represent
                        // an edge.  In this way, most edges are represented twice, and
                        // every triangle-subsegment bond is represented once.
                        while (notfound && (checktri.triangle != Mesh.dummytri))
                        {
                            checkdest = checktri.Dest();

                            if (shorg == checkdest)
                            {
                                // We have a match. Remove this triangle from the list.
                                //prevlink = vertexarray[aroundvertex][index];
                                vertexarray[aroundvertex].Remove(prevlink);
                                // Bond the subsegment to the triangle.
                                checktri.SegBond(ref subseg);
                                // Check if this is a boundary edge.
                                checktri.Sym(ref checkneighbor);
                                if (checkneighbor.triangle == Mesh.dummytri)
                                {
                                    // The next line doesn't insert a subsegment (because there's
                                    // already one there), but it sets the boundary markers of
                                    // the existing subsegment and its vertices.
                                    mesh.InsertSubseg(ref checktri, 1);
                                    hullsize++;
                                }
                                notfound = false;
                            }
                            index--;
                            // Find the next triangle in the stack.
                            prevlink = vertexarray[aroundvertex][index];
                            nexttri = vertexarray[aroundvertex][index];

                            checktri = nexttri;
                        }
                    }

                    i++;
                }
            }

            // Mark the remaining edges as not being attached to any subsegment.
            // Also, count the (yet uncounted) boundary edges.
            for (i = 0; i < mesh.vertices.Count; i++)
            {
                // Search the stack of triangles adjacent to a vertex.
                int index = vertexarray[i].Count - 1;
                nexttri = vertexarray[i][index];
                checktri = nexttri;

                while (checktri.triangle != Mesh.dummytri)
                {
                    // Find the next triangle in the stack before this
                    // information gets overwritten.
                    index--;
                    nexttri = vertexarray[i][index];
                    // No adjacent subsegment.  (This overwrites the stack info.)
                    checktri.SegDissolve();
                    checktri.Sym(ref checkneighbor);
                    if (checkneighbor.triangle == Mesh.dummytri)
                    {
                        mesh.InsertSubseg(ref checktri, 1);
                        hullsize++;
                    }

                    checktri = nexttri;
                }
            }

            mesh.hullsize = hullsize;
            mesh.edges = (3 * mesh.triangles.Count + hullsize) / 2;
        }
    }
}
