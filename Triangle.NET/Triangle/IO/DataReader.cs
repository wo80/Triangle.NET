// -----------------------------------------------------------------------
// <copyright file="io.cs" company="">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/triangle/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Globalization;
    using TriangleNet.Data;
    using TriangleNet.Log;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class DataReader
    {
        class StackTri
        {
            public Otri tri = default(Otri);
            public StackTri next;
        }

        #region Library

        /// <summary>
        /// Reconstruct a triangulation from its raw data representation.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>
        /// Reads an .ele file and reconstructs the original mesh.  If the -p switch
        /// is used, this procedure will also read a .poly file and reconstruct the
        /// subsegments of the original mesh.  If the -a switch is used, this
        /// procedure will also read an .area file and set a maximum area constraint
        /// on each triangle.
        ///
        /// Vertices that are not corners of triangles, such as nodes on edges of
        /// subparametric elements, are discarded.
        ///
        /// This routine finds the adjacencies between triangles (and subsegments)
        /// by forming one stack of triangles for each vertex. Each triangle is on
        /// three different stacks simultaneously. Each triangle's subsegment
        /// pointers are used to link the items in each stack. This memory-saving
        /// feature makes the code harder to read. The most important thing to keep
        /// in mind is that each triangle is removed from a stack precisely when
        /// the corresponding pointer is adjusted to refer to a subsegment rather
        /// than the next triangle of the stack.
        /// </remarks>
        public static int Reconstruct(Mesh mesh, MeshData input)
        {
            long hullsize = 0;

            Otri tri = default(Otri);
            Otri triangleleft = default(Otri);
            Otri checktri = default(Otri);
            Otri checkleft = default(Otri);
            Otri checkneighbor = default(Otri);
            Osub subseg = default(Osub);
            List<Otri>[] vertexarray; // Triangle
            Otri prevlink; // Triangle
            Otri nexttri; // Triangle
            Vertex tdest, tapex;
            Vertex checkdest, checkapex;
            Vertex shorg;
            Vertex segmentorg, segmentdest;
            int[] corner = new int[3];
            int[] end = new int[2];
            bool segmentmarkers = false;
            int boundmarker;
            int aroundvertex;
            bool notfound;
            int i = 0;

            int elements = input.Triangles == null ? 0 : input.Triangles.Length;
            int attribs = input.TriangleAttributes == null ? 0 : input.TriangleAttributes.Length;
            int numberofsegments = input.Segments == null ? 0 : input.Segments.Length;

            mesh.inelements = elements;
            mesh.eextras = attribs;

            // Create the triangles.
            for (i = 0; i < mesh.inelements; i++)
            {
                mesh.MakeTriangle(ref tri);
                // Mark the triangle as living.
                //tri.triangle.neighbors[0].triangle = tri.triangle;
            }

            if (Behavior.Poly)
            {
                mesh.insegments = numberofsegments;
                segmentmarkers = input.SegmentMarkers != null;

                // Create the subsegments.
                for (i = 0; i < mesh.insegments; i++)
                {
                    mesh.MakeSubseg(ref subseg);
                    // Mark the subsegment as living.
                    //subseg.ss.subsegs[0].ss = subseg.ss;
                }
            }

            // Allocate a temporary array that maps each vertex to some adjacent
            // triangle. I took care to allocate all the permanent memory for
            // triangles and subsegments first.
            vertexarray = new List<Otri>[mesh.vertices.Count];
            // Each vertex is initially unrepresented.
            for (i = 0; i < mesh.vertices.Count; i++)
            {
                Otri tmp = default(Otri);
                tmp.triangle = Mesh.dummytri;
                vertexarray[i] = new List<Otri>(3);
                vertexarray[i].Add(tmp);
            }

            i = 0;
            string debug = "";
            // Read the triangles from the .ele file, and link
            // together those that share an edge.
            foreach (var item in mesh.triangles.Values)
            {
                tri.triangle = item;

                // Copy the triangle's three corners.
                for (int j = 0; j < 3; j++)
                {
                    corner[j] = input.Triangles[i][j];
                    if ((corner[j] < 0) || (corner[j] >= mesh.invertices))
                    {
                        SimpleLogger.Instance.Error("Triangle has an invalid vertex index.", "MeshReader.Reconstruct()");
                        throw new Exception("Triangle has an invalid vertex index.");
                    }
                }

                // Read the triangle's attributes.
                for (int j = 0; j < mesh.eextras; j++)
                {
                    tri.triangle.attributes[j] = input.TriangleAttributes[i][j];
                }

                // TODO
                if (Behavior.VarArea)
                {
                    tri.triangle.area = input.TriangleAreas[i];
                }

                // Set the triangle's vertices.
                tri.orient = 0;
                tri.SetOrg(mesh.vertices[corner[0]]);
                tri.SetDest(mesh.vertices[corner[1]]);
                tri.SetApex(mesh.vertices[corner[2]]);

                debug += String.Format("Checking element {0} [{1}, {2}, {3}]\n", i, corner[0], corner[1], corner[2]);

                // Try linking the triangle to others that share these vertices.
                for (tri.orient = 0; tri.orient < 3; tri.orient++)
                {
                    // Take the number for the origin of triangleloop.
                    aroundvertex = corner[tri.orient];
                    int index = vertexarray[aroundvertex].Count - 1;
                    // Look for other triangles having this vertex.
                    nexttri = vertexarray[aroundvertex][index];
                    // Link the current triangle to the next one in the stack.
                    //tri.triangle.neighbors[tri.orient] = nexttri;
                    // Push the current triangle onto the stack.
                    vertexarray[aroundvertex].Add(tri);

                    checktri = nexttri;

                    debug += String.Format("  {0}: aroundvertex = {1}\n", tri.orient, aroundvertex);
                    if (checktri.triangle != Mesh.dummytri)
                    {
                        tdest = tri.Dest();
                        tapex = tri.Apex();

                        debug += String.Format("  No dummy: tdest ({0}, {1}), tapex ({2}, {3})\n",
                            tdest[0], tdest[1], tapex[0], tapex[1]);
                        // Look for other triangles that share an edge.
                        do
                        {
                            checkdest = checktri.Dest();
                            checkapex = checktri.Apex();

                            debug += String.Format("    checktri.orient {0}\n", checktri.orient);

                            debug += String.Format("    checkdest ({0}, {1}), checkapex ({2}, {3})\n",
                                checkdest[0], checkdest[1], checkapex[0], checkapex[1]);

                            if (tapex == checkdest)
                            {
                                debug += String.Format("    > tapex == checkdest\n");
                                // The two triangles share an edge; bond them together.
                                tri.Lprev(ref triangleleft);
                                triangleleft.Bond(ref checktri);
                            }
                            if (tdest == checkapex)
                            {
                                debug += String.Format("    > tdest == checkapex\n");
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

            // Prepare to count the boundary edges.
            hullsize = 0;
            if (Behavior.Poly)
            {
                // Read the segments from the .poly file, and link them
                // to their neighboring triangles.
                boundmarker = 0;
                i = 0;
                foreach (var item in mesh.subsegs.Values)
                {
                    subseg.ss = item;

                    end[0] = input.Segments[i][0];
                    end[1] = input.Segments[i][1];
                    if (segmentmarkers)
                    {
                        boundmarker = input.SegmentMarkers[i];
                    }

                    for (int j = 0; j < 2; j++)
                    {
                        if ((end[j] < 0) || (end[j] >= mesh.invertices))
                        {
                            SimpleLogger.Instance.Error("Segment has an invalid vertex index.", "MeshReader.Reconstruct()");
                            throw new Exception("Segment has an invalid vertex index.");
                        }
                    }

                    debug += String.Format("Checking segment {0} [{1}, {2}]\n", i, end[0], end[1]);
                    // set the subsegment's vertices.
                    subseg.ssorient = 0;
                    segmentorg = mesh.vertices[end[0]];
                    segmentdest = mesh.vertices[end[1]];
                    subseg.SetOrg(segmentorg);
                    subseg.SetDest(segmentdest);
                    subseg.SetSegOrg(segmentorg);
                    subseg.SetSegDest(segmentdest);
                    subseg.ss.boundary = boundmarker;
                    // Try linking the subsegment to triangles that share these vertices.
                    for (subseg.ssorient = 0; subseg.ssorient < 2; subseg.ssorient++)
                    {
                        // Take the number for the destination of subsegloop.
                        aroundvertex = end[1 - subseg.ssorient];
                        debug += String.Format("  {0}: aroundvertex = {1}\n", subseg.ssorient, aroundvertex);
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
                            debug += String.Format("  No dummy: shorg ({0}, {1}), checkdest ({2}, {3})\n", 
                                shorg[0], shorg[1], checkdest[0], checkdest[1]);
          
                            if (shorg == checkdest)
                            {
                                debug +="    shorg == checkdest\n";
                                // We have a match. Remove this triangle from the list.
                                //prevlink = vertexarray[aroundvertex][index];
                                vertexarray[aroundvertex].Remove(prevlink);
                                // Bond the subsegment to the triangle.
                                checktri.SegBond(ref subseg);
                                // Check if this is a boundary edge.
                                checktri.Sym(ref checkneighbor);
                                if (checkneighbor.triangle == Mesh.dummytri)
                                {
                                    debug +="    checkneighbor.tri == m->dummytri\n";
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

            debug += "\nMark the remaining edges\n\n";
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
                    debug += "  checktri.triangle != Mesh.dummytri\n";
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
                        debug += "checkneighbor.triangle == Mesh.dummytri (hullsize = " + hullsize + ")\n";
                    }

                    checktri = nexttri;
                }
            }

            debug += "\nmesh.subsegs.Count = " + mesh.subsegs.Count;

            return (int)hullsize;
        }

        #endregion
    }
}
