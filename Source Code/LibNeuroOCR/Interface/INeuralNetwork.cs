using LibNeuroOCR.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibNeuroOCR.Interface
{
    public interface INeuralNetwork
    {
        NList InputLayer { get; }
        List<NList> Layers { get; }
        NList OutputLayer { get; }

        void ConnectLayers();
        void ConnectLayers(NList layer1, NList layer2);
        void ConnectNeurons(INeuron source, INeuron destination);
        void ConnectNeurons(INeuron source, INeuron destination, double weight);
        List<double> GetOutput();
        List<double> RunNetwork(List<double> inputs);
        List<double> RunNetwork(params double[] inputs);
        void TrainNetwork(TrainingData td);
    }
}
