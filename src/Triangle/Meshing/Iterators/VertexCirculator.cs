﻿// -----------------------------------------------------------------------
// <copyright file="VertexCirculator.cs" company="">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Meshing.Iterators
{
    using System.Collections.Generic;
    using Geometry;
    using Topology;

    /// <summary>
    /// Vertex circulator to enumerate the vertices connected to a mesh vertex.
    /// </summary>
    public class VertexCirculator
    {
        private List<Otri> cache = new List<Otri>();

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexCirculator" /> class.
        /// </summary>
        public VertexCirculator(Mesh mesh)
        {
            mesh.MakeVertexMap();
        }

        /// <summary>
        /// Enumerate all vertices adjacent to given vertex.
        /// </summary>
        /// <param name="vertex">The center vertex.</param>
        /// <returns></returns>
        public IEnumerable<Vertex> EnumerateVertices(Vertex vertex)
        {
            BuildCache(vertex, true);

            foreach (var item in cache)
            {
                yield return item.Dest();
            }
        }

        /// <summary>
        /// Enumerate all triangles adjacent to given vertex.
        /// </summary>
        /// <param name="vertex">The center vertex.</param>
        /// <returns></returns>
        public IEnumerable<ITriangle> EnumerateTriangles(Vertex vertex)
        {
            BuildCache(vertex, false);

            foreach (var item in cache)
            {
                yield return item.tri;
            }
        }

        private void BuildCache(Vertex vertex, bool vertices)
        {
            cache.Clear();

            var init = vertex.tri;
            var next = default(Otri);
            var prev = default(Otri);

            init.Copy(ref next);

            // Move counter-clockwise around the vertex.
            while (next.tri.id != Mesh.DUMMY)
            {
                cache.Add(next);

                next.Copy(ref prev);
                next.Onext();

                if (next.Equals(init))
                {
                    break;
                }
            }

            if (next.tri.id == Mesh.DUMMY)
            {
                // We reached the boundary. To get all adjacent triangles, start
                // again at init triangle and now move clockwise.
                init.Copy(ref next);

                if (vertices)
                {
                    // Don't forget to add the vertex lying on the boundary.
                    prev.Lnext();
                    cache.Add(prev);
                }

                next.Oprev();

                while (next.tri.id != Mesh.DUMMY)
                {
                    cache.Insert(0, next);

                    next.Oprev();

                    if (next.Equals(init))
                    {
                        break;
                    }
                }
            }
        }
    }
}
