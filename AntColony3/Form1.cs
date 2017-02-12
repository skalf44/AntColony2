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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
