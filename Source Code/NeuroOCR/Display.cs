using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrainNet.NeuralFramework;
using System.Collections;
using System.IO;

namespace NeuroOCR
{
    //public partial class Display : MetroFramework.Forms.MetroForm
    public partial class Display:Form
    {
        public INeuralNetwork network = null;
        public Dictionary<char, List<ArrayList>> trainingData = new Dictionary<char, List<ArrayList>>();
        public Display()
        {
            InitializeComponent();
            System.IO.Directory.CreateDirectory("Networks");
            int pixels = drawer1.bmp.Width * drawer1.bmp.Height;
            Console.Write("Instantiating a {0} - {0} - 6 neural network...", pixels);
            network = new BackPropNetworkFactory().CreateNetwork(new ArrayList(new double[] { pixels, pixels, 6 }));//instantiate neural network
            Console.WriteLine("[OK!]");
            UpdateList();
            drawer1.vpressed += button4_Click;
        }

        public void UpdateList()
        {
            int selectedIndex = listBox1.SelectedIndex;
            listBox1.Items.Clear();
            for (int i = 48; i <= 57; i++)
            {
                if (!trainingData.ContainsKey((Char)i)) trainingData.Add((Char)i, new List<ArrayList>());
                listBox1.Items.Add(((Char)i) + " - " + trainingData[(Char)i].Count);
            }
            for (int i = 65; i <= 90; i++)
            {
                if (!trainingData.ContainsKey((Char)i)) trainingData.Add((Char)i, new List<ArrayList>());
                listBox1.Items.Add(((Char)i) + " - " + trainingData[(Char)i].Count);
            }
            for (int i = 97; i <= 122; i++)
            {
                if (!trainingData.ContainsKey((Char)i)) trainingData.Add((Char)i, new List<ArrayList>());
                listBox1.Items.Add(((Char)i) + " - " + trainingData[(Char)i].Count);
            }
            listBox1.SelectedIndex = selectedIndex;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            drawer1.Clear();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                trainingData[((String)listBox1.SelectedItem)[0]].Add(Program.BitmapToArray(drawer1.GetBitmap()));
                drawer1.Clear(false);
                UpdateList();
            }
            catch(NullReferenceException)
            {
                MessageBox.Show("Make sure that you have selected a character from the list!", "NeuroOCR - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //private void Display_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == 'v')
        //    {
        //        button4_Click(null, null);
        //    }
        //}

        private void button5_Click(object sender, EventArgs e)
        {
            new System.Threading.Thread(() => new TrainingNetworkDialog(this).ShowDialog()).Start();
            //new TrainingNetworkDialog(this).ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FileInfo f = new FileDialog().GetPath();
            if (f.Exists)
            {
                if (MessageBox.Show(f.Name + " already exists. Overwrite it?", "File already exists", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                {
                    return;
                }
                f.Delete();
            }
            new NetworkSerializer().SaveNetwork(f.FullName, network);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new NetworkSerializer().LoadNetwork(new FileDialog().GetPath().FullName, ref network);
        }


        private void button6_Click(object sender, EventArgs e)
        {
            drawer2.Clear();
        }

        private void drawer2_MouseDown(object sender, MouseEventArgs e)
        {
            //while (drawer2.mouseisdown)
            //{
            //    break;
            //}
        }

        public void RunNetwork()
        {
            ArrayList input = Program.BitmapToArray(drawer2.GetBitmap());
            ArrayList output = network.RunNetwork(input);
            BitArray binoutput = Program.RoundToBinary(output);
            listBox4.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            for (int i = 0; i < input.Count; i++)
            {
                listBox4.Items.Add(input[i]);
            }
            double totdist = 0d;
            for (int i = 0; i < output.Count; i++)
            {
                listBox2.Items.Add((double)output[i]);
                //listBox2.Items.Add(Math.Round((double)output[i], 0, MidpointRounding.AwayFromZero));
                if (binoutput[i]) listBox3.Items.Add(1);
                else listBox3.Items.Add(0);
                totdist = (binoutput[i]) ? totdist+ Math.Abs(1 - (double)output[i]) : totdist+Math.Abs(0 - (double)output[i]);
            }
            label2.Text = "Confidence: "+Math.Round(totdist / (output.Count / 2) * 100,2, MidpointRounding.AwayFromZero)+"%";
            try
            {
                label1.Text = Program.BinaryToChar(binoutput).ToString();
            }

            catch
            {
                label1.Text = "n/a";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            RunNetwork();
        }
    }
}
