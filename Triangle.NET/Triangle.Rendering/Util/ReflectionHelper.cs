
namespace TriangleNet.Rendering.Util
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    internal static class ReflectionHelper
    {
        public static bool TryCreateControl(string assemblyName, IEnumerable<string> dependencies,
            out IRenderControl control)
        {
            return TryCreateControl(assemblyName, dependencies, null, out control);
        }

        public static bool TryCreateControl(string assemblyName, IEnumerable<string> dependencies,
            string className, out IRenderControl control)
        {
            control = null;

            if (!FilesExist(assemblyName, dependencies))
            {
                return false;
            }

            assemblyName = Path.GetFileNameWithoutExtension(assemblyName);

            // Try create render control instance.
            try
            {
                // Load the assembly into the current application domain.
                var assembly = Assembly.Load(assemblyName);

                // Get all types implementing the IRenderControl interface.
                var type = typeof(IRenderControl);
                var matches = assembly.GetTypes().Where(s => type.IsAssignableFrom(s));

                var match = string.IsNullOrEmpty(className) ? matches.FirstOrDefault()
                    : matches.Where(s => s.Name == className).FirstOrDefault();

                if (match != null)
                {
                    // Create an instance.
                    control = (IRenderControl)Activator.CreateInstance(match);
                }
            }
            catch (Exception)
            {
                return false;
            }

            // Return true if render control was successfully created.
            return (control != null);
        }

        private static bool FilesExist(string assemblyName, IEnumerable<string> dependencies)
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

            return true;
        }
    }
}
