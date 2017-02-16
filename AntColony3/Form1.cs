using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntColony
{
    public partial class Form1 : Form
    {       
        int antSayisi, tepeSayisi;
        Form2 f2;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            antSayisi = Convert.ToInt32(textBox1.Text);
            tepeSayisi= Convert.ToInt32(textBox2.Text);
            f2 = new Form2(tepeSayisi,antSayisi);
         //   this.Controls.Add(f2);
            f2.Show();                                    
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            antSayisi = Convert.ToInt32(textBox1.Text);
           // tepeSayisi = Convert.ToInt32(textBox2.Text);
            AntColony3.CsvOkuma csvOkuma = new AntColony3.CsvOkuma(textBox3.Text);//BU KISIM ONEMLI !!!            
            tepeSayisi=csvOkuma.num_cols;
            double[,] uzaklikMatrisi = csvOkuma.getDoubleSet();
            csvOkuma.doublesetGoster();
            Console.WriteLine("??????????????????????*");
            Console.WriteLine(tepeSayisi+"");
            Hesap hesap = new Hesap(tepeSayisi, antSayisi, uzaklikMatrisi);

            //showFeromonMatrix(hesap.yollardakiFeromonlar);

           /* richTextBox1.Text += "\nKarincalarin Gittigi Yollar\n-----------\n";
            for (int i = 0; i < antSayisi; i++)
            {
                showAnts(hesap.ants[i]);
            }*/
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
