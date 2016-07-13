using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibNeuroOCR.Interface
{
    public interface INStrategy
    {
        double Activation(double value);
        double FindDelta(double output, double errorFactor);
        double FindNetValue(Dictionary<INeuron,double> inputs, double bias);
        double FindNewBias(double bias, double delta);
        void UpdateWeights(Dictionary<INeuron, double> connections, double delta);
    }
}
