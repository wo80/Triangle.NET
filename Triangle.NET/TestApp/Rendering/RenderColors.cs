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
        /// <summary>
        /// Gets a color scheme with black background.
        /// </summary>
        public static RenderColors Default()
        {
            var colors = new RenderColors();

            colors.Background = Color.FromArgb(0, 0, 0);
            colors.Point = new SolidBrush(Color.Green);
            colors.SteinerPoint = new SolidBrush(Color.Peru);
            colors.Triangle = new SolidBrush(Color.Black);
            colors.Line = new Pen(Color.FromArgb(30, 30, 30));
            colors.Segment = new Pen(Color.DarkBlue);
            colors.VoronoiLine = new Pen(Color.FromArgb(40, 50, 60));

            return colors;
        }
        /// <summary>
        /// Gets a color scheme with white background.
        /// </summary>
        public static RenderColors LightScheme()
        {
            var colors = new RenderColors();

            colors.Background = Color.White;
            colors.Point = new SolidBrush(Color.FromArgb(60, 80, 120));
            colors.SteinerPoint = new SolidBrush(Color.DarkGreen);
            colors.Triangle = new SolidBrush(Color.FromArgb(230, 240, 250));
            colors.Line = new Pen(Color.FromArgb(150, 150, 150));
            colors.Segment = new Pen(Color.SteelBlue);
            colors.VoronoiLine = new Pen(Color.FromArgb(160, 170, 180));

            return colors;
        }

        public Color Background;
        public Brush Point;
        public Brush SteinerPoint;
        public Brush Triangle;
        public Pen Line;
        public Pen Segment;
        public Pen VoronoiLine;
    }
}
