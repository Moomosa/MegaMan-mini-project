using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final_Project
{
    class Bullet
    {
        private PictureBox shot = new PictureBox();
        private Timer tickTimer = new Timer();
        private int formWidth;
        private int xSpeed;
        private Form myForm;

        public int X { get { return shot.Left; } }
        public int Y { get { return shot.Top; } }
        public int Width { get { return shot.Width; } }
        public int Height { get { return shot.Height; } }

        public Bullet(Form thisForm, int x, int y)
        {
            shot.Image = new Bitmap("Bullet.png");
            shot.SizeMode = PictureBoxSizeMode.AutoSize;
            shot.Left = x;
            shot.Top = y;
            thisForm.Controls.Add(shot);

            tickTimer.Interval = 20;
            tickTimer.Tick += TickTimer_Tick;
            tickTimer.Enabled = true;

            formWidth = thisForm.ClientSize.Width;
            xSpeed = 20;
            myForm = thisForm;
        }

        private void TickTimer_Tick(object sender, EventArgs e)
        {
            shot.Left += xSpeed;
            if (shot.Left >= formWidth)
                Gone();
        }
        public void Gone()
        {
            myForm.Controls.Remove(shot);
        }
    }
}