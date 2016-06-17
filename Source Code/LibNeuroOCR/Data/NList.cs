using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibNeuroOCR.Interface;
using LibNeuroOCR.Neuro;

namespace LibNeuroOCR.Data
{
    public class NList : List<INeuron>
    {
        public INeuron Add()
        {
            Neuron n = new Neuron();
            base.Add(n);
            return n;
        }

        public INeuron Add(ref INeuron n)
        {
            base.Add(n);
            return n;
        }

        public INeuron this[int i]
        {
            get { return base[i]; }
            set { base[i] = value; }
        }

        public void Insert(int index, INeuron obj)
        {
            base.Insert(index, obj);
        }

        public void Remove(INeuron obj)
        {
            base.Remove(obj);
        }
    }
}
