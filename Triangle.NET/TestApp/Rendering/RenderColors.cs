// -----------------------------------------------------------------------
// <copyright file="RenderColors.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.Rendering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Drawing;

    /// <summary>
    /// Mesh color scheme.
    /// </summary>
    public class RenderColors
    {
        public static RenderColors Default = new RenderColors()
        {
            Background = Color.FromArgb(0, 0, 0),
            Point = new SolidBrush(Color.Green),
            SteinerPoint = new SolidBrush(Color.Peru),
            Triangle = new SolidBrush(Color.Black),
            Line = new Pen(Color.FromArgb(30, 30, 30)),
            Segment = new Pen(Color.DarkBlue),
            VoronoiLine = new Pen(Color.FromArgb(40, 50, 60))
        };

        public Color Background;
        public Brush Point;
        public Brush SteinerPoint;
        public Brush Triangle;
        public Pen Line;
        public Pen Segment;
        public Pen VoronoiLine;
    }
}
