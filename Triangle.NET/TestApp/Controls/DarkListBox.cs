// -----------------------------------------------------------------------
// <copyright file="DarkListBox.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;
    using System.Drawing;

    /// <summary>
    /// Dark listbox control.
    /// </summary>
    public class DarkListBox : ListBox
    {
        Font _boldFont;

        /// <summary>
        /// Initializes a new instance of the <see cref="DarkListBox" /> control.
        /// </summary>
        public DarkListBox()
        {
            _boldFont = new Font(base.Font.FontFamily, base.Font.Size, FontStyle.Bold);

            this.DrawMode = DrawMode.OwnerDrawVariable;
            this.ItemHeight = 22;
            this.FontChanged += new EventHandler(ListBoxFontChanged);
            this.BackColor = Color.FromArgb(96, 96, 96);
        }

        void ListBoxFontChanged(object sender, EventArgs e)
        {
            _boldFont = new Font(base.Font.FontFamily, base.Font.Size, FontStyle.Bold);
        }

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            e.ItemHeight = 22;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (this.Items.Count == 0)
            {
                return;
            }

            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            int index = e.Index;

            string content = "[Error]";

            if (index < this.Items.Count && index >= 0)
            {
                content = this.Items[index].ToString();
            }

            Color color = (e.Index % 2) == 0 ? Color.FromArgb(85, 85, 85) : Color.FromArgb(90, 90, 90);

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                color = Color.FromArgb(100, 105, 110);
            }

            using (SolidBrush background = new SolidBrush(color))
            {
                e.Graphics.FillRectangle(background, e.Bounds);
            }

            using (SolidBrush pen = new SolidBrush(Color.White))
            {
                e.Graphics.DrawString(content, this.Font, pen,
                    new PointF(10, e.Bounds.Y + 3), StringFormat.GenericDefault);
            }
        }
    }
}
