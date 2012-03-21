// -----------------------------------------------------------------------
// <copyright file="SplayNode.cs" company="">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/triangle/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A node in the splay tree.
    /// </summary>
    /// <remarks>
    /// Only used in the sweepline algorithm.
    /// 
    /// Each node holds an oriented ghost triangle that represents a boundary edge
    /// of the growing triangulation. When a circle event covers two boundary edges
    /// with a triangle, so that they are no longer boundary edges, those edges are
    /// not immediately deleted from the tree; rather, they are lazily deleted when
    /// they are next encountered. (Since only a random sample of boundary edges are
    /// kept in the tree, lazy deletion is faster.) 'keydest' is used to verify that
    /// a triangle is still the same as when it entered the splay tree; if it has
    /// been rotated (due to a circle event), it no longer represents a boundary
    /// edge and should be deleted.
    /// </remarks>
    class SplayNode
    {
        public Otri keyedge;              // Lprev of an edge on the front.
        public Vertex keydest;            // Used to verify that splay node is still live.
        public SplayNode lchild, rchild;  // Children in splay tree.
    }
}
