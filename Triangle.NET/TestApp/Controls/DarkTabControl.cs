// -----------------------------------------------------------------------
// <copyright file="DarkTabControl.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// Original code on CodeProject: A .NET Flat TabControl (CustomDraw), Oscar Londono
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Drawing.Text;

    /// <summary>
    /// Summary description for FlatTabControl.
    /// </summary>
    public class DarkTabControl : System.Windows.Forms.TabControl
    {
        #region Designer

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #endregion
        
        private const int margin = 5;
        private Color backColor = ColorScheme.ColorGray68;

        /// <summary>
        /// Initializes a new instance of the <see cref="DarkTabControl" /> control.
        /// </summary>
		public DarkTabControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            base.Multiline = false;

			// double buffering
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.SelectedIndexChanged += (obj, evt) => { Invalidate(); };
		}

        #region Properties

        new public TabAlignment Alignment
        {
            get { return base.Alignment; }
            set
            {
                TabAlignment ta = value;
                if ((ta != TabAlignment.Top) && (ta != TabAlignment.Bottom))
                {
                    ta = TabAlignment.Top;
                }

                base.Alignment = ta;
            }
        }

        public override Color BackColor
        {
            get
            {
                return backColor;
            }
            set
            {
                base.BackColor = backColor;
            }
        }

        #endregion

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e); 
			
			DrawControl(e.Graphics);
		}

        private void DrawControl(Graphics g)
		{
            if (!Visible)
            {
                return;
            }

			Rectangle controlBounds = this.ClientRectangle;
			Rectangle tabBounds = this.DisplayRectangle;

			// Fill client area
			Brush br = new SolidBrush(this.BackColor);
			g.FillRectangle(br, controlBounds);
			br.Dispose();

			int width = tabBounds.Width + margin;

			// Clip region for drawing tabs
			Region clip = g.Clip;
			Rectangle region = new Rectangle(tabBounds.Left, controlBounds.Top, width - margin, controlBounds.Height);

			g.SetClip(region);

			// Draw tabs
            for (int i = 0; i < this.TabCount; i++)
            {
                DrawTab(g, this.TabPages[i], i);
            }

			g.Clip = clip;
		}

        private void DrawTab(Graphics g, TabPage tabPage, int index)
		{
			Rectangle tabBounds = this.GetTabRect(index);

			bool selected = (this.SelectedIndex == index);

			// Fill this tab with background color
            g.FillRectangle(selected ? Brushes.DimGray : ColorScheme.BrushGray68, tabBounds);

            if (selected)
            {
                // Clear bottom lines
                Pen pen = new Pen(tabPage.BackColor);

                switch (this.Alignment)
                {
                    case TabAlignment.Top:
                        g.DrawLine(pen, tabBounds.Left, tabBounds.Bottom, tabBounds.Right - 1, tabBounds.Bottom);
                        g.DrawLine(pen, tabBounds.Left, tabBounds.Bottom + 1, tabBounds.Right - 1, tabBounds.Bottom + 1);
                        break;

                    case TabAlignment.Bottom:
                        g.DrawLine(pen, tabBounds.Left, tabBounds.Top, tabBounds.Right - 1, tabBounds.Top);
                        g.DrawLine(pen, tabBounds.Left, tabBounds.Top - 1, tabBounds.Right - 1, tabBounds.Top - 1);
                        g.DrawLine(pen, tabBounds.Left, tabBounds.Top - 2, tabBounds.Right - 1, tabBounds.Top - 2);
                        break;
                }

                pen.Dispose();
            }

			// Draw string
			StringFormat stringFormat = new StringFormat();
			stringFormat.Alignment = StringAlignment.Center;  
			stringFormat.LineAlignment = StringAlignment.Center;

            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

			g.DrawString(tabPage.Text, Font, Brushes.White, tabBounds, stringFormat);
		}
	}
}
