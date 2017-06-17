// -----------------------------------------------------------------------
// <copyright file="ExtensionMethods.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MeshRenderer.Core
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class ExtensionMethods
    {
        #region Color extention methods

        /// <summary>
        /// Converts a Color to a float array containing normalized R, G ,B, A values.
        /// </summary>
        public static float[] ToFloatArray4(this Color color)
        {
            return new float[] {
                ((float)color.R) / 255.0f,
                ((float)color.G) / 255.0f,
                ((float)color.B) / 255.0f,
                ((float)color.A) / 255.0f
            };
        }

        #endregion
    }
}
