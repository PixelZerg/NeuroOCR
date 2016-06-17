using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibNeuroOCR.Interface
{
    public interface INetworkFactory
    {
        INeuralNetwork CreateNetwork(long inputneurons, long outputneurons);
    }
}
