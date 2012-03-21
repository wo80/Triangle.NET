// -----------------------------------------------------------------------
// <copyright file="ITriangulator.cs" company="">
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/triangle/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Algorithm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITriangulator
    {
        int Triangulate(Mesh mesh);
    }
}
