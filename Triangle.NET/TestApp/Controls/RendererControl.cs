// -----------------------------------------------------------------------
// <copyright file="MeshRenderer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.Controls
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Windows.Forms;
    using MeshExplorer.Rendering;
    using TriangleNet;
    using TriangleNet.IO;
    using TriangleNet.Data;
    using TriangleNet.Geometry;

    /// <summary>
    /// Renders a mesh using GDI.
    /// </summary>
    public class RendererControl : Control
    {
        // Rendering stuff
        private BufferedGraphics buffer;
        private BufferedGraphicsContext context;

        Zoom zoom;
        RenderData data;

        MeshRenderer meshRenderer;
        VoronoiRenderer voronoiRenderer;

        RenderColors renderColors;

        bool initialized = false;
        bool showVoronoi = false;

        string coordinate = String.Empty;

        Timer timer;

        public RenderData Data { get { return data; } }
        public bool ShowVoronoi
        {
            get { return showVoronoi; }
            set
            {
                showVoronoi = value;

                if (voronoiRenderer != null && showVoronoi)
                {
                    voronoiRenderer.Update();
                }

                this.Render();
            }
        }

        public RendererControl()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);

            renderColors = RenderColors.Default;

            this.BackColor = renderColors.Background;

            zoom = new Zoom();
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

        public void Initialize()
        {
            zoom.Initialize(this.ClientRectangle, this.ClientRectangle);
            InitializeBuffer();

            initialized = true;

            this.Invalidate();
        }

        public void SetData(InputGeometry mesh)
        {
            data.SetData(mesh);

            meshRenderer = new MeshRenderer(data);

            // Reset the zoom on new data
            zoom.Initialize(this.ClientRectangle, data.Bounds);

            initialized = true;

            this.Render();
        }

        public void SetData(Mesh mesh, bool initZoom)
        {
            data.SetData(mesh);

            if (initZoom)
            {
                // Reset the zoom on new data
                zoom.Initialize(this.ClientRectangle, data.Bounds);
            }

            meshRenderer = new MeshRenderer(data);
            voronoiRenderer = new VoronoiRenderer(mesh);

            if (showVoronoi)
            {
                voronoiRenderer.Update();
            }

            initialized = true;

            this.Render();
        }

        public void SetData(Mesh mesh)
        {
            SetData(mesh, false);
        }

        public void Zoom(PointF location, int delta)
        {
            if (!initialized) return;

            if (zoom.Update(delta, location.X / (float)this.Width, location.Y / (float)this.Height))
            {
                // Redraw
                this.Render();
            }
        }

        public void HandleResize()
        {
            zoom.Resize(this.ClientRectangle, data.Bounds);
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
                meshRenderer.RenderMesh(g, zoom, renderColors);
                voronoiRenderer.Render(g, zoom, renderColors);
                meshRenderer.RenderGeometry(g, zoom, renderColors);
            }
            else if (meshRenderer != null)
            {
                meshRenderer.Render(g, zoom, renderColors);
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
                zoom.Reset();
                this.Render();
            }
            else if (e.Button == MouseButtons.Left)
            {
                // Just in case ...
                timer.Stop();

                PointF c = zoom.ScreenToWorld((float)e.X / this.Width, (float)e.Y / this.Height);
                coordinate = String.Format("X:{0} Y:{1}",
                    c.X.ToString(Util.Nfi),
                    c.Y.ToString(Util.Nfi));

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
