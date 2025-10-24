using System;
using System.Collections.Generic;
using System.Text;

namespace Nest.Text
{
    public interface ITextBuilder : ILinesBuilder
    {
        public ITextBuilder Append(Action<ITextBuilder> builder_act);
        public TextBuilderOptions Options { get; }
        public IChainBuilder GetChainBuilder();
    }
}
