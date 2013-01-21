// -----------------------------------------------------------------------
// <copyright file="RendererControl.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshRenderer.Core.GDI
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Windows.Forms;
    using TriangleNet;
    using TriangleNet.Geometry;

    /// <summary>
    /// Renders a mesh using GDI.
    /// </summary>
    public class RenderControl : Control, IMeshRenderer
    {
        // Rendering stuff
        private BufferedGraphics buffer;
        private BufferedGraphicsContext context;

        Zoom zoom;
        RenderData data;

        MeshRenderer meshRenderer;
        VoronoiRenderer voronoiRenderer;

        ColorManager renderColors;

        bool initialized = false;
        bool showVoronoi = false;
        bool showRegions = true;

        string coordinate = String.Empty;

        Timer timer;

        /// <summary>
        /// Gets the currently displayed <see cref="RenderData"/>.
        /// </summary>
        public RenderData Data
        {
            get { return data; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderControl" /> class.
        /// </summary>
        public RenderControl()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);

            renderColors = ColorManager.Default();

            this.BackColor = renderColors.Background;

            zoom = new Zoom(true);
            context = new BufferedGraphicsContext();
            data = new RenderData();

            timer = new Timer();
            timer.Interval = 3000;
            timer.Tick += (sender, e) =>
            {
                timer.Stop();
                coordinate = String.Empty;
                this.Invalidate();
            };
        }

        public bool ShowVoronoi
        {
            get { return showVoronoi; }
            set
            {
                if (showVoronoi != value)
                {
                    this.Render();
                }
                showVoronoi = value;
            }
        }

        public bool ShowRegions
        {
            get { return showRegions; }
            set
            {
                if (showRegions != value)
                {
                    this.Render();
                }
                showRegions = value;
            }
        }

        /// <summary>
        /// Initialize the graphics buffer (should be called in the forms load event).
        /// </summary>
        public void Initialize()
        {
            zoom.Initialize(this.ClientRectangle);
            InitializeBuffer();

            initialized = true;

            this.Invalidate();
        }

        /// <summary>
        /// Updates the displayed input data.
        /// </summary>
        public void SetData(RenderData data)
        {
            this.data = data;

            meshRenderer = new MeshRenderer(data, renderColors);

            this.showVoronoi = data.VoronoiPoints != null;

            if (showVoronoi)
            {
                voronoiRenderer = new VoronoiRenderer(data);
            }

            // Reset the zoom on new data
            zoom.Initialize(this.ClientRectangle, data.Bounds);

            initialized = true;

            this.Render();
        }

        /// <summary>
        /// Zoom to the given location.
        /// </summary>
        /// <param name="location">The zoom focus.</param>
        /// <param name="delta">Indicates whether to zoom in or out.</param>
        public void Zoom(float x, float y, int delta)
        {
            if (!initialized) return;

            if (zoom.ZoomUpdate(delta, x, y))
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
            zoom.Initialize(this.ClientRectangle, data.Bounds);
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

                        // Bounds didn't change. Probably we just restored the window
                        // from minimized state.
                        return;
                    }

                    buffer.Dispose();
                }

                buffer = context.Allocate(Graphics.FromHwnd(this.Handle), this.ClientRectangle);

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

            Graphics g = buffer.Graphics;
            g.Clear(this.BackColor);

            if (!initialized || data == null)
            {
                return;
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (voronoiRenderer != null && this.showVoronoi)
            {
                meshRenderer.RenderMesh(g, zoom);
                voronoiRenderer.Render(g, zoom, renderColors);
                meshRenderer.RenderGeometry(g, zoom);
            }
            else if (meshRenderer != null)
            {
                meshRenderer.Render(g, zoom, showRegions);
            }

            this.Invalidate();
        }

        #region Control overrides

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (!initialized)
            {
                base.OnPaint(pe);
                return;
            }

            buffer.Render();

            if (!String.IsNullOrEmpty(coordinate) && data.Points != null)
            {
                Graphics g = pe.Graphics;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                g.DrawString(coordinate, this.Font, Brushes.White, 10, 10);
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (!initialized) return;

            if (e.Button == MouseButtons.Middle)
            {
                zoom.ZoomReset();
                this.Render();
            }
            else if (e.Button == MouseButtons.Left)
            {
                timer.Stop();

                var nfi = System.Globalization.CultureInfo.InvariantCulture.NumberFormat;

                PointF c = zoom.ScreenToWorld((float)e.X / this.Width, (float)e.Y / this.Height);
                coordinate = String.Format("X:{0} Y:{1}",
                    c.X.ToString(nfi),
                    c.Y.ToString(nfi));

                this.Invalidate();

                timer.Start();
            }

            base.OnMouseClick(e);
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
