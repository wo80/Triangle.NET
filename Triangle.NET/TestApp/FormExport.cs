using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MeshExplorer
{
    public partial class FormExport : Form
    {
        public FormExport()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var rect = this.ClientRectangle;
            rect.Height -= 40;

            e.Graphics.FillRectangle(Brushes.DimGray, rect);
        }
    }
}
