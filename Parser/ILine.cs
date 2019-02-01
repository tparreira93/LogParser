using System;
using System.Collections.Generic;
using System.Text;

namespace Parser
{
    public interface ILine
    {
        DateTime Date { get; }
        string Text { get; }
    }
}
