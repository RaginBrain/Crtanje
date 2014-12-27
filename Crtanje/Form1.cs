using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Crtanje
{

    //OBAVJEST VAZNO: Postoji klasa Tocka koja sluzi radi crtanja
    //                          * sadrzi Point(t) pa se svakoj kordinati (x,y) pristupa : Instanca.t.(x/y)


    public partial class Form1 : Form
    {
        //metoda ima problem kad je pravac vertikalan , JEDNA solucija je da se sustav postavi po tom pravcu u horizontalnom polozaju
        //                                              DRUGA solucija je da se izgradi funkcija koja bi vracala (X,Y) u odnosu na pravac (s obzirom na kut--->omjer)
        float Prosjecni_Y(List<Pravac> lst,int x,Tocka poc)
        {
            float rjesenje = 0;
            int koliko=0;
            x += poc.t.X;

            foreach (Pravac perica in lst)
            {
                //ovaj uvjet je blesav kad je pravac vertikalan 
                if (perica.T1.x <= x && x<= perica.T2.x)
                {
                    rjesenje += perica.IzracunajYza(x);
                    koliko++;
                }
            }
            rjesenje = (int)rjesenje / koliko;
            return rjesenje;
        }



        //nova funkcija 
        float Prosjecni_X(List<Pravac> lst, int y, Tocka poc)
        {
            float rjesenje = 0;
            int koliko = 0;
            y += poc.t.Y;

            foreach (Pravac perica in lst)
            {
                //ovaj uvjet je blesav kad je pravac vertikalan 
                if (perica.T1.y <= y && y <= perica.T2.y)
                {
                    rjesenje += perica.IzracunajXza(y);
                    koliko++;
                }
            }
            rjesenje = (int)rjesenje / koliko;
            return rjesenje;
        }




        List<Pravac> lista_crta=new List<Pravac>();
        Graphics drawArea;

        //tocke AA i BB su pocetna i zavrsna tocka
        Tocka AA = new Tocka(new Point(30, 100));
        Tocka BB = new Tocka(new Point(650, 200));

        List<Tocka> put = new List<Tocka>();

        Tocka[] niz_Tocaka = new Tocka[15];
        List<Tocka> list_Tocaka = new List<Tocka>();



        public List<Tocka> Rjesi(List<Tocka> put, List<Tocka> Neposjecene_tocke)
        {
            int pozicija = 0;
            Tocka min = Neposjecene_tocke[0];
            float mini = 2000;
            Tocka A=null;
            Tocka B=null;

            //petlja provjerava svaku dionicu sadasnjeg puta
            for (int j = 0; j < put.Count - 1; j++)
            {
                A = put[j];
                B = put[j+1];
                //--------------NOVO

                //--------------

                Pravac p = new Pravac(A, B);
                lista_crta.Add(p);

                for (int i = 0; i < Neposjecene_tocke.Count(); i++)
                {
                    lista_crta.Add(new Pravac(A, Neposjecene_tocke[i]));
                    lista_crta.Add(new Pravac(Neposjecene_tocke[i], B));
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
                foreach (Tocka t in Neposjecene_tocke)
                {
                    foreach (Tocka s in niz_tocaka_izbivenog_vektora)
                        if (s.Distanca(t) < mini)
                        {
                            mini = s.Distanca(t);
                            min = t;
                            pozicija = j+1;
                        }
                    
                }
                
            }


            //nasa je tocku , gleda ima li boljeg pravca za nju   (!!!TU mi triba nadogradnja jer zna bit GLUP!!!)
            

            float min_produzetak = put[pozicija - 1].Distanca(min) + put[pozicija].Distanca(min) - put[pozicija - 1].Distanca(put[pozicija]);

            //moguca optimizacija , ne triba gledat cili put nego samo OBLIZNJE(dosta je relativan pojam)
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
            Neposjecene_tocke.Remove(min);
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

            for (int i = 0; i < 25; i++)
            {

                list_Tocaka.Add ( new Tocka(new Point(r.Next(50, 600), r.Next(0, 260))));
            }

            using (StreamWriter sr = new StreamWriter("polje.txt"))
            {
                foreach (Tocka ze in list_Tocaka)
                    sr.WriteLine(ze.t.X + ";" + ze.t.Y);
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
            
            //ode se rjesava sve
            put=Rjesi(put, list_Tocaka);

           
            

            //ove 2 petlje crtaju put i tocke
            for(int i=0;i<put.Count-1;i++)
            {
                drawArea.DrawLine(blackPen, put[i].t, put[i + 1].t);
                put[i].DrawZuto(drawArea, blackPen);
            }

            foreach (Tocka t in list_Tocaka)
            {
                t.Draw(drawArea, blackPen);
            }
            //-----------
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            drawArea.Clear(Color.White);
            Pen blackPen = new Pen(Color.Black);
            

            for (int i = 0; i < put.Count - 1; i++)
            {
                drawArea.DrawLine(blackPen, put[i].t, put[i + 1].t);
                put[i].DrawZuto(drawArea, blackPen);
            }

            foreach (Tocka t in list_Tocaka)
            {
                t.Draw(drawArea, blackPen);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            list_Tocaka.Clear();
            using (StreamReader sr = new StreamReader("polje1.txt"))
            {
                int x;
                int y;
                string[] inf;
                string linija = sr.ReadLine();
                
                
                while (linija != null)
                {
                    inf = linija.Split(';');
                    x=int.Parse(inf[0]);
                    y=int.Parse(inf[1]);
                    list_Tocaka.Add(new Tocka(new Point(x, y)));
                    linija = sr.ReadLine();
                }
            }
        }

        //triba sastavit po (x,y)
        private void button5_Click(object sender, EventArgs e)
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

                niz_Tocaka[i] = new Tocka(new Point(r.Next(50, 600), r.Next(0, 260)));
                lista_crta.Add(new Pravac(AA, niz_Tocaka[i]));
                lista_crta.Add(new Pravac(niz_Tocaka[i], BB));
                niz_Tocaka[i].Draw(drawArea, blackPen);
            }

            foreach (Pravac perica in lista_crta)
            {
                perica.Draw(drawArea, blackPen);
            }


            int udaljenost_AB = Math.Abs(BB.t.Y - AA.t.Y);
            Tocka[] niz_tocaka_izbivenog_vektora = new Tocka[udaljenost_AB];


            for (int i = 0; i < udaljenost_AB; i++)
            {
                niz_tocaka_izbivenog_vektora[i] = new Tocka(new Point((int)Prosjecni_X(lista_crta, i, AA),AA.t.Y + i));
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
            min.DrawZuto(drawArea, blackPen);

            foreach (Tocka t in niz_tocaka_izbivenog_vektora)
            {

                t.DrawManje(drawArea, blackPen);
            }
        }


        
    }
}
