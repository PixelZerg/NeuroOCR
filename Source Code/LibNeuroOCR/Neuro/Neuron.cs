using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibNeuroOCR.Interface;
using LibNeuroOCR.Data;

namespace LibNeuroOCR.Neuro
{
    public class Neuron : INeuron
    {
        private double _bias = Utils.Rand();
        private double _delta = 0d;
        private NList _forwardConnections = new NList();
        private Dictionary<INeuron, double> _inputs = new Dictionary<INeuron, double>();
        private double _output = 0d;
        private INStrategy _strategy = null;

        public Neuron() { }

        public Neuron(INStrategy strategy)
        {
            _strategy = strategy;
        }

        public double BiasValue
        {
            get { return _bias; }
            set { _bias = value; }
        }

        public double DeltaValue
        {
            get { return _delta; }
            set { _delta = value; }
        }

        public NList ForwardConnections
        {
            get { return _forwardConnections; }
        }

        public Dictionary<INeuron, double> Inputs
        {
            get { return _inputs; }
        }

        public double OutputValue
        {
            get { return _output; }
            set { _output = value; }
        }

        public INStrategy Strategy
        {
            get { return _strategy; }
            set { _strategy = value; }
        }

        public void UpdateDelta(double errorFactor)
        {
            CheckStrategyNull();
            this.DeltaValue = this.Strategy.FindDelta(this.OutputValue, errorFactor);
        }

        public void UpdateFreeParams()
        {
            CheckStrategyNull();
            this.BiasValue = this.Strategy.FindNewBias(this.BiasValue, this.DeltaValue);
            this.Strategy.UpdateWeights(this._inputs, this.DeltaValue);
        }

        public void UpdateOutput()
        {
            CheckStrategyNull();
            double num = this.Strategy.FindNetValue(this.Inputs, this.BiasValue);
            this.OutputValue = this.Strategy.Activation(num);
        }

        public void CheckStrategyNull()
        {
            if (this.Strategy == null)
                throw new Exception.NullStrategyException("Neuron@" + this.GetHashCode() + "'s Strategy has not been initialised!", null);
        }
    }
}
