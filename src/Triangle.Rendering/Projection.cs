// -----------------------------------------------------------------------
// <copyright file="Projection.cs" company="">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Rendering
{
    using System;
    using System.Drawing;

    using TRectangle = Geometry.Rectangle;

    /// <summary>
    /// Manages a world to screen transformation (2D orthographic projection).
    /// </summary>
    /// <remarks>
    /// <para>
    /// The projection implementation is actually not world-to-screen, but NDC-to-screen
    /// (Normalized-Device-Coordinates). NDC here is - in contrast for example to OpenGL, the
    /// transformation of world coordinates to a unit rectangle with origin (0,0) and a max
    /// side length 1 (the width/height ratio is preserved). It's a simple translate-scale
    /// transform, which is automatically applied in <c>VertexBuffer.Create(points, bounds)</c>.
    /// </para>
    /// <para>
    /// Since the upper-left corner of the display is usually the screen coordinate origin
    /// (0,0), the projection will automatically invert the y-axis.
    /// </para>
    /// </remarks>
    public class Projection
    {
        // The original mesh bounds (needed for screen-to-world projection).
        private TRectangle world_;

        // Precomputed scaling factor for normalized coordinates.
        private double scale_;

        // The screen dimensions.
        private Rectangle screen;

        // The original mesh and the viewport in normalized coordinates.
        private RectangleF world, viewport;

        /// <summary>
        /// Gets or sets the current viewport (normalized coordinates).
        /// </summary>
        public RectangleF Viewport => viewport;

        /// <summary>
        /// Gets the zoom level.
        /// </summary>
        public int Level { get; private set; }

        private const int MAX_ZOOM = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="Projection"/> class.
        /// </summary>
        /// <param name="screen">The current screen (viewport) dimensions.</param>
        public Projection(Rectangle screen)
        {
            this.screen = screen;

            world = viewport = new RectangleF(screen.X, screen.Y, screen.Width, screen.Height);

            world_ = new TRectangle();
            scale_ = 0;

            Level = 1;
        }

        /// <summary>
        /// Initialize the projection.
        /// </summary>
        /// <param name="world">The world that should be transformed to screen coordinates.</param>
        public void Initialize(TRectangle world)
        {
            Level = 1;

            // Bounding box of original (non-normalized) coordinates.
            world_ = world;

            // Scaling factor for normalized coordinates.
            scale_ = Math.Max(world.Width, world.Height);

            // Dimensions in normalized coordinates.
            var ww = (float)(world.Width / scale_);
            var wh = (float)(world.Height / scale_);

            // Add a margin so there's some space around the screen borders.
            var margin = (ww < wh) ? wh * 0.05f : ww * 0.05f;

            var sw = screen.Width;
            var sh = screen.Height;

            var wRatio = ww / wh;
            var sRatio = sw / (float)sh;

            var scale = (sRatio < wRatio) ? (ww + margin) / sw : (wh + margin) / sh;

            // Center in normalized coordinates (left = bottom = 0)
            var centerX = ww / 2;
            var centerY = wh / 2;

            // Get the initial viewport (complete mesh centered on the screen)
            this.world = viewport = new RectangleF(
                centerX - sw * scale / 2,
                centerY - sh * scale / 2,
                sw * scale,
                sh * scale);
        }

        /// <summary>
        /// Handle resize of the screen (viewport).
        /// </summary>
        /// <param name="newScreen">The new screen (viewport) dimensions.</param>
        public void Resize(Rectangle newScreen)
        {
            // The viewport has to be updated, but we want to keep
            // the scaling and the center.

            // Get the screen scaling.
            var scaleX = newScreen.Width / (float)screen.Width;
            var scaleY = newScreen.Height / (float)screen.Height;

            screen = newScreen;

            var view = viewport;

            // Center of the viewport
            var centerX = (view.Left + view.Right) / 2;
            var centerY = (view.Bottom + view.Top) / 2;

            // The new viewport dimensions.
            var width = view.Width * scaleX;
            var height = view.Height * scaleY;

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

            var x = view.X + dx * view.Width / 4;
            var y = view.Y + dy * view.Height / 4;

            viewport = new RectangleF(x, y, view.Width, view.Height);

            return true;
        }

        /// <summary>
        /// Zoom in or out of the viewport.
        /// </summary>
        /// <param name="amount">Zoom amount.</param>
        /// <param name="focusX">Relative x point position (in [0..1] range).</param>
        /// <param name="focusY">Relative y point position (in [0..1] range).</param>
        public bool Zoom(int amount, float focusX, float focusY)
        {
            float width, height;

            // Invert y coordinate.
            focusY = 1 - focusY;

            if (amount > 0) // Zoom in
            {
                Level++;

                if (Level > MAX_ZOOM)
                {
                    Level = MAX_ZOOM;
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
            var x = viewport.X + viewport.Width * focusX;
            var y = viewport.Y + viewport.Height * focusY;

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

        /// <summary>
        /// Reset the zoom to initial state.
        /// </summary>
        public void Reset()
        {
            viewport = world;
            Level = 1;
        }

        /// <summary>
        /// Project a normalized device coordinate to screen coordinates.
        /// </summary>
        /// <param name="pt">Input normalized device coordinate, output screen coordinate.</param>
        public void NdcToScreen(ref PointF pt)
        {
            pt.X = (pt.X - viewport.X) / viewport.Width * screen.Width;
            pt.Y = (1 - (pt.Y - viewport.Y) / viewport.Height) * screen.Height;
        }

        /// <summary>
        /// Project a screen coordinate to world coordinates.
        /// </summary>
        /// <param name="pt">Normalized position on screen (both coordinates in [0..1] range).</param>
        /// <param name="x">The world x-coordinate.</param>
        /// <param name="y">The world y-coordinate.</param>
        public void ScreenToWorld(PointF pt, out double x, out double y)
        {
            // Position in normalized coordinates.
            var nx = viewport.X + viewport.Width * pt.X;
            var ny = viewport.Y + viewport.Height * (1 - pt.Y);

            // Translate and scale to world coordinates.
            x = world_.X + nx * scale_;
            y = world_.Y + ny * scale_;
        }
    }
}
