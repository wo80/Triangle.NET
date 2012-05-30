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

        int[] data;
        int max = 0;

        public AngleHistogram()
        {
            this.BackColor = Color.FromArgb(76, 76, 76);
            InitializeComponent();
        }

        public void SetData(int[] data)
        {
            if (data != null)
            {
                this.data = data;
                this.max = 0;

                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] > max)
                    {
                        max = data[i];
                    }
                }

                if (max == 0)
                {
                    this.data = null;
                    return;
                }

                double lg10 = Math.Ceiling(Math.Log10(max)) - 1;
                int norm = (int)Math.Pow(10, lg10);
                int mod = -max % norm;
                max = max + mod + norm;

                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.FillRectangle(new SolidBrush(this.BackColor), this.ClientRectangle);

            SizeF s1 = g.MeasureString("180", base.Font, this.Width);
            SizeF s2 = g.MeasureString(max.ToString(), base.Font, this.Width);

            // Draw bottom rect
            g.FillRectangle(Brushes.DimGray, s2.Width, this.Height - s1.Height - 4, this.Width, s1.Height + 4);

            // Draw Histogram
            if (data != null)
            {
                int n = data.Length;
                float width = (this.Width - s2.Width) / n;
                float value = 0;

                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] > 0)
                    {
                        // Scale to control height
                        value = (this.Height - s1.Height - 4) * data[i] / max;

                        g.FillRectangle(Brushes.DarkGreen,
                            s2.Width + i * width + width / 8,
                            this.Height - s1.Height - 4 - value,
                            3 * width / 4,
                            value);

                        if (value > 2)
                        {
                            g.FillRectangle(Brushes.Green,
                                s2.Width + i * width + width / 8,
                                this.Height - s1.Height - 4 - value,
                                3 * width / 4,
                                2);
                        }
                    }
                }
            }

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw data keys
            g.DrawString("0", this.Font, Brushes.White, s2.Width + 2, this.Height - s1.Height - 2);
            g.DrawString("90", this.Font, Brushes.White, this.Width / 2 - s1.Width / 3, this.Height - s1.Height - 2);
            g.DrawString("180", this.Font, Brushes.White, this.Width - s1.Width - 2, this.Height - s1.Height - 2);

            // Draw data values
            if (max > 0)
            {
                g.DrawString(max.ToString(), this.Font, Brushes.White, 2, 10);
                g.DrawString((max / 2).ToString(), this.Font, Brushes.White, 2, (this.Height - s1.Height) / 2);
            }
        }
    }
}
