using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

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
                int size = (int)((2000.0 - 200.0) / 100.0 * darkSlider1.Value + 200.0);
                return size - (size % 50);
            }
        }

        public string ImageName
        {
            get { return darkTextBox1.Text; }
            set { darkTextBox1.Text = value; }
        }

        public bool UseCompression
        {
            get { return cbUseCompression.Enabled && cbUseCompression.Checked; }
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

                cbUseCompression.Enabled = darkListBox1.SelectedIndex > 0;

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
