using System;
using System.Globalization;
using System.IO;

namespace MeshExplorer.IO
{
    // From http://stackoverflow.com/questions/12011789/streamwriter-and-iformatprovider

    public class FormattingStreamWriter : StreamWriter
    {
        private readonly IFormatProvider formatProvider;

        public FormattingStreamWriter(string path)
            : this(path, CultureInfo.InvariantCulture)
        {
        }

        public FormattingStreamWriter(string path, IFormatProvider formatProvider)
            : base(path)
        {
            this.formatProvider = formatProvider;
        }

        public override IFormatProvider FormatProvider
        {
            get
            {
                return this.formatProvider;
            }
        }
    }
}
