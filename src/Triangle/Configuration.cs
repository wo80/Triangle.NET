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
        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration" /> class.
        /// </summary>
        public Configuration()
            : this(() => RobustPredicates.Default, () => new TrianglePool())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration" /> class.
        /// </summary>
        /// <param name="predicates">Factory method for <see cref="IPredicates" />.</param>
        public Configuration(Func<IPredicates> predicates)
            : this(predicates, () => new TrianglePool())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration" /> class.
        /// </summary>
        /// <param name="predicates">Factory method for <see cref="IPredicates" />.</param>
        /// <param name="trianglePool">Factory method for <see cref="TrianglePool" />.</param>
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
