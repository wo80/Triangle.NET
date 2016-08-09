// -----------------------------------------------------------------------
// <copyright file="DarkCheckBox.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using System.Drawing.Text;

    /// <summary>
    /// Dark checkbox control.
    /// </summary>
    public class DarkCheckBox : ButtonBase
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

        enum eButtonState { Normal, MouseOver, Down }
        eButtonState m_State = eButtonState.Normal;

        // Make sure the control is invalidated when the text is changed.
        public override string Text
        {
            get { return base.Text; }
            set { base.Text = value; this.Invalidate(); }
        }

        int boxSize = 13;

        bool isChecked = false;
        public bool Checked
        {
            get { return isChecked; }
            set { isChecked = value; this.Invalidate(); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DarkCheckBox" /> control.
        /// </summary>
        public DarkCheckBox()
        {
            this.BackColor = Color.FromArgb(76, 76, 76);
            InitializeComponent();
        }

        private void DrawText(Graphics g, Color forecolor, Point location)
        {
            if (this.UseCompatibleTextRendering)
            {
                //using (StringFormat stringFormat = this.CreateStringFormat())
                {
                    if (this.Enabled)
                    {
                        g.DrawString(this.Text, base.Font, new SolidBrush(forecolor), location.X, location.Y);
                    }
                    else
                    {
                        g.DrawString(this.Text, base.Font, new SolidBrush(forecolor), location.X, location.Y);
                    }
                }
            }
            else
            {
                //TextFormatFlags textFormatFlags = this.CreateTextFormatFlags();
                if (this.Enabled)
                {
                    TextRenderer.DrawText((IDeviceContext)g, this.Text, this.Font, location, forecolor);
                }
                else
                {
                    //forecolor = TextRenderer.DisabledTextColor(this.BackColor);
                    TextRenderer.DrawText((IDeviceContext)g, this.Text, this.Font, location, forecolor);
                }
            }
        }

        #region Control overrides

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), this.ClientRectangle);

            Pen checkMark = new Pen(Color.White, 1.8f);
            checkMark.StartCap = LineCap.Round;
            checkMark.EndCap = LineCap.Round;

            // Colors and brushes
            Pen brushBorder = null;
            LinearGradientMode mode = LinearGradientMode.Vertical;
            LinearGradientBrush brushOuter = null;
            LinearGradientBrush brushInner = null;

            int y = (this.Height - boxSize) / 2;

            Rectangle newRect = new Rectangle(1, y, boxSize, boxSize);
            Color text_color = Color.White;

            brushOuter = new LinearGradientBrush(newRect, ColorScheme.ColorGray107, ColorScheme.ColorGray110, mode);
            e.Graphics.FillRectangle(brushOuter, newRect);

            newRect = new Rectangle(2, y + 1, boxSize - 3, boxSize - 3);

            if (Enabled)
            {
                if (base.Focused)
                    brushBorder = new Pen(Color.FromArgb(60, 60, 60), 1f);
                else
                    brushBorder = new Pen(Color.FromArgb(38, 38, 38), 1f);

                switch (m_State)
                {
                    case eButtonState.Normal:
                        brushInner = new LinearGradientBrush(newRect, Color.FromArgb(111, 111, 111), Color.FromArgb(80, 80, 80), mode);
                        e.Graphics.FillRectangle(brushInner, newRect);
                        break;

                    case eButtonState.MouseOver:
                        brushInner = new LinearGradientBrush(newRect, Color.FromArgb(118, 118, 118), Color.FromArgb(81, 81, 81), mode);
                        e.Graphics.FillRectangle(brushInner, newRect);
                        break;

                    case eButtonState.Down:
                        brushInner = new LinearGradientBrush(newRect, Color.FromArgb(92, 92, 92), Color.FromArgb(62, 62, 62), mode);
                        e.Graphics.FillRectangle(brushInner, newRect);
                        break;
                }

                e.Graphics.DrawRectangle(brushBorder, newRect);

            }
            else
            {
                brushInner = new LinearGradientBrush(newRect, Color.FromArgb(76, 76, 76), Color.FromArgb(65, 65, 65), mode);
                e.Graphics.FillRectangle(brushInner, newRect);

                brushBorder = new Pen(Color.FromArgb(48, 48, 48), 1f);
                e.Graphics.DrawRectangle(brushBorder, newRect);

                text_color = Color.FromArgb(160, 160, 160);
                checkMark.Color = Color.FromArgb(180, 180, 180);
            }

            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            SizeF szL = e.Graphics.MeasureString(this.Text, base.Font, this.Width);
            DrawText(e.Graphics, text_color, new Point(boxSize + 4, (int)((this.Height - szL.Height) / 2) + 1));

            if (this.isChecked)
            {
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

                e.Graphics.DrawLine(checkMark, 4, newRect.Bottom - boxSize / 2, newRect.Left + boxSize / 2.5f, newRect.Bottom - 2);
                e.Graphics.DrawLine(checkMark, newRect.Left + boxSize / 2.6f, newRect.Bottom - 2, newRect.Right, newRect.Top);
            }

            if (brushOuter != null) brushOuter.Dispose();
            if (brushInner != null) brushInner.Dispose();
            if (brushBorder != null) brushBorder.Dispose();
            if (checkMark != null) checkMark.Dispose();
        }

        protected override void OnMouseLeave(System.EventArgs e)
        {
            m_State = eButtonState.Normal;
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseEnter(System.EventArgs e)
        {
            m_State = eButtonState.MouseOver;
            this.Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            m_State = eButtonState.MouseOver;
            this.Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            m_State = eButtonState.Down;
            this.Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnClick(EventArgs e)
        {
            this.isChecked = !this.isChecked;
            this.Invalidate();
            base.OnClick(e);
        }

        #endregion
    }
}
