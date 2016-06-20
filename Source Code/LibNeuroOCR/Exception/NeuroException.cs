using System;
using Ex = System.Exception;

namespace LibNeuroOCR.Exception
{
    public class NeuroException : Ex
    {
        public NeuroException(string Message, Ex e) : base(Message)
        {
        }
    }
}
