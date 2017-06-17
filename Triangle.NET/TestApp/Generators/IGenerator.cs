// -----------------------------------------------------------------------
// <copyright file="IGenerator.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TriangleNet.Geometry;

    /// <summary>
    /// Interface for generating input geometries.
    /// </summary>
    public interface IGenerator
    {
        string Name { get; }
        string Description { get; }
        int ParameterCount { get; }
        string ParameterDescription(int paramIndex);
        string ParameterDescription(int paramIndex, double paramValue);
        InputGeometry Generate(double param1, double param2, double param3);
    }
}
