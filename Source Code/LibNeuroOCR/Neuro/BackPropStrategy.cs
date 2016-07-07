using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibNeuroOCR.Interface;
using LibNeuroOCR.Exception;

namespace LibNeuroOCR.Neuro
{
    public class BackPropStrategy : INStrategy
    {
        public double Activation(double value)
        {
            double ret = 0;
            try
            {
                ret = 1.0 / (1.0 + Math.Exp(value * -1.0));
            }
            catch (System.Exception e)
            {
                throw new NeuroException("Exception in Activation function", e);
            }
            return ret;
        }

        public double FindDelta(double output, double errorFactor)
        {
            double ret;
            try
            {
                ret = (output * (1.0 - output)) * errorFactor;
            }
            catch (System.Exception e)
            {
                throw new NeuroException("Exception in Finding Delta", e);
            }
            return ret;
        }

        public double FindNetValue(Dictionary<INeuron, double> inputs, double bias)
        {
            double ret = 0;
            //try
            //{
            //    double num2 = bias;
            //    foreach (INeuron neuron in Inputs.Neurons())
            //    {
            //        num2 += Inputs[neuron] * neuron.OutputValue;
            //    }
            //    num = num2;
            //}
            //catch (Exception exception1)
            //{
            //    ProjectData.SetProjectError(exception1);
            //    Exception e = exception1;
            //    throw new NeuronStrategyException("Exception in Finding Net Value", e);
            //}
            foreach (KeyValuePair<INeuron,double> pair in inputs)
            {
                ret += pair.Value * pair.Key.OutputValue;
            }
            return ret;
        }

        public double FindNewBias(double bias, double delta)
        {
            //double num;
            //try
            //{
            //    num = bias + (0.5 * delta);
            //}
            //catch (Exception exception1)
            //{
            //    ProjectData.SetProjectError(exception1);
            //    Exception e = exception1;
            //    throw new NeuronStrategyException("Exception in Finding New Bias Value", e);
            //}
            //return num;
            try
            {
                return bias + (0.5 * delta);
            }
            catch (System.Exception e)
            {
                throw new NeuroException("Exception whilst finding new bias value", e);
            }
        }

        public void UpdateWeights(ref Dictionary<INeuron, double> connections, double delta)
        {
            //try
            //{
            //    IEnumerator enumerator;
            //    try
            //    {
            //        enumerator = connections.Neurons().GetEnumerator();
            //        while (enumerator.MoveNext())
            //        {
            //            INeuron current = (INeuron)enumerator.Current;
            //            connections[current] += (0.5 * current.OutputValue) * delta;
            //        }
            //    }
            //    finally
            //    {
            //        if (enumerator is IDisposable)
            //        {
            //            ((IDisposable)enumerator).Dispose();
            //        }
            //    }
            //}
            //catch (Exception exception1)
            //{
            //    ProjectData.SetProjectError(exception1);
            //    Exception e = exception1;
            //    throw new NeuronStrategyException("Exception while updating the weight values", e);
            //}
            try
            {
                //foreach (KeyValuePair<INeuron, double> pair in connections)
                //{
                //    pair.Value += (0.5 * pair.Key.OutputValue) * delta;
                //}
                foreach (var item in connections.Keys)
                {
                    connections[item] += (0.5 * item.OutputValue) * delta;
                }
            }
            catch (System.Exception e)
            {
                throw new NeuroException("Exception whilst updating weight values", e);
            }
        }
    }
}
