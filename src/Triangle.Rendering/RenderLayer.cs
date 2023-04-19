
namespace TriangleNet.Rendering
{
    using System.Collections.Generic;
    using Geometry;
    using Meshing;
    using Buffer;
    using Util;

    using Color = System.Drawing.Color;

    public class RenderLayer : IRenderLayer
    {
        protected IBuffer<float> points;
        protected IBuffer<int> indices;

        protected IBuffer<int> partition;
        protected IBuffer<Color> colors;

        public RenderLayer()
        {
            IsEnabled = false;
        }

        /// <inheritdoc />
        public int Count { get; private set; }

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
                Count = 0;
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
                Count = points.Count / points.Size;
            }
            else
            {
                Count = buffer.Count / buffer.Size;
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
            var length = values.Length;

            var min = double.MaxValue;
            var max = double.MinValue;

            // Find min and max of given values.
            for (var i = 0; i < length; i++)
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

            for (var i = 0; i < length; i++)
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
