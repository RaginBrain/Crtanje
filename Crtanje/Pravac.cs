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


        public void Draw(Graphics g, Pen p)
        {
            g.DrawLine(p, T1.t, T2.t);
        }
    }
}
