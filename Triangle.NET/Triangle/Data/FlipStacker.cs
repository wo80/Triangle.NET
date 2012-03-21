// -----------------------------------------------------------------------
// <copyright file="FlipStacker.cs" company="">
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
    /// A stack of triangles flipped during the most recent vertex insertion.
    /// </summary>
    /// <remarks>
    /// The stack is used to undo the vertex insertion if the vertex encroaches
    /// upon a subsegment.
    /// </remarks>
    class FlipStacker
    {
        public Otri flippedtri;                       // A recently flipped triangle.
        public FlipStacker prevflip;               // Previous flip in the stack.
    }
}
