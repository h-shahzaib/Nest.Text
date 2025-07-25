using System;
using System.Collections.Generic;
using System.Text;

namespace Nest.Text
{
    public interface IBlockBuilder
    {
        public IChainBuilder B(Action<ITextBuilder> builder_act, Action<TextBuilderOptions>? options_act = null);
    }
}
