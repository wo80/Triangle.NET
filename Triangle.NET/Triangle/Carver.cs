// -----------------------------------------------------------------------
// <copyright file="Carver.cs">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/triangle/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet
{
    using TriangleNet.Data;
    using System;

    /// <summary>
    /// Carves holes into the triangulation.
    /// </summary>
    class Carver
    {
        Mesh mesh;

        public Carver(Mesh mesh)
        {
            this.mesh = mesh;
        }

        /// <summary>
        /// Virally infect all of the triangles of the convex hull that are not 
        /// protected by subsegments. Where there are subsegments, set boundary 
        /// markers as appropriate.
        /// </summary>
        void InfectHull()
        {
            Otri hulltri = default(Otri);
            Otri nexttri = default(Otri);
            Otri starttri = default(Otri);
            Osub hullsubseg = default(Osub);
            Vertex horg, hdest;

            // Find a triangle handle on the hull.
            hulltri.triangle = Mesh.dummytri;
            hulltri.orient = 0;
            hulltri.SymSelf();
            // Remember where we started so we know when to stop.
            hulltri.Copy(ref starttri);
            // Go once counterclockwise around the convex hull.
            do
            {
                // Ignore triangles that are already infected.
                if (!hulltri.IsInfected())
                {
                    // Is the triangle protected by a subsegment?
                    hulltri.SegPivot(ref hullsubseg);
                    if (hullsubseg.ss == Mesh.dummysub)
                    {
                        // The triangle is not protected; infect it.
                        if (!hulltri.IsInfected())
                        {
                            hulltri.Infect();
                            mesh.viri.Add(hulltri.triangle);
                        }
                    }
                    else
                    {
                        // The triangle is protected; set boundary markers if appropriate.
                        if (hullsubseg.ss.boundary == 0)
                        {
                            hullsubseg.ss.boundary = 1;
                            horg = hulltri.Org();
                            hdest = hulltri.Dest();
                            if (horg.mark == 0)
                            {
                                horg.mark = 1;
                            }
                            if (hdest.mark == 0)
                            {
                                hdest.mark = 1;
                            }
                        }
                    }
                }
                // To find the next hull edge, go clockwise around the next vertex.
                hulltri.LnextSelf();
                hulltri.Oprev(ref nexttri);
                while (nexttri.triangle != Mesh.dummytri)
                {
                    nexttri.Copy(ref hulltri);
                    hulltri.Oprev(ref nexttri);
                }
            } while (!hulltri.Equal(starttri));
        }

        /// <summary>
        /// Spread the virus from all infected triangles to any neighbors not 
        /// protected by subsegments. Delete all infected triangles.
        /// </summary>
        /// <remarks>
        /// This is the procedure that actually creates holes and concavities.
        ///
        /// This procedure operates in two phases. The first phase identifies all
        /// the triangles that will die, and marks them as infected. They are
        /// marked to ensure that each triangle is added to the virus pool only
        /// once, so the procedure will terminate.
        ///
        /// The second phase actually eliminates the infected triangles. It also
        /// eliminates orphaned vertices.
        /// </remarks>
        void Plague()
        {
            Otri testtri = default(Otri);
            Otri neighbor = default(Otri);
            Triangle virusloop;
            Osub neighborsubseg = default(Osub);
            Vertex testvertex;
            Vertex norg, ndest;
            //Vertex deadorg, deaddest, deadapex;
            bool killorg;

            // Loop through all the infected triangles, spreading the virus to
            // their neighbors, then to their neighbors' neighbors.
            for (int i = 0; i < mesh.viri.Count; i++)
			{
                virusloop = mesh.viri[i];
                testtri.triangle = virusloop;
                // A triangle is marked as infected by messing with one of its pointers
                // to subsegments, setting it to an illegal value.  Hence, we have to
                // temporarily uninfect this triangle so that we can examine its
                // adjacent subsegments.
                // TODO: Not true in the C# version (so we could skip this).
                testtri.Uninfect();

                // Check each of the triangle's three neighbors.
                for (testtri.orient = 0; testtri.orient < 3; testtri.orient++)
                {
                    // Find the neighbor.
                    testtri.Sym(ref neighbor);
                    // Check for a subsegment between the triangle and its neighbor.
                    testtri.SegPivot(ref neighborsubseg);
                    // Check if the neighbor is nonexistent or already infected.
                    if ((neighbor.triangle == Mesh.dummytri) || neighbor.IsInfected())
                    {
                        if (neighborsubseg.ss != Mesh.dummysub)
                        {
                            // There is a subsegment separating the triangle from its
                            //   neighbor, but both triangles are dying, so the subsegment
                            //   dies too.
                            mesh.SubsegDealloc(neighborsubseg.ss);
                            if (neighbor.triangle != Mesh.dummytri)
                            {
                                // Make sure the subsegment doesn't get deallocated again
                                //   later when the infected neighbor is visited.
                                neighbor.Uninfect();
                                neighbor.SegDissolve();
                                neighbor.Infect();
                            }
                        }
                    }
                    else
                    {   // The neighbor exists and is not infected.
                        if (neighborsubseg.ss == Mesh.dummysub)
                        {
                            // There is no subsegment protecting the neighbor, so
                            // the neighbor becomes infected.
                            neighbor.Infect();
                            // Ensure that the neighbor's neighbors will be infected.
                            mesh.viri.Add(neighbor.triangle);
                        }
                        else
                        {               // The neighbor is protected by a subsegment.
                            // Remove this triangle from the subsegment.
                            neighborsubseg.TriDissolve();
                            // The subsegment becomes a boundary.  Set markers accordingly.
                            if (neighborsubseg.ss.boundary == 0)
                            {
                                neighborsubseg.ss.boundary = 1;
                            }
                            norg = neighbor.Org();
                            ndest = neighbor.Dest();
                            if (norg.mark == 0)
                            {
                                norg.mark = 1;
                            }
                            if (ndest.mark == 0)
                            {
                                ndest.mark = 1;
                            }
                        }
                    }
                }
                // Remark the triangle as infected, so it doesn't get added to the
                // virus pool again.
                testtri.Infect();
            }

            for (int i = 0; i < mesh.viri.Count; i++)
            {
                virusloop = mesh.viri[i];
                testtri.triangle = virusloop;

                // Check each of the three corners of the triangle for elimination.
                // This is done by walking around each vertex, checking if it is
                // still connected to at least one live triangle.
                for (testtri.orient = 0; testtri.orient < 3; testtri.orient++)
                {
                    testvertex=testtri.Org();
                    // Check if the vertex has already been tested.
                    if (testvertex != null)
                    {
                        killorg = true;
                        // Mark the corner of the triangle as having been tested.
                        testtri.SetOrg(null);
                        // Walk counterclockwise about the vertex.
                        testtri.Onext(ref neighbor);
                        // Stop upon reaching a boundary or the starting triangle.
                        while ((neighbor.triangle != Mesh.dummytri) &&
                               (!neighbor.Equal(testtri)))
                        {
                            if (neighbor.IsInfected())
                            {
                                // Mark the corner of this triangle as having been tested.
                                neighbor.SetOrg(null);
                            }
                            else
                            {
                                // A live triangle.  The vertex survives.
                                killorg = false;
                            }
                            // Walk counterclockwise about the vertex.
                            neighbor.OnextSelf();
                        }
                        // If we reached a boundary, we must walk clockwise as well.
                        if (neighbor.triangle == Mesh.dummytri)
                        {
                            // Walk clockwise about the vertex.
                            testtri.Oprev(ref neighbor);
                            // Stop upon reaching a boundary.
                            while (neighbor.triangle != Mesh.dummytri)
                            {
                                if (neighbor.IsInfected())
                                {
                                    // Mark the corner of this triangle as having been tested.
                                    neighbor.SetOrg(null);
                                }
                                else
                                {
                                    // A live triangle.  The vertex survives.
                                    killorg = false;
                                }
                                // Walk clockwise about the vertex.
                                neighbor.OprevSelf();
                            }
                        }
                        if (killorg)
                        {
                            // Deleting vertex
                            testvertex.type = VertexType.UndeadVertex;
                            mesh.undeads++;
                        }
                    }
                }

                // Record changes in the number of boundary edges, and disconnect
                // dead triangles from their neighbors.
                for (testtri.orient = 0; testtri.orient < 3; testtri.orient++)
                {
                    testtri.Sym(ref neighbor);
                    if (neighbor.triangle == Mesh.dummytri)
                    {
                        // There is no neighboring triangle on this edge, so this edge
                        // is a boundary edge.  This triangle is being deleted, so this
                        // boundary edge is deleted.
                        mesh.hullsize--;
                    }
                    else
                    {
                        // Disconnect the triangle from its neighbor.
                        neighbor.Dissolve();
                        // There is a neighboring triangle on this edge, so this edge
                        // becomes a boundary edge when this triangle is deleted.
                        mesh.hullsize++;
                    }
                }
                // Return the dead triangle to the pool of triangles.
                mesh.TriangleDealloc(testtri.triangle);
            }
            // Empty the virus pool.
            mesh.viri.Clear();
        }

        /// <summary>
        /// Spread regional attributes and/or area constraints (from a .poly file) 
        /// throughout the mesh.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="area"></param>
        /// <remarks>
        /// This procedure operates in two phases. The first phase spreads an
        /// attribute and/or an area constraint through a (segment-bounded) region.
        /// The triangles are marked to ensure that each triangle is added to the
        /// virus pool only once, so the procedure will terminate.
        ///
        /// The second phase uninfects all infected triangles, returning them to
        /// normal.
        /// </remarks>
        void RegionPlague(double attribute, double area)
        {
            Otri testtri = default(Otri);
            Otri neighbor = default(Otri);
            Osub neighborsubseg = default(Osub);

            // Loop through all the infected triangles, spreading the attribute
            // and/or area constraint to their neighbors, then to their neighbors'
            // neighbors.
            for (int i = 0; i < mesh.viri.Count; i++)
            {
                testtri.triangle = mesh.viri[i];
                // A triangle is marked as infected by messing with one of its pointers
                // to subsegments, setting it to an illegal value.  Hence, we have to
                // temporarily uninfect this triangle so that we can examine its
                // adjacent subsegments.
                // TODO: Not true in the C# version (so we could skip this).
                testtri.Uninfect();
                if (Behavior.RegionAttrib)
                {
                    // Set an attribute (Note: the attributes array was resized before).
                    testtri.triangle.attributes[mesh.eextras] = attribute;
                }
                if (Behavior.VarArea)
                {
                    // Set an area constraint.
                    testtri.triangle.area = area;
                }

                // Check each of the triangle's three neighbors.
                for (testtri.orient = 0; testtri.orient < 3; testtri.orient++)
                {
                    // Find the neighbor.
                    testtri.Sym(ref neighbor);
                    // Check for a subsegment between the triangle and its neighbor.
                    testtri.SegPivot(ref neighborsubseg);
                    // Make sure the neighbor exists, is not already infected, and
                    //  isn't protected by a subsegment.
                    if ((neighbor.triangle != Mesh.dummytri) && !neighbor.IsInfected()
                        && (neighborsubseg.ss == Mesh.dummysub))
                    {
                        // Infect the neighbor.
                        neighbor.Infect();
                        // Ensure that the neighbor's neighbors will be infected.
                        mesh.viri.Add(neighbor.triangle);
                    }
                }
                // Remark the triangle as infected, so it doesn't get added to the
                // virus pool again.
                testtri.Infect();
            }

            // Uninfect all triangles.
            foreach (var virus in mesh.viri)
            {
                testtri.triangle = virus;
                testtri.Uninfect();
            }

            // Empty the virus pool.
            mesh.viri.Clear();
        }
        /// <summary>
        /// Find the holes and infect them.  Find the area constraints and infect 
        /// them. Infect the convex hull. Spread the infection and kill triangles. 
        /// Spread the area constraints.
        /// </summary>
        /// <param name="holelist"></param>
        /// <param name="holes"></param>
        /// <param name="regionlist"></param>
        /// <param name="regions"></param>
        public void CarveHoles()
        {
            Otri searchtri = default(Otri);
            Otri tri = default(Otri);
            Vertex searchorg, searchdest;
            LocateResult intersect;

            int numRegions = mesh.regions.Count;
            Otri[] regiontris = (numRegions > 0) ? new Otri[numRegions] : null;

            if (!Behavior.Convex)
            {
                // Mark as infected any unprotected triangles on the boundary.
                // This is one way by which concavities are created.
                InfectHull();
            }

            if (!Behavior.NoHoles)
            {
                // Infect each triangle in which a hole lies.
                foreach (var hole in mesh.holes)
	            {
                    // Ignore holes that aren't within the bounds of the mesh.
                    if ((hole.X >= mesh.xmin) && (hole.X <= mesh.xmax)
                        && (hole.Y >= mesh.ymin) && (hole.Y <= mesh.ymax))
                    {
                        // Start searching from some triangle on the outer boundary.
                        searchtri.triangle = Mesh.dummytri;
                        searchtri.orient = 0;
                        searchtri.SymSelf();
                        // Ensure that the hole is to the left of this boundary edge;
                        // otherwise, locate() will falsely report that the hole
                        // falls within the starting triangle.
                        searchorg = searchtri.Org();
                        searchdest = searchtri.Dest();
                        if (Primitives.CounterClockwise(searchorg.pt, searchdest.pt, hole) > 0.0)
                        {
                            // Find a triangle that contains the hole.
                            intersect = mesh.Locate(hole, ref searchtri);
                            if ((intersect != LocateResult.Outside) && (!searchtri.IsInfected()))
                            {
                                // Infect the triangle. This is done by marking the triangle
                                // as infected and including the triangle in the virus pool.
                                searchtri.Infect();
                                mesh.viri.Add(searchtri.triangle);
                            }
                        }
                    }
                }
            }

            // Now, we have to find all the regions BEFORE we carve the holes, because locate() won't
            // work when the triangulation is no longer convex. (Incidentally, this is the reason why
            // regional attributes and area constraints can't be used when refining a preexisting mesh,
            // which might not be convex; they can only be used with a freshly triangulated PSLG.)
            if (numRegions > 0)
            {
                int i = 0;

                // Find the starting triangle for each region.
                foreach (var region in mesh.regions)
                {
                    regiontris[i].triangle = Mesh.dummytri;
                    // Ignore region points that aren't within the bounds of the mesh.
                    if ((region.pt.X >= mesh.xmin) && (region.pt.X <= mesh.xmax) &&
                        (region.pt.Y >= mesh.ymin) && (region.pt.Y <= mesh.ymax))
                    {
                        // Start searching from some triangle on the outer boundary.
                        searchtri.triangle = Mesh.dummytri;
                        searchtri.orient = 0;
                        searchtri.SymSelf();
                        // Ensure that the region point is to the left of this boundary
                        // edge; otherwise, locate() will falsely report that the
                        // region point falls within the starting triangle.
                        searchorg = searchtri.Org();
                        searchdest = searchtri.Dest();
                        if (Primitives.CounterClockwise(searchorg.pt, searchdest.pt, region.pt) > 0.0)
                        {
                            // Find a triangle that contains the region point.
                            intersect = mesh.Locate(region.pt, ref searchtri);
                            if ((intersect != LocateResult.Outside) && (!searchtri.IsInfected()))
                            {
                                // Record the triangle for processing after the
                                // holes have been carved.
                                searchtri.Copy(ref regiontris[i]);
                            }
                        }
                    }

                    i++;
                }
            }

            if (mesh.viri.Count > 0)
            {
                // Carve the holes and concavities.
                Plague();
            }
            // The virus pool should be empty now.

            if (numRegions > 0)
            {
                if (Behavior.RegionAttrib)
                {
                    // Make the triangle's attributes larger.
                    double[] attributes = new double[mesh.eextras + 1];

                    // Assign every triangle a regional attribute of zero.
                    tri.orient = 0;
                    foreach (var t in mesh.triangles.Values)
                    {
                        Array.Copy(tri.triangle.attributes, attributes, mesh.eextras);
                        tri.triangle = t;
                        tri.triangle.attributes = attributes;
                    }
                }

                for (int i = 0; i < numRegions; i++)
                {
                    if (regiontris[i].triangle != Mesh.dummytri)
                    {
                        // Make sure the triangle under consideration still exists.
                        // It may have been eaten by the virus.
                        if (!Otri.IsDead(regiontris[i].triangle))
                        {
                            // Put one triangle in the virus pool.
                            regiontris[i].Infect();
                            mesh.viri.Add(regiontris[i].triangle);
                            // Apply one region's attribute and/or area constraint.
                            RegionPlague(mesh.regions[i].attribute, mesh.regions[i].area);
                            // The virus pool should be empty now.
                        }
                    }
                }

                if (Behavior.RegionAttrib)
                {
                    // Note the fact that each triangle has an additional attribute.
                    mesh.eextras++;
                }
            }

            // Free up memory.
            if (((mesh.holes.Count > 0) && !Behavior.NoHoles) || !Behavior.Convex || (numRegions > 0))
            {
                mesh.viri.Clear();
            }

            if (numRegions > 0)
            {
                regiontris = null;
            }
        }
    }
}
