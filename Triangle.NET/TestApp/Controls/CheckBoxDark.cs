// -----------------------------------------------------------------------
// <copyright file="CheckBoxDark.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace TestApp.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CheckBoxDark : ButtonBase
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

        // make sure the control is invalidated(repainted) when the text is changed
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

        //--------------------------------------------------------------------------------
        public CheckBoxDark()
        {
            this.BackColor = Color.FromArgb(76, 76, 76);
            InitializeComponent();
        }

        //--------------------------------------------------------------------------------
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

            if (Enabled)
            {
                if (base.Focused)
                    brushBorder = new Pen(Color.FromArgb(60, 60, 60), 1f);
                else
                    brushBorder = new Pen(Color.FromArgb(38, 38, 38), 1f);

                brushOuter = new LinearGradientBrush(newRect, Color.FromArgb(82, 82, 82), Color.FromArgb(96, 96, 96), mode);
                e.Graphics.FillRectangle(brushOuter, newRect);

                newRect = new Rectangle(2, y + 1, boxSize - 3, boxSize - 3);

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

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            if (this.isChecked)
            {
                e.Graphics.DrawLine(checkMark, 4, newRect.Bottom - boxSize / 2, newRect.Left + boxSize / 2.5f, newRect.Bottom - 2);
                e.Graphics.DrawLine(checkMark, newRect.Left + boxSize / 2.6f, newRect.Bottom - 2, newRect.Right, newRect.Top);
            }

            SizeF szL = e.Graphics.MeasureString(this.Text, base.Font, this.Width);
            e.Graphics.DrawString(this.Text, base.Font, new SolidBrush(text_color), boxSize + 4, (this.Height - szL.Height) / 2);

            if (brushOuter != null) brushOuter.Dispose();
            if (brushInner != null) brushInner.Dispose();
            if (brushBorder != null) brushBorder.Dispose();
            if (checkMark != null) checkMark.Dispose();
        }

        //--------------------------------------------------------------------------------
        protected override void OnMouseLeave(System.EventArgs e)
        {
            m_State = eButtonState.Normal;
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        //--------------------------------------------------------------------------------
        protected override void OnMouseEnter(System.EventArgs e)
        {
            m_State = eButtonState.MouseOver;
            this.Invalidate();
            base.OnMouseEnter(e);
        }

        //--------------------------------------------------------------------------------
        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            m_State = eButtonState.MouseOver;
            this.Invalidate();
            base.OnMouseUp(e);
        }

        //--------------------------------------------------------------------------------
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
    }
}
