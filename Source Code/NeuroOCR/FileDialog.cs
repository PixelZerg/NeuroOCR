using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace NeuroOCR
{
    public partial class FileDialog : Form
    {
        public FileInfo[] files = new DirectoryInfo("Networks").GetFiles("*.xml", SearchOption.TopDirectoryOnly);
        public FileDialog()
        {
            InitializeComponent();
            UpdateListbox();
        }

        public FileInfo GetPath()
        {
            this.ShowDialog();
            string ret = textBox1.Text;
            if (!textBox1.Text.Contains("."))
            {
                ret += ".xml";
            }
            return new FileInfo("Networks\\"+ret);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void UpdateListbox()
        {
            List<string> nl = new List<string>();
            foreach (FileInfo file in files)
            {
                if (file.Name.Contains(textBox1.Text))
                {
                    nl.Add("Networks\\" + file.Name);
                }
            }
            listBox1.Items.Clear();
            listBox1.Items.AddRange(nl.ToArray());
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //UpdateListbox();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int seli = listBox1.SelectedIndex;
            textBox1.Text = files[listBox1.SelectedIndex].Name;
            try
            {
                listBox1.SelectedIndex = seli;
            }
            catch
            {
                try
                {
                    listBox1.SelectedIndex = 0;
                }
                catch { }
            }
        }
    }
}
