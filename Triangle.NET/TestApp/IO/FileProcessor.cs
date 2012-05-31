// -----------------------------------------------------------------------
// <copyright file="FileProcessor.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using MeshExplorer.IO.Formats;
    using TriangleNet.IO;
    using TriangleNet.Geometry;
    using TriangleNet;

    /// <summary>
    /// Provides static methods to read and write mesh files.
    /// </summary>
    public static class FileProcessor
    {
        static Dictionary<string, IMeshFormat> container = new Dictionary<string,IMeshFormat>();

        public static InputGeometry Open(string path)
        {
            string ext = Path.GetExtension(path);

            IMeshFormat provider;

            if (container.ContainsKey(ext))
            {
                provider = container[ext];
            }
            else
            {
                provider = CreateInstance(ext);
            }

            return provider.Read(path);
        }

        public static void Save(string path, Mesh data)
        {
            string ext = Path.GetExtension(path);

            IMeshFormat provider;

            if (container.ContainsKey(ext))
            {
                provider = container[ext];
            }
            else
            {
                provider = CreateInstance(ext);
            }

            provider.Write(path, data);
        }

        private static IMeshFormat CreateInstance(string ext)
        {
            // TODO: automate by using IMeshFormat's Extensions property.

            IMeshFormat provider = null;

            if (ext == ".node" || ext == ".poly")
            {
                provider = new TriangleFile();
            }
            else if (ext == ".json")
            {
                provider = new JsonFile();
            }
            else if (ext == ".dat")
            {
                provider = new DatFile();
            }
            else if (ext == ".mphtxt")
            {
                //provider = new COMSOL();
            }
            else if (ext == ".vtk")
            {
                //provider = new VtkFile();
            }

            if (provider == null)
            {
                throw new NotImplementedException("File format not implemented.");
            }

            container.Add(ext, provider);

            return provider;
        }
    }
}
