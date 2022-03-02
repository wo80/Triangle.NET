// -----------------------------------------------------------------------
// <copyright file="Projection.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Rendering
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Manages a world to screen transformation (2D orthographic projection).
    /// </summary>
    public class Projection
    {
        // The screen.
        Rectangle screen;

        // The complete mesh.
        RectangleF world;

        RectangleF viewport;

        /// <summary>
        /// Gets or sets the current viewport (visible mesh).
        /// </summary>
        public RectangleF Viewport => viewport;

        /// <summary>
        /// Gets the current scale.
        /// </summary>
        public float Scale => screen.Width / viewport.Width;

        /// <summary>
        /// Gets the zoom level.
        /// </summary>
        public int Level { get; private set; }

        // The y-direction of windows screen coordinates is upside down,
        // so invertY should be set to true.
        bool invertY;

        const int maxZoomLevel = 100;

        public Projection(Rectangle screen, bool invertY = true)
        {
            this.screen = screen;
            this.invertY = invertY;

            world = viewport = screen;

            Level = 1;
        }

        /// <summary>
        /// Inititialize the projection.
        /// </summary>
        /// <param name="world">The world that should be transformed to screen coordinates.</param>
        public void Initialize(Geometry.Rectangle world)
        {
            Level = 1;

            float ww = (float)world.Width;
            float wh = (float)world.Height;

            int sw = screen.Width;
            int sh = screen.Height;

            // Add a margin so there's some space around the border
            float margin = (ww < wh) ? wh * 0.05f : ww * 0.05f;

            float wRatio = ww / wh;
            float sRatio = sw / (float)sh;

            float scale = (sRatio < wRatio) ? (ww + margin) / sw :  (wh + margin) / sh;

            float centerX = (float)world.X + ww / 2;
            float centerY = (float)world.Y + wh / 2;

            // Get the initial viewport (complete mesh centered on the screen)
            this.world = viewport = new RectangleF(
                centerX - sw * scale / 2,
                centerY - sh * scale / 2,
                sw * scale,
                sh * scale);
        }

        /// <summary>
        /// Handle resize of the screen.
        /// </summary>
        /// <param name="newScreen">The new screen dimensions.</param>
        public void Resize(Rectangle newScreen)
        {
            // The viewport has to be updated, but we want to keep
            // the scaling and the center.

            // Get the screen scaling.
            float scaleX = newScreen.Width / (float)screen.Width;
            float scaleY = newScreen.Height / (float)screen.Height;

            screen = newScreen;

            var view = viewport;

            // Center of the viewport
            float centerX = (view.Left + view.Right) / 2;
            float centerY = (view.Bottom + view.Top) / 2;

            // The new viewport dimensions.
            float width = view.Width * scaleX;
            float height = view.Height * scaleY;

            viewport = new RectangleF(
                centerX - width / 2,
                centerY - height / 2,
                width, height);

            // Do the same for the world:
            centerX = (world.Left + world.Right) / 2;
            centerY = (world.Bottom + world.Top) / 2;

            width = world.Width * scaleX;
            height = world.Height * scaleY;

            world = new RectangleF(
                centerX - width / 2,
                centerY - height / 2,
                width, height);
        }

        public bool Translate(int dx, int dy)
        {
            if (Level == 1)
            {
                return false;
            }

            var view = viewport;

            float x = view.X + dx * view.Width / 4;
            float y = view.Y + dy * view.Height / 4;

            viewport = new RectangleF(x, y, view.Width, view.Height);

            return true;
        }

        /// <summary>
        /// Zoom in or out of the viewport.
        /// </summary>
        /// <param name="amount">Zoom amount</param>
        /// <param name="focusX">Relative x point position</param>
        /// <param name="focusY">Relative y point position</param>
        public bool Zoom(int amount, float focusX, float focusY)
        {
            float width, height;

            if (invertY)
            {
                focusY = 1 - focusY;
            }

            if (amount > 0) // Zoom in
            {
                Level++;

                if (Level > maxZoomLevel)
                {
                    Level = maxZoomLevel;
                    return false;
                }

                width = viewport.Width / 1.1f;
                height = viewport.Height / 1.1f;
            }
            else
            {
                Level--;

                if (Level < 1)
                {
                    Reset();
                    return false;
                }

                width = viewport.Width * 1.1f;
                height = viewport.Height * 1.1f;
            }

            // Current focus on viewport
            float x = viewport.X + viewport.Width * focusX;
            float y = viewport.Y + viewport.Height * focusY;

            // New left and top positions
            x = x - width * focusX;
            y = y - height * focusY;

            // Check if outside of world
            if (x < world.X)
            {
                x = world.X;
            }
            else if (x + width > world.Right)
            {
                x = world.Right - width;
            }

            if (y < world.Y)
            {
                y = world.Y;
            }
            else if (y + height > world.Bottom)
            {
                y = world.Bottom - height;
            }

            // Set new viewport
            viewport = new RectangleF(x, y, width, height);

            return true;
        }

        public void Reset()
        {
            viewport = world;
            Level = 1;
        }

        public void WorldToScreen(ref PointF pt)
        {
            pt.X = (pt.X - viewport.X) / viewport.Width * screen.Width;
            pt.Y = (1 - (pt.Y - viewport.Y) / viewport.Height) * screen.Height;
        }

        public void ScreenToWorld(PointF pt, out double x, out double y)
        {
            x = viewport.X + viewport.Width * pt.X;
            y = viewport.Y + viewport.Height * (1 - pt.Y);
        }
    }
}
