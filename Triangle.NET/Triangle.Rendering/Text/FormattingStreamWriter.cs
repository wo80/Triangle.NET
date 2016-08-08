
namespace TriangleNet.Rendering.Text
{
    using System;
    using System.Globalization;
    using System.IO;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// From http://stackoverflow.com/questions/12011789/streamwriter-and-iformatprovider
    /// </remarks>
    public class FormattingStreamWriter : StreamWriter
    {
        private readonly IFormatProvider formatProvider;

        /// <summary>
        /// Initializes a new instance of the StreamWriter class for the specified file
        /// by using the default encoding and buffer size.
        /// </summary>
        /// <param name="path">The complete file path to write to.</param>
        public FormattingStreamWriter(string path)
            : this(path, CultureInfo.InvariantCulture)
        {
        }

        /// <summary>
        /// Initializes a new instance of the StreamWriter class for the specified stream
        /// by using UTF-8 encoding and the default buffer size.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        public FormattingStreamWriter(Stream stream)
            : this(stream, CultureInfo.InvariantCulture)
        {
        }

        /// <summary>
        /// Initializes a new instance of the StreamWriter class for the specified file
        /// by using the default encoding and buffer size.
        /// </summary>
        /// <param name="path">The complete file path to write to.</param>
        /// <param name="formatProvider">The format provider.</param>
        public FormattingStreamWriter(string path, IFormatProvider formatProvider)
            : base(path)
        {
            this.formatProvider = formatProvider;
        }

        /// <summary>
        /// Initializes a new instance of the StreamWriter class for the specified stream
        /// by using UTF-8 encoding and the default buffer size.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="formatProvider">The format provider.</param>
        public FormattingStreamWriter(Stream stream, IFormatProvider formatProvider)
            : base(stream)
        {
            this.formatProvider = formatProvider;
        }

        /// <summary>
        /// Gets an object that controls formatting.
        /// </summary>
        public override IFormatProvider FormatProvider
        {
            get
            {
                return this.formatProvider;
            }
        }
    }
}
