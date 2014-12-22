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
        Tocka AA = new Tocka(new Point(30, 100));
        Tocka BB = new Tocka(new Point(650, 100));
        List<Tocka> put = new List<Tocka>();

        Tocka[] niz_Tocaka = new Tocka[15];
        List<Tocka> list_Tocaka = new List<Tocka>();
        


      /*  public List<Tocka> Generiranje(List<Tocka> put,int broj_tocaka, Graphics drawArea)
        {
            
            List<Pravac> lista_crta = new List<Pravac>();
            Tocka[] niz_Tocaka = new Tocka[broj_tocaka];
            Random r = new Random();
            Pen blackPen = new Pen(Color.Black);
            //*******************************************


            A.Draw(drawArea, blackPen);
            B.Draw(drawArea, blackPen);

            int udaljenost_AB = B.t.X - A.t.X;

            Pravac p = new Pravac(A, B);
            lista_crta.Add(p);

            for (int i = 0; i < broj_tocaka; i++)
            {

                niz_Tocaka[i] = new Tocka(new Point(r.Next(A.t.X, B.t.X), r.Next(0, 260)));
                lista_crta.Add(new Pravac(A, niz_Tocaka[i]));
                lista_crta.Add(new Pravac(niz_Tocaka[i], B));
                niz_Tocaka[i].Draw(drawArea, blackPen);
            }





            Tocka min = niz_Tocaka[0];
            float mini = 200;
            foreach (Tocka t in niz_Tocaka)
            {
                foreach (Tocka s in niz_tocaka_izbivenog_vektora)
                    if (s.Distanca(t) < mini)
                    {
                        mini = s.Distanca(t);
                        min = t;
                    }
            }



        }*/



        public List<Tocka> Rjesi(List<Tocka> put, List<Tocka> niz_Tocaka)
        {
            int pozicija = 0;
            Tocka min = niz_Tocaka[0];
            float mini = 2000;
            Tocka A=null;
            Tocka B=null;

            //petlja provjerava svaku dionicu sadasnjeg puta
            for (int j = 0; j < put.Count - 1; j++)
            {
                A = put[j];
                B = put[j+1];
                Pravac p = new Pravac(A, B);
                lista_crta.Add(p);

                for (int i = 0; i < niz_Tocaka.Count(); i++)
                {
                    lista_crta.Add(new Pravac(A, niz_Tocaka[i]));
                    lista_crta.Add(new Pravac(niz_Tocaka[i], B));
                }


                //slaže se niz točaka prosjecne_krivulje(izbiveni_vektor)
                int udaljenost_AB = Math.Abs(B.t.X - A.t.X);
                Tocka[] niz_tocaka_izbivenog_vektora = new Tocka[udaljenost_AB];


                for (int i = 0; i < udaljenost_AB; i++)
                {
                    niz_tocaka_izbivenog_vektora[i] = new Tocka(new Point(A.t.X + i, (int)Prosjecni_Y(lista_crta, i, A)));
                }
                //----


                //provjerava koja je tocka najblize izbivenom vektoru
                foreach (Tocka t in niz_Tocaka)
                {
                    foreach (Tocka s in niz_tocaka_izbivenog_vektora)
                        if (s.Distanca(t) < mini)
                        {
                            mini = s.Distanca(t);
                            min = t;
                            pozicija = j+1;
                        }
                    //gleda i u vec potegnutim crtama

                    //triba napravit put bez A i B

                    
                    /*
                    List<Tocka> put_bez = new List<Tocka>(put);
                    int zabranjeni = put_bez.IndexOf(A) - 1;
                    put_bez.Remove(A);
                    put_bez.Remove(B);

                    if(put_bez.Count>2)
                        for (int znj = 0; znj < put.Count - 1;znj++ )
                        {
                            if (znj != zabranjeni)
                            {
                                Pravac perislav = new Pravac(put[znj], put[znj + 1]);
                                List<Tocka> tocke_pravca = perislav.pretvori_u_tocke();
                                foreach (Tocka s in tocke_pravca)
                                    if (s.Distanca(t) < mini)
                                    {
                                        mini = s.Distanca(t);
                                        min = t;
                                        pozicija = j + 1;
                                    }
                            }
                        }*/
                }
                
                //----
            }
            //nasa je tocku , gleda ima li boljeg pravca za nju
            float min_produzetak = put[pozicija - 1].Distanca(min) + put[pozicija].Distanca(min) - put[pozicija - 1].Distanca(put[pozicija]);
            for (int h = 1; h < put.Count - 1; h++)
            {
                float produzetak = put[h - 1].Distanca(min) + put[h].Distanca(min) - put[h - 1].Distanca(put[h]);
                if (produzetak < min_produzetak)
                {
                    min_produzetak = produzetak;
                    pozicija = h;
                }
            }

            //zavrsi posao
            list_Tocaka.Remove(min);
            put.Insert(pozicija,min);

            return put;
            
            
        }

        public Form1()
        {
            InitializeComponent();
            drawArea = pictureBox1.CreateGraphics();
            put.Add(AA);
            put.Add(BB);
            Random r = new Random();

            Pen blackPen = new Pen(Color.Black);

            for (int i = 0; i < 15; i++)
            {

                list_Tocaka.Add ( new Tocka(new Point(r.Next(50, 600), r.Next(0, 260))));
            }
            
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
            


            AA.Draw(drawArea, blackPen);
            BB.Draw(drawArea, blackPen);

            Pravac p = new Pravac(AA, BB);
            lista_crta.Add(p);
            
            for (int i = 0; i < 15; i++)
            {
               
                niz_Tocaka[i] = new Tocka(new Point(r.Next(50,600),r.Next(0,260)));
                lista_crta.Add(new Pravac(AA, niz_Tocaka[i]));
                lista_crta.Add(new Pravac(niz_Tocaka[i], BB));
                niz_Tocaka[i].Draw(drawArea, blackPen);
            }

            foreach (Pravac perica in lista_crta)
            {
                perica.Draw(drawArea, blackPen);
            }


            int udaljenost_AB = BB.t.X - AA.t.X;
            Tocka[] niz_tocaka_izbivenog_vektora = new Tocka[udaljenost_AB];


            for (int i = 0; i < udaljenost_AB; i++)
            {
                niz_tocaka_izbivenog_vektora[i] = new Tocka(new Point(AA.t.X + i, (int)Prosjecni_Y(lista_crta, i, AA)));
            }

            Tocka min = niz_Tocaka[0];
            float mini = 200;


            foreach (Tocka t in niz_Tocaka)
            {
                foreach(Tocka s in niz_tocaka_izbivenog_vektora)
                    if(s.Distanca(t)< mini)
                    {
                        mini=s.Distanca(t);
                        min=t;
                    }
            }
            min.DrawZuto(drawArea, blackPen);

            foreach (Tocka t in niz_tocaka_izbivenog_vektora)
            {
                
                t.DrawManje(drawArea, blackPen);
            }









            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {


            drawArea.Clear(Color.White);
            Pen blackPen = new Pen(Color.Black);
            put=Rjesi(put, list_Tocaka);
           Point[] niz_put = new Point[list_Tocaka.Count];
 
            for(int i=0;i<put.Count-1;i++)
            {
                drawArea.DrawLine(blackPen, put[i].t, put[i + 1].t);
                put[i].DrawZuto(drawArea, blackPen);
            }

            foreach (Tocka t in list_Tocaka)
            {
                t.Draw(drawArea, blackPen);
            }
          
        }


        
    }
}
