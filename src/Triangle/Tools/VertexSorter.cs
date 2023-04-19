﻿// -----------------------------------------------------------------------
// <copyright file="VertexSorter.cs" company="">
// Triangle Copyright (c) 1993, 1995, 1997, 1998, 2002, 2005 Jonathan Richard Shewchuk
// Triangle.NET code by Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet.Tools
{
    using System;
    using Geometry;

    /// <summary>
    /// Sort an array of points using quicksort.
    /// </summary>
    public class VertexSorter
    {
        private const int RANDOM_SEED = 57113;

        private Random rand;

        private Vertex[] points;

        private VertexSorter(Vertex[] points, int seed)
        {
            this.points = points;
            rand = new Random(seed);
        }

        /// <summary>
        /// Sorts the given vertex array by x-coordinate.
        /// </summary>
        /// <param name="array">The vertex array.</param>
        /// <param name="seed">Random seed used for pivoting.</param>
        public static void Sort(Vertex[] array, int seed = RANDOM_SEED)
        {
            var qs = new VertexSorter(array, seed);

            qs.QuickSort(0, array.Length - 1);
        }

        /// <summary>
        /// Impose alternating cuts on given vertex array.
        /// </summary>
        /// <param name="array">The vertex array.</param>
        /// <param name="length">The number of vertices to sort.</param>
        /// <param name="seed">Random seed used for pivoting.</param>
        public static void Alternate(Vertex[] array, int length, int seed = RANDOM_SEED)
        {
            var qs = new VertexSorter(array, seed);

            var divider = length >> 1;

            // Re-sort the array of vertices to accommodate alternating cuts.
            if (length - divider >= 2)
            {
                if (divider >= 2)
                {
                    qs.AlternateAxes(0, divider - 1, 1);
                }

                qs.AlternateAxes(divider, length - 1, 1);
            }
        }

        #region Quicksort

        /// <summary>
        /// Sort an array of vertices by x-coordinate, using the y-coordinate as a secondary key.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <remarks>
        /// Uses quicksort. Randomized O(n log n) time. No, I did not make any of
        /// the usual quicksort mistakes.
        /// </remarks>
        private void QuickSort(int left, int right)
        {
            var oleft = left;
            var oright = right;
            var arraysize = right - left + 1;
            int pivot;
            double pivotx, pivoty;
            Vertex temp;

            var array = points;

            if (arraysize < 32)
            {
                // Insertion sort
                for (var i = left + 1; i <= right; i++)
                {
                    var a = array[i];
                    var j = i - 1;
                    while (j >= left && (array[j].x > a.x || (array[j].x == a.x && array[j].y > a.y)))
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
            pivotx = array[pivot].x;
            pivoty = array[pivot].y;
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
                while ((left <= right) && ((array[left].x < pivotx) ||
                    ((array[left].x == pivotx) && (array[left].y < pivoty))));

                // Search for a vertex whose x-coordinate is too small for the right.
                do
                {
                    right--;
                }
                while ((left <= right) && ((array[right].x > pivotx) ||
                    ((array[right].x == pivotx) && (array[right].y > pivoty))));

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
                QuickSort(oleft, left);
            }

            if (oright > right + 1)
            {
                // Recursively sort the right subset.
                QuickSort(right + 1, oright);
            }
        }

        #endregion

        #region Alternate axes

        /// <summary>
        /// Sorts the vertices as appropriate for the divide-and-conquer algorithm with 
        /// alternating cuts.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="axis"></param>
        /// <remarks>
        /// Partitions by x-coordinate if axis == 0; by y-coordinate if axis == 1.
        /// For the base case, subsets containing only two or three vertices are
        /// always sorted by x-coordinate.
        /// </remarks>
        private void AlternateAxes(int left, int right, int axis)
        {
            var size = right - left + 1;
            var divider = size >> 1;

            if (size <= 3)
            {
                // Recursive base case:  subsets of two or three vertices will be
                // handled specially, and should always be sorted by x-coordinate.
                axis = 0;
            }

            // Partition with a horizontal or vertical cut.
            if (axis == 0)
            {
                VertexMedianX(left, right, left + divider);
            }
            else
            {
                VertexMedianY(left, right, left + divider);
            }

            // Recursively partition the subsets with a cross cut.
            if (size - divider >= 2)
            {
                if (divider >= 2)
                {
                    AlternateAxes(left, left + divider - 1, 1 - axis);
                }

                AlternateAxes(left + divider, right, 1 - axis);
            }
        }

        /// <summary>
        /// An order statistic algorithm, almost. Shuffles an array of vertices so that the
        /// first 'median' vertices occur lexicographically before the remaining vertices.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="median"></param>
        /// <remarks>
        /// Uses the x-coordinate as the primary key. Very similar to the QuickSort()
        /// procedure, but runs in randomized linear time.
        /// </remarks>
        private void VertexMedianX(int left, int right, int median)
        {
            var arraysize = right - left + 1;
            int oleft = left, oright = right;
            int pivot;
            double px, py; // pivot x and y coordinatex
            Vertex temp;

            var array = points;

            if (arraysize == 2)
            {
                // Recursive base case.
                if ((array[left].x > array[right].x) ||
                    ((array[left].x == array[right].x) &&
                     (array[left].y > array[right].y)))
                {
                    temp = array[right];
                    array[right] = array[left];
                    array[left] = temp;
                }
                return;
            }

            // Choose a random pivot to split the array.
            pivot = rand.Next(left, right);
            px = array[pivot].x;
            py = array[pivot].y;

            left--;
            right++;
            while (left < right)
            {
                // Search for a vertex whose x-coordinate is too large for the left.
                do
                {
                    left++;
                }
                while ((left <= right) && ((array[left].x < px) ||
                    ((array[left].x == px) && (array[left].y < py))));

                // Search for a vertex whose x-coordinate is too small for the right.
                do
                {
                    right--;
                }
                while ((left <= right) && ((array[right].x > px) ||
                    ((array[right].x == px) && (array[right].y > py))));

                if (left < right)
                {
                    // Swap the left and right vertices.
                    temp = array[left];
                    array[left] = array[right];
                    array[right] = temp;
                }
            }

            // Unlike in QuickSort(), at most one of the following conditionals is true.
            if (left > median)
            {
                // Recursively shuffle the left subset.
                VertexMedianX(oleft, left - 1, median);
            }

            if (right < median - 1)
            {
                // Recursively shuffle the right subset.
                VertexMedianX(right + 1, oright, median);
            }
        }

        /// <summary>
        /// An order statistic algorithm, almost.  Shuffles an array of vertices so that 
        /// the first 'median' vertices occur lexicographically before the remaining vertices.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="median"></param>
        /// <remarks>
        /// Uses the y-coordinate as the primary key. Very similar to the QuickSort()
        /// procedure, but runs in randomized linear time.
        /// </remarks>
        private void VertexMedianY(int left, int right, int median)
        {
            var arraysize = right - left + 1;
            int oleft = left, oright = right;
            int pivot;
            double px, py; // pivot x and y coordinatex
            Vertex temp;

            var array = points;

            if (arraysize == 2)
            {
                // Recursive base case.
                if ((array[left].y > array[right].y) ||
                    ((array[left].y == array[right].y) &&
                     (array[left].x > array[right].x)))
                {
                    temp = array[right];
                    array[right] = array[left];
                    array[left] = temp;
                }
                return;
            }

            // Choose a random pivot to split the array.
            pivot = rand.Next(left, right);
            px = array[pivot].y;
            py = array[pivot].x;

            left--;
            right++;
            while (left < right)
            {
                // Search for a vertex whose x-coordinate is too large for the left.
                do
                {
                    left++;
                }
                while ((left <= right) && ((array[left].y < px) ||
                    ((array[left].y == px) && (array[left].x < py))));

                // Search for a vertex whose x-coordinate is too small for the right.
                do
                {
                    right--;
                }
                while ((left <= right) && ((array[right].y > px) ||
                    ((array[right].y == px) && (array[right].x > py))));

                if (left < right)
                {
                    // Swap the left and right vertices.
                    temp = array[left];
                    array[left] = array[right];
                    array[right] = temp;
                }
            }

            // Unlike in QuickSort(), at most one of the following conditionals is true.
            if (left > median)
            {
                // Recursively shuffle the left subset.
                VertexMedianY(oleft, left - 1, median);
            }

            if (right < median - 1)
            {
                // Recursively shuffle the right subset.
                VertexMedianY(right + 1, oright, median);
            }
        }

        #endregion
    }
}
