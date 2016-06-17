using LibNeuroOCR.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibNeuroOCR.Interface
{
    public interface INeuron
    {
        double BiasValue { get; set; }
        double DeltaValue { get; set; }
        NList ForwardConnections { get; }
        Dictionary<INeuron, double> Inputs { get; }
        double OutputValue { get; set; }
        INStrategy Strategy { get; set; }

        void UpdateDelta(double errorFactor);
        void UpdateFreeParams();
        void UpdateOutput();
    }
}
