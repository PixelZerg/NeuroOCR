namespace NeuroOCR
{
    partial class Drawer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Drawer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.Name = "Drawer";
            this.Size = new System.Drawing.Size(430, 399);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Drawer_Paint);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Drawer_KeyPress);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Drawer_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Drawer_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
