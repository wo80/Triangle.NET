// -----------------------------------------------------------------------
// <copyright file="RenderManager.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MeshRenderer.Core
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    /// <summary>
    /// This is a proxy to an actual IMeshRenderer instance.
    /// </summary>
    public class RenderManager : IMeshRenderer
    {
        IMeshRenderer renderer;

        public Control RenderControl
        {
            get { return (Control)renderer; }
            set
            {
                if (value is IMeshRenderer)
                {
                    renderer = (IMeshRenderer)value;
                }
            }
        }

        public bool ShowVoronoi
        {
            get { return renderer.ShowVoronoi; }
            set { renderer.ShowVoronoi = value; }
        }

        public bool ShowRegions
        {
            get { return renderer.ShowRegions; }
            set { renderer.ShowRegions = value; }
        }

        public void Initialize()
        {
            renderer.Initialize();
        }

        public void Zoom(float x, float y, int delta)
        {
            renderer.Zoom(x, y, delta);
        }

        public void HandleResize()
        {
            renderer.HandleResize();
        }

        public void SetData(RenderData data)
        {
            renderer.SetData(data);
        }

        public void CreateDefaultControl()
        {
            this.RenderControl = new MeshRenderer.Core.GDI.RenderControl();
        }

        public bool CreateControl(string assemblyName)
        {
            return CreateControl(assemblyName, null);
        }

        public bool CreateControl(string assemblyName, string[] dependencies)
        {
            // Check if assembly exists
            if (!File.Exists(assemblyName))
            {
                return false;
            }

            // Check if dependencies exists
            if (dependencies != null)
            {
                foreach (var item in dependencies)
                {
                    if (!File.Exists(item))
                    {
                        return false;
                    }
                }
            }

            assemblyName = Path.GetFileNameWithoutExtension(assemblyName);

            // Try creating renderer instance.
            try
            {
                // Load the assembly into the current application domain.
                Assembly assembly = Assembly.Load(assemblyName);

                // Get all types implementing the IMeshRenderer interface.
                var type = typeof(IMeshRenderer);
                var types = assembly.GetTypes().Where(s => type.IsAssignableFrom(s)).ToArray();

                if (types.Length > 0)
                {
                    // Create an instance.
                    renderer = (IMeshRenderer)Activator.CreateInstance(types[0]);
                }

            }
            catch (Exception)
            {
                return false;
            }

            // Return true if render control was successfully created.
            return (renderer != null);
        }
    }
}
