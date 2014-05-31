
namespace TriangleNet.Rendering.GDI.Native
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// The TRIVERTEX structure contains color information and position information.
    /// </summary>
    /// <remarks>
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/dd145142.aspx
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal struct TriVertex
    {
        /// <summary>
        /// The x-coordinate, in logical units, of the upper-left corner of the rectangle
        /// </summary>
        public int x;

        /// <summary>
        /// The y-coordinate, in logical units, of the upper-left corner of the rectangle
        /// </summary>
        public int y;

        /// <summary>
        /// The color information at the point of x, y
        /// </summary>
        public ushort Red;

        /// <summary>
        /// The color information at the point of x, y
        /// </summary>
        public ushort Green;

        /// <summary>
        /// The color information at the point of x, y
        /// </summary>
        public ushort Blue;

        /// <summary>
        /// The color information at the point of x, y
        /// </summary>
        public ushort Alpha;
    }
}
