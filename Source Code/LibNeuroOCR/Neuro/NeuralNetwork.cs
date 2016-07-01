using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibNeuroOCR.Data;
using LibNeuroOCR.Interface;
using LibNeuroOCR.Exception;
using System.Collections;
using Exception = System.Exception;

namespace LibNeuroOCR.Neuro
{
    public class NeuralNetwork : INeuralNetwork
    {
        public List<NList> _layers = new List<NList>();

        public void CheckInadequateLayers()
        {
            if (this._layers.Count < 2)
            {
                throw new InadequateLayersException("You need at least 2 layers in your neural network if you would like to train it", null);
            }
        }

        public NList InputLayer
        {
            get
            {
                CheckInadequateLayers();
                return this._layers[0];
            }
        }

        public List<NList> Layers
        {
            get
            {
                return _layers;
            }
        }

        public NList OutputLayer
        {
            get
            {
                CheckInadequateLayers();
                return this._layers[this._layers.Count - 1];
            }
        }

        public void ConnectLayers(NList l1, NList l2)
        {
            CheckInadequateLayers();
            try
            {
                for (int i = 0; i < l1.Count; i++)
                {
                    for (int j = 0; j < l2.Count; j++)
                    {
                        this.ConnectNeurons(l1[i], l2[j]);
                    }
                    continue;
                }
            }
            catch (System.Exception e)
            {
                throw new NeuroException("Error", e);
            }
        }

        public void ConnectLayers()
        {
            try
            {
                long num2 = this._layers.Count - 1;
                for (long i = 1L; i <= num2; i += 1L)
                {
                    this.ConnectLayers(this._layers[(int)(i - 1L)], this._layers[(int)i]);
                }
            }
            catch (System.Exception e)
            {
                throw new NeuroException("Error occurred while trying to connect neuron layers. See stack trace for details", e);
            }
        }

        public void ConnectNeurons(INeuron source, INeuron destination)
        {
            CheckInadequateLayers();
            this.ConnectNeurons(source, destination, Utils.Rand());
        }

        public void ConnectNeurons(INeuron source, INeuron destination, double weight)
        {
            CheckInadequateLayers();
            destination.Inputs.Add(source, weight);
            source.ForwardConnections.Add(ref destination);
        }

        public List<double> GetOutput()
        {
            throw new NotImplementedException();
        }

        public List<double> RunNetwork(List<double> inputs)
        {
            List<double> output = new List<double>();
            for (int i = 0; i <= inputs.Count-1; i ++)
            {
                try
                {
                    inputs[i] = (double)(inputs[(int)i]);
                }
                catch (System.Exception e)
                {
                    throw new NeuroException("Unable to convert the  input value at location " + (i + 1) + " to double", null);
                }
            }
            try
            {
                long num2 = 0L;
                using (IEnumerator enumerator2 = this.InputLayer.GetEnumerator())
                {
                    while (enumerator2.MoveNext())
                    {
                        INeuron current = (INeuron)enumerator2.Current;
                        current.OutputValue = (double)(inputs[(int)num2]);
                        num2 += 1L;
                    }
                }
                long num3 = this._layers.Count - 1;
                for (num2 = 1L; num2 <= num3; num2 += 1L)
                {
                    IEnumerator enumerator = null;
                    NList layer = this._layers[(int)num2];
                    try
                    {
                        enumerator = layer.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            ((INeuron)enumerator.Current).UpdateOutput();
                        }
                    }
                    finally
                    {
                        if (enumerator is IDisposable)
                        {
                            ((IDisposable)enumerator).Dispose();
                        }
                    }
                }
                output = this.GetOutput();
            }
            catch (System.Exception e)
            {
                throw new NeuroException("Error occurred while running the network. ", e);
            }
            return output;
        }

        public void TrainNetwork(TrainingData td)
        {
            throw new NotImplementedException();
        }
    }
}
