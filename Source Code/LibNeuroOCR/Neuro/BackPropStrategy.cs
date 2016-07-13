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
            foreach (KeyValuePair<INeuron,double> pair in inputs)
            {
                ret += pair.Value * pair.Key.OutputValue;
            }
            return ret;
        }

        public double FindNewBias(double bias, double delta)
        {
            try
            {
                return bias + (0.5 * delta);
            }
            catch (System.Exception e)
            {
                throw new NeuroException("Exception whilst finding new bias value", e);
            }
        }

        public void UpdateWeights(Dictionary<INeuron, double> connections, double delta)
        {
            try
            {
                Dictionary<INeuron, double> newconnections = new Dictionary<INeuron, double>();
                foreach (var item in connections.Keys)
                {
                    //connections[item] += (0.5 * item.OutputValue) * delta;
                    newconnections.Add(item, (0.5 * item.OutputValue) * delta);
                }
                connections = newconnections;
            }
            catch (System.Exception e)
            {
                throw new NeuroException("Exception whilst updating weight values: "+e, e);
            }
        }
    }
}
