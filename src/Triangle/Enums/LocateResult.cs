namespace TriangleNet
{
    /// <summary>
    /// Labels that signify the result of point location.
    /// </summary>
    /// <remarks>The result of a search indicates that the point falls in the 
    /// interior of a triangle, on an edge, on a vertex, or outside the mesh.
    /// </remarks>
    public enum LocateResult { InTriangle, OnEdge, OnVertex, Outside };
}