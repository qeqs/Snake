using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Snake
{
    class Constants
    {
        public int step = 25;
        public int score = 0;
        public int score_step = 5;
    }
    class Body//player
    {

        public Rectangle tail;
        public bool created;//useless

        public Body(int x1, int y1, int x2, int y2)
        {

            tail = new Rectangle(x1, y1, x2, y2);
        }
    }
    class Food
    {
        public Rectangle block;
        public bool eaten;
        public Food(int x1, int y1, int x2, int y2)
        {
            block = new Rectangle(x1 + 1, y1 + 1, x2 - 1, y2 - 1);
            eaten = false;
        }
    }
    class Draw
    {
        Graphics g;
        Bitmap bm;
        public Draw(int x, int y)
        {
            bm = new Bitmap(x, y);
            g = Graphics.FromImage(bm);
        }
        public Image Frame(List<Body> b, List<Food> f)
        {
            g.Clear(Color.AntiqueWhite);
            for (int i = 0; i < b.Count; i++)
            {
                g.DrawRectangle(Pens.Black, b[i].tail);

            }
            g.FillRectangle(Brushes.Red, b[0].tail);//fill head
            for (int i = 0; i < f.Count; i++)
            {
                g.FillRectangle(Brushes.Black, f[i].block);
            }
            return bm as Image;
        }
    }
}
