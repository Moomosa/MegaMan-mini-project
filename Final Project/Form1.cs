using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final_Project                         //All Images are located in Image Dump Folder as well
{
    public partial class Form1 : Form
    {
        Random rng = new Random();
        Player megaMan;
        Rush rush;
        Rush badRush;
        Boss currentBoss;
        List<Boss> bossList = new List<Boss>();
        List<Bullet> shotsFired = new List<Bullet>();
        List<Explosion> explodeList = new List<Explosion>();
        SoundPlayer song2 = new SoundPlayer("Boss2.wav");
        SoundPlayer song3 = new SoundPlayer("Boss3.wav");
        SoundPlayer song4 = new SoundPlayer("Boss4.wav");
        SoundPlayer boom = new SoundPlayer("Explode.wav");
        SoundPlayer won = new SoundPlayer("Clear.wav");

        public enum KeyMove { none, up, down, forward, shoot }
        public int formWidth;
        private int[] direction1 = new int[] { 0, 5, 5, 5, 0, -5, -5, -5, 0, 8, 8, 8, 0, -8, -8, -8 };
        private int[] direction2 = new int[] { -5, -5, 0, 5, 5, 0, 5, -5, -8, -8, 0, 8, 8, 0, 8, -8 };

        KeyMove leftKey = KeyMove.none;
        KeyMove shotKey = KeyMove.none;

        public Form1()
        {
            InitializeComponent();
            formWidth = ClientSize.Width;
            megaMan = new Player(this, 15, 300);
            rush = new Rush(this, megaMan.X - 5, megaMan.Y + 55, Environment.CurrentDirectory + "/Rush");
            bossList.Add(new Boss(this, 1000, 300, Environment.CurrentDirectory + "/drill", song4));
            bossList.Add(new Boss(this, 1000, 250, Environment.CurrentDirectory + "/quick", song2));
            bossList.Add(new Boss(this, 1000, 50, Environment.CurrentDirectory + "/snake", song3));
            bossList.Add(new Boss(this, 1000, 150, Environment.CurrentDirectory + "/skull", song4));
            NewBoss();
        }
        public void NewBoss()
        {
            Boss tmp = bossList[rng.Next(0, bossList.Count)];
            this.Controls.Add(tmp.bossPic);
            currentBoss = tmp;
            badRush = new Rush(this, currentBoss.X, currentBoss.Y, Environment.CurrentDirectory + "/BadRush");
            tmp.soundPlayer.PlayLooping();
        }


        private async void tmrMain_Tick(object sender, EventArgs e)
        {
            if (leftKey == KeyMove.up)
                megaMan.MoveUpDown(true);
            if (leftKey == KeyMove.down)
                megaMan.MoveUpDown(false);
            if (leftKey == KeyMove.forward)
                currentBoss.Running(true);
            if (leftKey == KeyMove.forward && shotKey != KeyMove.shoot)
                megaMan.Running(true, false);
            if (leftKey == KeyMove.forward && shotKey == KeyMove.shoot)
                megaMan.Running(true, true);
            if (leftKey == KeyMove.none)
            {
                megaMan.Running(false, false);
                currentBoss.Running(false);
            }
            if (leftKey == KeyMove.none && shotKey == KeyMove.shoot)
                megaMan.Running(false, true);

            rush.Flying(megaMan.Y, megaMan.X);
            badRush.Flying(currentBoss.Y, currentBoss.X);

            for (int i = 0; i < shotsFired.Count; i++)
            {
                if (CrashTest(currentBoss, shotsFired[i]))
                {
                    currentBoss.Damaged(true);
                    if (currentBoss.Health == 0)
                    {
                        currentBoss.soundPlayer.Stop();
                        Destroyed();                            //Calls the explosions     
                        currentBoss.Pop();                      //Removes boss and places a new one           
                        bossList.Remove(currentBoss);
                        if (bossList.Count == 0)
                        {
                            await Task.Delay(2700);             //Current "beating" the game
                            won.PlaySync();
                            this.Close();
                        }
                        else
                        {
                            await Task.Delay(2700);
                            NewBoss();                           //Get a new boss from the list
                        }
                    }
                    if (shotsFired.Count > 0)                   //This was apparently the solution to an exemption
                    {
                        shotsFired[i].Gone();
                        shotsFired.RemoveAt(i);
                    }
                }
                if (shotsFired.Count != 0)
                    if (shotsFired[i].X + shotsFired[i].Width > formWidth)
                        shotsFired.RemoveAt(i);
            }

            for (int i = 0; i < explodeList.Count; i++)
            {
                if (explodeList[i].Offscreen())
                {
                    explodeList[i].Gone();
                    explodeList.RemoveAt(i);
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) leftKey = KeyMove.up;
            if (e.KeyCode == Keys.Down) leftKey = KeyMove.down;
            if (e.KeyCode == Keys.Right) leftKey = KeyMove.forward;
            if (e.KeyCode == Keys.Space)
            {
                shotKey = KeyMove.shoot;
                if (shotsFired.Count < 3)
                    shotsFired.Add(new Bullet(this, megaMan.X + (megaMan.Width - 5), megaMan.Y + 22));
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) leftKey = KeyMove.none;
            if (e.KeyCode == Keys.Down) leftKey = KeyMove.none;
            if (e.KeyCode == Keys.Right) leftKey = KeyMove.none;
            if (e.KeyCode == Keys.Space) shotKey = KeyMove.none;
        }

        private bool CrashTest(Boss boss, Bullet bullet)    //Checks if bullet hits boss
        {
            if (boss.X > bullet.X + bullet.Width)
                return false;
            if (bullet.X + bullet.Width < boss.X)
                return false;
            if (boss.Y + boss.Height < bullet.Y)
                return false;
            if (bullet.Y + bullet.Height < boss.Y)
                return false;
            return true;
        }

        private void Destroyed()        // This creates all 16 explosion entities
        {
            boom.Play();
            for (int i = 0; i < 16; i++)
                explodeList.Add(new Explosion(this, (currentBoss.X + currentBoss.Width / 2), (currentBoss.Y + currentBoss.Height / 2), Environment.CurrentDirectory + "/Explosion", direction1[i], direction2[i]));
        }
    }
}