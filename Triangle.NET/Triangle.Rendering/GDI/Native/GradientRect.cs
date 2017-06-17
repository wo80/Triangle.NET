
namespace TriangleNet.Rendering.GDI.Native
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// The GRADIENT_RECT structure specifies the index of two vertices in the
    /// pVertex array in the GradientFill function. These two vertices form the
    /// upper-left and lower-right boundaries of a rectangle.
    /// </summary>
    /// <remarks>
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/dd144958.aspx
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal struct GradientRect
    {
        /// <summary>
        /// The upper-left corner of a rectangle.
        /// </summary>
        public uint UpperLeft;

        /// <summary>
        /// The lower-right corner of a rectangle.
        /// </summary>
        public uint LowerRight;
    }

}
