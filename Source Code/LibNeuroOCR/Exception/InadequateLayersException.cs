using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibNeuroOCR.Exception
{
    public class InadequateLayersException : NeuroException
    {
        public InadequateLayersException(string Message, System.Exception e) : base(Message, e)
        {
        }
    }
}
