using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibNeuroOCR.Data
{
    public class TrainingData
    {
        private List<double> _inputs = new List<double>();
        private List<double> _outputs = new List<double>();

        public TrainingData()
        {
        }

        public TrainingData(List<double> input, List<double> output)
        {
            this._inputs = input;
            this._outputs = output;
        }

        public List<double> Inputs
        {
            get { return this._inputs; }
        }

        public List<double> Outputs
        {
            get { return this._outputs; }
        }
    }
}
