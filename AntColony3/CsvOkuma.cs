using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColony3
{
   public class CsvOkuma
    {
        public int num_rows, num_cols;
        public String[,] dataset;
        public double[,] normalset;
        public double[,] doubleset;
        public int[,] intset;

        public CsvOkuma(string s)
        {
            dataset = LoadCsv(s);
        }
        private string[,] LoadCsv(string filename)
        {
            string whole_file = System.IO.File.ReadAllText(filename);
            whole_file = whole_file.Replace('\n', '\r');
            string[] lines = whole_file.Split(new char[] { '\r' }, StringSplitOptions.RemoveEmptyEntries);

            num_rows = lines.Length;
            num_cols = lines[0].Split(';').Length;

            string[,] values = new string[num_rows, num_cols];
            for (int r = 0; r < num_rows; r++)
            {
                string[] line_r = lines[r].Split(';');
                for (int c = 0; c < num_cols; c++)
                {
                    values[r, c] = line_r[c];
                }
            }
            return values;
        }
        public String[,] getDataSet()
        {
            return dataset;
        }
        public double[,] getDoubleSet()
        {
            toDouble();
            return doubleset;
        }
        public double[,] getNormalSet()
        {
            toNormalize();
            return normalset;
        }

        public int[,] getIntSet()
        {
            toInt();
            return intset;
        }
        public void datasetGoster()
        {
            Console.WriteLine("----Veriler Dataset----");
            for (int i = 0; i < num_rows; i++)
            {
                for (int j = 0; j < num_cols; j++)
                {
                    Console.Write(dataset[i, j] + ";");
                }
                Console.WriteLine("-------------");
            }
        }
        public void doublesetGoster()
        {
            Console.WriteLine("----Veriler Doubleset----");
            for (int i = 0; i < num_rows; i++)
            {
                for (int j = 0; j < num_cols; j++)
                {
                    Console.Write(doubleset[i, j] + ";");
                }
                Console.WriteLine("---------------");
            }
        }
        public void normalsetGoster()
        {
            Console.WriteLine("----Veriler Normalset----");
            for (int i = 0; i < num_rows; i++)
            {
                for (int j = 0; j < num_cols; j++)
                {
                    Console.Write(normalset[i, j] + ";");
                }
                Console.WriteLine("-------------");
            }
        }
        public void intsetGoster()
        {
            Console.WriteLine("----Veriler intset----");
            for (int i = 0; i < num_rows; i++)
            {
                for (int j = 0; j < num_cols; j++)
                {
                    Console.Write(intset[i, j] + ";");
                }
                Console.WriteLine("---------------");
            }
        }
        public void toInt()
        {
            intset = new int[num_rows, num_cols];
            for (int i = 0; i < num_rows; i++)
            {
                for (int j = 0; j < num_cols; j++)
                {
                    intset[i, j] = Convert.ToInt32(dataset[i, j]);
                }
            }
        }
        public void toDouble()
        {
            doubleset = new double[num_rows, num_cols];
            for (int i = 0; i < num_rows; i++)
            {
                for (int j = 0; j < num_cols; j++)
                {
                    doubleset[i, j] = Convert.ToDouble(dataset[i, j]);
                }
            }
        }
        public void toNormalize()
        {
            toDouble();
            normalset = new double[num_rows, num_cols];
            for (int j = 0; j < num_rows; j++)
            {
                int sutun = sutunMin(j);
                for (int i = 0; i < num_cols; i++)
                {
                    normalset[i, j] = (doubleset[i, j] - doubleset[i, sutun]) / 255;
                }
            }
        }
        public int sutunMin(int j)
        {
            double min = Double.MaxValue;
            int satir = 0;
            for (int k = 0; k < num_rows; k++)
            {
                if (doubleset[k, j] < min)
                {
                    min = doubleset[k, j];
                    satir = j;
                }
            }
            return satir;
        }
    }
}
