using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibNeuroOCR.Interface;
using LibNeuroOCR.Data;

namespace LibNeuroOCR.Neuro
{
    public class BackPropFactory : INetworkFactory
    {
        public INeuralNetwork CreateNetwork(params long[] neurons)
        {
            NeuralNetwork nn = new NeuralNetwork();
            BackPropStrategy bps = new BackPropStrategy();
            foreach (var item in neurons)
            {
                NList nl = new NList();
                for (int i = 0; i < item ; i++)
                {
                    INeuron n = new Neuron(bps);
                    nl.Add(ref n);
                }
                nn.Layers.Add(nl);
            }
            nn.ConnectLayers();
            return nn;
        }
        public INeuralNetwork CreateNetwork(long inputneurons, long outputneurons)
        {
            return this.CreateNetwork(inputneurons, inputneurons, outputneurons);
        }
    }
}
