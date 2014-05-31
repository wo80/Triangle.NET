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

        /// <summary>
        /// Gets or sets the current viewport (visible mesh).
        /// </summary>
        public RectangleF Viewport { get; set; }

        /// <summary>
        /// Gets the current scale.
        /// </summary>
        public float Scale
        {
            get { return screen.Width / Viewport.Width; }
        }

        /// <summary>
        /// Gets the zoom level.
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// Gets or sets a clip margin (default is 5% of viewport width on each side).
        /// </summary>
        public float ClipMargin { get; set; }

        // The y-direction of windows screen coordinates is upside down,
        // so inverY must be set to true.
        bool invertY = false;

        int maxZoomLevel = 50;

        public Projection(Rectangle screen, bool invertY = true)
        {
            this.screen = screen;
            this.world = screen;
            this.Viewport = screen;

            this.Level = 1;

            this.ClipMargin = this.Viewport.Width * 0.05f;

            this.invertY = invertY;
        }

        public void Initialize(BoundingBox world)
        {
            Initialize(this.screen, world);
        }

        public void Initialize(Rectangle screen, BoundingBox world)
        {
            this.screen = screen;

            this.Level = 1;

            // Add a margin so there's some space around the border
            float worldMargin = (world.Width < world.Height) ? world.Height * 0.05f : world.Width * 0.05f;

            // Get the initial viewport (complete mesh centered on the screen)
            float screenRatio = screen.Width / (float)screen.Height;
            float worldRatio = world.Width / world.Height;

            float scale = (world.Width + worldMargin) / screen.Width;

            if (screenRatio > worldRatio)
            {
                scale = (world.Height + worldMargin) / screen.Height;
            }

            float centerX = world.Left + world.Width / 2;
            float centerY = world.Bottom + world.Height / 2;

            // TODO: Add initial margin
            this.Viewport = new RectangleF(centerX - screen.Width * scale / 2,
                centerY - screen.Height * scale / 2,
                screen.Width * scale,
                screen.Height * scale);

            this.ClipMargin = this.Viewport.Width * 0.05f;

            this.world = this.Viewport;
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
                this.Level++;

                if (this.Level > maxZoomLevel)
                {
                    this.Level = maxZoomLevel;
                    return false;
                }

                width = Viewport.Width / 1.1f;
                height = Viewport.Height / 1.1f;
            }
            else
            {
                this.Level--;

                if (this.Level < 1)
                {
                    this.Level = 1;
                    this.Viewport = this.world;
                    return false;
                }

                width = Viewport.Width * 1.1f;
                height = Viewport.Height * 1.1f;
            }

            // Current focus on viewport
            float x = Viewport.X + Viewport.Width * focusX;
            float y = Viewport.Y + Viewport.Height * focusY;

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
            this.Viewport = new RectangleF(x, y, width, height);

            this.ClipMargin = this.Viewport.Width * 0.05f;

            return true;
        }

        public void Reset()
        {
            this.Viewport = this.world;
            this.Level = 1;
        }

        public void WorldToScreen(ref PointF pt)
        {
            pt.X = (pt.X - Viewport.X) / Viewport.Width * screen.Width;
            pt.Y = (1 - (pt.Y - Viewport.Y) / Viewport.Height) * screen.Height;
        }

        public void ScreenToWorld(ref PointF pt)
        {
            pt.X = Viewport.X + Viewport.Width * pt.X;
            pt.Y = Viewport.Y + Viewport.Height * (1 - pt.Y);
        }

        [Obsolete]
        public PointF WorldToScreen(float x, float y)
        {
            return new PointF((x - Viewport.X) / Viewport.Width * screen.Width,
                (1 - (y - Viewport.Y) / Viewport.Height) * screen.Height);
        }

        [Obsolete]
        public PointF ScreenToWorld(float x, float y)
        {
            return new PointF(Viewport.X + Viewport.Width * x,
                Viewport.Y + Viewport.Height * (1 - y));
        }
    }
}
