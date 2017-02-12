using System;
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
        double alpha = 0.3, betha = 0.6,p=0.2;
        double yolToplamlari;
        static Random rnd = new Random();
        public Ant[] ants;
        int antSayisi = 0;
        public Hesap(int _tepeSayisi,int _antSayisi,double[,] uzaklikVerileri)
        {
            tepeSayisi = _tepeSayisi;
            antSayisi = _antSayisi;
            yollardakiFeromonlar = new double[tepeSayisi,tepeSayisi];       
            yollardakiUzakliklar = uzaklikVerileri;

            antOlustur(_antSayisi);
            rastgeleFeromonAta();
            yolUzaklikTersi();
            for (int iterasyon = 0; iterasyon < 100; iterasyon++)
            {          
                antRastgeleKonumlandir();
              
                int _mevcutTepe = 0;
                for (int t = 0; t < tepeSayisi; t++)
                {
                    for (int i = 0; i < antSayisi; i++)//tüm karincalar icin gidilebilen tepeler belirlendi
                    {
                        ants[i].gidilebilenTepeler = gidilebilenTepeleriBelirle(ants[i]);
                        if (ants[i].gidilebilenTepeler == null) continue;
                        if (ants[i].gidilebilenTepeler.Count > 0)
                        {
                            _mevcutTepe = ants[i].gidilenTepeler.ElementAt(ants[i].gidilenTepeler.Count - 1);
                            ants[i].gidilenTepeler.Add(gecisKurali(_mevcutTepe, ants[i]));
                        }
                        ants[i].gidilebilenTepelerSifirla();
                    }
                }

                for (int i = 0; i < antSayisi; i++)
                {
                    ants[i].toplamYolGuncelle(antToplamYol(ants[i]));
                }

                buharlastirma();

                for (int i = 0; i < antSayisi; i++)
                {
                    lokalFeromonGuncelle(ants[i]);
                }

                globalFeromonGuncelle(ants);

               /* for (int i = 0; i < antSayisi; i++)
                {
                    ants[i].gidilenTepelerSifirla();
                    ants[i].toplamYolGuncelle(0);
                }*/

                showFeromon();
                showAntsGidilenTepeler();

                for (int i = 0; i < antSayisi; i++)
                {
                    ants[i].gidilenTepelerSifirla();
                    ants[i].toplamYolGuncelle(0);
                }
            }               
        }

      /*  public void uzaklikAta(int i,int j,double uzaklik)
        {
            yollardakiUzakliklar[i, j] = uzaklik;
        }*/

        public void rastgeleFeromonAta()
        {
            for(int i = 0; i < tepeSayisi-1; i++)
            {
                for(int j = i + 1; j < tepeSayisi; j++)
                {
                    yollardakiFeromonlar[i, j] =rnd.NextDouble();
                    yollardakiFeromonlar[j,i] = yollardakiFeromonlar[i,j];
                }
            }
        }
        public void antRastgeleKonumlandir()
        {
            int tepe = 0;
            for(int i=0;i<antSayisi;i++)
            {
                tepe = rnd.Next() % tepeSayisi;
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
        //public void gecisKurali(int mevcutTepe,List<int>gidilebilenTepeler)
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

            /*ant.gidilenTepeler.Add(gidilcekTepe);
            ant.gidilebilenTepelerSifirla();*/
            //Gidilcek Tepeyi Döndür ! 
            return gidilcekTepe;
        }
        public List<int> gidilebilenTepeleriBelirle(Ant ant)
        {
            if(ant.gidilenTepeler.Count>0)
            {
                // int mevcutTepe = ant.gidilebilenTepeler.ElementAt(ant.gidilenTepeler.Count - 1);
                int mevcutTepe = ant.gidilenTepeler.ElementAt(ant.gidilenTepeler.Count - 1);
                for (int j = 0; j < tepeSayisi; j++)
                {
                    if (yollardakiUzakliklar[mevcutTepe, j] != 0)
                    {
                        if (!ant.gidilenTepeler.Contains(j))
                        {
                            ant.gidilebilenTepeler.Add(j);
                        }
                    }
                }
            }

            //gidilebilen Tepeleri Döndür !
            return ant.gidilebilenTepeler;
        }
        public int gecisKuraliFeromonunMaxOldugu(int mevcutTepe, List<int> gidilebilenTepeler)
        {
            double max = 0;
            double d = 0;
            int index = 0;
            for (int i = 0; i < gidilebilenTepeler.Count; i++)
            {
                // d = gecisKuraliferomonHesabi(mevcutTepe, gidilebilenTepeler.IndexOf(i));
                d = gecisKuraliferomonHesabi(mevcutTepe, gidilebilenTepeler.ElementAt(i));
                if (max < d)
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
            // double[,] olasiliklar = new double[2,gidilebilenTepeler.Count];
            double[] olasiliklar = new double[gidilebilenTepeler.Count];
            double[] roulette = new double[gidilebilenTepeler.Count+1];
            int p = rnd.Next() % gidilebilenTepeler.Count;
            int gidilcekTepe = 0;

            for(int i = 0; i < gidilebilenTepeler.Count; i++)
            {
                // gecisKuraliFeromonToplami += gecisKuraliferomonHesabi(mevcutTepe, gidilebilenTepeler.IndexOf(i));
                gecisKuraliFeromonToplami += gecisKuraliferomonHesabi(mevcutTepe, gidilebilenTepeler.ElementAt(i));
            }

            for(int i=0;i<gidilebilenTepeler.Count;i++)
            {
                //  olasiliklar[0, i] = i;//Hangi tepeye gidicegini tuttuk
                // olasiliklar[1, i] = gecisKuraliferomonHesabi(mevcutTepe, gidilebilenTepeler.IndexOf(i)) / gecisKuraliFeromonToplami;//Hangi olasilikla gidecegini tuttuk
             //   olasiliklar[i] = gecisKuraliferomonHesabi(mevcutTepe, gidilebilenTepeler.IndexOf(i)) / gecisKuraliFeromonToplami;//Hangi olasilikla gidecegini tuttuk
                olasiliklar[i] = gecisKuraliferomonHesabi(mevcutTepe, gidilebilenTepeler.ElementAt(i)) / gecisKuraliFeromonToplami;
            }

            //roulette icin aralik belirle
            //roulette[0] = olasiliklar[1, 0];
            //roulette[0] = olasiliklar[1, 0];
            roulette[0] = 0;
            for (int i = 0; i < gidilebilenTepeler.Count; i++)
            {
                //roulette[i+1] = roulette[i] + olasiliklar[1, i];
                roulette[i + 1] = roulette[i] + olasiliklar[i];
            }
          
            for(int i = 0; i < gidilebilenTepeler.Count; i++)
            {
                if(p>=roulette[i] && p <= roulette[i+1])
                {
                    gidilcekTepe = gidilebilenTepeler.ElementAt(i);//tepeleri 0. indiste tutmustuk
                }
            }

            return gidilcekTepe;
        }
        public void buharlastirma()
        {
            for(int i = 0; i < tepeSayisi; i++)
            {
                for(int j = 0; j < tepeSayisi; j++)
                {
                    yollardakiFeromonlar[i, j] = (1 - p) * yollardakiFeromonlar[i, j];
                }
            }            
        }
        public double antToplamYol(Ant ant)
        {
            double toplamYol = 0;
            for (int i = 0; i < ant.gidilenTepeler.Count - 1; i++)
            {
                //yollardakiFeromonlar[ant.gidilenTepeler.ElementAt(i), ant.gidilenTepeler.ElementAt(i + 1)]+=1/;
                toplamYol += 1 / yollardakiUzakliklar[ant.gidilenTepeler.ElementAt(i), ant.gidilenTepeler.ElementAt(i + 1)];
            }
            return toplamYol;
        }
        public void lokalFeromonGuncelle(Ant ant)//her karinca icin cagirilcak
        {         
            for (int i = 0; i < ant.gidilenTepeler.Count - 1; i++)
            {
                //yollardakiFeromonlar[ant.gidilenTepeler.ElementAt(i), ant.gidilenTepeler.ElementAt(i + 1)]+=1/;
                yollardakiFeromonlar[ant.gidilenTepeler.ElementAt(i), ant.gidilenTepeler.ElementAt(i + 1)] += 1 / ant.toplamYol;
                //alttaki ve ustteki yollardakiUzakliklardi
                yollardakiFeromonlar[ant.gidilenTepeler.ElementAt(i+1), ant.gidilenTepeler.ElementAt(i )] += 1 / ant.toplamYol;//simetrigini ekledim
            }
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

            for(int i = 0; i < ants[indis].gidilenTepeler.Count-1; i++)
            {
                int k = _ants[indis].gidilenTepeler.ElementAt(i);
                int t = _ants[indis].gidilenTepeler.ElementAt(i+1);

                yollardakiFeromonlar[k, t] = (1 - p) * yollardakiFeromonlar[k,t]+1/_ants[indis].toplamYol;
                yollardakiFeromonlar[t,k] = (1 - p) * yollardakiFeromonlar[t,k]+1/_ants[indis].toplamYol;//simetrigni ekledim
                
            }            
        }
        public int[] maxFeromonTepe()
        {
            double max = 0;
            int[] tepe = new int[2];
            for(int i = 1; i < tepeSayisi-1; i++)
            {
                for(int j = i+1; j < tepeSayisi; j++)
                {
                    if (max < yollardakiFeromonlar[i, j])
                    {
                        max = yollardakiFeromonlar[i, j];
                        tepe[0] = i;
                        tepe[1] = j;
                    }
                }
            }

            return tepe;
        }
        public void showFeromon()
        {
            Console.WriteLine("------------");
            for(int i=0;i< tepeSayisi;i++)
            {
                Console.WriteLine("");
                for (int j=0;j<tepeSayisi;j++)
                {
                    Console.Write(yollardakiFeromonlar[i,j]+" ; ");
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
