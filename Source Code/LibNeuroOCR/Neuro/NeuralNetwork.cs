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
            for (int i = 0; i <= inputs.Count - 1; i++)
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
                //int no = 0;
                //using (IEnumerator enumerator2 = this.InputLayer.GetEnumerator())
                //{
                //    while (enumerator2.MoveNext())
                //    {
                //        INeuron current = (INeuron)enumerator2.Current;
                //        current.OutputValue = (double)(inputs[(int)num2]);
                //        num2 += 1L;
                //    }
                //}
                int no = 0;
                foreach (var item in this.InputLayer)
                {
                    item.OutputValue = (double)(inputs[no]);
                    no++;
                }
                for (no = 1; no <= this.Layers.Count - 1; no += 1)
                {
                    //IEnumerator enumerator = null;
                    //NList layer = this._layers[(int)no];
                    //enumerator = layer.GetEnumerator();
                    //while (enumerator.MoveNext())
                    //{
                    //    ((INeuron)enumerator.Current).UpdateOutput();
                    //}
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
            long num;
            CheckInadequateLayers();
            if (td.Inputs.Count != this.InputLayer.Count)
            {
                throw new NeuroException("Number of inputs doesn'''t match number of neurons in input layer", null);
            }
            if (td.Outputs.Count != this.OutputLayer.Count)
            {
                throw new NeuroException("Number of outputs doesn'''t match number of neurons in output layer", null);
            }
            long num9 = td.Inputs.Count - 1;
            for (num = 0L; num <= num9; num += 1L)
            {
                try
                {
                    td.Inputs[(int)num] = td.Inputs[(int)num];
                }
                catch (System.Exception e)
                {
                    throw new NeuroException("Unable to convert the input value at location " + (num + 1L) + " to double", null);
                }
            }
            long num8 = td.Outputs.Count - 1;
            for (num = 0L; num <= num8; num += 1L)
            {
                try
                {
                    td.Outputs[(int)num] = td.Outputs[(int)num];
                }
                catch (System.Exception e)
                {
                    throw new NeuroException("Unable to convert the output value at location " + (num + 1L) + " to double", null);
                }
            }
            try
            {
                //INeuron current;
                //long num3 = 0L;
                //using (IEnumerator enumerator6 = this.InputLayer.GetEnumerator())
                //{
                //    while (enumerator6.MoveNext())
                //    {
                //        current = (INeuron)enumerator6.Current;
                //        current.OutputValue = td.Inputs[(int)num3];
                //        num3 += 1L;
                //    }
                //}
                for (int i = 0; i < this.InputLayer.Count; i++)
                {
                    this.InputLayer[i].OutputValue = td.Inputs[i];
                }

                for (int i = 1; i <= this.Layers.Count-1; i += 1)
                {
                    //IEnumerator enumerator5;
                    //NList layer2 = this._layers[(int)num2];
                    //try
                    //{
                    //    enumerator5 = layer2.GetEnumerator();
                    //    while (enumerator5.MoveNext())
                    //    {
                    //        current = (INeuron)enumerator5.Current;
                    //        current.UpdateOutput();
                    //    }
                    //}
                    //finally
                    //{
                    //    if (enumerator5 is IDisposable)
                    //    {
                    //        ((IDisposable)enumerator5).Dispose();
                    //    }
                    //}
                    foreach (var item in this._layers[i])
                    {
                        item.UpdateOutput();
                    }
                }
                //int num3 = 0;
                //using (IEnumerator enumerator4 = this.OutputLayer.GetEnumerator())
                //{
                //    while (enumerator4.MoveNext())
                //    {
                //        current = (INeuron)enumerator4.Current;
                //        current.UpdateDelta(td.Outputs[(int)num3]- current.OutputValue);
                //        num3 += 1;
                //    }
                //}
                for (int i = 0; i < this.OutputLayer.Count; i++)
                {
                    this.OutputLayer[i].UpdateDelta(td.Outputs[i] - this.OutputLayer[i].OutputValue);
                }
                for (int i = this._layers.Count - 2; i >= 1L; i ++)
                {
                    IEnumerator enumerator3;
                    //NList layer = this._layers[i];
                    //try
                    //{
                    //    enumerator3 = layer.GetEnumerator();
                    //    while (enumerator3.MoveNext())
                    //    {
                    //        current = (INeuron)enumerator3.Current;
                    //        double errorFactor = 0.0;
                    //        foreach (INeuron neuron2 in current.ForwardConnections)
                    //        {
                    //            errorFactor += neuron2.DeltaValue * neuron2.Inputs[current];
                    //        }
                    //        current.UpdateDelta(errorFactor);
                    //    }
                    //}
                    //finally
                    //{
                    //    if (enumerator3 is IDisposable)
                    //    {
                    //        ((IDisposable)enumerator3).Dispose();
                    //    }
                    //}
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
                    //IEnumerator enumerator;
                    //try
                    //{
                    //    enumerator = this._layers[i].GetEnumerator();
                    //    while (enumerator.MoveNext())
                    //    {
                    //        ((INeuron)enumerator.Current).UpdateFreeParams();
                    //    }
                    //}
                    //finally
                    //{
                    //    if (enumerator is IDisposable)
                    //    {
                    //        ((IDisposable)enumerator).Dispose();
                    //    }
                    //}
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
