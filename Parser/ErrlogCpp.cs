using System;
using System.Collections.Generic;
using System.Text;

namespace Parser
{
    public class ErrlogCpp : IParsedFile
    {
        public ErrlogCpp(IList<ILine> lines)
        {
            Lines = lines;
        }

        public IList<ILine> Lines { get; private set; }
    }
}
