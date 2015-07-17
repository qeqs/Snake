using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            c = new Constants();
        }
        int w;//keys
        Graphics g;
        Constants c;
        Draw d;
        int x, y;
        List<Body> b;
        List<Food> f;
        Random rand = new Random();
        int[] xx;
        int[] yy;
        Font ff;
        void Exit()//end game
        {
            timer1.Enabled = false;
            g.Clear(Color.AntiqueWhite);
            g.DrawString(c.score.ToString(), ff, Brushes.Black, x / 2-c.step, y / 2-c.step);
            c.score = 0;
            game = false;
            button1.Enabled = true;
            button1.Visible = true;
            
        }
        bool pause = false;
        bool game = false;
        void Pause()
        {
            if (!pause)
            {
                timer1.Enabled = false;
                pause = true;
                g.DrawString("Pause", ff, Brushes.Black, x / 2 - c.step, y / 2 - c.step);
            }
            else
            {
                pause = false;
                timer1.Enabled = true;
            }

        }
        void New(bool tick)
        {
            g = CreateGraphics();
             button1.Enabled = false;
            button1.Visible = false;
            game = true;
            ff = new Font("Arial", 40, FontStyle.Bold);//end game string
            w = 1;//turn var
            int k = 0;//kostil
            xx = new int[x / 25];//coord net
            yy = new int[y / 25];
            d = new Draw(x,y);
            b = new List<Body>();
            f = new List<Food>();
            b.Add(new Body(100, 300, c.step, c.step));
            for (int i = 0; i < x / 25; i++)
            {
                k += c.step;
                xx[i] = k;
            }
            k = 0;
            for (int i = 0; i < y / 25; i++)
            {
                k += c.step;
                yy[i] = k;
            }
            f.Add(new Food(xx[rand.Next(0, x / 25 - 1)], yy[rand.Next(0, y / 25 - 1)], c.step, c.step));
            if(tick)
            for (int i = 3; i > 0; i--)
            {
                g.Clear(Color.AntiqueWhite);
                g.DrawString(i.ToString(), ff, Brushes.Black, x / 2 - c.step, y / 2 - c.step);
                System.Threading.Thread.Sleep(1000);
            }
                timer1.Enabled = true;
        }
        
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            New(true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            x = ClientSize.Width;
            y = ClientSize.Height;
            
            
        }
        int s = 1;//counter for turn method
        void Chain(int x,int y)//snake move
        {
          
            if (s != b.Count)
            {
                int tx = b[s].tail.X;
                int ty = b[s].tail.Y;
                b[s].tail.Y = y;
                b[s].tail.X = x;
                ++s;
                Chain(tx, ty);//recursion
            }
            

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Chain(b[0].tail.X, b[0].tail.Y);//turning method
            s = 1;//turn counter
            switch (w)//1-up,2-down,3-left,4-right
            {
                case 1:
                    b[0].tail.Y -= c.step;
                    break;
                case 2:
                    b[0].tail.Y += c.step;
                    break;
                case 3:
                    b[0].tail.X -= c.step;
                    break;
                case 4:
                    b[0].tail.X += c.step;
                    break;
            }
           
            for (int i = 0; i < f.Count; i++)
            {
                if (b[0].tail.IntersectsWith(f[i].block))//if head eats food
                {
                    c.score+=c.score_step;
                    switch(w)//after eating add body for right coordinates
                    {
                        case 1:                    
                    b.Add(new Body(b[b.Count - 1].tail.X, b[b.Count - 1].tail.Y+c.step, c.step, c.step));
                    break;
                        case 2:
                    b.Add(new Body(b[b.Count - 1].tail.X, b[b.Count - 1].tail.Y - c.step, c.step, c.step));
                    break;
                        case 3:
                    b.Add(new Body(b[b.Count - 1].tail.X+c.step, b[b.Count - 1].tail.Y, c.step, c.step));
                    break;
                        case 4:
                    b.Add(new Body(b[b.Count - 1].tail.X-c.step, b[b.Count - 1].tail.Y, c.step, c.step));
                    break;
                    }
                    b[b.Count - 1].created = true;//useless
                    f.Add(new Food(xx[rand.Next(0, x / 25 - 1)], yy[rand.Next(0, y / 25 - 1)], c.step, c.step));
                    f[i].eaten = true;//have been eaten
                }
                
                if(f[i].eaten&&b[b.Count-1].tail.IntersectsWith(f[i].block))
                {
                    f.RemoveAt(i);
                }
            }
            label4.Text = c.score.ToString();
         //   g.DrawImage(d.Frame(b, f), ClientRectangle);
            g.DrawImage(d.Frame(b, f),0,0,x,y);

            //-------------------------condition of end-----------------------

            if (b[0].tail.X < 0 || b[0].tail.Y < 24 || b[0].tail.X + c.step > x || b[0].tail.Y + c.step > y)//if out of clientrect
                Exit();
            for (int i = 1; i < b.Count; i++)
            {
                if (b[0].tail.IntersectsWith(b[i].tail))//if eat itself
                    Exit();

            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(game)
            switch (e.KeyValue)//38 - up, 40 - down, 37 - left, 39 - right
            {
                case 38:
                    if (w != 2)
                        w = 1;
                    break;
                case 40:
                    if (w != 1)
                        w = 2;
                    break;
                case 37:
                    if (w != 4)
                        w = 3;
                    break;
                case 39:
                    if (w != 3)
                        w = 4;
                    break;
                case 19:
                    Pause();
                break;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        //-----------------Difficulty menu---------------
        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Interval = 100;
            c.score_step = 5;
            label2.Text = "Medium";
        }

        private void highToolStripMenuItem_Click(object sender, EventArgs e)
        {
            c.score_step = 10;
            timer1.Interval = 50;
            label2.Text = "High";
        }

        private void lowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            c.score_step = 1;
            timer1.Interval = 200;
            label2.Text = "Low";
        }
        //-------------------------------------------------
        private void Form1_ClientSizeChanged(object sender, EventArgs e)//доработать-------------------------------------------------
        {
            
            x = ClientSize.Width;
            y = ClientSize.Height;
            
            if (game)
            {             
                New(false);
                Pause();
            }
        }

        private void button1_Click(object sender, EventArgs e)//new game button
        {
            New(true);
        }

        private void button1_LocationChanged(object sender, EventArgs e)
        {

        }


    }
}
