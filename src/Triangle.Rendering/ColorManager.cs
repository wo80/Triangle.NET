
namespace TriangleNet.Rendering
{
    using System.Collections.Generic;
    using System.Drawing;
    using TriangleNet.Rendering.Util;

    public class ColorManager
    {
        #region Public properties

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        public Color Background { get; set; }

        /// <summary>
        /// Gets or sets the brush used for points.
        /// </summary>
        public Color Point { get; set; }

        /// <summary>
        /// Gets or sets the brush used for steiner points.
        /// </summary>
        public Color SteinerPoint { get; set; }

        /// <summary>
        /// Gets or sets the pen used for mesh edges.
        /// </summary>
        public Color Line { get; set; }

        /// <summary>
        /// Gets or sets the pen used for mesh segments.
        /// </summary>
        public Color Segment { get; set; }

        /// <summary>
        /// Gets or sets the pen used for Voronoi edges.
        /// </summary>
        public Color VoronoiLine { get; set; }

        #endregion

        /// <summary>
        /// Gets or sets a dictionary which maps region ids (or partition indices) to a color.
        /// </summary>
        public Dictionary<uint, Color> ColorDictionary { get; set; }

        /// <summary>
        /// Gets or sets a color map used for function plotting.
        /// </summary>
        public ColorMap ColorMap { get; set; }

        /// <summary>
        /// Creates an instance of the <see cref="ColorManager"/> class with default (dark) color scheme.
        /// </summary>
        public static ColorManager Default()
        {
            var colors = new ColorManager();

            colors.Background = Color.FromArgb(0, 0, 0);
            colors.Point = Color.Green;
            colors.SteinerPoint = Color.Peru;
            colors.Line = Color.FromArgb(30, 30, 30);
            colors.Segment = Color.DarkBlue;
            colors.VoronoiLine = Color.FromArgb(40, 50, 60);

            return colors;
        }

        public Dictionary<uint, Color> CreateColorDictionary(int length)
        {
            var keys = new uint[length];

            for (uint i = 0; i < length; i++)
            {
                keys[i] = i;
            }

            return CreateColorDictionary(keys);
        }

        public Dictionary<uint, Color> CreateColorDictionary(IEnumerable<uint> keys)
        {
            ColorDictionary = new Dictionary<uint, Color>();

            int i = 0, n = regionColors.Length;

            foreach (var key in keys)
            {
                ColorDictionary.Add(key, regionColors[i]);

                i = (i + 1) % n;
            }

            return ColorDictionary;
        }

        // Change or add as many colors as you like...
        private static Color[] regionColors = {
            Color.FromArgb(127,   0, 255,   0),
            Color.FromArgb(127, 255,   0,   0),
            Color.FromArgb(127,   0,   0, 255),
            Color.FromArgb(127,   0, 255, 255),
            Color.FromArgb(127, 255, 255,   0),
            Color.FromArgb(127, 255,   0, 255),
            Color.FromArgb(127, 127,   0, 255),
            Color.FromArgb(127,   0, 127, 255)
        };
    }
}
