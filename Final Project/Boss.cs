using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Media;

namespace Final_Project
{
    class Boss
    {

        private PictureBox boss = new PictureBox();
        private List<Image> State = new List<Image>();
        private int width = 24;
        private int height = 60;
        private int health = 10;
        private int drawState = 0;
        private Form myForm;
        private SoundPlayer sound;

        public int X { get { return boss.Left; } }
        public int Y { get { return boss.Top; } }
        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public int Health { get { return health; } }
        public PictureBox bossPic { get { return boss; } }
        public SoundPlayer soundPlayer { get { return sound; } }

        public Boss(Form thisForm, int X, int Y, string directory, SoundPlayer song)
        {
            foreach (string file in Directory.GetFiles(directory))
            {
                if (!file.Contains(".png"))
                    continue;
                State.Add(Image.FromFile(file));
            }

            boss.Image = State[0];
            boss.SizeMode = PictureBoxSizeMode.AutoSize;
            boss.Left = X;
            boss.Top = Y;
            myForm = thisForm;
            sound = song;
        }

        public void Running(bool run)
        {
            if (run)
            {
                if (boss.Left > 400)
                {
                    boss.Left -= 5;
                    switch (drawState)
                    {
                        case 0:
                            boss.Image = State[1];
                            break;
                        case 5:
                            boss.Image = State[2];
                            break;
                        case 10:
                            boss.Image = State[3];
                            break;
                        case 15:
                            boss.Image = State[2];
                            break;
                    }
                    if (++drawState == 20) drawState = 0;
                }
                else
                    boss.Image = State[0];
            }
            else
                boss.Image = State[0];
        }

        public void Damaged(bool hit)
        {
            if (hit)            
                health--;            
        }

        public void Pop()
        {
            myForm.Controls.Remove(boss);
            boss.Left = 1000;
            health = 10;
        }
    }
}