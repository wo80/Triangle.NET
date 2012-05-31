// -----------------------------------------------------------------------
// <copyright file="Settings.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Settings
    {
        // Open file dialog
        public string OfdDirectory { get; set; }
        public string OfdFilter { get; set; }
        public int OfdFilterIndex{ get; set; }

        // Save file dialog
        public string SfdDirectory { get; set; }
        public string SfdFilter { get; set; }
        public int SfdFilterIndex { get; set; }

        public string CurrentFile { get; set; }

        public bool RefineMode { get; set; }
        public bool ExceptionThrown { get; set; }

        public Settings()
        {
            if (Directory.Exists(@"..\..\..\Data\"))
            {
                OfdDirectory = Path.GetFullPath(@"..\..\..\Data\");
            }
            else if (Directory.Exists(@"Data\"))
            {
                OfdDirectory = Path.GetFullPath(@"Data\");
            }
            else
            {
                //System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
                OfdDirectory = Application.StartupPath;
            }

            OfdFilter = "Triangle file (*.node;*.poly)|*.node;*.poly";
            OfdFilter += "|Triangle.NET JSON (*.json)|*.json";
            OfdFilter += "|Polygon data (*.dat)|*.dat";
            //OfdFilter += "|COMSOL mesh (*.mphtxt)|*.mphtxt";
            //OfdFilter += "|AVS UCD data (*.ucd)|*.ucd";
            //OfdFilter += "|VTK data (*.vtk)|*.vtk";

            OfdFilterIndex = 0;

            SfdDirectory = OfdDirectory;
            SfdFilter = OfdFilter;
            SfdFilterIndex = 1;

            CurrentFile = "";

            RefineMode = false;
            ExceptionThrown = false;
        }
    }
}
