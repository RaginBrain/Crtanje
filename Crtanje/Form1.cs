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

            foreach (Pravac pravac in lst)
            {

                //ovaj uvjet je blesav kad je pravac vertikalan 
                if (pravac.T1.x <= x && x<= pravac.T2.x)
                {
                    rjesenje += pravac.IzracunajYza(x);
                    koliko++;
                }
            }

            rjesenje = (int)rjesenje / koliko;
            return rjesenje;
        }

        float Prosjecni_Kosi_Y(List<Pravac> lst, int x, Tocka poc,Pravac kosi, Tocka rubA, Tocka krajB)
        {
            float rjesenje = 0;
            int koliko = 0;
            x += poc.t.X;

            
            foreach (Pravac pravac in lst)
            {           
                                
                if (pravac.T1.x <= x && x <= pravac.T2.x)
                {
                    
                }
            } 

           


            return rjesenje;
        }

       


        List<Pravac> lista_crta=new List<Pravac>();
        Graphics drawArea;

        //tocke AA i BB su pocetna i zavrsna tocka
        Tocka AA = new Tocka(new Point(150, 200));
        Tocka BB = new Tocka(new Point(500, 100));

        List<Tocka> put = new List<Tocka>();

        Tocka[] niz_Tocaka = new Tocka[15];
        List<Tocka> list_Tocaka = new List<Tocka>();



        public List<Tocka> Rjesi(List<Tocka> put, List<Tocka> niz_Tocaka)
        {
            int pozicija = 0;
            Tocka min = niz_Tocaka[0];
            float mini = 2000;
            Tocka A = null;
            Tocka B = null;

            //petlja provjerava svaku dionicu sadasnjeg puta
            for (int j = 0; j < put.Count - 1; j++)
            {
                A = put[j];
                B = put[j + 1];
                Pravac p = new Pravac(A, B);
                lista_crta.Add(p);

                for (int i = 0; i < niz_Tocaka.Count(); i++)
                {
                    lista_crta.Add(new Pravac(A, niz_Tocaka[i]));
                    lista_crta.Add(new Pravac(niz_Tocaka[i], B));
                }


                //slaže se niz točaka prosjecne_krivulje (izbiveni_vektor)
                int udaljenost_AB = Math.Abs(B.t.X - A.t.X);
                Tocka[] niz_tocaka_izbivenog_vektora = new Tocka[udaljenost_AB];


                for (int i = 0; i < udaljenost_AB; i++)
                {
                    niz_tocaka_izbivenog_vektora[i] = new Tocka(new Point(A.t.X + i, (int)Prosjecni_Y(lista_crta, i, A)));
                }
                //----


                //provjerava koja je tocka najblize izbivenom vektoru (krivulji)
                foreach (Tocka t in niz_Tocaka)
                {
                    foreach (Tocka s in niz_tocaka_izbivenog_vektora)
                        if (s.Distanca(t) < mini)
                        {
                            mini = s.Distanca(t);
                            min = t;
                            pozicija = j + 1;
                        }
                }
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
            put.Insert(pozicija, min);

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
            Tocka[] niz_Tocaka = new Tocka[5];
            


            AA.Draw(drawArea, blackPen);
            BB.Draw(drawArea, blackPen);

            Pravac p = new Pravac(AA, BB);
            lista_crta.Add(p);
            
            for (int i = 0; i < 5; i++)
            {
               
                niz_Tocaka[i] = new Tocka(new Point(r.Next(50,600),r.Next(0,260)));
                lista_crta.Add(new Pravac(AA, niz_Tocaka[i]));
                lista_crta.Add(new Pravac(niz_Tocaka[i], BB));
                niz_Tocaka[i].Draw(drawArea, blackPen);
            }

            foreach (Pravac prav in lista_crta)
            {
                prav.Draw(drawArea, blackPen);
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
            Rjesi(put, list_Tocaka);

           
            

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

        
        private void button5_Click(object sender, EventArgs e)
        {
           
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            


        }

        private void button7_Click(object sender, EventArgs e)
        {
            Pen blackPen = new Pen(Color.Black);
            Pravac p = new Pravac(AA, BB);

            AA.Draw(drawArea, blackPen);
            BB.Draw(drawArea, blackPen);
            p.Draw(drawArea, blackPen);


            Pravac ort = new Pravac(p, BB);

            Tocka OB = new Tocka(new Point((int)ort.IzracunajXza(AA.t.Y), AA.t.Y));
            OB.Draw(drawArea, blackPen);


            int udaljenost_AB = BB.t.X - AA.t.X;
            int udaljenost_na_X_osi = OB.t.X - AA.t.X;

            float korak = (float)udaljenost_na_X_osi / (float)udaljenost_AB;
            int broj_ponavljanja =(int)(Math.Round(udaljenost_na_X_osi / korak));

            //skenopostav

            lista_crta.Clear();

            drawArea.Clear(Color.White);
            Random r = new Random();



            niz_Tocaka = new Tocka[10];
            lista_crta.Add(p);
            for (int i = 0; i < 10; i++)
            {

                niz_Tocaka[i] = new Tocka(new Point(r.Next(50, 400), r.Next(0, 260)));
                lista_crta.Add(new Pravac(AA, niz_Tocaka[i]));
                lista_crta.Add(new Pravac(niz_Tocaka[i], BB));
                niz_Tocaka[i].Draw(drawArea, blackPen);
            }

            foreach (Pravac prav in lista_crta)
            {
                prav.Draw(drawArea, blackPen);
            }



            Tocka[] niz_tocaka_izbivenog_vektora = new Tocka[udaljenost_AB];

            

            float X_os = AA.t.X;
            for (int i = 0; i < udaljenost_AB; i++)
            {
                float finalna_udaljenost = 0;
                bool unutra;
                int koliko=0;
                X_os += korak;
                Pravac ort_virtualna=new Pravac(p,new Tocka(new Point((int)Math.Round(X_os),AA.t.Y)));
                Tocka sjec_na_kosini = ort_virtualna.sjeciste_pravaca(p);
                


                foreach (Pravac virt_prav in lista_crta)
                {
                    unutra = false;
                    
                    Tocka virt_sjeciste = ort_virtualna.sjeciste_pravaca(virt_prav);

                    Pravac gran_ort_a = new Pravac(p, virt_prav.T1);
                    Pravac gran_ort_b = new Pravac(p, virt_prav.T2);

                    float granica_A_vrijednost = gran_ort_a.IzracunajXza(virt_sjeciste.t.Y);
                    float granica_B_vrijednost = gran_ort_b.IzracunajXza(virt_sjeciste.t.Y);



                    if (virt_sjeciste.t.X > granica_A_vrijednost && virt_sjeciste.t.X <= granica_B_vrijednost)
                        unutra = true;

                    if (unutra)
                    {
                        koliko++;
                        if (virt_sjeciste.t.X < sjec_na_kosini.t.X)
                            finalna_udaljenost += sjec_na_kosini.Distanca(virt_sjeciste);
                        else
                            finalna_udaljenost -= sjec_na_kosini.Distanca(virt_sjeciste);
                    }

                }
                if (koliko != 11)
                    MessageBox.Show(koliko.ToString());
                finalna_udaljenost = finalna_udaljenost / koliko;
                

                
                double finalni_X;
                double finalni_Y;
                if (finalna_udaljenost > 0)
                {
                    finalna_udaljenost *= finalna_udaljenost;
                    //Math.Round(test3.T1.t.X+Math.Sqrt(test3.cos*10000)),(int)Math.Round(test3.T1.t.Y+Math.Sqrt(test3.sin*10000)))
                    finalni_X = sjec_na_kosini.t.X - Math.Sqrt(ort_virtualna.cos * finalna_udaljenost);
                    finalni_Y = sjec_na_kosini.t.Y - Math.Sqrt(ort_virtualna.sin * finalna_udaljenost);
                }
                else
                {
                    finalna_udaljenost *= finalna_udaljenost;
                    finalni_X = sjec_na_kosini.t.X + Math.Sqrt(ort_virtualna.cos * finalna_udaljenost);
                    finalni_Y = sjec_na_kosini.t.Y + Math.Sqrt(ort_virtualna.sin * finalna_udaljenost);
                }

                niz_tocaka_izbivenog_vektora[i] = new Tocka(new Point((int)Math.Round(finalni_X),(int)Math.Round(finalni_Y)));

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

        private void button5_Click_2(object sender, EventArgs e)
        {
            Pen blackPen = new Pen(Color.Black);
            Pravac test1 = new Pravac(new Tocka(new Point(200,100)), new Tocka(new Point(300,150)));
            Pravac test2 = new Pravac(new Tocka(new Point(200,200)), new Tocka(new Point(350, 50)));
            Pravac test3 = new Pravac(test2, new Tocka(new Point(100, 50)));

            Tocka sjec = test3.sjeciste_pravaca(test2);
            sjec.Draw(drawArea, blackPen);

            //Tocka z = test1.sjeciste_pravaca(test2);

            //test1.Draw(drawArea, blackPen);
            //test2.Draw(drawArea, blackPen);
            //test3.Draw(drawArea, blackPen);
            //test3.T1.Draw(drawArea, blackPen);
            Tocka testna= new Tocka(new Point((int)Math.Round(test3.T1.t.X+Math.Sqrt(test3.cos*10000)),(int)Math.Round(test3.T1.t.Y+Math.Sqrt(test3.sin*10000))));
            MessageBox.Show(test3.T1.Distanca(testna).ToString());
            

            //testna.DrawZuto(drawArea, blackPen);
            
            //z.Draw(drawArea, blackPen);
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        


        
    }
}
