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

    public class TextBoxDark : Control
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

            this.textBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox.Location = new System.Drawing.Point(3, 2);
            this.textBox.Name = "textBox";
            this.textBox.TabIndex = 0;
            // 
            // TextBoxDark
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.textBox);
            this.Cursor = Cursors.IBeam;
            this.Size = new System.Drawing.Size(150, 22);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        #endregion

        TextBox textBox;

        public TextBoxDark()
        {
            InitializeComponent();

            this.MouseClick += delegate(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left) textBox.Focus();
            };

            textBox.Font = this.Font;
            textBox.Location = new Point(3, (this.Height - textBox.Height) / 2 + 1);
            textBox.Width = this.Width - 8;
            textBox.TextAlign = HorizontalAlignment.Right;
            textBox.ForeColor = this.ForeColor;
            textBox.MaxLength = 6;

            textBox.GotFocus += delegate(object sender, EventArgs e)
            {
                textBox.ForeColor = Color.White;
            };
            textBox.LostFocus += delegate(object sender, EventArgs e)
            {
                textBox.ForeColor = this.ForeColor;
            };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = this.ClientRectangle;

            Brush brushOuter = new LinearGradientBrush(rect, Color.FromArgb(82, 82, 82), Color.FromArgb(96, 96, 96),
                LinearGradientMode.Vertical);

            Pen brushBorder = new Pen(Color.FromArgb(38, 38, 38), 1f);

            e.Graphics.FillRectangle(brushOuter, rect);

            rect = new Rectangle(1, 1, this.Width - 3, this.Height - 3);
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), rect);

            e.Graphics.DrawRectangle(brushBorder, rect);

            brushOuter.Dispose();
            brushBorder.Dispose();

            base.OnPaint(e);
        }

        #region Property overrides

        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                textBox.Font = value;
                base.Font = value;
            }
        }

        public override String Text
        {
            get
            {
                return textBox.Text;
            }
            set
            {
                textBox.Text = value;
            }
        }

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                textBox.ForeColor = value;
                base.ForeColor = value;
            }
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                textBox.BackColor = value;
                base.BackColor = value;
            }
        }

        #endregion

        #region Textbox properties

        public HorizontalAlignment TextAlign
        {
            get
            {
                return textBox.TextAlign;
            }
            set
            {
                textBox.TextAlign = value;
            }
        }

        #endregion
    }
}
