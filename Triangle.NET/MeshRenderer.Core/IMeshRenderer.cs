// -----------------------------------------------------------------------
// <copyright file="IMeshRenderer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MeshRenderer.Core
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMeshRenderer
    {
        void Zoom(float x, float y, int delta);
        void HandleResize();

        void Initialize();

        void SetData(RenderData data);

        //void SetPoints(float[] points, int inputPoints);
        //void SetTriangles(uint[] triangles);
        //void SetSegments(uint[] segments);
    }
}
