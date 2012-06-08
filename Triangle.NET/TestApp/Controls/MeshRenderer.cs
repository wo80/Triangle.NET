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
    public class MeshRenderer : Control
    {
        // Rendering stuff
        private BufferedGraphics buffer;
        private BufferedGraphicsContext context;

        Pen lines = new Pen(Color.FromArgb(30, 30, 30));

        Zoom zoom;
        RenderData data;
        bool initialized = false;
        VoronoiRenderer voronoi;
        bool showVoronoi = false;

        string coordinate = String.Empty;

        Timer timer;

        public long RenderTime { get; private set; }
        public RenderData Data { get { return data; } }
        public bool ShowVoronoi
        {
            get { return showVoronoi; }
            set
            {
                showVoronoi = value;

                if (voronoi != null && showVoronoi)
                {
                    voronoi.Update();
                }

                this.Render();
            }
        }

        public MeshRenderer()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);

            this.BackColor = Color.Black;

            zoom = new Zoom();
            context = new BufferedGraphicsContext();
            data = new RenderData();

            timer = new Timer();
            timer.Interval = 3000;
            timer.Tick += (sender, e) => {
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

            // Reset the zoom on new data
            zoom.Initialize(this.ClientRectangle, data.Bounds);

            initialized = true;

            this.Render();
        }

        public void SetData(Mesh mesh)
        {
            voronoi = new VoronoiRenderer(mesh);
            voronoi.Update();

            data.SetData(mesh);

            initialized = true;

            this.Render();
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

        private void RenderPoints(Graphics g)
        {
            PointF pt;
            PointF[] pts = data.Points;
            int i, n;

            // Draw input points
            n = data.NumberOfInputPoints;
            for (i = 0; i < n; i++)
            {
                if (zoom.ViewportContains(pts[i]))
                {
                    pt = zoom.WorldToScreen(pts[i]);
                    g.FillEllipse(Brushes.Green, pt.X - 1.5f, pt.Y - 1.5f, 3, 3);
                    //g.FillEllipse(Brushes.Black, pt.X - 2, pt.Y - 2, 4, 4);
                    //g.DrawEllipse(Pens.Green, pt.X - 2, pt.Y - 2, 4, 4);
                }
            }

            // Draw Steiner points
            n = pts.Length;
            for (; i < n; i++)
            {
                if (zoom.ViewportContains(pts[i]))
                {
                    pt = zoom.WorldToScreen(pts[i]);
                    g.FillEllipse(Brushes.Peru, pt.X - 1.5f, pt.Y - 1.5f, 3, 3);
                    //g.FillEllipse(Brushes.Black, pt.X - 2, pt.Y - 2, 4, 4);
                    //g.DrawEllipse(Pens.Peru, pt.X - 2, pt.Y - 2, 4, 4);
                }
            }
        }

        private void RenderTriangles(Graphics g)
        {
            PointF p0, p1, p2;
            PointF[] pts = data.Points;

            var triangles = data.Triangles;

            // Draw triangles
            foreach (var tri in triangles)
            {
                if (zoom.ViewportContains(pts[tri.P0]) ||
                    zoom.ViewportContains(pts[tri.P1]) ||
                    zoom.ViewportContains(pts[tri.P2]))
                {
                    p0 = zoom.WorldToScreen(pts[tri.P0]);
                    p1 = zoom.WorldToScreen(pts[tri.P1]);
                    p2 = zoom.WorldToScreen(pts[tri.P2]);

                    g.DrawLine(lines, p0, p1);
                    g.DrawLine(lines, p1, p2);
                    g.DrawLine(lines, p2, p0);
                }
            }
        }

        private void RenderEdges(Graphics g)
        {
            PointF p0, p1;
            PointF[] pts = data.Points;

            var edges = data.Edges;

            // Draw edges
            foreach (var edge in edges)
            {
                if (zoom.ViewportContains(pts[edge.P0]) ||
                    zoom.ViewportContains(pts[edge.P1]))
                {
                    p0 = zoom.WorldToScreen(pts[edge.P0]);
                    p1 = zoom.WorldToScreen(pts[edge.P1]);

                    g.DrawLine(lines, p0, p1);
                }
            }
        }

        private void RenderSegments(Graphics g)
        {
            PointF p0, p1;
            PointF[] pts = data.Points;

            var segments = data.Segments;

            foreach (var seg in segments)
            {
                if (zoom.ViewportContains(pts[seg.P0]) ||
                    zoom.ViewportContains(pts[seg.P1]))
                {
                    p0 = zoom.WorldToScreen(pts[seg.P0]);
                    p1 = zoom.WorldToScreen(pts[seg.P1]);

                    g.DrawLine(Pens.DarkBlue, p0, p1);
                }
            }
        }

        private void Render()
        {
            coordinate = String.Empty;

            Graphics g = buffer.Graphics;
            g.Clear(this.BackColor);

            if (!initialized || data == null)
            {
                return;
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            if (data.Edges != null)
            {
                this.RenderEdges(g);
            }
            else if (data.Triangles != null)
            {
                this.RenderTriangles(g);
            }

            if (voronoi != null && this.showVoronoi)
            {
                voronoi.Render(g, zoom);
            }

            if (data.Segments != null)
            {
                this.RenderSegments(g);
            }

            if (data.Points != null)
            {
                this.RenderPoints(g);
            }

            stopwatch.Stop();

            this.RenderTime = stopwatch.ElapsedMilliseconds;

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
