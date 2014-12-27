using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Crtanje
{
    class Pravac
    {
        public Tocka T1;
        public Tocka T2;

        public Pravac(Tocka t1, Tocka t2)
        {
            T1 = t1;
            T2 = t2;           
        }

        public float IzracunajYza(float x)
        {
            float ipsilon = (float)(((float)(T2.t.Y - T1.t.Y) / (float)(T2.t.X - T1.t.X)) * (x - T1.t.X)) + T1.t.Y;
            return ipsilon;
        }

        public float IzracunajXza(float y)
        {
            float iks = ((float)  (T2.t.X - T1.t.X)/(T2.t.Y - T1.t.Y)  ) * (float)(y - T1.t.Y) + T1.x;
            return iks;
        }


        public void Draw(Graphics g, Pen p)
        {
            g.DrawLine(p, T1.t, T2.t);
        }
        public List<Tocka> pretvori_u_tocke()
        {
            List<Tocka> tocke = new List<Tocka>();
            int udaljenost = Math.Abs(T2.t.X - T1.t.X);
            for (int z = 0; z < udaljenost; z++)
            {
                Point pojnt =(new Point((T1.t.X + z), (int)IzracunajYza((T1.t.X + z))));
                tocke.Add(new Tocka(pojnt));
            }
            return tocke;


        }
    }
}
