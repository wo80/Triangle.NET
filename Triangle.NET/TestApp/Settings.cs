// -----------------------------------------------------------------------
// <copyright file="Settings.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
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
    /// Stores some of the data used in the main application.
    /// </summary>
    public class Settings
    {
        // String resources
        public static string ImportString = "The selected file has associated mesh information. " +
            "You can choose to import the mesh or just read the geometry.";

        public static string VoronoiString = "Make sure you use the \"Confoming Delaunay\" option " +
            "when building the Voronoi diagram from a constrained mesh.";

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

            SfdDirectory = OfdDirectory;
            SfdFilter = "Triangle file (*.node;*.poly)|*.node;*.poly";
            SfdFilter += "|Triangle.NET JSON (*.json)|*.json";
            SfdFilterIndex = 1;

            OfdFilter = SfdFilter;
            //OfdFilter += "|Polygon data (*.dat)|*.dat";
            //OfdFilter += "|COMSOL mesh (*.mphtxt)|*.mphtxt";
            //OfdFilter += "|AVS UCD data (*.ucd)|*.ucd";
            //OfdFilter += "|VTK data (*.vtk)|*.vtk";

            OfdFilterIndex = 0;

            CurrentFile = "";

            RefineMode = false;
            ExceptionThrown = false;
        }
    }
}
