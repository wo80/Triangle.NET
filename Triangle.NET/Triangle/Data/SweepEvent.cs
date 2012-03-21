// -----------------------------------------------------------------------
// <copyright file="SweepEvent.cs" company="">
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
    /// A node in a heap used to store events for the sweepline Delaunay algorithm.
    /// </summary>
    /// <remarks>
    /// Only used in the sweepline algorithm.
    /// 
    /// Nodes do not point directly to their parents or children in the heap. Instead, each
    /// node knows its position in the heap, and can look up its parent and children in a
    /// separate array. To distinguish site events from circle events, all circle events are
    /// given an invalid (smaller than 'xmin') x-coordinate 'xkey'.
    /// </remarks>
    class SweepEvent
    {
        public double xkey, ykey;     // Coordinates of the event.
        public Vertex vertexEvent;    // Vertex event.
        public Otri otriEvent;        // Circle event.
        public int heapposition;      // Marks this event's position in the heap.
    }
}
