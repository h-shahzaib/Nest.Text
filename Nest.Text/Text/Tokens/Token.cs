using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nest.Text.Tokens
{
    internal abstract class Token(TextBuilderOptions options)
    {
        public Token? ParentToken { get; set; }
        public TextBuilderOptions Options { get; } = new(options);
    }
}
