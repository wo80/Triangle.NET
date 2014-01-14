
namespace MeshExplorer.Topology
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.Windows.Forms;
    using MeshRenderer.Core;
    using TriangleNet;

    public class TopologyRenderControl : Control
    {
        // Rendering stuff
        private BufferedGraphics buffer;
        private BufferedGraphicsContext context;

        Zoom zoom;
        TopologyRenderer renderer;

        bool initialized = false;

        public Zoom Zoom
        {
            get { return zoom; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderControl" /> class.
        /// </summary>
        public TopologyRenderControl()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);

            this.BackColor = Color.Black;

            zoom = new Zoom(true);
            context = new BufferedGraphicsContext();
        }

        /// <summary>
        /// Initialize the graphics buffer (should be called in the forms load event).
        /// </summary>
        public void Initialize(Mesh mesh)
        {
            renderer = new TopologyRenderer(mesh);

            zoom.Initialize(this.ClientRectangle);
            //zoom.ClipMargin = 10.0f;

            var b = mesh.Bounds;
            zoom.Update(new BoundingBox((float)b.Xmin, (float)b.Xmax,
                (float)b.Ymin, (float)b.Ymax));

            InitializeBuffer();

            initialized = true;

            this.Render();
        }

        public void Update(OrientedTriangle otri)
        {
            renderer.SelectTriangle(otri.Triangle == null ? null : otri);
            this.Render();
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
            if (buffer == null)
            {
                return;
            }

            Graphics g = buffer.Graphics;
            g.Clear(this.BackColor);

            if (!initialized || renderer == null)
            {
                return;
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            renderer.Render(g, zoom);

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
