// -----------------------------------------------------------------------
// <copyright file="DarkButton.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Text;
    using System.Windows.Forms;

    public class DarkButton : Button
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DarkButton" /> control.
        /// </summary>
        public DarkButton()
        {
            InitializeComponent();
        }

        #region Control overrides

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            float down = 0.0f;

            // Colors and brushes
            Pen brushBorder = null;
            LinearGradientMode mode = LinearGradientMode.Vertical;
            LinearGradientBrush brushOuter = null;
            LinearGradientBrush brushInner = null;

            Rectangle newRect = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
            Color text_color = Color.White;

            if (Enabled)
            {
                if (base.Focused)
                    brushBorder = new Pen(Color.FromArgb(24, 24, 24), 1f);
                else
                    brushBorder = new Pen(Color.FromArgb(56, 56, 56), 1f);

                switch (m_State)
                {
                    case eButtonState.Normal:
                        brushOuter = new LinearGradientBrush(newRect, Color.FromArgb(123, 123, 123), Color.FromArgb(77, 77, 77), mode);
                        brushInner = new LinearGradientBrush(newRect, Color.FromArgb(104, 104, 104), Color.FromArgb(71, 71, 71), mode);
                        e.Graphics.FillRectangle(brushOuter, newRect);
                        newRect.Inflate(-1, -1);
                        e.Graphics.FillRectangle(brushInner, newRect);
                        newRect.Inflate(1, 1);
                        break;

                    case eButtonState.MouseOver:
                        brushOuter = new LinearGradientBrush(newRect, Color.FromArgb(140, 140, 140), Color.FromArgb(87, 87, 87), mode);
                        brushInner = new LinearGradientBrush(newRect, Color.FromArgb(118, 118, 118), Color.FromArgb(81, 81, 81), mode);
                        e.Graphics.FillRectangle(brushOuter, newRect);
                        newRect.Inflate(-1, -1);
                        e.Graphics.FillRectangle(brushInner, newRect);
                        newRect.Inflate(1, 1);
                        break;

                    case eButtonState.Down:
                        down = 1.0f;
                        brushOuter = new LinearGradientBrush(newRect, Color.FromArgb(108, 108, 108), Color.FromArgb(68, 68, 68), mode);
                        brushInner = new LinearGradientBrush(newRect, Color.FromArgb(92, 92, 92), Color.FromArgb(62, 62, 62), mode);
                        e.Graphics.FillRectangle(brushOuter, newRect);
                        newRect.Inflate(-1, -1);
                        e.Graphics.FillRectangle(brushInner, newRect);
                        newRect.Inflate(1, 1);
                        break;
                }

                e.Graphics.DrawRectangle(brushBorder, newRect);
            }
            else
            {
                text_color = Color.FromArgb(110, 110, 110);
                brushBorder = new Pen(Color.FromArgb(48, 48, 48), 1f);
                brushOuter = new LinearGradientBrush(newRect, Color.FromArgb(82, 82, 82), Color.FromArgb(67, 67, 67), mode);
                brushInner = new LinearGradientBrush(newRect, Color.FromArgb(76, 76, 76), Color.FromArgb(65, 65, 65), mode);
                e.Graphics.FillRectangle(brushOuter, newRect);
                newRect.Inflate(-1, -1);
                e.Graphics.FillRectangle(brushInner, newRect);
                newRect.Inflate(1, 1);
                e.Graphics.DrawRectangle(brushBorder, newRect);
            }


            string largetext = this.Text;

            SizeF szL = e.Graphics.MeasureString(this.Text, base.Font, this.Width);
            if (Enabled)
            {
                e.Graphics.DrawString(largetext, base.Font, Brushes.Black,
                    new RectangleF(new PointF((this.Width - szL.Width) / 2, (this.Height - szL.Height) / 2 + 1 + down), szL));
            }
            e.Graphics.DrawString(largetext, base.Font, new SolidBrush(text_color),
                    new RectangleF(new PointF((this.Width - szL.Width) / 2, (this.Height - szL.Height) / 2 + down), szL));

            brushOuter.Dispose();
            brushInner.Dispose();
            brushBorder.Dispose();
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

        #endregion
    }
}