
namespace TriangleNet.Rendering.GDI.Native
{
    using System;

    /// <summary>
    /// Specifies gradient fill mode
    /// </summary>
    [Flags]
    internal enum GradientFillMode : uint
    {
        /// <summary>
        /// In this mode, two endpoints describe a rectangle. The rectangle is defined
        /// to have a constant color (specified by the TRIVERTEX structure) for the
        /// left and right edges. GDI interpolates the color from the left to right
        /// edge and fills the interior
        /// </summary>
        GRADIENT_FILL_RECT_H = 0,
        /// <summary>
        /// In this mode, two endpoints describe a rectangle. The rectangle is
        /// defined to have a constant color (specified by the TRIVERTEX structure)
        /// for the top and bottom edges. GDI interpolates the color from the top
        /// to bottom edge and fills the interior
        /// </summary>
        GRADIENT_FILL_RECT_V = 1,
        /// <summary>
        /// In this mode, an array of TRIVERTEX structures is passed to GDI
        /// along with a list of array indexes that describe separate triangles.
        /// GDI performs linear interpolation between triangle vertices and fills
        /// the interior. Drawing is done directly in 24- and 32-bpp modes.
        /// Dithering is performed in 16-, 8-, 4-, and 1-bpp mode
        /// </summary>
        GRADIENT_FILL_TRIANGLE = 2
    }
}
