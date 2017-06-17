
namespace TriangleNet.Rendering.Util
{
    using System;
    using System.Drawing;

    public class ColorMap
    {
        #region Colormap definitions

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

                map.colors[size - i - 1] = ColorFromRgb(rgb[0], rgb[1], rgb[2]);
            }

            return map;
        }

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

                map.colors[i] = ColorFromRgb(rgb[0], rgb[1], rgb[2]);
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

        private static int Clamp(int index, int max)
        {
            if (index < 0)
            {
                index = 0;
            }
            else if (index > max)
            {
                index = max;
            }

            return index;
        }

        #endregion

        private Color[] colors;

        private ColorMap(int size)
        {
            this.colors = new Color[size];
        }

        public ColorMap(Color[] colors)
        {
            this.colors = colors;
        }

        public Color GetColor(double value, double min, double max)
        {
            int n = this.colors.Length;
			int i = (int)Math.Floor(n * (max - value) / (max - min));

            return this.colors[Clamp(i, n - 1)];
		}
    }
}
