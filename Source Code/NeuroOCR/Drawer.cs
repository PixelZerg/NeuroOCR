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
        public bool mouseisdown = false;
        public Bitmap bmp = null;
        public EventHandler vpressed = null;
        public Drawer()
        {
            //bmp = new Bitmap(this.Width, this.Height);
            InitializeComponent();
            SetupDimensions(20, 20);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
            }
        }

        public void SetupDimensions(int width, int height)
        {
            bmp = new Bitmap(width, height);
        }

        public Bitmap GetBitmap(bool wait = true)
        {
            mouseisdown = false;
            if(wait)
            System.Threading.Thread.Sleep(100);
            return bmp;
        }

        private void Drawer_Paint(object sender, PaintEventArgs e)
        {
            if (bmp != null)
            {
                using (Image image = ScaleImage(bmp, this.Width, this.Height))
                {
                    //e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    e.Graphics.DrawImage(image, Point.Empty);
                }
            }
        }
        public static Image ScaleImage(Image image, int newWidth, int newHeight)
        {
            //    var ratioX = (double)maxWidth / image.Width;
            //    var ratioY = (double)maxHeight / image.Height;
            //    var ratio = Math.Min(ratioX, ratioY);

            //    var newWidth = (int)(image.Width * ratio);
            //    var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                //graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return newImage;
        }
        public void Clear(bool wait = true)
        {
            mouseisdown = false;
            if(wait)System.Threading.Thread.Sleep(100);
            //bmp = new Bitmap(bmp.Width, bmp.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
            }
            this.Invalidate();
            this.Update();
        }
        private void Drawer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'c')
            {
                Clear();
            }
            else if (e.KeyChar == 'v')
            {
                if (vpressed != null)
                    vpressed(null, null);
            }
        }

        private void Drawer_MouseDown(object sender, MouseEventArgs e)
        {
            mouseisdown = true;
            new System.Threading.Thread(() =>
            {
                int no = 0;
                while (mouseisdown)
                {
                    Point pos = this.PointToClient(Cursor.Position);
                    try
                    {
                        using (Graphics g = Graphics.FromImage(bmp))
                        {
                            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                            g.FillRectangle(Brushes.Black, (int)(((decimal)pos.X / (decimal)this.Width) * bmp.Width), (int)(((decimal)pos.Y / (decimal)this.Width) * bmp.Height), 1, 1);
                        }
                        if (no % 8000 == 0)
                        {
                            this.Invalidate();
                            this.Update();
                        }
                    }
                    catch { }
                    no++;
                }
            }).Start();
        }

        private void Drawer_MouseUp(object sender, MouseEventArgs e)
        {
            mouseisdown = false;
            this.Invalidate();
            this.Update();
        }
    }
}
