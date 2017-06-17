// -----------------------------------------------------------------------
// <copyright file="Util.cs" company="">
// Christian Woltering, Triangle.NET, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.IO;

    /// <summary>
    /// Utility class.
    /// </summary>
    public static class Util
    {
        internal static NumberFormatInfo Nfi = CultureInfo.InvariantCulture.NumberFormat;

        internal static Random Random = new Random(DateTime.Now.Millisecond);

        internal static bool TryReadLine(StreamReader reader, out string[] token)
        {
            token = null;

            if (reader.EndOfStream)
            {
                return false;
            }

            string line = reader.ReadLine().Trim();

            while (String.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
            {
                if (reader.EndOfStream)
                {
                    return false;
                }

                line = reader.ReadLine().Trim();
            }

            token = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            return true;
        }

        internal static string DoubleToString(double d)
        {
            double max = 999999;
            double min = 0.00001;

            string spec = "0.00000";

            if (d < min || d > max)
            {
                spec = "0.###e-000";
            }

            return d.ToString(spec, Util.Nfi);
        }

        internal static string AngleToString(double d)
        {
            double max = 180 - 10E-14;
            double min = 10E-14;

            string spec = "0.00000";

            if (d < min || d > max)
            {
                spec = "0.#";
            }

            return d.ToString(spec, Util.Nfi);
        }
    }
}
