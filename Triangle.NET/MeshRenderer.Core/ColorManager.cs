using System;
using System.Collections.Generic;
using System.Drawing;

namespace MeshRenderer.Core
{
    public class ColorManager
    {
        /// <summary>
        /// Gets a color scheme with black background.
        /// </summary>
        public static ColorManager Default()
        {
            var colors = new ColorManager();

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
        public static ColorManager LightScheme()
        {
            var colors = new ColorManager();

            colors.Background = Color.White;
            colors.Point = new SolidBrush(Color.FromArgb(60, 80, 120));
            colors.SteinerPoint = new SolidBrush(Color.DarkGreen);
            colors.Triangle = new SolidBrush(Color.FromArgb(230, 240, 250));
            colors.Line = new Pen(Color.FromArgb(150, 150, 150));
            colors.Segment = new Pen(Color.SteelBlue);
            colors.VoronoiLine = new Pen(Color.FromArgb(160, 170, 180));

            return colors;
        }

        internal Color background;
        internal SolidBrush point;
        internal SolidBrush steinerPoint;
        internal SolidBrush triangle;
        internal Pen line;
        internal Pen segment;
        internal Pen voronoiLine;

        #region Public properties

        public Color Background
        {
            get { return background; }
            set { background = value; }
        }

        public SolidBrush Point
        {
            get { return point; }
            set
            {
                if (point != null) point.Dispose();
                point = value;
            }
        }

        public SolidBrush SteinerPoint
        {
            get { return steinerPoint; }
            set
            {
                if (steinerPoint != null) steinerPoint.Dispose();
                steinerPoint = value;
            }
        }

        public SolidBrush Triangle
        {
            get { return triangle; }
            set
            {
                if (triangle != null) triangle.Dispose();
                triangle = value;
            }
        }

        public Pen Line
        {
            get { return line; }
            set
            {
                if (line != null) line.Dispose();
                line = value;
            }
        }

        public Pen Segment
        {
            get { return segment; }
            set
            {
                if (segment != null) segment.Dispose();
                segment = value;
            }
        }

        public Pen VoronoiLine
        {
            get { return voronoiLine; }
            set
            {
                if (voronoiLine != null) voronoiLine.Dispose();
                voronoiLine = value;
            }
        }

        #endregion
    }
}
