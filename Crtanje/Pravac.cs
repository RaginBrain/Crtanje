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
        public float koef_smjera;
        public float koef_smjera_poY;

       

        public Pravac(Tocka t1, Tocka t2)
        {
            T1 = t1;
            T2 = t2;
            if ((T2.t.X - T1.t.X) != 0)
                koef_smjera = (float)(T2.t.Y - T1.t.Y) / (float)(T2.t.X - T1.t.X);
            else
                koef_smjera = 0;

            if (T2.t.Y - T1.t.Y != 0)
                koef_smjera_poY = (float)(T2.t.X - T1.t.X) / (T2.t.Y - T1.t.Y);
            else
                koef_smjera_poY = 0;
        }

        public Pravac(Pravac p, Tocka toc)
        {
            koef_smjera = -1 / p.koef_smjera;
            koef_smjera_poY = -1 / p.koef_smjera_poY;
            T1 = toc;
        }

        public float IzracunajYza(float x)
        {
            float ipsilon = (float)(koef_smjera * (x - T1.t.X)) + T1.t.Y;
            return ipsilon;
        }

        public float IzracunajXza(float y)
        {
            float iks = (koef_smjera_poY* (float)(y - T1.t.Y) + T1.x);
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
        
        public double[] Segmentni_oblik()
        {
            double[] seg = new double[3];
            seg[0] = T2.t.X - T1.t.X;
            seg[1] = -(T2.t.Y - T1.t.Y);
            seg[2]=(T1.t.X*(T2.t.Y-T1.t.Y))- (T1.t.Y*(T2.t.X-T1.t.X));
            return seg;
        }


        public Tocka sjeciste_pravaca(Pravac p)
        {
           
            double[] prva_jednadzba = this.Segmentni_oblik();
            double[] druga_jednandzba = p.Segmentni_oblik();

            double[] temp =new double[3];

            double Ykvota_prve = prva_jednadzba[0];
            double Ykvota_druge= druga_jednandzba[0];

            for (int i = 0; i < 3; i++)
            {
                prva_jednadzba[i] *= Ykvota_druge;
                druga_jednandzba[i] *= Ykvota_prve;
            }

            if (prva_jednadzba[0] == druga_jednandzba[0])
                for (int i = 0; i < 3; i++)
                    druga_jednandzba[i] *= -1;

            for (int i = 0; i < 3; i++)
                prva_jednadzba[i] += druga_jednandzba[i];

           

            double X = -prva_jednadzba[2] / prva_jednadzba[1];
            double Y = -(druga_jednandzba[1] * X + druga_jednandzba[2]) / druga_jednandzba[0];

            Tocka sjec = new Tocka(new Point((int)Math.Round(X), (int)Math.Round(Y)));
           
            return sjec;

            

        }


    }
}
