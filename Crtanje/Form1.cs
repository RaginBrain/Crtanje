using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Crtanje
{
    public partial class Form1 : Form
    {
        float Prosjecni_Y(List<Pravac> lst,int x,Tocka poc)
        {
            float rjesenje = 0;
            int koliko=0;
            x += poc.t.X;

            foreach (Pravac perica in lst)
            {
                if (perica.T1.x <= x && x<= perica.T2.x)
                {
                    rjesenje += perica.IzracunajYza(x);
                    koliko++;
                }
            }
            rjesenje = (int)rjesenje / koliko;
            return rjesenje;
        }

        List<Pravac> lista_crta=new List<Pravac>();
        Graphics drawArea;
        Tocka A = new Tocka(new Point(30, 100));
        Tocka B = new Tocka(new Point(650, 100));

        public void Generiranje(Tocka prva, Tocka druga,int broj_tocaka, Graphics drawArea)
        {


        }

        public Form1()
        {
            InitializeComponent();
            drawArea = pictureBox1.CreateGraphics();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        public void button1_Click(object sender, EventArgs e)
        {
            lista_crta.Clear();
            
            drawArea.Clear(Color.White);
            Random r = new Random();


            Pen blackPen = new Pen(Color.Black);
            Tocka[] niz_Tocaka = new Tocka[15];
            

            Tocka A = new Tocka(new Point(30, 100));
            A.Draw(drawArea, blackPen);

            Tocka B = new Tocka(new Point(650, 100));
            B.Draw(drawArea, blackPen);

            Pravac p = new Pravac(A, B);
            lista_crta.Add(p);
            
            for (int i = 0; i < 10; i++)
            {
               
                niz_Tocaka[i] = new Tocka(new Point(r.Next(50,600),r.Next(0,260)));
                lista_crta.Add(new Pravac(A, niz_Tocaka[i]));
                lista_crta.Add(new Pravac(niz_Tocaka[i], B));
                niz_Tocaka[i].Draw(drawArea, blackPen);
            }

            foreach (Pravac perica in lista_crta)
            {
                perica.Draw(drawArea, blackPen);
            }


            Tocka[] niz_tocaka_izbivenog_vektora = new Tocka[124];
            for (int i = 0; i < 124; i++)
            {
                niz_tocaka_izbivenog_vektora[i] = new Tocka(new Point(A.t.X+i * 5, (int)Prosjecni_Y(lista_crta,i*5,A)));
            }

            foreach (Tocka t in niz_tocaka_izbivenog_vektora)
            {
                t.DrawManje(drawArea, blackPen);
            }









            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }


        
    }
}
