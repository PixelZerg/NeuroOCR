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
        private double _bias;
        private double _delta;
        private NList _forwardConnections;
        private Dictionary<INeuron, double> _inputs;
        private double _output;
        private INStrategy _strategy;

        public double BiasValue
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public double DeltaValue
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public NList ForwardConnections
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Dictionary<INeuron, double> Inputs
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public double OutputValue
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public INStrategy Strategy
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void UpdateDelta(double errorFactor)
        {
            throw new NotImplementedException();
        }

        public void UpdateFreeParams()
        {
            throw new NotImplementedException();
        }

        public void UpdateOutput()
        {
            throw new NotImplementedException();
        }
    }
}
