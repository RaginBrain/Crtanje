using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Crtanje
{
   public class Tocka
    {
        public bool rjesena;
        public Point t;
        Rectangle rect;
        Brush b;
        Brush green;
        public float x;
        public float y;
        

        public Tocka(Point T)
        {
            t = T;
            rect = new Rectangle(new Point(T.X-4,T.Y-4),new Size(8,8) );
            b= new SolidBrush(Color.Red);
            green = new SolidBrush(Color.Green);
            x = T.X;
            y = T.Y;

            rjesena = false;
        }

        public void Draw(Graphics g, Pen p)
        {
            g.DrawArc(p, rect, 0, 360);
            g.FillPie(b, rect, 0, 360);
            

        }
        public void DrawManje(Graphics g, Pen p)
        {
            g.DrawArc(p, new Rectangle(rect.X,rect.Y,rect.Width-2,rect.Height-2), 0, 360);
            g.FillPie(green, rect, 0, 360);


        }

    }
}
