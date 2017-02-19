using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColony
{
   public class Ant
    {
        public List<int> gidilenTepeler=new List<int>();//ilk tepe baslangic noktasi olcak
        public List<int> gidilebilenTepeler = new List<int>();
        public double toplamYol,sure=0;
        public List<String> gidilenYol = new List<String>();//gidilenTepeler 1,2,3 diye tutuyor gidilenyol 1,2;2,3 seklinde tutuyor.(kod yaziminda kolaylik saglamasi icin yaptim) gidilcek yol kalmazsa gidilenYol 0'lanacak . her iterasyonda 0 lanacak.
        public List<String> gidilenYolTamami = new List<String>();//gidilecek olan tepeye gidilmemişse ekleme yapılcak
        public bool yemekBuldu = false;     
        
        public Ant()
        {          

        }
        public void gidilebilenTepelerSifirla()
        {
            gidilebilenTepeler.Clear();
        }
        public void gidilenTepelerSifirla()
        {
            gidilenTepeler.Clear();
        }

        public void sifirla()
        {            
            gidilenTepelerSifirla();
            gidilenYol.Clear();
            gidilenYolTamami.Clear();/// yeni ekledim
            toplamYolGuncelle(0);            
        }

        public void toplamYolGuncelle(double t)
        {
            toplamYol = t;
        }
    }
}
