using Nest.Text.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nest.Text
{
    internal class TextBuilderContext
    {
        public TextBuilderContext()
        {
            Tokens = [];
            Options = new();
        }

        public TextBuilderContext(TextBuilderOptions options)
        {
            Tokens = [];
            Options = new(options);
        }

        public TextBuilderContext(List<Token> tokens)
        {
            Tokens = tokens;
            Options = new();
        }

        public TextBuilderContext(List<Token> tokens, TextBuilderOptions options)
        {
            Tokens = tokens;
            Options = new(options);
        }

        public List<Token> Tokens { get; set; }
        public TextBuilderOptions Options { get; set; }
    }
}
