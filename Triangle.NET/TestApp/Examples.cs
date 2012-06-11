// -----------------------------------------------------------------------
// <copyright file="Examples.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TriangleNet;
    using TriangleNet.IO;
    using TriangleNet.Geometry;
    using MeshExplorer.IO;

    /// <summary>
    /// Code of the online examples.
    /// </summary>
    public static class Examples
    {
        // Make sure this path points to the polygon sample data.
        static readonly string pathToData = @"..\..\..\Data\";

        static ImageWriter imageWriter = new ImageWriter();

        /// <summary>
        /// Generating Delaunay triangulations
        /// </summary>
        public static void Example1()
        {
            imageWriter.SetColorSchemeLight();

            // Create a mesh instance.
            Mesh mesh = new Mesh();

            // Read spiral node file and gernerate the delaunay triangulation 
            // of the point set.
            mesh.Triangulate(pathToData + "spiral.node");
            imageWriter.WritePng(mesh, "spiral.png", 180);

            // Read face polygon file and gernerate the delaunay triangulation 
            // of the PSLG. We reuse the mesh instance here.
            InputGeometry data = FileReader.Read(pathToData + "face.poly");
            mesh.Triangulate(data);
            imageWriter.WritePng(mesh, "face.png", 200);

            // Generate a conforming delaunay triangulation of the face polygon.
            mesh.SetOption(Options.ConformingDelaunay, true);
            mesh.Triangulate(data);
            imageWriter.WritePng(mesh, "face-CDT.png", 200);
        }

        /// <summary>
        /// Quality meshing: angle and size constraints
        /// </summary>
        public static void Example2()
        {
            imageWriter.SetColorSchemeLight();

            // Create a mesh instance.
            Mesh mesh = new Mesh();

            // Read spiral node file and gernerate the delaunay triangulation. 
            // Set the mesh quality option to true, which will set a default
            // minimum angle of 20 degrees.
            InputGeometry data = FileReader.ReadNodeFile(pathToData + "spiral.node");
            mesh.SetOption(Options.Quality, true);
            mesh.Triangulate(data);
            imageWriter.WritePng(mesh, "spiral-Angle-20.png", 200);

            // Set a minimum angle of 30 degrees. 
            mesh.SetOption(Options.MinAngle, 35);
            mesh.Triangulate(data);
            imageWriter.WritePng(mesh, "spiral-Angle-35.png", 200);

            // Reset the minimum angle and add a global area constraint.
            mesh.SetOption(Options.MinAngle, 20);
            mesh.SetOption(Options.MaxArea, 0.2);
            mesh.Triangulate(data);
            imageWriter.WritePng(mesh, "spiral-Area.png", 200);
        }

        /// <summary>
        /// Refining preexisting meshes
        /// </summary>
        public static void Example3()
        {
            imageWriter.SetColorSchemeLight();

            // Create a mesh instance.
            Mesh mesh = new Mesh();

            // Gernerate a quality delaunay triangulation of box
            // polygon, containing the convex hull.
            mesh.SetOption(Options.Quality, true);
            mesh.SetOption(Options.Convex, true);
            mesh.Triangulate(pathToData + "box.poly");
            imageWriter.WritePng(mesh, "box.png", 200);

            // Save the current mesh to .node and .ele files
            FileWriter.WriteNodes(mesh, "box.1.node");
            FileWriter.WriteElements(mesh, "box.1.ele");

            // Refine the mesh by setting a global area constraint.
            mesh.Refine(0.2);
            imageWriter.WritePng(mesh, "box-Refine-1.png", 200);

            // Refine again by setting a smaller area constraint.
            mesh.Refine(0.05);
            imageWriter.WritePng(mesh, "box-Refine-2.png", 200);

            // Load the previously saved box.1 mesh. Since a box.1.area
            // file exist, the variable area constraint option is set
            // and will be applied for refinement.
            mesh.Load(pathToData + "box.1.node");
            mesh.SetOption(Options.MinAngle, 0);
            mesh.Refine();
            imageWriter.WritePng(mesh, "box-Refine-3.png", 200);
        }

        /// <summary>
        /// Drawing the Voronoi diagram.
        /// </summary>
        public static void Example4()
        {
            //imageWriter.SetColorSchemeLight();

            //// Create mesh data (random point set)
            ////data.Points = Util.CreateCirclePoints(0, 0, 5, 50); // Ooops, TODO !!!
            //InputGeometry data = PolygonGenerator.CreateStarPoints(0, 0, 5, 10);

            //// Create a mesh instance.
            //Mesh mesh = new Mesh();

            //// Gernerate a delaunay triangulation
            //mesh.Triangulate(data);
            //ImageWriter.WritePng(mesh, "circle-mesh.png", 400);
            //ImageWriter.WriteVoronoiPng(mesh, "circle-voronoi.png", 400);
        }

        /// <summary>
        /// Smoothing a mesh.
        /// </summary>
        public static void Example5()
        {
            //ImageWriter.SetColorSchemeLight();

            //// Create a mesh instance.
            //Mesh mesh = new Mesh();

            //mesh.SetOption(Options.Quality, true);
            //mesh.SetOption(Options.MinAngle, 25);
            //mesh.SetOption(Options.MaxArea, 0.0075);
            //mesh.Triangulate(pathToData + "Smooth-Slit.poly");
            //mesh.Smooth();

            //ImageWriter.WritePng(mesh, "slit-smooth.png", 300);
        }

        /// <summary>
        /// Smoothing a mesh.
        /// </summary>
        public static void ExampleXYZ()
        {
            //ImageWriter.SetColorSchemeLight();

            //Mesh mesh = new Mesh();

            //mesh.SetOption(Options.Quality, true);
            //mesh.SetOption(Options.MinAngle, 25);
            //mesh.SetOption(Options.MaxArea, 0.05);

            //mesh.Triangulate(pathToData + "Smooth-Square.poly");

            //ImageWriter.WritePng(mesh, "test1.png", 300);

            //mesh.SetOption(Options.MaxArea, 0.01);

            //// Refine with new max area
            //mesh.Refine();

            //ImageWriter.WritePng(mesh, "test2.png", 300);

            //mesh.SetOption(Options.SteinerPoints, 50);
            //mesh.Triangulate(pathToData + "Smooth-Square.poly");

            //ImageWriter.WritePng(mesh, "test3.png", 300);
        }
    }
}
