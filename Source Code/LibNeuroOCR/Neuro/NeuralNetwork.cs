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
                for (int i = 1; i <= num2; i ++)
                {
                    this.ConnectLayers(this._layers[i - 1], this._layers[i]);
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
            for (int i = 0; i < inputs.Count; i++)
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
                int no = 0;
                foreach (var item in this.InputLayer)
                {
                    item.OutputValue = (double)(inputs[no]);
                    no++;
                }
                for (no = 1; no < this.Layers.Count; no += 1)
                {
                    foreach (var item in this._layers[(int)no])
                    {
                        try
                        {
                            item.UpdateOutput();
                        }
                        catch (System.Exception e)
                        {
                            throw new NeuroException("Error occured whilst updating neuron: " + item, e);
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
            CheckInadequateLayers();
            if (td.Inputs.Count != this.InputLayer.Count)
            {
                throw new NeuroException("Number of inputs doesn'''t match number of neurons in input layer", null);
            }
            if (td.Outputs.Count != this.OutputLayer.Count)
            {
                throw new NeuroException("Number of outputs doesn'''t match number of neurons in output layer", null);
            }
            for (int i = 0; i < td.Inputs.Count; i++)
            {
                try
                {
                    td.Inputs[i] = td.Inputs[i];
                }
                catch (System.Exception e)
                {
                    throw new NeuroException("Error at input value loc:  " + (i + 1), e);
                }
            }
            for (int i = 0; i < td.Outputs.Count; i++)
            {
                try
                {
                    td.Outputs[(int)i] = td.Outputs[i];
                }
                catch (System.Exception e)
                {
                    throw new NeuroException("Error at output value loc: " + (i + 1L), e);
                }
            }
            try
            {
                for (int i = 0; i < this.InputLayer.Count; i++)
                {
                    this.InputLayer[i].OutputValue = td.Inputs[i];
                }

                for (int i = 1; i < this.Layers.Count; i ++)
                {
                    foreach (var item in this._layers[i])
                    {
                        item.UpdateOutput();
                    }
                }
                for (int i = 0; i < this.OutputLayer.Count; i++)
                {
                    this.OutputLayer[i].UpdateDelta(td.Outputs[i] - this.OutputLayer[i].OutputValue);
                }
                for (int i = this._layers.Count - 2; i >= 1L; i ++)
                {
                    IEnumerator enumerator3;
                    foreach (var item in this._layers[i])
                    {
                        double errorFactor = 0.0;
                        foreach (INeuron n in item.ForwardConnections)
                        {
                            errorFactor += n.DeltaValue * n.Inputs[item];
                        }
                        item.UpdateDelta(errorFactor);
                    }
                }
                for (int i = 1; i <= this._layers.Count - 1; i ++)
                {
                    foreach (var item in this._layers[i])
                    {
                        item.UpdateFreeParams();
                    }
                }
            }
            catch (System.Exception e)
            {
                throw new NeuroException("Error occurred while training network. ", e);
            }
        }
    }
}
