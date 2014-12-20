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
        Brush zuti;
        public float x;
        public float y;
        

        public Tocka(Point T)
        {
            t = T;
            rect = new Rectangle(new Point(T.X-4,T.Y-4),new Size(8,8) );
            b= new SolidBrush(Color.Red);
            green = new SolidBrush(Color.Green);
            zuti = new SolidBrush(Color.Yellow);
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

        public void DrawZuto(Graphics g, Pen p)
        {
            g.DrawArc(p, new Rectangle(rect.X, rect.Y, rect.Width - 2, rect.Height - 2), 0, 360);
            g.FillPie(zuti, rect, 0, 360);
        }
        


        public float Distanca(Tocka X)
        {
            double razlika_X = (X.t.X - t.X) * (X.t.X - t.X);
            double razlika_y=(X.t.Y - t.Y) * (X.t.Y - t.Y);
            double razlika=razlika_X+razlika_y;

            double rez = Math.Sqrt( razlika );
            return (float)rez;
        }

    }
}
