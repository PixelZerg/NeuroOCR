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
            List<double> l = new List<double>();
            foreach (INeuron neuron in this.OutputLayer)
            {
                l.Add(neuron.OutputValue);
            }
            return l;
        }

        public List<double> RunNetwork(params double[] inputs)
        {
            List<double> l = new List<double>();
            l.AddRange(inputs);
            return RunNetwork(l);
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
                throw new NeuroException("Error occurred while running the network. "+Environment.NewLine+e, e);
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
            //for (int i = 0; i < td.Inputs.Count; i++)
            //{
            //    try
            //    {
            //        td.Inputs[i] = td.Inputs[i];
            //    }
            //    catch (System.Exception e)
            //    {
            //        throw new NeuroException("Error at input value loc:  " + (i + 1), e);
            //    }
            //}
            //for (int i = 0; i < td.Outputs.Count; i++)
            //{
            //    try
            //    {
            //        td.Outputs[(int)i] = td.Outputs[i];
            //    }
            //    catch (System.Exception e)
            //    {
            //        throw new NeuroException("Error at output value loc: " + (i + 1L), e);
            //    }
            //} //redundant ^^^^^^
            try
            {
                for (int i = 0; i < this.InputLayer.Count; i++)
                {
                    this.InputLayer[i].OutputValue = td.Inputs[i];
                }

                for (int i = 1; i < this.Layers.Count; i++)
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
                //for (int i = this._layers.Count - 2; i >= 1; i++)
                //for(int i = 0;i<this._layers.Count;i++)
                for(int i = 1; i <= this.Layers.Count-2;i++)
                {
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
                for (int i = 1; i <= this._layers.Count - 1; i++)
                {
                    foreach (var item in this._layers[i])
                    {
                        item.UpdateFreeParams();
                    }
                }
            }
            catch (System.Exception e)
            {
                throw new NeuroException("Error occurred while training network: " + Environment.NewLine + e, e);
            }
        }

        //public void TrainNetwork(TrainingData t)
        //{
        //    if (this._layers.Count < 2)
        //    {
        //        throw new InadequateLayersException("You should have atleast two layers in your neural network to train it", null);
        //    }
        //    if (t.Inputs.Count != this.InputLayer.Count)
        //    {
        //        throw new NeuroException("Number of inputs doesn'''t match number of neurons in input layer", null);
        //    }
        //    if (t.Outputs.Count != this.OutputLayer.Count)
        //    {
        //        throw new NeuroException("Number of outputs doesn'''t match number of neurons in output layer", null);
        //    }
        //    long arg_7B_0 = 0L;
        //    long num = (long)(checked(t.Inputs.Count - 1));
        //    checked
        //    {
        //        for (long num2 = arg_7B_0; num2 <= num; num2 += 1L)
        //        {
        //            try
        //            {
        //                //t.Inputs.set_Item((int)num2, DoubleType.FromObject(t.Inputs.get_Item((int)num2)));
        //                t.Inputs[(int)num2] = (double)t.Inputs[(int)num2];
        //            }
        //            catch (System.Exception e)
        //            {
        //                throw new NeuroException("Unable to convert the input value at location " + (num2 + 1L) + " to double", null);
        //            }
        //        }
        //        long arg_F9_0 = 0L;
        //        long num3 = unchecked((long)(checked(t.Outputs.Count - 1)));
        //        for (long num2 = arg_F9_0; num2 <= num3; num2 += 1L)
        //        {
        //            try
        //            {
        //                t.Outputs[((int)num2)] = (double)t.Outputs[((int)num2)];
        //            }
        //            catch (System.Exception e)
        //            {
        //                throw new NeuroException("Unable to convert the output value at location " + (num2 + 1L) + " to double", null);
        //            }
        //        }
        //        try
        //        {
        //            long num4 = 0L;
        //            IEnumerator enumerator = this.InputLayer.GetEnumerator();

        //            try
        //            {
        //                while (enumerator.MoveNext())
        //                {
        //                    INeuron neuron = (INeuron)enumerator.Current;
        //                    neuron.OutputValue = (double)(t.Inputs[((int)num4)]);
        //                    num4 += 1L;
        //                }
        //            }
        //            finally
        //            {
        //                //IEnumerator enumerator;
        //                if (enumerator is IDisposable)
        //                {
        //                    ((IDisposable)enumerator).Dispose();
        //                }
        //            }
        //            long num5 = 1L;
        //            long arg_1EE_0 = 1L;
        //            long num6 = unchecked((long)(checked(this._layers.Count - 1)));
        //            IEnumerator enumerator2 = null;
        //            for (num5 = arg_1EE_0; num5 <= num6; num5 += 1L)
        //            {
        //                NList neuronLayer = this._layers[(int)num5];
        //                enumerator2 = neuronLayer.GetEnumerator();
        //                try
        //                {
        //                    while (enumerator2.MoveNext())
        //                    {
        //                        INeuron neuron = (INeuron)enumerator2.Current;
        //                        neuron.UpdateOutput();
        //                    }
        //                }
        //                finally
        //                {
        //                    if (enumerator2 is IDisposable)
        //                    {
        //                        ((IDisposable)enumerator2).Dispose();
        //                    }
        //                }
        //            }
        //            num4 = 0L;
        //            IEnumerator enumerator3 = this.OutputLayer.GetEnumerator();
        //            try
        //            {
        //                while (enumerator3.MoveNext())
        //                {
        //                    INeuron neuron = (INeuron)enumerator3.Current;
        //                    neuron.UpdateDelta((double)((t.Outputs[((int)num4)] - neuron.OutputValue)));
        //                    num4 += 1L;
        //                }
        //            }
        //            finally
        //            {
        //                if (enumerator3 is IDisposable)
        //                {
        //                    ((IDisposable)enumerator3).Dispose();
        //                }
        //            }
        //            for (num4 = unchecked((long)(checked(this._layers.Count - 2))); num4 >= 1L; num4 += -1L)
        //            {
        //                NList neuronLayer2 = this._layers[(int)num4];
        //                unchecked
        //                {
        //                    IEnumerator enumerator4 = neuronLayer2.GetEnumerator();
        //                    try
        //                    {
        //                        while (enumerator4.MoveNext())
        //                        {
        //                            INeuron neuron = (INeuron)enumerator4.Current;
        //                            IEnumerator enumerator5 = neuron.ForwardConnections.GetEnumerator();
        //                            double num7 = 0.0;
        //                            try
        //                            {
        //                                while (enumerator5.MoveNext())
        //                                {
        //                                    INeuron neuron2 = (INeuron)enumerator.Current;
        //                                    num7 += neuron2.DeltaValue * neuron2.Inputs[neuron];
        //                                }
        //                            }
        //                            finally
        //                            {
        //                                if (enumerator5 is IDisposable)
        //                                {
        //                                    ((IDisposable)enumerator5).Dispose();
        //                                }
        //                            }
        //                            neuron.UpdateDelta(num7);
        //                        }
        //                    }
        //                    finally
        //                    {
        //                        if (enumerator4 is IDisposable)
        //                        {
        //                            ((IDisposable)enumerator4).Dispose();
        //                        }
        //                    }
        //                }
        //            }
        //            long arg_3D0_0 = 1L;
        //            long num8 = unchecked((long)(checked(this._layers.Count - 1)));
        //            IEnumerator enumerator6 = this._layers[(int)num4].GetEnumerator();
        //            for (num4 = arg_3D0_0; num4 <= num8; num4 += 1L)
        //            {
        //                try
        //                {
        //                    while (enumerator6.MoveNext())
        //                    {
        //                        INeuron neuron = (INeuron)enumerator6.Current;
        //                        neuron.UpdateFreeParams();
        //                    }
        //                }
        //                finally
        //                {
        //                    if (enumerator6 is IDisposable)
        //                    {
        //                        ((IDisposable)enumerator6).Dispose();
        //                    }
        //                }
        //            }
        //        }
        //        catch (System.Exception e)
        //        {
        //            throw new NeuroException("Error occurred while training network. "+e, e);
        //        }
        //    }
        //}

    }
}
