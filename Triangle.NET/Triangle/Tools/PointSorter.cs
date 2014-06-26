// -----------------------------------------------------------------------
// <copyright file="PointSorter.cs" company="">
// Original Triangle code by Jonathan Richard Shewchuk, http://www.cs.cmu.edu/~quake/triangle.html
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Tools
{
    using System;
    using TriangleNet.Geometry;

    /// <summary>
    /// Sort an array of points using quicksort.
    /// </summary>
    public class PointSorter
    {
        static Random rand = new Random(57113);

        Point[] points;

        public void Sort(Point[] array)
        {
            this.points = array;

            VertexSort(0, array.Length - 1);
        }

        /// <summary>
        /// Sort an array of vertices by x-coordinate, using the y-coordinate as a secondary key.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <remarks>
        /// Uses quicksort. Randomized O(n log n) time. No, I did not make any of
        /// the usual quicksort mistakes.
        /// </remarks>
        void VertexSort(int left, int right)
        {
            int oleft = left;
            int oright = right;
            int arraysize = right - left + 1;
            int pivot;
            double pivotx, pivoty;
            Point temp;

            var array = this.points;

            if (arraysize < 32)
            {
                // Insertion sort
                for (int i = left + 1; i <= right; i++)
                {
                    var a = array[i];
                    int j = i - 1;
                    while (j >= left && (array[j].X > a.X || (array[j].X == a.X && array[j].Y > a.Y)))
                    {
                        array[j + 1] = array[j];
                        j--;
                    }
                    array[j + 1] = a;
                }

                return;
            }

            // Choose a random pivot to split the array.
            pivot = rand.Next(left, right);
            pivotx = array[pivot].X;
            pivoty = array[pivot].Y;
            // Split the array.
            left--;
            right++;
            while (left < right)
            {
                // Search for a vertex whose x-coordinate is too large for the left.
                do
                {
                    left++;
                }
                while ((left <= right) && ((array[left].X < pivotx) ||
                    ((array[left].X == pivotx) && (array[left].Y < pivoty))));
                // Search for a vertex whose x-coordinate is too small for the right.
                do
                {
                    right--;
                }
                while ((left <= right) && ((array[right].X > pivotx) ||
                    ((array[right].X == pivotx) && (array[right].Y > pivoty))));

                if (left < right)
                {
                    // Swap the left and right vertices.
                    temp = array[left];
                    array[left] = array[right];
                    array[right] = temp;
                }
            }
            if (left > oleft)
            {
                // Recursively sort the left subset.
                VertexSort(oleft, left);
            }
            if (oright > right + 1)
            {
                // Recursively sort the right subset.
                VertexSort(right + 1, oright);
            }
        }
    }
}
