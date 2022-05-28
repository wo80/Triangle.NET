// -----------------------------------------------------------------------
// <copyright file="AdjacencyMatrix.cs" company="">
// Original Matlab code by John Burkardt, Florida State University
// Triangle.NET code by Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Tools
{
    using System;

    /// <summary>
    /// The adjacency matrix of the mesh.
    /// </summary>
    public class AdjacencyMatrix
    {
        // Number of adjacency entries.
        int nnz;

        // Pointers into the actual adjacency structure adj. Information about row k is
        // stored in entries pcol(k) through pcol(k+1)-1 of adj. Size: N + 1
        int[] pcol;

        // The adjacency structure. For each row, it contains the column indices 
        // of the nonzero entries. Size: nnz
        int[] irow;

        /// <summary>
        /// Gets the number of columns (nodes of the mesh).
        /// </summary>
        public readonly int ColumnCount;

        /// <summary>
        /// Gets the column pointers.
        /// </summary>
        public int[] ColumnPointers => pcol;

        /// <summary>
        /// Gets the row indices.
        /// </summary>
        public int[] RowIndices => irow;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdjacencyMatrix" /> class.
        /// </summary>
        /// <param name="mesh">The mesh.</param>
        /// <remarks>
        /// NOTE: as a side effect, computing the adjacency matrix will affect the
        /// node numbering of the mesh.
        /// </remarks>
        public AdjacencyMatrix(Mesh mesh)
        {
            int n = mesh.vertices.Count;

            // Undead vertices should not be considered in the adjacency matrix.
            ColumnCount = n - mesh.undeads;

            // Renumber nodes, excluding undeads.
            int i = 0;
            foreach (var vertex in mesh.vertices.Values)
            {
                vertex.id = vertex.type == VertexType.UndeadVertex ? -i : i++;
            }

            // Set up the adj_row adjacency pointer array.
            pcol = AdjacencyCount(mesh);
            nnz = pcol[ColumnCount];

            // Set up the adj adjacency array.
            irow = AdjacencySet(mesh, pcol);

            SortIndices();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdjacencyMatrix" /> class.
        /// </summary>
        /// <param name="pcol">The column pointers.</param>
        /// <param name="irow">The row indices.</param>
        /// <exception cref="ArgumentException"></exception>
        public AdjacencyMatrix(int[] pcol, int[] irow)
        {
            ColumnCount = pcol.Length - 1;

            nnz = pcol[ColumnCount];

            this.pcol = pcol;
            this.irow = irow;

            if (pcol[0] != 0)
            {
                throw new ArgumentException("Expected 0-based indexing.", nameof(pcol));
            }

            if (irow.Length < nnz)
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Computes the bandwidth of an adjacency matrix.
        /// </summary>
        /// <returns>Bandwidth of the adjacency matrix.</returns>
        public int Bandwidth()
        {
            int band_hi;
            int band_lo;
            int col;
            int i, j;

            band_lo = 0;
            band_hi = 0;

            for (i = 0; i < ColumnCount; i++)
            {
                for (j = pcol[i]; j < pcol[i + 1]; j++)
                {
                    col = irow[j];
                    band_lo = Math.Max(band_lo, i - col);
                    band_hi = Math.Max(band_hi, col - i);
                }
            }

            return band_lo + 1 + band_hi;
        }

        #region Adjacency matrix

        /// <summary>
        /// Counts adjacencies in a triangulation.
        /// </summary>
        /// <remarks>
        /// This routine is called to count the adjacencies, so that the
        /// appropriate amount of memory can be set aside for storage when
        /// the adjacency structure is created.
        ///
        /// The triangulation is assumed to involve 3-node triangles.
        ///
        /// Two nodes are "adjacent" if they are both nodes in some triangle.
        /// Also, a node is considered to be adjacent to itself.
        /// </remarks>
        int[] AdjacencyCount(Mesh mesh)
        {
            int n = ColumnCount;
            int n1, n2, n3;
            int tid, nid;

            int[] pcol = new int[n + 1];

            // Set every node to be adjacent to itself.
            for (int i = 0; i < n; i++)
            {
                pcol[i] = 1;
            }

            // Examine each triangle.
            foreach (var tri in mesh.triangles)
            {
                tid = tri.id;

                n1 = tri.vertices[0].id;
                n2 = tri.vertices[1].id;
                n3 = tri.vertices[2].id;

                // Add edge (1,2) if this is the first occurrence, that is, if 
                // the edge (1,2) is on a boundary (nid <= 0) or if this triangle
                // is the first of the pair in which the edge occurs (tid < nid).
                nid = tri.neighbors[2].tri.id;

                if (nid < 0 || tid < nid)
                {
                    pcol[n1] += 1;
                    pcol[n2] += 1;
                }

                // Add edge (2,3).
                nid = tri.neighbors[0].tri.id;

                if (nid < 0 || tid < nid)
                {
                    pcol[n2] += 1;
                    pcol[n3] += 1;
                }

                // Add edge (3,1).
                nid = tri.neighbors[1].tri.id;

                if (nid < 0 || tid < nid)
                {
                    pcol[n3] += 1;
                    pcol[n1] += 1;
                }
            }

            // We used PCOL to count the number of entries in each column.
            // Convert it to pointers into the ADJ array.
            for (int i = n; i > 0; i--)
            {
                pcol[i] = pcol[i - 1];
            }

            pcol[0] = 0;
            for (int i = 1; i <= n; i++)
            {
                pcol[i] = pcol[i - 1] + pcol[i];
            }

            return pcol;
        }

        /// <summary>
        /// Sets adjacencies in a triangulation.
        /// </summary>
        /// <remarks>
        /// This routine can be used to create the compressed column storage
        /// for a linear triangle finite element discretization of Poisson's
        /// equation in two dimensions.
        /// </remarks>
        int[] AdjacencySet(Mesh mesh, int[] pcol)
        {
            int n = ColumnCount;

            int[] col = new int[n];

            // Copy of the adjacency rows input.
            Array.Copy(pcol, col, n);

            int i, nnz = pcol[n];

            // Output list, stores the actual adjacency information.
            int[] list = new int[nnz];

            // Set every node to be adjacent to itself.
            for (i = 0; i < n; i++)
            {
                list[col[i]] = i;
                col[i] += 1;
            }

            int n1, n2, n3; // Vertex numbers.
            int tid, nid; // Triangle and neighbor id.

            // Examine each triangle.
            foreach (var tri in mesh.triangles)
            {
                tid = tri.id;

                n1 = tri.vertices[0].id;
                n2 = tri.vertices[1].id;
                n3 = tri.vertices[2].id;

                // Add edge (1,2) if this is the first occurrence, that is, if 
                // the edge (1,2) is on a boundary (nid <= 0) or if this triangle
                // is the first of the pair in which the edge occurs (tid < nid).
                nid = tri.neighbors[2].tri.id;

                if (nid < 0 || tid < nid)
                {
                    list[col[n1]++] = n2;
                    list[col[n2]++] = n1;
                }

                // Add edge (2,3).
                nid = tri.neighbors[0].tri.id;

                if (nid < 0 || tid < nid)
                {
                    list[col[n2]++] = n3;
                    list[col[n3]++] = n2;
                }

                // Add edge (3,1).
                nid = tri.neighbors[1].tri.id;

                if (nid < 0 || tid < nid)
                {
                    list[col[n1]++] = n3;
                    list[col[n3]++] = n1;
                }
            }

            return list;
        }

        /// <summary>
        /// Sort indices.
        /// </summary>
        public void SortIndices()
        {
            int k1, k2, n = ColumnCount;

            var list = irow;

            // Ascending sort the entries for each column.
            for (int i = 0; i < n; i++)
            {
                k1 = pcol[i];
                k2 = pcol[i + 1];
                Array.Sort(list, k1, k2 - k1);
            }
        }

        #endregion
    }
}
