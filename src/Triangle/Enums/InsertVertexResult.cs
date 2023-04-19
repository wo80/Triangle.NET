namespace TriangleNet
{
    /// <summary>
    /// Labels that signify the result of vertex insertion.
    /// </summary>
    /// <remarks>The result indicates that the vertex was inserted with complete 
    /// success, was inserted but encroaches upon a subsegment, was not inserted 
    /// because it lies on a segment, or was not inserted because another vertex 
    /// occupies the same location.
    /// </remarks>
    internal enum InsertVertexResult { Successful, Encroaching, Violating, Duplicate };
}