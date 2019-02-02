using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Parser
{
    public interface IErrlog
    {
        IList<ILine> Lines { get; }
    }
}
