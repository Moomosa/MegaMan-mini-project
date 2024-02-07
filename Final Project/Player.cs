using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final_Project
{
    class Player
    {
        private PictureBox megaMan = new PictureBox();
        public static List<Image> State = new List<Image>();
        private int x;
        private int y;
        private int width = 24;
        private int height = 60;
        private int ySpeed = 10;
        private int formHeight;
        private int drawState = 0;

        public int X { get { return x; } }
        public int Y { get { return y; } }
        public int Width { get { return width; } }

        public Player(Form thisForm, int X, int Y)
        {
            foreach (string file in Directory.GetFiles(Environment.CurrentDirectory + "/MegaMan"))
            {
                if (!file.Contains(".png"))
                    continue;
                State.Add(Image.FromFile(file));
            }

            megaMan.Image = State[0];
            megaMan.SizeMode = PictureBoxSizeMode.AutoSize;
            x = X;
            y = Y;
            megaMan.Left = X;
            megaMan.Top = Y;
            thisForm.Controls.Add(megaMan);
            formHeight = thisForm.ClientSize.Height;
        }

        public void MoveUpDown(bool dir)
        {
            if (dir)
            {
                y -= ySpeed;
                if (y < 0)
                    y = 0;
            }
            else
            {
                y += ySpeed;
                if (y + height > formHeight)
                    y = formHeight - height;
            }
            megaMan.Top = y;
        }

        public void Running(bool run, bool shooting)
        {
            if (run)
            {
                switch (drawState)
                {
                    case 0:
                        if (shooting)
                            megaMan.Image = State[4];
                        else
                            megaMan.Image = State[1];
                        break;
                    case 5:
                        if (shooting)
                            megaMan.Image = State[5];
                        else
                            megaMan.Image = State[2];
                        break;
                    case 10:
                        if (shooting)
                            megaMan.Image = State[6];
                        else
                            megaMan.Image = State[3];
                        break;
                    case 15:
                        if (shooting)
                            megaMan.Image = State[5];
                        else
                            megaMan.Image = State[2];
                        break;
                }
                if (++drawState == 20) drawState = 0;
            }
            else
                megaMan.Image = State[0];
            if (!run && shooting)
                megaMan.Image = State[7];
        }
    }
}