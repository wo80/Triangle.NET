
namespace TriangleNet.Rendering.GDI.Native
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// The GRADIENT_TRIANGLE structure specifies the index of three
    /// vertices in the pVertex array in the GradientFill function.
    /// These three vertices form one triangle
    /// </summary>
    /// <remarks>
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/dd144959.aspx
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal struct GradientTriangle
    {
        /// <summary>
        /// The first point of the triangle where sides intersect.
        /// </summary>
        public uint Vertex1;

        /// <summary>
        /// The second point of the triangle where sides intersect.
        /// </summary>
        public uint Vertex2;

        /// <summary>
        /// The third point of the triangle where sides intersect.
        /// </summary>
        public uint Vertex3;
    }
}
