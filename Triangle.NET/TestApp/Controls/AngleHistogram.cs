// -----------------------------------------------------------------------
// <copyright file="AngleHistogram.cs" company="">
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
    /// Displays an angle histogram.
    /// </summary>
    /// <remarks>
    /// The angle histogram is divided into two parts: the minimum angles
    /// on the left side (0 to 60 degrees) and the maximum angles on the
    /// right (60 to 180 degrees).
    /// </remarks>
    public class AngleHistogram : Control
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

        int[] maxAngles;
        int[] minAngles;

        Brush fillBlue1 = new SolidBrush(Color.FromArgb(60, 100, 140));
        Brush fillBlue2 = new SolidBrush(Color.FromArgb(110, 150, 200));

        Brush textBack = new SolidBrush(Color.FromArgb(72, 0, 0, 0));

        // The maximum number of angles
        int maxAngleCount = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="AngleHistogram" /> control.
        /// </summary>
        public AngleHistogram()
        {
            this.BackColor = ColorScheme.ColorGray78;
            InitializeComponent();
        }

        /// <summary>
        /// Updates the histogram data and invalidates the control.
        /// </summary>
        public void SetData(int[] dataMin, int[] dataMax)
        {
            maxAngleCount = 0;

            this.minAngles = dataMin;
            this.maxAngles = dataMax;

            ParseData(dataMin);
            ParseData(dataMax);

            if (maxAngleCount == 0)
            {
                this.maxAngles = null;
                return;
            }

            this.Invalidate();
        }

        private void ParseData(int[] data)
        {
            if (data != null)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] > maxAngleCount)
                    {
                        maxAngleCount = data[i];
                    }
                }
            }
        }

        int padding = 1;
        int paddingBottom = 0;
        int paddingTop = 15;

        private void DrawHistogram(Graphics g, int offset, int left, int size, int[] data, Brush brush, Brush brushTop)
        {
            int count = maxAngleCount;
            int totalHeight = this.Height - paddingBottom - paddingTop;

            int n = offset == 0 ? data.Length / 3 : data.Length;
            float value = 0;

            for (int i = offset; i < n; i++)
            {
                if (data[i] > 0)
                {
                    // Scale to control height
                    value = totalHeight * data[i] / count;

                    // Fill bar
                    g.FillRectangle(brush,
                        left + i * size, this.Height - paddingBottom - value,
                        size - 1, value);

                    // Draw top of bar (just a little effect ...)
                    if (value > 2)
                    {
                        g.FillRectangle(brushTop,
                            left + i * size, this.Height - paddingBottom - value,
                            size - 1, 2);
                    }
                }
            }
        }

        /// <summary>
        /// Draws the labels on the bottom.
        /// </summary>
        private void DrawStrings(Graphics g, SizeF fSize, int size, int middle)
        {
            int fHeight = (int)(fSize.Height + 2);
            g.FillRectangle(textBack, 0, this.Height - fHeight, this.Width, fHeight);

            g.DrawString("0", this.Font, Brushes.White, padding, this.Height - fSize.Height - 1);
            g.DrawString("60", this.Font, Brushes.White,
                this.minAngles.Length * size / 3.0f - 2 * fSize.Width,
                this.Height - fSize.Height - 1);

            g.DrawString("60", this.Font, Brushes.White, middle, this.Height - fSize.Height - 1);
            g.DrawString("180", this.Font, Brushes.White,
                this.Width - 3 * fSize.Width,
                this.Height - fSize.Height - 1);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.FillRectangle(new SolidBrush(this.BackColor), this.ClientRectangle);

            if (this.minAngles == null || this.maxAngles == null)
            {
                return;
            }

            SizeF fSize = g.MeasureString("0", this.Font, this.Width);

            int n = this.minAngles.Length;

            // Hack --- TODO: Change stats class
            if (n != this.maxAngles.Length)
            {
                n = this.minAngles.Length + this.maxAngles.Length;
            }

            // Each bar takes up this space
            int size = (this.Width - 2 * padding) / (n + 1);

            // Make pixel align
            int middle = this.Width - padding - n * size;

            DrawHistogram(g, 0, padding, size, this.minAngles, Brushes.DarkGreen, Brushes.Green);
            DrawHistogram(g, n / 3, middle, size, this.maxAngles, fillBlue1, fillBlue2);

            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            DrawStrings(g, fSize, size, middle + n / 3 * size);
        }
    }
}
