using System;
using System.Collections.Generic;
using System.Text;

namespace Nest.Text.Tokens
{
    internal class BlockToken(TextBuilderOptions options, TextBuilder builder) : Token(options)
    {
        public TextBuilder Builder { get; } = builder;
    }
}
