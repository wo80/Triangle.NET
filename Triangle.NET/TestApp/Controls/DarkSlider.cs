// -----------------------------------------------------------------------
// <copyright file="DarkSlider.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// Original code on CodeProject: Owner-drawn trackbar (slider), Michal Brylka
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
    /// Encapsulates control that visualy displays certain integer value and allows user to change 
    /// it within desired range. It imitates <see cref="System.Windows.Forms.TrackBar"/> as far as 
    /// mouse usage is concerned.
    /// </summary>
    public class DarkSlider : Control
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

        #region Events

        /// <summary>
        /// Fires when Slider position has changed
        /// </summary>
        public event EventHandler ValueChanging;

        private void OnValueChanging()
        {
            var evt = ValueChanging;

            if (evt != null)
            {
                evt(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Fires when Slider position has changed
        /// </summary>
        public event EventHandler ValueChanged;

        private void OnValueChanged()
        {
            var evt = ValueChanged;

            if (evt != null)
            {
                evt(this, EventArgs.Empty);
            }
        }

        #endregion

        #region Properties

        int thumbSize = 7;
        private Rectangle thumbRect; //bounding rectangle of thumb area
        private Rectangle barRect; //bounding rectangle of bar area
        private bool mouseInThumbRegion = false;

        private int trackerValue = 50;
        /// <summary>
        /// Gets or sets the value of Slider.
        /// </summary>
        /// <value>The value.</value>
        public int Value
        {
            get { return trackerValue; }
            set
            {
                if (value >= barMinimum & value <= barMaximum)
                {
                    trackerValue = value;
                    if (ValueChanged != null) ValueChanged(this, new EventArgs());
                    Invalidate();
                }
                // ArgumentOutOfRangeException("Value is outside appropriate range (min, max)");
            }
        }


        private int barMinimum = 0;
        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        /// <value>The minimum value.</value>
        public int Minimum
        {
            get { return barMinimum; }
            set
            {
                if (value < barMaximum)
                {
                    barMinimum = value;
                    if (trackerValue < barMinimum)
                    {
                        trackerValue = barMinimum;
                        if (ValueChanged != null) ValueChanged(this, new EventArgs());
                    }
                    Invalidate();
                }
                // ArgumentOutOfRangeException("Minimal value is greather than maximal one");
            }
        }


        private int barMaximum = 100;
        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        /// <value>The maximum value.</value>
        public int Maximum
        {
            get { return barMaximum; }
            set
            {
                if (value > barMinimum)
                {
                    barMaximum = value;
                    if (trackerValue > barMaximum)
                    {
                        trackerValue = barMaximum;
                        if (ValueChanged != null) ValueChanged(this, new EventArgs());
                    }
                    Invalidate();
                }
                // ArgumentOutOfRangeException("Maximal value is lower than minimal one");
            }
        }

        private uint criticalPercent = 0;
        /// <summary>
        /// Gets or sets trackbar's small change. It affects how to behave when directional keys are pressed
        /// </summary>
        /// <value>The small change value.</value>
        public uint CriticalPercent
        {
            get { return criticalPercent; }
            set { criticalPercent = value; }
        }

        private Color thumbOuterColor = Color.White;
        private Color thumbInnerColor = Color.Gainsboro;
        private Color thumbPenColor = Color.Silver;
        private Color barOuterColor = Color.SkyBlue;
        private Color barInnerColor = Color.DarkSlateBlue;
        private Color barPenColor = Color.Gainsboro;
        private Color elapsedOuterColor = Color.DarkGreen;
        private Color elapsedInnerColor = Color.Chartreuse;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSlider"/> control.
        /// </summary>
        public DarkSlider()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw
                | ControlStyles.Selectable
                | ControlStyles.SupportsTransparentBackColor
                | ControlStyles.UserMouse
                | ControlStyles.UserPaint, true);

            BackColor = Color.Transparent;

            Minimum = 0;
            Maximum = 100;
            Value = 50;
        }

        #endregion

        #region Paint

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!Enabled)
            {
                DrawDisabledSlider(e.Graphics);
            }
            else
            {
                //if (mouseEffects && mouseInRegion)
                //{
                //    Color[] lightenedColors = LightenColors(thumbOuterColor, thumbInnerColor, thumbPenColor,
                //                                            barOuterColor, barInnerColor, barPenColor,
                //                                            elapsedOuterColor, elapsedInnerColor);
                //    DrawColorSlider(e, lightenedColors[0], lightenedColors[1], lightenedColors[2], lightenedColors[3],
                //                    lightenedColors[4], lightenedColors[5], lightenedColors[6], lightenedColors[7]);
                //}
                //else
                {
                    DrawColorSlider(e.Graphics);
                }
            }
        }

        private void DrawDisabledSlider(Graphics g)
        {
            try
            {
                //adjust drawing rects
                barRect = new Rectangle(1, this.Height / 2, this.Width - 2, 5);

                Brush sliderLGBrushH = new LinearGradientBrush(barRect, ColorScheme.ColorGray122,
                    ColorScheme.ColorGray107, LinearGradientMode.Horizontal);

                //draw bar
                {
                    // Background gradient
                    g.FillRectangle(sliderLGBrushH, barRect);
                    // Background fill
                    g.FillRectangle(ColorScheme.SliderBorderBrush,
                        barRect.Left + 1, barRect.Top, barRect.Width - 2, barRect.Height - 1);
                    // Bar fill
                    g.FillRectangle(ColorScheme.SliderFillBrush,
                        barRect.Left + 2, barRect.Top + 1, barRect.Width - 4, barRect.Height - 3);
                }

                sliderLGBrushH.Dispose();
            }
            catch (Exception)
            { }
            finally
            { }
        }

        /// <summary>
        /// Draws the colorslider control using passed colors.
        /// </summary>
        private void DrawColorSlider(Graphics g)
        {
            try
            {
                //set up thumbRect aproprietly
                int track = (((trackerValue - barMinimum) * (ClientRectangle.Width - thumbSize)) / (barMaximum - barMinimum));
                thumbRect = new Rectangle(track, this.Height / 2 - 3, thumbSize - 1, 10);

                //adjust drawing rects
                barRect = new Rectangle(1, this.Height / 2, this.Width - 2, 5);

                //get thumb shape path 
                GraphicsPath thumbPath = new GraphicsPath();
                thumbPath.AddPolygon(new Point[] {
                    new Point(thumbRect.Left, thumbRect.Top),
                    new Point(thumbRect.Right, thumbRect.Top),
                    new Point(thumbRect.Right, thumbRect.Bottom - 4),
                    new Point(thumbRect.Left + thumbRect.Width / 2, thumbRect.Bottom),
                    new Point(thumbRect.Left, thumbRect.Bottom - 4)
                });

                Brush sliderLGBrushH = new LinearGradientBrush(barRect, ColorScheme.ColorGray122,
                    ColorScheme.ColorGray107, LinearGradientMode.Horizontal);

                Brush barFill = (criticalPercent > 0 && trackerValue > criticalPercent) ? Brushes.Peru : Brushes.Green;

                //draw bar
                {
                    // Background gradient
                    g.FillRectangle(sliderLGBrushH, barRect);
                    // Background fill
                    g.FillRectangle(ColorScheme.SliderBorderBrush,
                        barRect.Left + 1, barRect.Top, barRect.Width - 2, barRect.Height - 1);
                    // Bar fill
                    g.FillRectangle(ColorScheme.SliderFillBrush,
                        barRect.Left + 2, barRect.Top + 1, barRect.Width - 4, barRect.Height - 3);
                    // Elapsed bar fill

                    g.FillRectangle(barFill,
                        barRect.Left + 2, barRect.Top + 1, thumbRect.Left + thumbSize / 2 - 2, barRect.Height - 3);

                    //draw bar band                    
                    //g.DrawRectangle(barPen, barRect);
                }

                sliderLGBrushH.Dispose();

                //draw thumb
                Brush brushInner = new LinearGradientBrush(thumbRect,
                    Color.FromArgb(111, 111, 111), Color.FromArgb(80, 80, 80),
                    LinearGradientMode.Vertical);

                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.FillPath(brushInner, thumbPath);
                g.DrawPath(Pens.Black, thumbPath);

                brushInner.Dispose();
                //draw thumb band
                //Color newThumbPenColor = thumbPenColorPaint;
                //if (mouseEffects && (Capture || mouseInThumbRegion))
                //    newThumbPenColor = ControlPaint.Dark(newThumbPenColor);
                //g.DrawPath(thumbPen, thumbPath);
            }
            catch (Exception)
            { }
            finally
            { }
        }

        #endregion

        #region Overided events

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            mouseInThumbRegion = false;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left && this.Enabled)
            {
                this.Capture = true;
                OnValueChanging();
                OnMouseMove(e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            mouseInThumbRegion = thumbRect.Contains(e.Location);
            if (Capture & e.Button == MouseButtons.Left)
            {
                Point pt = e.Location;
                int p = pt.X;
                int margin = thumbSize >> 1;
                p -= margin;
                float coef = (float)(barMaximum - barMinimum) /
                             (float)(ClientSize.Width - 2 * margin);
                trackerValue = (int)(p * coef + barMinimum);

                if (trackerValue <= barMinimum)
                {
                    trackerValue = barMinimum;
                }
                else if (trackerValue >= barMaximum)
                {
                    trackerValue = barMaximum;
                }

                OnValueChanging();
            }
            Invalidate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (this.Enabled)
            {
                base.OnMouseUp(e);
                this.Capture = false;
                mouseInThumbRegion = thumbRect.Contains(e.Location);
                OnValueChanged();
                Invalidate();
            }
        }

        #endregion

        #region Help routines

        /// <summary>
        /// Sets the trackbar value so that it wont exceed allowed range.
        /// </summary>
        /// <param name="val">The value.</param>
        private void SetProperValue(int val)
        {
            if (val < barMinimum) Value = barMinimum;
            else if (val > barMaximum) Value = barMaximum;
            else Value = val;
        }

        #endregion
    }
}
