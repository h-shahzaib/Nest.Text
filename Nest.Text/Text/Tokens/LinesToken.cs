using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nest.Text.Tokens
{
    internal class LinesToken(TextBuilderOptions options, string lines) : Token(options)
    {
        public string Lines { get; } = lines;
    }
}
