using System;
using Ex = System.Exception;

namespace LibNeuroOCR.Exception
{
    public class  NullStrategyException : NeuroException
    {
        public NullStrategyException(string Message, Ex e) : base(Message, e)
        {
        }
    }
}
