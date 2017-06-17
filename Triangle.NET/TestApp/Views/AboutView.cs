using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TriangleNet;
using System.Diagnostics;
using TriangleNet.Geometry;

namespace MeshExplorer.Views
{
    public partial class AboutView : UserControl, IView
    {
        public AboutView()
        {
            InitializeComponent();
        }

        private void lbCodeplex_Clicked(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo("http://triangle.codeplex.com/");
                Process.Start(info);
            }
            catch (Exception)
            { }
        }

        #region IView

        public void HandleNewInput(InputGeometry geometry)
        {
        }

        public void HandleMeshImport(InputGeometry geometry, Mesh mesh)
        {
        }

        public void HandleMeshUpdate(Mesh mesh)
        {
        }

        public void HandleMeshChange(Mesh mesh)
        {
        }

        #endregion
    }
}
