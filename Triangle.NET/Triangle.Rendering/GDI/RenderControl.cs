// -----------------------------------------------------------------------
// <copyright file="RendererControl.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Rendering.GDI
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Windows.Forms;

    /// <summary>
    /// Renders a mesh using GDI.
    /// </summary>
    public class RenderControl : Control, IRenderControl
    {
        // Rendering stuff
        private BufferedGraphics buffer;
        private BufferedGraphicsContext context;

        //ColorManager renderColors;

        bool initialized = false;

        string coordinate = String.Empty;

        Timer timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderControl" /> class.
        /// </summary>
        public RenderControl()
        {
            //this.SetStyle(ControlStyles.UserPaint, true);
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            //renderColors = ColorManager.Default();

            this.BackColor = Color.Black;

            context = BufferedGraphicsManager.Current;// new BufferedGraphicsContext();

            timer = new Timer();
            timer.Interval = 3000;
            timer.Tick += (sender, e) =>
            {
                timer.Stop();
                coordinate = String.Empty;
                this.Invalidate();
            };
        }

        public IRenderer Renderer { get; set; }

        /// <summary>
        /// Initialize the graphics buffer (should be called in the forms load event).
        /// </summary>
        public void Initialize()
        {
            //zoom.Initialize(this.ClientRectangle);
            InitializeBuffer();

            initialized = true;

            this.Invalidate();
        }

        public override void Refresh()
        {
            this.Render();
        }

        public void HandleMouseClick(float x, float y, MouseButtons button)
        {
            if (!initialized) return;

            var zoom = this.Renderer.Context.Zoom;

            if (button == MouseButtons.Middle)
            {
                zoom.Reset();
                this.Render();
            }
            else if (button == MouseButtons.Left)
            {
                timer.Stop();

                var nfi = System.Globalization.CultureInfo.InvariantCulture.NumberFormat;

                PointF c = new PointF(x / this.Width, y / this.Height);
                zoom.ScreenToWorld(ref c);
                coordinate = String.Format(nfi, "X:{0} Y:{1}", c.X, c.Y);

                this.Invalidate();

                timer.Start();
            }
        }

        /// <summary>
        /// Zoom to the given location.
        /// </summary>
        /// <param name="location">The zoom focus.</param>
        /// <param name="delta">Indicates whether to zoom in or out.</param>
        public void HandleMouseWheel(float x, float y, int delta)
        {
            if (!initialized) return;

            var zoom = this.Renderer.Context.Zoom;

            if (zoom.Zoom(delta, x, y))
            {
                // Redraw
                this.Render();
            }
        }

        /// <summary>
        /// Update graphics buffer and zoom after a resize.
        /// </summary>
        public void HandleResize()
        {
            var zoom = this.Renderer.Context.Zoom;
            var bounds = this.Renderer.Context.Bounds;

            zoom.Initialize(this.ClientRectangle, bounds);
            InitializeBuffer();
        }

        private void InitializeBuffer()
        {
            if (this.Width > 0 && this.Height > 0)
            {
                if (buffer != null)
                {
                    if (this.ClientRectangle == buffer.Graphics.VisibleClipBounds)
                    {
                        this.Invalidate();

                        // Bounds didn't change. Probably we just restored the
                        // window from minimized state.
                        return;
                    }

                    buffer.Dispose();
                }

                //buffer = context.Allocate(Graphics.FromHwnd(this.Handle), this.ClientRectangle);
                buffer = context.Allocate(this.CreateGraphics(), this.ClientRectangle);

                if (initialized)
                {
                    this.Render();
                }
            }
        }

        private void Render()
        {
            coordinate = String.Empty;

            if (buffer == null)
            {
                return;
            }

            var g = buffer.Graphics;
            var renderer = this.Renderer as LayerRenderer;

            g.Clear(renderer.Context.ColorManager.Background);

            if (!initialized || renderer == null)
            {
                return;
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;

            renderer.RenderTarget = g;
            renderer.Render();

            this.Invalidate();
        }

        #region Protected overrides

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!initialized)
            {
                base.OnPaint(e);
                return;
            }

            buffer.Render();

            if (!String.IsNullOrEmpty(coordinate) && Renderer.Context.HasData)
            {
                Graphics g = e.Graphics;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                g.DrawString(coordinate, this.Font, Brushes.White, 10, 10);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Do nothing
            if (!initialized)
            {
                base.OnPaintBackground(pevent);
            }
        }

        #endregion
    }
}
