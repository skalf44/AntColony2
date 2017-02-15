﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColony
{
    class Hesap
    {
        int tepeSayisi=0;
        public double[,] yollardakiFeromonlar;
        double[,] yollardakiUzakliklar;
        double alpha = 0.5, betha =0.5,p=0.1;
        static Random rnd = new Random();
        public Ant[] ants;
        public int yiyecekTepesi=6, yuvaTepesi=0;
        int antSayisi = 0;
        double cezaPuani = 1;
        public Hesap(int _tepeSayisi,int _antSayisi,double[,] uzaklikVerileri)
        {
            tepeSayisi = _tepeSayisi;
            antSayisi = _antSayisi;
            yollardakiFeromonlar = new double[tepeSayisi,tepeSayisi];       
            yollardakiUzakliklar = uzaklikVerileri;

            antOlustur(_antSayisi);
            rastgeleFeromonAta();
            yolUzaklikTersi();
            
            for (int iterasyon = 0; iterasyon < 1000; iterasyon++)
            {
                gidis(yuvaTepesi, yiyecekTepesi);
                gidis(yiyecekTepesi,yuvaTepesi);               
            }               
        }
        public void gidis(int _yuvaTepesi, int _yiyecekTepesi)
        {
            antKonumlandir(_yuvaTepesi);

            int _mevcutTepe = 0;
            int time = 0;
            for (int i = 0; i < antSayisi; i++)
            {
                time = 0;
                ants[i].sure = 0;
                while (!ants[i].gidilenTepeler.Contains(_yiyecekTepesi))
                {
                    time++;
                    _mevcutTepe = ants[i].gidilenTepeler.ElementAt(ants[i].gidilenTepeler.Count - 1);
                    ants[i] = gidilebilenTepeleriBelirle(ants[i]);
                    int teta = gecisKurali(_mevcutTepe, ants[i]);
                    String s0 = _mevcutTepe + "," +teta;
                    String s1 = teta+","+_mevcutTepe;
                    ants[i].gidilenTepeler.Add(teta);
                    ants[i].gidilenYol.Add(s1);

                    if (!ants[i].gidilenYolTamami.Contains(s0) && !ants[i].gidilenYolTamami.Contains(s1))
                        ants[i].gidilenYolTamami.Add(s0);

                    ants[i].gidilebilenTepelerSifirla();
                }
                ants[i].sure = time;

            }

            for (int i = 0; i < antSayisi; i++)
            {
                ants[i].toplamYolGuncelle(antToplamYol(ants[i]));
            }

            for (int i = 0; i < antSayisi; i++)
            {
                buharlastirma(ants[i]);
            }
            for (int i = 0; i < antSayisi; i++)
            {
                lokalFeromonGuncelle(ants[i]);
            }

            globalFeromonGuncelle2(ants);

            showFeromon();
            showAntsGidilenTepeler();

            for (int i = 0; i < antSayisi; i++)
            {
                ants[i].gidilenTepelerSifirla();
                ants[i].gidilenYol.Clear();
                ants[i].toplamYolGuncelle(0);
            }
        }

        public void rastgeleFeromonAta()
        {
            for(int i = 0; i < tepeSayisi-1; i++)
            {
                for(int j = i + 1; j < tepeSayisi; j++)
                {
                    yollardakiFeromonlar[i, j] =1;
                    yollardakiFeromonlar[j,i] = yollardakiFeromonlar[i,j];
                }
            }
        }
        public void antKonumlandir(int yuva)
        {
            int tepe = 0;
            for(int i=0;i<antSayisi;i++)
            {
                //tepe = rnd.Next() % 1;
                tepe = yuva;
                    ants[i].gidilenTepeler.Add(tepe);
            }
               //ant lari rastgele konumlandirdik
        }
        public void antOlustur(int _antSayisi)
        {
            ants = new Ant[_antSayisi];
            for (int i = 0; i < _antSayisi; i++)
            {
                ants[i] = new Ant();               
            }
        }
        public void yolUzaklikTersi()
        {
            for(int i = 0; i < tepeSayisi; i++)
            {
                for(int j=0;j<tepeSayisi; j++)
                {
                    if (yollardakiUzakliklar[i, j] == 0) continue;
                    yollardakiUzakliklar[i, j] = 1 / yollardakiUzakliklar[i, j];
                }
            }
        }
        public int gecisKurali(int mevcutTepe, Ant ant)
        {
            int q = rnd.Next()%101;
            int gidilcekTepe = 0;

            if (q <= 90)
            {
              gidilcekTepe = gecisKuraliFeromonunMaxOldugu(mevcutTepe,ant.gidilebilenTepeler);
             
            }
            else
            {
              gidilcekTepe = rouletteWheel(mevcutTepe, ant.gidilebilenTepeler);
               
            }
            
            //Gidilcek Tepeyi Döndür ! 
          
            return gidilcekTepe;
        }

        public Ant gidilebilenTepeleriBelirle(Ant ant)
        {
            if(ant.gidilenTepeler.Count>0)
            {
             
                int mevcutTepe = ant.gidilenTepeler.ElementAt(ant.gidilenTepeler.Count - 1);
                for (int j = 0; j < tepeSayisi; j++)
                {
                    if (yollardakiUzakliklar[mevcutTepe, j] != 0 && yollardakiUzakliklar[j, mevcutTepe] != 0)
                    {
                        
                        if (!ant.gidilenYol.Contains(mevcutTepe+","+j) && !ant.gidilenYol.Contains(j + "," + mevcutTepe))
                        {
                            ant.gidilebilenTepeler.Add(j);
                        }
                    }
                }
            }
            if (ant.gidilebilenTepeler.Count == 0)//gidebilecegi yol yoksa geri donecek ve geri dondugu tepe gidilen tepelere eklencek
            {
                ant.gidilenYol.Clear();
                gidilebilenTepeleriBelirle(ant);
            }
      
            return ant;
        }
        public int gecisKuraliFeromonunMaxOldugu(int mevcutTepe, List<int> gidilebilenTepeler)
        {
            double max = 0;
            double d = 0;
            int index=0;
            for (int i = 0; i < gidilebilenTepeler.Count; i++)
            {

                d = gecisKuraliferomonHesabi(mevcutTepe, gidilebilenTepeler.ElementAt(i));
                if (max <= d)
                {
                    max = d;
                    index = i;
                   
                }
            }
          
            return gidilebilenTepeler.ElementAt(index);
        }
        double gecisKuraliferomonHesabi(int mevcut,int gidilecek)
        {           
            
            return (Math.Pow(yollardakiUzakliklar[mevcut,gidilecek],alpha)* Math.Pow(yollardakiFeromonlar[mevcut, gidilecek], betha) );
            
        }
        public int rouletteWheel(int mevcutTepe, List<int> gidilebilenTepeler)
        {
            double gecisKuraliFeromonToplami = 0;
            double[] olasiliklar = new double[gidilebilenTepeler.Count];
            double[] roulette = new double[gidilebilenTepeler.Count+1];
            int p = rnd.Next() % 1;
            int gidilcekTepe = -1;

            for(int i = 0; i < gidilebilenTepeler.Count; i++)
            {
                gecisKuraliFeromonToplami += gecisKuraliferomonHesabi(mevcutTepe, gidilebilenTepeler.ElementAt(i));
            }

            for(int i=0;i<gidilebilenTepeler.Count;i++)
            {                
                olasiliklar[i] = gecisKuraliferomonHesabi(mevcutTepe, gidilebilenTepeler.ElementAt(i)) / gecisKuraliFeromonToplami;
            }
            roulette[0] = 0;
            for (int i = 0; i < gidilebilenTepeler.Count; i++)
            {
                roulette[i + 1] = roulette[i] + olasiliklar[i];
            }
          
            for(int i = 0; i < gidilebilenTepeler.Count; i++)
            {
                if(p>=roulette[i] && p <= roulette[i+1])
                {
                    gidilcekTepe = gidilebilenTepeler.ElementAt(i);
                }
               
            }
            if (gidilebilenTepeler.Count==0 || gidilcekTepe==-1)
            {
                Console.Write("Girmedi !!!!!!!!!!!!!!!!");
            }
            Console.WriteLine("Roulette**");
            for(int i = 0; i < gidilebilenTepeler.Count+1; i++)
            {
                Console.Write(roulette[i]+";");
            }
            Console.WriteLine("****");
            return gidilcekTepe;
        }
        public void buharlastirma(Ant ant)
        {
            int []xy=new int[2];
            for(int i=0;i<ant.gidilenYolTamami.Count;i++)
            {
                xy = splitAntYol(ant, i);
                yollardakiFeromonlar[xy[0], xy[1]] =(1 - p) * yollardakiFeromonlar[xy[0],xy[1]];
                yollardakiFeromonlar[xy[1], xy[0]] = yollardakiFeromonlar[xy[0], xy[1]];
            }              
        }
        public double antToplamYol(Ant ant)
        {
            double toplamYol = -2;
            for (int i = 0; i < ant.gidilenTepeler.Count - 1; i++)
            {
                int m, g;
                List<String> saydim = new List<String>();//bulundugu iterasyonda aynı yola birden fazla kez feromon koymaması icin
                m = ant.gidilenTepeler.ElementAt(i);
                g = ant.gidilenTepeler.ElementAt(i + 1);
                if(!saydim.Contains(m+","+g) && !saydim.Contains(g+","+m))
                {
                    saydim.Add(m + "," + g);
                    if(yollardakiUzakliklar[ant.gidilenTepeler.ElementAt(i), ant.gidilenTepeler.ElementAt(i + 1)]!=0)
                     toplamYol += 1 / yollardakiUzakliklar[ant.gidilenTepeler.ElementAt(i), ant.gidilenTepeler.ElementAt(i + 1)];
                }                
            }
            return toplamYol;
        }

        public int[] splitAntYol(Ant ant,int element)
        {
            String[] ss = new String[2];
            int[] xy = new int[2];
            ss = ant.gidilenYolTamami.ElementAt(element).Split(',');
            xy[0] = Convert.ToInt32(ss[0]);
            xy[1] = Convert.ToInt32(ss[1]);
            return xy;
        }
        public void lokalFeromonGuncelle(Ant ant)//her karinca icin cagirilcak
        {

            for (int i = 0; i < ant.gidilenTepeler.Count - 1; i++)
            {
                int m, g;
                List<String> saydim = new List<String>();//bulundugu iterasyonda aynı yola birden fazla kez feromon koymaması icin
                m = ant.gidilenTepeler.ElementAt(i);
                g = ant.gidilenTepeler.ElementAt(i + 1);
                if (!saydim.Contains(m + "," + g) && !saydim.Contains(g + "," + m))
                {
                    saydim.Add(m + "," + g);

                    // yollardakiFeromonlar[m,g] += 1 / ant.toplamYol;
                    //yollardakiFeromonlar[m, g] -=ant.sure/(ant.sure*cezaPuani);
                    yollardakiFeromonlar[m, g] += 1 / (ant.toplamYol)+1/ant.sure;
                    yollardakiFeromonlar[g, m] = yollardakiFeromonlar[m, g];
                }
            }
            

/*
            for (int i = 0; i < ant.gidilenYol.Count - 1; i++)
            {
                int[] xy = new int[2];

                xy=splitAntYol(ant, i);
                yollardakiFeromonlar[xy[0],xy[1]] += 1 / ant.toplamYol;
                yollardakiFeromonlar[xy[1],xy[0]] = yollardakiFeromonlar[xy[0], xy[1]];//simetrigini ekledim
            }*/
        }
     
        public void globalFeromonGuncelle(Ant[] _ants)
        {
            double minYol = double.MaxValue;
            int indis = 0;
            for(int i=0;i< _ants.Count(); i++)//en iyi tur belirlenir
            {
                if (_ants[i].toplamYol < minYol)
                {
                    minYol = _ants[i].toplamYol;
                    indis = i;
                }
            }

            for (int i = 0; i < _ants[indis].gidilenYol.Count - 1; i++)
            {

                int[] xy = new int[2];
                xy = splitAntYol(_ants[indis], i);

                yollardakiFeromonlar[xy[0],xy[1]] = (1 - p) * yollardakiFeromonlar[xy[0],xy[1]]+1/_ants[indis].toplamYol;
                yollardakiFeromonlar[xy[1],xy[0]] = yollardakiFeromonlar[xy[0], xy[1]];//simetrigni ekledim
                
            }
        }


        double globalRotaDegeri = double.MaxValue;
        List<String> globalRota = new List<String>();
        public void globalFeromonGuncelle2(Ant[] _ants)
        {
            double minYol = double.MaxValue;
            int indis = 0;
            for (int i = 0; i < _ants.Count(); i++)//en iyi tur belirlenir
            {
                if (_ants[i].toplamYol < minYol)
                {
                    minYol = _ants[i].toplamYol;
                    indis = i;
                }
            }
            if (globalRotaDegeri > minYol)
            {
                globalRota.Clear();
                globalRotaDegeri = minYol;

                for (int i = 0; i <_ants[indis].gidilenYolTamami.Count; i++)//en iyi tur belirlenir
                {
                    globalRota.Add(ants[indis].gidilenYolTamami.ElementAt(i));
                }

            }
            for (int i = 0; i < globalRota.Count; i++)
            {
                String[] ss = new String[2];
                int[] xy = new int[2];
                ss = globalRota.ElementAt(i).Split(',');
                xy[0] = Convert.ToInt32(ss[0]);
                xy[1] = Convert.ToInt32(ss[1]);

                yollardakiFeromonlar[xy[0], xy[1]] = (1 - p) * yollardakiFeromonlar[xy[0], xy[1]] + 1 / _ants[indis].toplamYol;
                yollardakiFeromonlar[xy[1], xy[0]] = yollardakiFeromonlar[xy[0], xy[1]];//simetrigni ekledim

            }
        }

        public void showFeromon()
        {
            Console.WriteLine("------------");
            for(int i=0;i< tepeSayisi;i++)
            {
                Console.WriteLine("");
                for (int j=0;j<tepeSayisi;j++)
                {
                    Console.Write("\t"+Math.Round(yollardakiFeromonlar[i, j],3) +" ; ");
                }
            }
        }
        public void showAntsGidilenTepeler()
        {
            Console.WriteLine("\n---Karinca Gidilen Tepeler---");
            for (int i = 0; i < antSayisi; i++)
            {
                Console.WriteLine("");
                for (int j = 0; j < ants[i].gidilenTepeler.Count; j++)
                {
                    Console.Write(ants[i].gidilenTepeler.ElementAt(j) + "\t");

                }
               Console.WriteLine( " Toplam Yol = " + ants[i].toplamYol);
            }
            
        }
    }
}
