using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nest.Text.Tokens
{
    internal class LineToken(TextBuilderOptions options, string line = "") : Token(options)
    {
        public string Line { get; } = line;
    }
}
