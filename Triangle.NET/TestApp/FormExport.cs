using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MeshExplorer
{
    public partial class FormExport : Form
    {
        public FormExport()
        {
            InitializeComponent();
        }

        public int ImageFormat
        {
            get { return darkListBox1.SelectedIndex; }
        }

        public int ImageSize
        {
            get
            {
                string s = lbSize.Text;
                s = s.Substring(0, s.Length - 3);
                int size = 0;
                int.TryParse(s, out size);
                return size;
            }
        }

        public string ImageName
        {
            get { return darkTextBox1.Text; }
            set { darkTextBox1.Text = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var rect = this.ClientRectangle;
            rect.Height -= 40;

            e.Graphics.FillRectangle(Brushes.DimGray, rect);
        }

        private void darkListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filename = darkTextBox1.Text;

            if (!String.IsNullOrWhiteSpace(filename))
            {
                string ext = ".png";

                if (darkListBox1.SelectedIndex == 1)
                {
                    ext = ".eps";
                }
                else if (darkListBox1.SelectedIndex == 2)
                {
                    ext = ".svg";
                }

                darkTextBox1.Text = Path.ChangeExtension(filename, ext);
            }
        }

        private void darkSlider1_ValueChanging(object sender, EventArgs e)
        {
            int size = (int)((2000.0 - 200.0) / 100.0 * darkSlider1.Value + 200.0);

            size = size - (size % 50);

            lbSize.Text = size + " px";
        }
    }
}
