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
        public TrainingData(double input, double output)
        {
            this._inputs.Add(input);
            this._outputs.Add(output);
        }
        public TrainingData(double[] input, double[] output)
        {
            this._inputs.AddRange(input);
            this._outputs.AddRange(output);
        }
        /// <summary>
        /// Specify the amount of inputs in the first argument then pass all your data in the next arguments.
        /// </summary>
        public TrainingData(int inputno, params double[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (i < inputno)
                {
                    this._inputs.Add(data[i]);
                }
                else
                {
                    this._outputs.Add(data[i]);
                }
            }
            //Console.WriteLine("e");
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
