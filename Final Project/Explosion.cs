using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Final_Project
{
    class Explosion
    {
        private PictureBox explode = new PictureBox();        
        private List<Image> State = new List<Image>();
        private Timer tickTimer = new Timer();
        private Form myForm;
        private int formWidth;
        private int formHeight;
        private int xSpeed = 10;
        private int ySpeed = 10;
        private int drawState = 0;

        public Explosion(Form thisForm, int X, int Y, string directory, int xFast, int yFast)
        {
            foreach (string file in Directory.GetFiles(directory))
            {
                if (!file.Contains(".png"))
                    continue;
                State.Add(Image.FromFile(file));
            }

            explode.Image = State[0];
            explode.SizeMode = PictureBoxSizeMode.AutoSize;
            explode.Left = X;
            explode.Top = Y;
            xSpeed = xFast;
            ySpeed = yFast;
            myForm = thisForm;
            formWidth = thisForm.Width;
            formHeight = thisForm.Height;
            myForm.Controls.Add(explode);

            tickTimer.Interval = 20;
            tickTimer.Tick += TickTimer_Tick;
            tickTimer.Enabled = true;
        }

        private void TickTimer_Tick(object sender, EventArgs e)
        {
            Pop(true);
        }

        public void Pop(bool dead)
        {
            if (dead)
            {
                explode.Left += xSpeed;
                explode.Top += ySpeed;
                switch (drawState)
                {
                    case 0:
                        explode.Image = State[0];
                        break;
                    case 5:
                        explode.Image = State[1];
                        break;
                    case 10:
                        explode.Image = State[2];
                        break;
                    case 15:
                        explode.Image = State[3];
                        break;
                    case 20:
                        explode.Image = State[4];
                        break;
                }
                if (++drawState == 25) drawState = 0;
            }
        }

        public void Gone()
        {
            myForm.Controls.Remove(explode);
        }

        public bool Offscreen()
        {
            if (explode.Left > formWidth)
                return true;
            if (explode.Left < 0)
                return true;
            if (explode.Top < 0)
                return true;
            if (explode.Top > formHeight)
                return true;
            return false;
        }
    }
}