
namespace TriangleNet.Rendering.Text
{
    using System.Collections.Generic;
    using TriangleNet.Geometry;

    static class EdgeIterator
    {
        /// <summary>
        /// Enumerate the edges of the mesh.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="skipSegments"></param>
        /// <returns></returns>
        /// <remarks>
        /// In contrast to the <see cref="TriangleNet.Meshing.Iterators.EdgeIterator"/> this
        /// method will return objects that include the vertex information (and not only the
        /// indices).
        /// </remarks>
        public static  IEnumerable<ISegment> EnumerateEdges(Mesh mesh, bool skipSegments = true)
        {
            foreach (var t in mesh.Triangles)
            {
                for (int i = 0; i < 3; i++)
                {
                    int nid = t.GetNeighborID(i);

                    if ((t.ID < nid) || (nid < 0))
                    {
                        var s = t.GetSegment(i);

                        if (skipSegments && s == null)
                        {
                            // Since segments will be processed separately, don't
                            // include them in the enumeration.
                            yield return new Segment(
                                t.GetVertex((i + 1) % 3),
                                t.GetVertex((i + 2) % 3));
                        }
                        else
                        {
                            if (s == null)
                            {
                                yield return new Segment(
                                    t.GetVertex((i + 1) % 3),
                                    t.GetVertex((i + 2) % 3));
                            }
                            else
                            {
                                yield return s;
                            }
                        }
                    }
                }
            }
        }
    }
}
