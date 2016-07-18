using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using BrainNet.NeuralFramework;

namespace NeuroOCR
{
    public partial class TrainingNetworkDialog : Form
    {
        public int traintimes = 1000;
        public Display parent = null;
        public NetworkHelper helper = null;
        public TrainingNetworkDialog(Display _parent)
        {
            parent = _parent;
            helper = new NetworkHelper(parent.network);
            InitializeComponent();
        }

        public void UpdateTitle(string text)
        {
            this.Text = text;
        }

        private void TrainingNetworkDialog_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            this.UpdateTitle("Training Network - 0%");
            int no = 0;
            foreach (var pair in parent.trainingData)
            {
                this.label1.Text = "Training Character \""+pair.Key+"\" (0%)...";
                int na = 0;
                foreach (var data in pair.Value)
                {
                    for (int i = 0; i < traintimes; i++)
                    {
                        parent.network.TrainNetwork(new TrainingData(data, Program.BitArrayToArray(Program.CharToBinary(pair.Key))));
                        progressBar2.Value = (int)(((decimal)((na*traintimes)+i) / (decimal)(pair.Value.Count*traintimes)) * (decimal)1000);
                        this.label1.Text = "Training Character \"" + pair.Key + "\" (" + Math.Round((((decimal)((na * traintimes) + i) / (decimal)(pair.Value.Count * traintimes)) * (decimal)100), 1, MidpointRounding.AwayFromZero) + "%)...";
                    }
                    progressBar2.Value = (int)(((decimal)na / (decimal)pair.Value.Count) * (decimal)1000);
                    na++;
                }
                progressBar1.Value = (int)(((decimal)no / (decimal)parent.trainingData.Count) * (decimal)1000);
                this.UpdateTitle("Training Network - " + Math.Round(((decimal)no / (decimal)parent.trainingData.Count) * (decimal)100,1, MidpointRounding.AwayFromZero) + "%");
                no++;
            }
            parent.trainingData.Clear();
            parent.UpdateList();
            this.Close();
        }

        
    }
}
