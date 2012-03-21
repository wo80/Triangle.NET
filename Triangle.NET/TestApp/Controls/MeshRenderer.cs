// -----------------------------------------------------------------------
// <copyright file="MeshRenderer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace TestApp
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Drawing.Drawing2D;
    using TriangleNet;
    using System.Diagnostics;
    using TriangleNet.IO;
    using TestApp.Rendering;

    /// <summary>
    /// Renders a mesh using GDI.
    /// </summary>
    public class MeshRenderer : System.Windows.Forms.Control
    {
        // Rendering stuff
        private BufferedGraphics buffer;
        private BufferedGraphicsContext context;

        Pen lines = new Pen(Color.FromArgb(30, 30, 30));

        Zoom zoom;
        MeshDataInternal data;
        bool initialized = false;

        public long RenderTime { get; private set; }

        public MeshRenderer()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);

            this.BackColor = Color.Black;

            zoom = new Zoom();
            context = new BufferedGraphicsContext();
            data = new MeshDataInternal();
        }

        public void SetData(MeshData meshdata, bool input)
        {
            data.SetData(meshdata);

            if (input)
            {
                // Reset the zoom on new data
                zoom.Initialize(this.ClientRectangle, data.Bounds);
            }

            initialized = true;

            this.Render();
        }

        public void SetData(Mesh mesh, bool input)
        {
            data.SetData(mesh);

            if (input)
            {
                // Reset the zoom on new data
                zoom.Initialize(this.ClientRectangle, data.Bounds);
            }
            
            initialized = true;

            this.Render();
        }

        public void Zoom(Point location, int delta)
        {
            if (!initialized) return;

            if (zoom.Update(delta, location.X / (float)this.Width, location.Y / (float)this.Height))
            {
                // Redraw
                this.Render();
            }
        }

        private void IntializeBuffer()
        {
            if (buffer != null)
            {
                buffer.Dispose();
            }

            buffer = context.Allocate(Graphics.FromHwnd(this.Handle), this.ClientRectangle);
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

            int[] tri;

            // Draw triangles
            int n = data.Triangles.Length;
            for (int i = 0; i < n; i++)
            {
                tri = data.Triangles[i];

                if (zoom.ViewportContains(pts[tri[0]]) ||
                    zoom.ViewportContains(pts[tri[1]]) ||
                    zoom.ViewportContains(pts[tri[2]]))
                {
                    p0 = zoom.WorldToScreen(pts[tri[0]]);
                    p1 = zoom.WorldToScreen(pts[tri[1]]);
                    p2 = zoom.WorldToScreen(pts[tri[2]]);

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

            int[] tri;

            // Draw triangles
            int n = data.Edges.Length;
            for (int i = 0; i < n; i++)
            {
                tri = data.Edges[i];

                if (zoom.ViewportContains(pts[tri[0]]) ||
                    zoom.ViewportContains(pts[tri[1]]))
                {
                    p0 = zoom.WorldToScreen(pts[tri[0]]);
                    p1 = zoom.WorldToScreen(pts[tri[1]]);

                    g.DrawLine(lines, p0, p1);
                }
            }
        }

        private void RenderSegments(Graphics g)
        {
            PointF p0, p1;
            PointF[] pts = data.Points;

            int[] tri;

            // Draw triangles
            int n = data.Segments.Length;
            for (int i = 0; i < n; i++)
            {
                tri = data.Segments[i];

                if (zoom.ViewportContains(pts[tri[0]]) ||
                    zoom.ViewportContains(pts[tri[1]]))
                {
                    p0 = zoom.WorldToScreen(pts[tri[0]]);
                    p1 = zoom.WorldToScreen(pts[tri[1]]);

                    g.DrawLine(Pens.DarkBlue, p0, p1);
                }
            }
        }

        private void Render()
        {
            Graphics g = buffer.Graphics;
            g.Clear(this.BackColor);

            if (!initialized)
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

            if (data.Segments != null)
            {
                this.RenderSegments(g);
            }

            this.RenderPoints(g);

            stopwatch.Stop();

            this.RenderTime = stopwatch.ElapsedMilliseconds;

            this.Invalidate();
        }

        #region Control overrides

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = buffer.Graphics;
            g.SmoothingMode = SmoothingMode.Default;

            Pen pen1 = new Pen(Color.FromArgb(82, 82, 82));
            Pen pen2 = new Pen(Color.FromArgb(40, 40, 40));

            g.DrawLine(pen1, 0, 0, this.Width, 0);
            g.DrawLine(pen2, 0, 1, this.Width, 1);

            pen1.Dispose();
            pen2.Dispose();

            buffer.Render();
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            if (buffer == null) return;

            // Redraw

            base.OnClientSizeChanged(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (!initialized) return;

            if (e.Button == MouseButtons.Middle)
            {
                zoom.Reset();
                this.Render();
            }

            base.OnMouseClick(e);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Do nothing
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            IntializeBuffer();

            //_coordinates.SetBounds(this.ClientRectangle);
        }

        #endregion
    }
}
