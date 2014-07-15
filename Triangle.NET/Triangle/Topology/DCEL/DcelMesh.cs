
namespace TriangleNet.Topology.DCEL
{
    using System.Collections.Generic;
    using TriangleNet.Geometry;

    public class DcelMesh
    {
        protected List<Vertex> vertices;
        protected List<HalfEdge> edges;
        protected List<Face> faces;

        public DcelMesh()
            : this(true)
        {
        }

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
                    var twin = edge.twin = new HalfEdge(edge.next.origin);
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
