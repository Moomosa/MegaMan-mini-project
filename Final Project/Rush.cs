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
    class Rush
    {
        private PictureBox rush = new PictureBox();
        private List<Image> State = new List<Image>();
        private int drawState = 0;

        public Rush(Form thisForm, int X, int Y, string directory)
        {
            foreach (string file in Directory.GetFiles(directory))
            {
                if (!file.Contains(".png"))
                    continue;
                State.Add(Image.FromFile(file));
            }

            rush.Image = State[0];
            rush.SizeMode = PictureBoxSizeMode.AutoSize;
            rush.Left = X;
            rush.Top = Y;
            thisForm.Controls.Add(rush);
        }

        public void Flying(int Y, int X)
        {
            switch (drawState)
            {
                case 0:
                    rush.Image = State[0];
                    break;
                case 5:
                    rush.Image = State[1];
                    break;
            }
            if (++drawState == 10) drawState = 0;
            rush.Top = Y + 55;
            rush.Left = X - 10;
        }
    }
}