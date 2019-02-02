using System;
using System.Collections.Generic;
using System.Text;

namespace Parser
{
    public class ErrlogCpp : IErrlog
    {
        public ErrlogCpp(IList<ILine> lines)
        {
            Lines = lines;
        }

        public IList<ILine> Lines { get; private set; }

        public IList<ILine> FindRelevant()
        {
            List<ILine> lines = new List<ILine>();



            return lines;
        }
    }
}
