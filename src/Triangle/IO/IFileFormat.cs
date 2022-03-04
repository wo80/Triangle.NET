// -----------------------------------------------------------------------
// <copyright file="IFileFormat.cs" company="">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.IO
{
    public interface IFileFormat
    {
        bool IsSupported(string file);
    }
}
