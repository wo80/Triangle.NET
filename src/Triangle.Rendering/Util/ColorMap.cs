
namespace TriangleNet.Rendering.Util
{
    using System;
    using System.Drawing;

    /// <summary>
    /// A simple color map implementation.
    /// </summary>
    public class ColorMap
    {
        #region Colormap definitions

        /// <summary>
        /// Create a jet (or rainbow) color map.
        /// </summary>
        /// <param name="size">The size of the color map.</param>
        /// <returns>The jet color map.</returns>
        public static ColorMap Jet(int size)
        {
            ColorMap map = new ColorMap(size);
            float v, step = 1.0f / (size - 1);
            float[] rgb = new float[3];

            for (int i = 0; i < size; i += 1)
            {
                v = 4 * i * step;

                rgb[0] = Math.Min(v - 1.5f, 4.5f - v);
                rgb[1] = Math.Min(v - 0.5f, 3.5f - v);
                rgb[2] = Math.Min(v + 0.5f, 2.5f - v);

                Clamp(rgb, 0.0f, 1.0f);

                map.map[size - i - 1] = ColorFromRgb(rgb[0], rgb[1], rgb[2]);
            }

            return map;
        }

        /// <summary>
        /// Create a hot color map.
        /// </summary>
        /// <param name="size">The size of the color map.</param>
        /// <returns>The jet color map.</returns>
        public static ColorMap Hot(int size)
        {
            ColorMap map = new ColorMap(size);
            float v, step = 1.0f / (size - 1);
            float[] rgb = new float[3];

            for (int i = 0; i < size; i += 1)
            {
                v = 2.5f * i * step;

                rgb[0] = v;
                rgb[1] = v - 1;
                rgb[2] = 2 * v - 4;

                Clamp(rgb, 0.0f, 1.0f);

                map.map[i] = ColorFromRgb(rgb[0], rgb[1], rgb[2]);
            }

            return map;
        }

        #endregion

        #region Helper

        private static Color ColorFromRgb(float r, float g, float b)
        {
            byte max = byte.MaxValue;

            return Color.FromArgb((byte)(r * max), (byte)(g * max), (byte)(b * max));
        }

        private static void Clamp(float[] values, float min, float max)
        {
            int n = values.Length;

            for (int i = 0; i < n; i += 1)
            {
                values[i] = Math.Min(max, Math.Max(min, values[i]));
            }
        }

        private static (float, float) GetMinMax(float[] values)
        {
            float min = float.MaxValue;
            float max = float.MinValue;

            foreach (var a in values)
            {
                if (a < min) min = a;
                if (a > max) max = a;
            }

            return (min, max);
        }

        private static (double, double) GetMinMax(double[] values)
        {
            double min = double.MaxValue;
            double max = double.MinValue;

            for (int i = 0; i < values.Length; i++)
            {
                double a = values[i];

                if (a < min) min = a;
                if (a > max) max = a;
            }

            return (min, max);
        }

        #endregion

        private readonly Color[] map;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorMap"/> class.
        /// </summary>
        /// <param name="size">The size of the color map.</param>
        private ColorMap(int size)
            : this(new Color[size])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorMap"/> class.
        /// </summary>
        /// <param name="colors">The colors of the color map.</param>
        public ColorMap(Color[] colors)
        {
            map = colors;
        }

        /// <summary>
        /// For each input value assign the corresponding color.
        /// </summary>
        /// <param name="values">The input values (associated to vertex).</param>
        /// <param name="colors">The color array target containing the <see cref="Color"/>s on output.</param>
        public void GetColors(float[] values, Color[] colors)
        {
            (float min, float max) = GetMinMax(values);

            GetColors(values, min, max, colors);
        }

        /// <summary>
        /// For each input value assign the corresponding color.
        /// </summary>
        /// <param name="values">The input values (associated to vertex).</param>
        /// <param name="min">The minimum of the input values.</param>
        /// <param name="max">The maximum of the input values.</param>
        /// <param name="colors">The color array target containing the <see cref="Color"/>s on output.</param>
        public void GetColors(float[] values, float min, float max, Color[] colors)
        {
            if (max <= min) return;

            int length = Math.Min(values.Length, colors.Length);

            int n = map.Length;

            for (int i = 0; i < length; i++)
            {
                int k = (int)Math.Floor(n * (max - values[i]) / (max - min));

                k = Math.Max(Math.Min(k, n - 1), 0);

                colors[i] = map[k];
            }
        }

        /// <summary>
        /// For each input value assign the corresponding color.
        /// </summary>
        /// <param name="values">The input values (associated to vertex).</param>
        /// <param name="rgba">The color array target containing RGBA float values on output.</param>
        public void GetColors(float[] values, float[] rgba)
        {
            (float min, float max) = GetMinMax(values);

            GetColors(values, min, max, rgba);
        }

        /// <summary>
        /// For each input value assign the corresponding color.
        /// </summary>
        /// <param name="values">The input values (associated to vertex).</param>
        /// <param name="min">The minimum of the input values.</param>
        /// <param name="max">The maximum of the input values.</param>
        /// <param name="rgba">The color array target containing RGBA float values on output.</param>
        public void GetColors(float[] values, float min, float max, float[] rgba)
        {
            if (max <= min) return;

            int n = map.Length;

            for (int i = 0; i < values.Length; i++)
            {
                int k = (int)Math.Floor(n * (max - values[i]) / (max - min));

                k = Math.Max(Math.Min(k, n - 1), 0);

                var color = map[k];

                k = 4 * i;

                rgba[k] = color.R / 255f;
                rgba[k + 1] = color.G / 255f;
                rgba[k + 2] = color.B / 255f;
                rgba[k + 3] = color.A / 255f;

                i++;
            }
        }

        /// <summary>
        /// For each input value assign the corresponding color.
        /// </summary>
        /// <param name="values">The input values (associated to vertex).</param>
        /// <param name="rgba">The color array target containing RGBA float values on output.</param>
        public void GetColors(double[] values, float[] rgba)
        {
            (double min, double max) = GetMinMax(values);

            GetColors(values, min, max, rgba);
        }

        /// <summary>
        /// For each input value assign the corresponding color.
        /// </summary>
        /// <param name="values">The input values (associated to vertex).</param>
        /// <param name="min">The minimum of the input values.</param>
        /// <param name="max">The maximum of the input values.</param>
        /// <param name="rgba">The color array target containing RGBA float values on output.</param>
        public void GetColors(double[] values, double min, double max, float[] rgba)
        {
            if (max <= min) return;

            int n = map.Length;

            for (int i = 0; i < values.Length; i++)
            {
                int k = (int)Math.Floor(n * (max - values[i]) / (max - min));

                k = Math.Max(Math.Min(k, n - 1), 0);

                var color = map[k];

                k = 4 * i;

                rgba[k] = color.R / 255f;
                rgba[k + 1] = color.G / 255f;
                rgba[k + 2] = color.B / 255f;
                rgba[k + 3] = color.A / 255f;

                i++;
            }
        }
    }
}
