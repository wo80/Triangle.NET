
namespace TriangleNet.Rendering
{
    using System.Collections.Generic;
    using System.Drawing;
    using TriangleNet.Rendering.Util;

    public class ColorManager
    {
        Color background;
        SolidBrush point;
        SolidBrush steinerPoint;
        Pen line;
        Pen segment;
        Pen voronoiLine;

        #region Public properties

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public Color Background
        {
            get { return background; }
            set { background = value; }
        }

        /// <summary>
        /// Gets or sets the brush used for points.
        /// </summary>
        public SolidBrush Point
        {
            get { return point; }
            set
            {
                if (point != null) point.Dispose();
                point = value;
            }
        }

        /// <summary>
        /// Gets or sets the brush used for steiner points.
        /// </summary>
        public SolidBrush SteinerPoint
        {
            get { return steinerPoint; }
            set
            {
                if (steinerPoint != null) steinerPoint.Dispose();
                steinerPoint = value;
            }
        }

        /// <summary>
        /// Gets or sets the pen used for mesh edges.
        /// </summary>
        public Pen Line
        {
            get { return line; }
            set
            {
                if (line != null) line.Dispose();
                line = value;
            }
        }

        /// <summary>
        /// Gets or sets the pen used for mesh segments.
        /// </summary>
        public Pen Segment
        {
            get { return segment; }
            set
            {
                if (segment != null) segment.Dispose();
                segment = value;
            }
        }

        /// <summary>
        /// Gets or sets the pen used for Voronoi edges.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a dictionary which maps region ids (or partition indices) to a color.
        /// </summary>
        public Dictionary<int, Color> ColorDictionary { get; set; }

        /// <summary>
        /// Gets or sets a colormap which is used for function plotting.
        /// </summary>
        public ColorMap ColorMap { get; set; }

        /// <summary>
        /// Creates an instance of the <see cref="ColorManager"/> class with default (dark) color scheme.
        /// </summary>
        public static ColorManager Default()
        {
            var colors = new ColorManager();

            colors.Background = Color.FromArgb(0, 0, 0);
            colors.Point = new SolidBrush(Color.Green);
            colors.SteinerPoint = new SolidBrush(Color.Peru);
            colors.Line = new Pen(Color.FromArgb(30, 30, 30));
            colors.Segment = new Pen(Color.DarkBlue);
            colors.VoronoiLine = new Pen(Color.FromArgb(40, 50, 60));

            return colors;
        }

        public void CreateColorDictionary(int length)
        {
            var keys = new int[length];

            for (int i = 0; i < length; i++)
            {
                keys[i] = i;
            }

            CreateColorDictionary(keys, length);
        }

        public void CreateColorDictionary(IEnumerable<int> keys, int length)
        {
            this.ColorDictionary = new Dictionary<int, Color>();

            int i = 0, n = regionColors.Length;

            foreach (var key in keys)
            {
                this.ColorDictionary.Add(key, regionColors[i]);

                i = (i + 1) % n;
            }
        }

        internal void Dispose(Dictionary<int, SolidBrush> brushes)
        {
            foreach (var brush in brushes.Values)
            {
                brush.Dispose();
            }
        }

        internal Dictionary<int, SolidBrush> GetBrushDictionary()
        {
            var brushes = new Dictionary<int, SolidBrush>();

            foreach (var item in ColorDictionary)
            {
                brushes.Add(item.Key, new SolidBrush(item.Value));
            }

            return brushes;
        }

        // Change or add as many colors as you like...
        private static Color[] regionColors = {
            Color.Transparent,
            Color.FromArgb(200,   0, 255,   0),
            Color.FromArgb(200, 255,   0,   0),
            Color.FromArgb(200,   0,   0, 255),
            Color.FromArgb(200,   0, 255, 255),
            Color.FromArgb(200, 255, 255,   0),
            Color.FromArgb(200, 255,   0, 255),
            Color.FromArgb(200, 127,   0, 255),
            Color.FromArgb(200,   0, 127, 255)
        };
    }
}
