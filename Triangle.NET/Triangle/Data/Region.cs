// -----------------------------------------------------------------------
// <copyright file="Region.cs" company="">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://home.edo.tu-dortmund.de/~woltering/triangle/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    struct Region
    {
        internal Point2 pt;
        internal double attribute;
        internal double area;

        public Region(double[] region)
        {
            pt = new Point2(region[0], region[1]);
            attribute = region[2];
            area = region[3];
        }
    }
}
