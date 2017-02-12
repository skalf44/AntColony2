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
        public double toplamYol;      
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

        public void toplamYolGuncelle(double t)
        {
            toplamYol = t;
        }
    }
}
