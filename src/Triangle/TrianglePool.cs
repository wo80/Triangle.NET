// -----------------------------------------------------------------------
// <copyright file="TrianglePool.cs" company="">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet
{
    using System;
    using System.Collections.Generic;
    using Topology;

    /// <summary>
    /// Pool datastructure storing triangles of a <see cref="Mesh" />.
    /// </summary>
    public class TrianglePool : ICollection<Triangle>
    {
        // Determines the size of each block in the pool.
        private const int BLOCKSIZE = 1024;

        // The total number of currently allocated triangles.

        // The number of triangles currently used.
        private int count;

        // The pool.
        private Triangle[][] pool;

        // A stack of free triangles.
        private Stack<Triangle> stack;

        /// <summary>
        /// Gets the total number of currently allocated triangles.
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrianglePool" /> class.
        /// </summary>
        public TrianglePool()
        {
            Capacity = 0;

            // On startup, the pool should be able to hold 2^16 triangles.
            var n = Math.Max(1, 65536 / BLOCKSIZE);

            pool = new Triangle[n][];
            pool[0] = new Triangle[BLOCKSIZE];

            stack = new Stack<Triangle>(BLOCKSIZE);
        }

        /// <summary>
        /// Gets a triangle from the pool.
        /// </summary>
        /// <returns></returns>
        public Triangle Get()
        {
            Triangle triangle;

            if (stack.Count > 0)
            {
                triangle = stack.Pop();
                triangle.hash = -triangle.hash - 1;

                Cleanup(triangle);
            }
            else if (count < Capacity)
            {
                triangle = pool[count / BLOCKSIZE][count % BLOCKSIZE];
                triangle.id = triangle.hash;

                Cleanup(triangle);

                count++;
            }
            else
            {
                triangle = new Triangle();
                triangle.hash = Capacity;
                triangle.id = triangle.hash;

                var block = Capacity / BLOCKSIZE;

                if (pool[block] == null)
                {
                    pool[block] = new Triangle[BLOCKSIZE];

                    // Check if the pool has to be resized.
                    if (block + 1 == pool.Length)
                    {
                        Array.Resize(ref pool, 2 * pool.Length);
                    }
                }

                // Add triangle to pool.
                pool[block][Capacity % BLOCKSIZE] = triangle;

                count = ++Capacity;
            }

            return triangle;
        }

        /// <summary>
        /// Release triangle (making it a free triangle).
        /// </summary>
        public void Release(Triangle triangle)
        {
            stack.Push(triangle);

            // Mark the triangle as free (used by enumerator).
            triangle.hash = -triangle.hash - 1;
        }

        /// <summary>
        /// Restart the triangle pool.
        /// </summary>
        public TrianglePool Restart()
        {
            foreach (var triangle in stack)
            {
                // Reset hash to original value.
                triangle.hash = -triangle.hash - 1;
            }

            stack.Clear();

            count = 0;

            return this;
        }

        /// <summary>
        /// Samples a number of triangles from the pool.
        /// </summary>
        /// <param name="k">The number of triangles to sample.</param>
        /// <param name="random"></param>
        /// <returns></returns>
        internal IEnumerable<Triangle> Sample(int k, Random random)
        {
            int i, count = Count;

            if (k > count)
            {
                // TODO: handle Sample special case.
                k = count;
            }

            Triangle t;

            // TODO: improve sampling code (to ensure no duplicates).

            while (k > 0)
            {
                i = random.Next(0, count);

                t = pool[i / BLOCKSIZE][i % BLOCKSIZE];

                if (t.hash >= 0)
                {
                    k--;
                    yield return t;
                }
            }
        }

        private void Cleanup(Triangle triangle)
        {
            triangle.label = 0;
            triangle.area = 0.0;
            triangle.infected = false;

            for (var i = 0; i < 3; i++)
            {
                triangle.vertices[i] = null;

                triangle.subsegs[i] = default;
                triangle.neighbors[i] = default;
            }
        }

        /// <summary>
        /// Not supported for this <see cref="ICollection{Triangle}" />.
        /// </summary>
        public void Add(Triangle item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Clear the pool.
        /// </summary>
        public void Clear()
        {
            stack.Clear();

            var blocks = (Capacity / BLOCKSIZE) + 1;

            for (var i = 0; i < blocks; i++)
            {
                var block = pool[i];

                // Number of triangles in current block:
                var length = (Capacity - i * BLOCKSIZE) % BLOCKSIZE;

                for (var j = 0; j < length; j++)
                {
                    block[j] = null;
                }
            }

            Capacity = count = 0;
        }

        /// <inheritdoc />
        public bool Contains(Triangle item)
        {
            var i = item.hash;

            if (i < 0 || i > Capacity)
            {
                return false;
            }

            return pool[i / BLOCKSIZE][i % BLOCKSIZE].hash >= 0;
        }

        /// <inheritdoc />
        public void CopyTo(Triangle[] array, int index)
        {
            using var enumerator = GetEnumerator();

            while (enumerator.MoveNext())
            {
                array[index] = enumerator.Current;
                index++;
            }
        }

        /// <inheritdoc />
        public int Count => count - stack.Count;

        /// <inheritdoc />
        public bool IsReadOnly => true;

        /// <inheritdoc />
        public bool Remove(Triangle item)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IEnumerator<Triangle> GetEnumerator()
        {
            return new Enumerator(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class Enumerator : IEnumerator<Triangle>
        {
            // TODO: enumerator should be able to tell if collection changed.

            private int count;

            private Triangle[][] pool;

            private int index, offset;

            public Enumerator(TrianglePool pool)
            {
                count = pool.Count;
                this.pool = pool.pool;

                index = 0;
                offset = 0;
            }

            public Triangle Current { get; private set; }

            public void Dispose()
            {
            }

            object System.Collections.IEnumerator.Current => Current;

            public bool MoveNext()
            {
                while (index < count)
                {
                    Current = pool[offset / BLOCKSIZE][offset % BLOCKSIZE];

                    offset++;

                    if (Current.hash >= 0)
                    {
                        index++;
                        return true;
                    }
                }

                return false;
            }

            public void Reset()
            {
                index = offset = 0;
            }
        }
    }
}
