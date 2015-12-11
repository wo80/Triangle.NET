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
    using System.Globalization;
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
            //this.SetStyle(ControlStyles.Selectable, true);
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

        /// <summary>
        /// Update graphics buffer and zoom after a resize.
        /// </summary>
        public void HandleResize()
        {
            var zoom = this.Renderer.Context.Zoom;

            zoom.Resize(this.ClientRectangle);
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

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (!initialized) return;

            var zoom = this.Renderer.Context.Zoom;

            if (zoom.Zoom(e.Delta, (float)e.X / Width, (float)e.Y / Height))
            {
                // Redraw
                this.Render();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            // We need to manually set the focus to get proper handling of
            // the KeyUp and MouseWheel events.
            this.Focus();

            if (!initialized) return;

            var zoom = this.Renderer.Context.Zoom;

            if (e.Button == MouseButtons.Middle)
            {
                zoom.Reset();
                this.Render();
            }
            else if (e.Button == MouseButtons.Left)
            {
                timer.Stop();

                PointF c = new PointF((float)e.X / Width, (float)e.Y / Height);
                zoom.ScreenToWorld(ref c);
                coordinate = String.Format(NumberFormatInfo.InvariantInfo,
                    "X:{0} Y:{1}", c.X, c.Y);

                this.Invalidate();

                timer.Start();
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (!initialized) return;

            var zoom = this.Renderer.Context.Zoom;

            bool redraw = false;

            if (e.KeyCode == Keys.Up)
            {
                redraw = zoom.Translate(0, 1);
            }
            else if (e.KeyCode == Keys.Down)
            {
                redraw = zoom.Translate(0, -1);
            }
            else if (e.KeyCode == Keys.Left)
            {
                redraw = zoom.Translate(-1, 0);
            }
            else if (e.KeyCode == Keys.Right)
            {
                redraw = zoom.Translate(1, 0);
            }

            if (redraw)
            {
                this.Render();
            }

            e.Handled = true;
        }

        #endregion
    }
}
