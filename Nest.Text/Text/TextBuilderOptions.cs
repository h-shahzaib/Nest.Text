using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Nest.Text
{
    /// <summary>
    /// Represents configuration options for a <see cref="TextBuilder"/>.
    /// </summary>
    public class TextBuilderOptions
    {
        private readonly Dictionary<char, char> m_CharReplacements;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBuilderOptions"/> class with default values.
        /// </summary>
        public TextBuilderOptions()
        {
            m_CharReplacements = [];
            BlockStyle = BlockStyle.IndentOnly;
            IndentChar = ' ';
            IndentSize = 4;
            LineBreak = Environment.NewLine;
            AddImplicitLineBreaks = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBuilderOptions"/> class by copying values from another instance.
        /// </summary>
        /// <param name="options">The options to copy from.</param>
        public TextBuilderOptions(TextBuilderOptions options)
        {
            m_CharReplacements = new Dictionary<char, char>(options.m_CharReplacements);
            BlockStyle = options.BlockStyle;
            IndentChar = options.IndentChar;
            IndentSize = options.IndentSize;
            LineBreak = options.LineBreak;
            AddImplicitLineBreaks = options.AddImplicitLineBreaks;
        }

        /// <summary>
        /// Gets or sets the style used for opening & closing the text blocks.
        /// </summary>
        public BlockStyle BlockStyle { get; set; }

        /// <summary>
        /// Gets or sets the character to be used for indentation
        /// </summary>
        public char IndentChar { get; set; }

        /// <summary>
        /// Gets or sets the number of <see cref="IndentChar"/> used per indentation level.
        /// </summary>
        public int IndentSize { get; set; }

        /// <summary>
        /// Characters to be used as a line break
        /// </summary>
        public string LineBreak { get; set; }
        internal char[] LineBreakChars => LineBreak.ToCharArray();

        /// <summary>
        /// Controls whether the library will add implicit line breaks where appropriate
        /// </summary>
        public bool AddImplicitLineBreaks { get; set; }

        /// <summary>
        /// Registers a character replacement to use during text generation.
        /// </summary>
        /// <param name="original_char">The character to be replaced.</param>
        /// <param name="replace_with">The character to replace with.</param>
        public void RegisterCharReplacement(char original_char, char replace_with)
        {
            if (original_char == ' ')
                throw new NotSupportedException("Replacing the space character (' ') is not supported.");
            else if (LineBreak.Contains(original_char))
                throw new NotSupportedException(
                    $"Replacing line break characters is not supported. Character: '\\u{(int)original_char:X4}'"
                );

            m_CharReplacements[original_char] = replace_with;
        }

        /// <summary>
        /// Removes a previously registered character replacement.
        /// </summary>
        /// <param name="original_char">The original character whose replacement should be removed.</param>
        public void RemoveCharReplacement(char original_char)
        {
            m_CharReplacements.Remove(original_char);
        }

        /// <summary>
        /// Gets a read-only view of the current character replacements.
        /// </summary>
        /// <returns>A read-only dictionary of character replacements.</returns>
        public IReadOnlyDictionary<char, char> GetCharReplacements()
        {
            return new ReadOnlyDictionary<char, char>(m_CharReplacements);
        }
    }
}
