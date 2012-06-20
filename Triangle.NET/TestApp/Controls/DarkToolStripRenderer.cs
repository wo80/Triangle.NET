// -----------------------------------------------------------------------
// <copyright file="DarkToolStripRenderer.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    /// <summary>
    /// Toolstrip render for dark menu.
    /// </summary>
    public class DarkToolStripRenderer : ToolStripRenderer
    {
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = e.Item.Enabled ? Color.White : Color.Gray;

            base.OnRenderItemText(e);
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            int h = e.Item.Height;
            int size = h / 2 - 2; // Box size
            Rectangle rect = new Rectangle(10, (h - size) / 2, size - 2, size);

            var mode = e.Graphics.SmoothingMode;

            using (Pen pen = new Pen(Color.White, 1.6f))
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.DrawLine(pen, rect.Left, rect.Bottom - size / 2, rect.Left + size / 2.5f, rect.Bottom - 2);
                e.Graphics.DrawLine(pen, rect.Left + size / 2.6f, rect.Bottom - 2, rect.Right, rect.Top);
            }

            e.Graphics.SmoothingMode = mode;
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            if (!e.Item.Selected && !e.Item.Pressed)
            {
                e.ArrowColor = ColorScheme.ColorGray89;
            }

            base.OnRenderArrow(e);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            e.Graphics.FillRectangle(ColorScheme.BrushGray78, 0, 2, e.Item.Width, 1);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Enabled)
            {
                if (e.Item.Selected || e.Item.Pressed)
                {
                    e.Graphics.FillRectangle(Brushes.DimGray, 0, 0, e.Item.Width, e.Item.Height);
                }
                else
                {
                    e.Graphics.FillRectangle(ColorScheme.BrushGray68, 0, 0, e.Item.Width, e.Item.Height);
                }
            }

            //base.OnRenderMenuItemBackground(e);
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            e.Graphics.FillRectangle(ColorScheme.BrushGray68, e.AffectedBounds);
            //base.OnRenderToolStripBackground(e);
        }
    }
}
