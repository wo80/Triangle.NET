// -----------------------------------------------------------------------
// <copyright file="BaseGenerator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MeshExplorer.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using TriangleNet.Geometry;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class BaseGenerator : IGenerator
    {
        private static int MAX_PARAMS = 3;

        protected string name = "Name";
        protected string description = "Description";
        protected int parameter = 0;

        protected string[] descriptions = new string[MAX_PARAMS];
        protected int[][] ranges = new int[MAX_PARAMS][];

        public virtual string Name { get { return name; } }
        public virtual string Description { get { return description; } }
        public virtual int ParameterCount { get { return parameter; } }

        public virtual string ParameterDescription(int paramIndex)
        {
            if (descriptions[paramIndex] == null)
            {
                return String.Empty;
            }

            return descriptions[paramIndex];
        }

        public virtual string ParameterDescription(int paramIndex, double paramValue)
        {
            int[] range = ranges[paramIndex];

            if (range == null)
            {
                return String.Empty;
            }

            int num = GetParamValueInt(paramIndex, paramValue);
            return num.ToString();
        }

        public abstract InputGeometry Generate(double param0, double param1, double param2);

        protected int GetParamValueInt(int paramIndex, double paramOffset)
        {
            int[] range = ranges[paramIndex];

            if (range == null)
            {
                return 0;
            }

            return (int)((range[1] - range[0]) / 100.0 * paramOffset + range[0]);
        }

        protected double GetParamValueDouble(int paramIndex, double paramOffset)
        {
            int[] range = ranges[paramIndex];

            if (range == null)
            {
                return 0;
            }

            return ((range[1] - range[0]) / 100.0 * paramOffset + range[0]);
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
