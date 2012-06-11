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
        static Dictionary<string, IMeshFile> container = new Dictionary<string, IMeshFile>();

        public static bool ContainsMeshData(string path)
        {
            IMeshFile provider = GetProviderInstance(path);

            return provider.ContainsMeshData(path);
        }

        public static InputGeometry Read(string path)
        {
            IMeshFile provider = GetProviderInstance(path);

            return provider.Read(path);
        }

        public static Mesh Import(string path)
        {
            IMeshFile provider = GetProviderInstance(path);

            return provider.Import(path);
        }

        public static void Save(string path, Mesh mesh)
        {
            IMeshFile provider = GetProviderInstance(path);

            provider.Write(mesh, path);
        }

        private static IMeshFile GetProviderInstance(string path)
        {
            string ext = Path.GetExtension(path);

            IMeshFile provider;

            if (container.ContainsKey(ext))
            {
                provider = container[ext];
            }
            else
            {
                provider = CreateProviderInstance(ext);
            }

            return provider;
        }

        private static IMeshFile CreateProviderInstance(string ext)
        {
            // TODO: automate by using IMeshFormat's Extensions property.

            IMeshFile provider = null;

            if (ext == ".node" || ext == ".poly" || ext == ".ele")
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

            if (provider == null)
            {
                throw new NotImplementedException("File format not implemented.");
            }

            container.Add(ext, provider);

            return provider;
        }
    }
}
