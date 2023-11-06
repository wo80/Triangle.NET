using System.Collections.Generic;
using System.Drawing;

namespace TriangleNet.Rendering.GDI
{
    internal class Helper
    {
        public static void Dispose(Dictionary<uint, SolidBrush> brushes)
        {
            foreach (var brush in brushes.Values)
            {
                brush.Dispose();
            }
        }

        public static Dictionary<uint, SolidBrush> GetBrushDictionary(Dictionary<uint, Color> ColorDictionary)
        {
            var brushes = new Dictionary<uint, SolidBrush>();

            foreach (var item in ColorDictionary)
            {
                brushes.Add(item.Key, new SolidBrush(item.Value));
            }

            return brushes;
        }
    }
}
