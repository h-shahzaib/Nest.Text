using Nest.Text.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nest.Text
{
    public sealed class TextBuilder : ITextBuilder, IChainBuilder
    {
        private static readonly char[] s_NewLineChars = Environment.NewLine.ToCharArray();

        private readonly TextBuilderContext m_Context;

        public static ITextBuilder Create() => new TextBuilder();
        public static ITextBuilder Create(TextBuilderOptions options) => new TextBuilder(options);

        internal TextBuilder() => m_Context = new();
        internal TextBuilder(TextBuilderOptions options) => m_Context = new(options);
        internal TextBuilder(TextBuilderContext context) => m_Context = context;

        internal Token? ParentToken { get; private set; } = null;
        internal bool IsRootBuilder { get; private set; } = true;

        public TextBuilderOptions Options => m_Context.Options;

        public IChainBuilder L(string line = "")
        {
            line = line.Trim(s_NewLineChars);

            Token token;

            if (line.Contains(Environment.NewLine))
                token = new LinesToken(m_Context.Options, line);
            else
                token = new LineToken(m_Context.Options, line);

            m_Context.Tokens.Add(token);

            return GetChainBuilder(token, m_Context);
        }

        public IChainBuilder L(params string[] lines)
        {
            var lines_str = string.Join(Environment.NewLine, lines).Trim(s_NewLineChars);

            Token token;

            if (lines_str.Contains(Environment.NewLine))
                token = new LinesToken(m_Context.Options, lines_str);
            else
                token = new LineToken(m_Context.Options, lines_str);

            m_Context.Tokens.Add(token);

            return GetChainBuilder(token, m_Context);
        }

        public IChainBuilder B(Action<ITextBuilder> builder_act, Action<TextBuilderOptions>? options_act = null)
        {
            TextBuilderOptions? options;
            if (options_act != null)
            {
                options = new TextBuilderOptions(m_Context.Options);
                options_act.Invoke(options);
            }
            else
                options = m_Context.Options;

            var builder = new TextBuilder(options);
            builder_act.Invoke(builder);
            builder.IsRootBuilder = false;

            if (builder.m_Context.Tokens.Count > 0)
            {
                var token = new BlockToken(m_Context.Options, builder);
                m_Context.Tokens.Add(token);
                return GetChainBuilder(token, m_Context);
            }

            return this;
        }

        private TextBuilder GetChainBuilder(Token token, TextBuilderContext context)
        {
            if (ParentToken != null)
            {
                token.ParentToken = ParentToken;
                ParentToken = token;
                return this;
            }
            else
            {
                var chain_builder = new TextBuilder(context);
                chain_builder.ParentToken = token;
                chain_builder.IsRootBuilder = false;
                return chain_builder;
            }
        }

        public ITextBuilder Append(Action<ITextBuilder> builder_act)
        {
            builder_act.Invoke(this);
            return this;
        }

        public IChainBuilder Chain(Action<IChainBuilder> builder_act)
        {
            builder_act.Invoke(this);
            return this;
        }

        public override string ToString()
        {
            var output = new StringBuilder();
            BuildText(output, this, 0);
            return output.ToString();
        }

        private static void BuildText(StringBuilder output, TextBuilder text_builder, int indent_char_count)
        {
            var new_line = Environment.NewLine;

            for (int i = 0; i < text_builder.m_Context.Tokens.Count; i++)
            {
                var token = text_builder.m_Context.Tokens[i];

                var indent = new string(token.Options.IndentChar, indent_char_count);
                if (!text_builder.IsRootBuilder && token.Options.IndentSize > 0)
                    indent = new string(token.Options.IndentChar, indent_char_count + token.Options.IndentSize);

                if (i != 0)
                    output.AppendLine();

                if (token is LineToken line_token)
                {
                    output.Append(indent + ApplyReplacements(token.Options, line_token.Line));
                }
                else if (token is LinesToken lines_token)
                {
                    output.Append(string.Join(new_line, lines_token.Lines.Split([Environment.NewLine], StringSplitOptions.None).Select(i => indent + ApplyReplacements(token.Options, i))));
                }
                else if (token is BlockToken block_token)
                {
                    if (block_token.Options.BlockStyle == BlockStyle.CurlyBraces)
                        output.AppendLine(indent + '{');

                    BuildText(output, block_token.Builder, indent.Length);

                    if (block_token.Options.BlockStyle == BlockStyle.CurlyBraces)
                        output.Append(new_line + indent + '}');
                }

                if (ShouldAddLineBreak(text_builder.m_Context.Tokens, i, token))
                    output.AppendLine();
            }
        }

        private static bool ShouldAddLineBreak(List<Token> tokens, int index, Token first_token)
        {
            if (TokenIsEmpty(first_token))
                return false;

            var second_token = index + 1 < tokens.Count ? tokens[index + 1] : null;
            if (second_token is null)
                return false;

            if (TokenIsEmpty(second_token))
                return false;

            if (first_token is LinesToken && first_token.ParentToken is null)
                return true;

            if (second_token is LinesToken && second_token.ParentToken is null)
                return true;

            var third_token = index + 2 < tokens.Count ? tokens[index + 2] : null;
            if (second_token.ParentToken is null && third_token is not null && third_token.ParentToken is not null && ReferenceEquals(third_token.ParentToken, second_token))
                return true;

            if (first_token.ParentToken is not null && second_token.ParentToken is null)
                return true;

            return false;
        }

        private static bool TokenIsEmpty(Token token)
        {
            return token switch
            {
                LineToken line_token => string.IsNullOrWhiteSpace(line_token.Line),
                LinesToken lines_token => string.IsNullOrWhiteSpace(lines_token.Lines),
                _ => false
            };
        }

        private static string ApplyReplacements(TextBuilderOptions options, string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;

            var output = str;
            foreach (var replacement in options.GetCharReplacements())
                output = output.Replace(replacement.Key, replacement.Value);

            return output;
        }
    }
}
