// -----------------------------------------------------------------------
// <copyright file="DcelMesh.cs">
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Topology.DCEL
{
    using System.Collections.Generic;
    using TriangleNet.Geometry;

    public class DcelMesh
    {
        protected List<Vertex> vertices;
        protected List<HalfEdge> edges;
        protected List<Face> faces;

        /// <summary>
        /// Initializes a new instance of the <see cref="DcelMesh" /> class.
        /// </summary>
        public DcelMesh()
            : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="" /> class.
        /// </summary>
        /// <param name="initialize">If false, lists will not be initialized.</param>
        protected DcelMesh(bool initialize)
        {
            if (initialize)
            {
                vertices = new List<Vertex>();
                edges = new List<HalfEdge>();
                faces = new List<Face>();
            }
        }

        /// <summary>
        /// Gets the vertices of the Voronoi diagram.
        /// </summary>
        public List<Vertex> Vertices
        {
            get { return vertices; }
        }

        /// <summary>
        /// Gets the list of half-edges specify the Voronoi diagram topology.
        /// </summary>
        public List<HalfEdge> HalfEdges
        {
            get { return edges; }
        }

        /// <summary>
        /// Gets the faces of the Voronoi diagram.
        /// </summary>
        public List<Face> Faces
        {
            get { return faces; }
        }

        /// <summary>
        /// Gets the collection of edges of the Voronoi diagram.
        /// </summary>
        public IEnumerable<IEdge> Edges
        {
            get { return EnumerateEdges(); }
        }

        /// <summary>
        /// Check if the DCEL ist consistend.
        /// </summary>
        /// <param name="closed">If true, faces are assumed to be closed (i.e. all edges must have
        /// a valid next pointer).</param>
        /// <returns></returns>
        public bool IsConsistent(bool closed = true)
        {
            int horrors = 0;

            // Check faces
            foreach (var face in faces)
            {
                if (face.edge == null)
                {
                    horrors++;
                }
            }

            // Check half-edges
            foreach (var edge in edges)
            {
                var twin = edge.twin;

                if (edge.origin == null)
                {
                    horrors++;
                }

                if (twin == null)
                {
                    horrors++;
                }
                else if (twin.twin != null && edge.id != twin.twin.id)
                {
                    horrors++;
                }

                if (closed)
                {
                    if (edge.next == null)
                    {
                        horrors++;
                    }
                    else if (twin != null && edge.next.origin.id != twin.origin.id)
                    {
                        horrors++;
                    }
                }

                if (edge.face == null)
                {
                    horrors++;
                }
            }

            // Check vertices
            foreach (var vertex in vertices)
            {
                if (vertex.leaving == null)
                {
                    horrors++;
                }
            }

            return horrors == 0;
        }

        /// <summary>
        /// Search for half-edge without twin and add a twin. Connect twins to form connected
        /// boundary contours.
        /// </summary>
        /// <remarks>
        /// This method assumes that all faces are closed (i.e. no edge.next pointers are null).
        /// </remarks>
        internal void ResolveBoundaryEdges()
        {
            // Maps vertices to leaving boundary edge.
            var map = new Dictionary<int, HalfEdge>();

            // TODO: parallel?
            foreach (var edge in this.edges)
            {
                if (edge.twin == null)
                {
                    var twin = edge.twin = new HalfEdge(edge.next.origin, Face.Empty);
                    twin.twin = edge;

                    map.Add(twin.origin.id, twin);
                }
            }

            int j = edges.Count;

            foreach (var edge in map.Values)
            {
                edge.id = j++;
                edge.next = map[edge.twin.origin.id];

                this.edges.Add(edge);
            }
        }

        /// <summary>
        /// Enumerates all edges of the DCEL.
        /// </summary>
        /// <remarks>
        /// This method assumes that each half-edge has a twin (i.e. NOT null).
        /// </remarks>
        protected virtual IEnumerable<IEdge> EnumerateEdges()
        {
            var edges = new List<IEdge>(this.edges.Count / 2);

            foreach (var edge in this.edges)
            {
                var twin = edge.twin;

                // Report edge only once.
                if (edge.id < twin.id)
                {
                    edges.Add(new Edge(edge.origin.id, twin.origin.id));
                }
            }

            return edges;
        }
    }
}
