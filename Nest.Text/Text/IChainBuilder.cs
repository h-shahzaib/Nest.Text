using System;
using System.Collections.Generic;
using System.Text;

namespace Nest.Text
{
    public interface IChainBuilder : IBlockBuilder, ILinesBuilder
    {
        public IChainBuilder Chain(Action<IChainBuilder> builder_act);
        public TextBuilderOptions Options { get; }
    }
}
