// -----------------------------------------------------------------------
// <copyright file="IView.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.Views
{
    using TriangleNet;
    using TriangleNet.Geometry;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IView
    {
        void HandleNewInput(IPolygon geometry);
        void HandleMeshImport(IPolygon geometry, Mesh mesh);
        void HandleMeshUpdate(Mesh mesh);
        void HandleMeshChange(Mesh mesh);
    }
}
