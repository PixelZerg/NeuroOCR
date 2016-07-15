using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NeuroOCR
{
    public partial class Drawer : UserControl
    {
        Bitmap bmp;
        Action mouseisdown = null;
        public Drawer()
        {
            InitializeComponent();
            SetupDimensions(20, 20);
        }

        public void SetupDimensions(int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
        }

        private void Drawer_Paint(object sender, PaintEventArgs e)
        {
            if (bmp != null)
            {
                e.Graphics.DrawImage(bmp, Point.Empty);
            }
        }

        private void Drawer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'c')
            {
                //Console.WriteLine("CLEAR!");
            }
        }

        private void Drawer_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void MouseIsDown()
        {

        }
    }
}
