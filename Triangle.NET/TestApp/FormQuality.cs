using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TriangleNet.IO;
using MeshExplorer.Rendering;

namespace MeshExplorer
{
    public partial class FormQuality : Form
    {
        public FormQuality()
        {
            InitializeComponent();
        }

        public void UpdateQuality(RenderData data)
        {
            if (data != null)
            {
                //MeshQuality q = new MeshQuality();
                //string s = q.Update(data);

                //label1.Text = s;
            }
        }

        private void FormQuality_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
    }
}
