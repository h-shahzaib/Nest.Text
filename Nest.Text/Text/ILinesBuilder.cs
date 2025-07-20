using System;
using System.Collections.Generic;
using System.Text;

namespace Nest.Text
{
    public interface ILinesBuilder
    {
        public IChainBuilder L(string line = "");
        public IChainBuilder L(params string[] lines);
    }
}
