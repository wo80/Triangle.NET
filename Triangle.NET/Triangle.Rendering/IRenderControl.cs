// -----------------------------------------------------------------------
// <copyright file="IMeshRenderer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Rendering
{
    using System;
    using System.Windows.Forms;
    using System.Drawing;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IRenderControl
    {
        IRenderer Renderer { get; set; }
        Rectangle ClientRectangle { get; }

        void Initialize();
        void Refresh();

        void HandleResize();
    }
}
