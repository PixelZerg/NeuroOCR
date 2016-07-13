using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibNeuroOCR
{
    public static class Utils
    {
        public static Random r = new Random();
        public static double Rand()
        {
            return r.Next(-1, 0);
        }
    }
}
