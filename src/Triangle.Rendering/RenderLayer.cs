
namespace TriangleNet.Rendering
{
    using System.Collections.Generic;
    using TriangleNet.Geometry;
    using TriangleNet.Meshing;
    using TriangleNet.Rendering.Buffer;
    using TriangleNet.Rendering.Util;

    using Color = System.Drawing.Color;

    public class RenderLayer : IRenderLayer
    {
        int count;

        protected IBuffer<float> points;
        protected IBuffer<int> indices;

        protected IBuffer<int> partition;
        protected IBuffer<Color> colors;

        public RenderLayer()
        {
            this.IsEnabled = false;
        }

        /// <inheritdoc />
        public int Count => count;

        /// <inheritdoc />
        public IBuffer<float> Points => points;

        /// <inheritdoc />
        public IBuffer<int> Indices => indices;

        /// <inheritdoc />
        public IBuffer<int> Partition => partition;

        /// <inheritdoc />
        public IBuffer<Color> Colors => colors;

        /// <inheritdoc />
        public bool IsEnabled { get; set; }

        /// <inheritdoc />
        public bool IsEmpty()
        {
            return (points == null || points.Count == 0);
        }

        /// <inheritdoc />
        public void Reset(bool clear)
        {
            if (clear)
            {
                count = 0;
                points = null;
            }

            indices = null;
            partition = null;
            colors = null;
        }

        /// <inheritdoc />
        public void SetPoints(IBuffer<float> buffer, bool reset = true)
        {
            if (!reset && points != null && points.Count < buffer.Count)
            {
                // NOTE: we keep the old size to be able to render new Steiner
                //       points in a different color than existing points.
                count = points.Count / points.Size;
            }
            else
            {
                count = buffer.Count / buffer.Size;
            }

            points = buffer;
        }

        /// <inheritdoc />
        public void SetIndices(IBuffer<int> buffer)
        {
            indices = buffer;
        }

        /// <inheritdoc />
        public void AttachLayerData(float[] values, ColorMap colormap)
        {
            int length = values.Length;

            double min = double.MaxValue;
            double max = double.MinValue;

            // Find min and max of given values.
            for (int i = 0; i < length; i++)
            {
                if (values[i] < min)
                {
                    min = values[i];
                }

                if (values[i] > max)
                {
                    max = values[i];
                }
            }

            var colorData = new Color[length];

            for (int i = 0; i < length; i++)
            {
                colorData[i] = colormap.GetColor(values[i], min, max);
            }

            colors = new ColorBuffer(colorData, 1);
        }

        /// <inheritdoc />
        public void AttachLayerData(int[] partition)
        {
            this.partition = new IndexBuffer(partition, 1);
        }
    }
}
