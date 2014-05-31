
namespace TriangleNet.Rendering.GDI.Native
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// PInvoke signatures for GradientFill methods.
    /// </summary>
    /// <remarks>
    /// Minimum requirements: Windows 2000 Professional
    /// 
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/dd144957.aspx
    /// </remarks>
    internal static class NativeMethods
    {
        /// <summary>
        /// The GradientFill function fills rectangle and triangle structures
        /// </summary>
        /// <param name="hdc">Handle to the destination device contex</param>
        /// <param name="pVertex">Array of TRIVERTEX structures that each define a triangle vertex</param>
        /// <param name="nVertex">The number of vertices in pVertex</param>
        /// <param name="pMesh">Array of elements</param>
        /// <param name="nMesh">The number of elements in pMesh</param>
        /// <param name="ulMode">Specifies gradient fill mode</param>
        /// <returns>If the function succeeds, the return value is true, false</returns>
        public static bool GradientFill([In] IntPtr hdc, TriVertex[] pVertex, uint nVertex, uint[] pMesh, uint nMesh,
                                        GradientFillMode ulMode)
        {
            return Native.GradientFill(hdc, pVertex, nVertex, pMesh, nMesh, ulMode);
        }

        /// <summary>
        /// The GradientFill function fills rectangle and triangle structures
        /// </summary>
        /// <param name="hdc">Handle to the destination device contex</param>
        /// <param name="pVertex">Array of TRIVERTEX structures that each define a triangle vertex</param>
        /// <param name="nVertex">The number of vertices in pVertex</param>
        /// <param name="pMesh">Array of GRADIENT_TRIANGLE structures in triangle mode</param>
        /// <param name="nMesh">The number of elements in pMesh</param>
        /// <param name="ulMode">Specifies gradient fill mode</param>
        /// <returns>If the function succeeds, the return value is true, false</returns>
        public static bool GradientFill([In] IntPtr hdc, TriVertex[] pVertex, uint nVertex, GradientTriangle[] pMesh,
                                        uint nMesh, GradientFillMode ulMode)
        {
            return Native.GradientFill(hdc, pVertex, nVertex, pMesh, nMesh, ulMode);
        }

        /// <summary>
        /// The GradientFill function fills rectangle and triangle structures
        /// </summary>
        /// <param name="hdc">Handle to the destination device contex</param>
        /// <param name="pVertex">Array of TRIVERTEX structures that each define a triangle vertex</param>
        /// <param name="nVertex">The number of vertices in pVertex</param>
        /// <param name="pMesh">an array of GRADIENT_RECT structures in rectangle mode</param>
        /// <param name="nMesh">The number of elements in pMesh</param>
        /// <param name="ulMode">Specifies gradient fill mode</param>
        /// <returns>If the function succeeds, the return value is true, false</returns>
        public static bool GradientFill([In] IntPtr hdc, TriVertex[] pVertex, uint nVertex, GradientRect[] pMesh,
                                        uint nMesh, GradientFillMode ulMode)
        {
            return Native.GradientFill(hdc, pVertex, nVertex, pMesh, nMesh, ulMode);
        }

        #region Nested type: Native

        internal class Native
        {
            [DllImport("msimg32.dll", EntryPoint = "GradientFill", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GradientFill([In] IntPtr hdc,
                [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 2)] TriVertex[] pVertex,
                uint nVertex, uint[] pMesh, uint nMesh, GradientFillMode ulMode);

            [DllImport("msimg32.dll", EntryPoint = "GradientFill", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GradientFill([In] IntPtr hdc,
                [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 2)] TriVertex[] pVertex,
                uint nVertex, GradientRect[] pMesh, uint nMesh, GradientFillMode ulMode);

            [DllImport("msimg32.dll", EntryPoint = "GradientFill", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GradientFill([In] IntPtr hdc,
                [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 2)] TriVertex[] pVertex,
                uint nVertex, GradientTriangle[] pMesh, uint nMesh, GradientFillMode ulMode);
        }

        #endregion
    }
}