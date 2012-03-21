// -----------------------------------------------------------------------
// <copyright file="MeshDataInternal.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace TestApp.Rendering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TriangleNet.IO;
    using System.Drawing;
    using TriangleNet;

    public class MeshDataInternal
    {
        public PointF[] Points;
        public int[][] Triangles;
        public int[][] Edges;
        public int[][] Segments;
        public int NumberOfInputPoints;
        public RectangleF Bounds;

        public void SetData(Mesh mesh)
        {
            NumberOfInputPoints = mesh.NumberOfInputPoints;

            SetData(mesh.GetMeshData(true, true, false), mesh.NumberOfInputPoints);
        }

        public void SetData(MeshData data)
        {
            SetData(data, data.Points.Length);
        }

        public void SetData(MeshData data, int inputCount)
        {
            NumberOfInputPoints = inputCount;

            int n = data.Points.Length;

            // Reset
            Triangles = null;
            Edges = null;
            Segments = null;

            // Copy points
            this.Points = new PointF[n];

            // Bounds
            float minx = float.MaxValue;
            float maxx = float.MinValue;
            float miny = float.MaxValue;
            float maxy = float.MinValue;

            float x, y;

            for (int i = 0; i < n; i += 1)
            {
                x = (float)data.Points[i][0];
                y = (float)data.Points[i][1];
                // Update bounding box
                if (minx > x) minx = x;
                if (maxx < x) maxx = x;
                if (miny > y) miny = y;
                if (maxy < y) maxy = y;

                this.Points[i] = new PointF(x, y);
            }

            this.Bounds = new RectangleF(minx, miny, maxx - minx, maxy - miny);

            n = data.Edges == null ? 0 : data.Edges.Length;

            // Copy edges
            if (data.Edges != null && n > 0)
            {
                Edges = new int[n][];

                for (int i = 0; i < n; i++)
                {
                    Edges[i] = new int[2];
                    Edges[i][0] = data.Edges[i][0];
                    Edges[i][1] = data.Edges[i][1];
                }
            }

            n = data.Segments == null ? 0 : data.Segments.Length;

            // Copy segments
            if (data.Segments != null && n > 0)
            {
                Segments = new int[n][];

                for (int i = 0; i < n; i++)
                {
                    Segments[i] = new int[2];
                    Segments[i][0] = data.Segments[i][0];
                    Segments[i][1] = data.Segments[i][1];
                }
            }

            n = data.Triangles == null ? 0 : data.Triangles.Length;

            // Copy triangles
            if (data.Triangles != null && n > 0)
            {
                Triangles = new int[n][];

                for (int i = 0; i < n; i++)
                {
                    Triangles[i] = new int[3];
                    Triangles[i][0] = data.Triangles[i][0];
                    Triangles[i][1] = data.Triangles[i][1];
                    Triangles[i][2] = data.Triangles[i][2];
                }
            }
        }
    }
}
