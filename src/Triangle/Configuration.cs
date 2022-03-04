// -----------------------------------------------------------------------
// <copyright file="Configuration.cs" company="">
// Triangle.NET Copyright (c) 2012-2022 Christian Woltering
// </copyright>
// -----------------------------------------------------------------------

namespace TriangleNet
{
    using System;

    /// <summary>
    /// Configure advanced aspects of the library.
    /// </summary>
    public class Configuration
    {
        public Configuration()
            : this(() => RobustPredicates.Default, () => new TrianglePool())
        {
        }

        public Configuration(Func<IPredicates> predicates)
            : this(predicates, () => new TrianglePool())
        {
        }

        public Configuration(Func<IPredicates> predicates, Func<TrianglePool> trianglePool)
        {
            Predicates = predicates;
            TrianglePool = trianglePool;
        }

        /// <summary>
        /// Gets or sets the factory method for the <see cref="IPredicates"/> implementation.
        /// </summary>
        public Func<IPredicates> Predicates { get; set; }

        /// <summary>
        /// Gets or sets the factory method for the <see cref="TrianglePool"/>.
        /// </summary>
        public Func<TrianglePool> TrianglePool { get; set; }
    }
}
