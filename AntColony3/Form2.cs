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
    public partial class Form2 : Form
    {
        TextBox[,] uzaklikVerisi;
        double[,] uzaklikMatrisi;
        int tepeSayisi, antSayisi;
        public Form2(int _tepeSayisi, int _antSayisi)
        {
            InitializeComponent();
            tepeSayisi = _tepeSayisi;
            antSayisi = _antSayisi;
            uzaklikVerisi = new TextBox[tepeSayisi, tepeSayisi];
            uzaklikMatrisi = new double[tepeSayisi, tepeSayisi];
            for (int i = 0; i < tepeSayisi; i++)
            {
                for (int j = 0; j < tepeSayisi; j++)
                {
                    uzaklikVerisi[i, j] = new TextBox();
                    uzaklikVerisi[i, j].Text = "0";
                }
            }
            int[] boy = new int[2];
            int x = 50, y = 50;
            for (int i = 0; i < tepeSayisi; i++)
            {
                for (int j = 0; j < tepeSayisi; j++)
                {

                    uzaklikVerisi[i, j].Width = 20;
                    uzaklikVerisi[i, j].Height = 10;

                    uzaklikVerisi[i, j].Top = y;
                    uzaklikVerisi[i, j].Left = x;
                    this.Controls.Add(uzaklikVerisi[i, j]);
                    x += 30;
                    if (i == j)
                    {
                        uzaklikVerisi[i, j].Text = "0";
                    }
                }

                y += 30;
                x = 50;
            }

            textBoxDegerAta();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < tepeSayisi; i++)
            {
                for (int j = i; j < tepeSayisi; j++)
                {
                    uzaklikMatrisi[i, j] = Convert.ToDouble(uzaklikVerisi[i, j].Text);
                    uzaklikMatrisi[j, i] = uzaklikMatrisi[i, j];
                }
            }

            
            Hesap hesap = new Hesap(tepeSayisi, antSayisi, uzaklikMatrisi);
            
            showFeromonMatrix(hesap.yollardakiFeromonlar);

            richTextBox1.Text += "\nKarincalarin Gittigi Yollar\n-----------\n";
            for (int i=0;i<antSayisi;i++)
            {
                showAnts(hesap.ants[i]);
            }
        }
     
         public void showFeromonMatrix(double[,] fero)
        {            
            richTextBox1.Text += "Yollardaki Feromonlar\n-----------\n";
            for (int i = 0; i < tepeSayisi; i++)
            {
                richTextBox1.Text += "\n";
                for (int j = 0; j < tepeSayisi; j++)
                {
                    richTextBox1.Text+=Math.Round(fero[i, j],3) + "\t";
                }
            }
        }

        public void showAnts(Ant ant)
        {
        
            for (int j = 0; j < ant.gidilenTepeler.Count; j++)
            {
                richTextBox1.Text += ant.gidilenTepeler.ElementAt(j)+ "\t";                
            }
           
        }

        public void textBoxDegerAta()
        {
            for(int i = 0; i < tepeSayisi; i++)
            {
                for(int j= i;j<tepeSayisi;j++)
                {
                    
                    uzaklikVerisi[i, j].Text = j+"";                    
                    uzaklikVerisi[j, i].Text = uzaklikVerisi[i, j].Text;
                    if (i == j) uzaklikVerisi[i, j].Text = "0";
                    else
                        uzaklikVerisi[i, j].BackColor = Color.Yellow;
                }
            }
        }

    }
}
