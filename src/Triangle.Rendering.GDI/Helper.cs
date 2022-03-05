using System.Collections.Generic;
using System.Drawing;

namespace TriangleNet.Rendering.GDI
{
    internal class Helper
    {
        public static void Dispose(Dictionary<int, SolidBrush> brushes)
        {
            foreach (var brush in brushes.Values)
            {
                brush.Dispose();
            }
        }

        public static Dictionary<int, SolidBrush> GetBrushDictionary(Dictionary<int, Color> ColorDictionary)
        {
            var brushes = new Dictionary<int, SolidBrush>();

            foreach (var item in ColorDictionary)
            {
                brushes.Add(item.Key, new SolidBrush(item.Value));
            }

            return brushes;
        }
    }
}
