// -----------------------------------------------------------------------
// <copyright file="DarkToolStripRenderer.cs" company="">
// TODO: Update copyright text.
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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DarkToolStripRenderer : ToolStripRenderer
    {
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = e.Item.Enabled ? Color.White : Color.Gray;

            base.OnRenderItemText(e);
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
