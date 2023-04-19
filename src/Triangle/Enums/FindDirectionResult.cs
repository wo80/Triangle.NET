namespace TriangleNet
{
    /// <summary>
    /// Labels that signify the result of direction finding.
    /// </summary>
    /// <remarks>The result indicates that a segment connecting the two query 
    /// points falls within the direction triangle, along the left edge of the 
    /// direction triangle, or along the right edge of the direction triangle.
    /// </remarks>
    internal enum FindDirectionResult { Within, Leftcollinear, Rightcollinear };
}