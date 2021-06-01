using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public const int razmer = 2;
        public double mu = 0.1;
        public int ma = 50;
        public double[] x_mid = new double[2];
        public PointF []  point =new PointF [1000];
        public PointF[] point2 = new PointF[1000];
        public PointF[] point3 = new PointF[1000]; 
        public int flag = 0,flag2=0;
        public int len = 0,len2=0,len3=0;       
        public double[] x = new double[razmer];
        public double[] tb1 = new double[6];
        public double[] tb2 = new double[6];
        public double pogr,f=0.0,f2=0.0;
        //public int cps;
        public int ox;
        HG qwerty = new HG();
        public Form1()
        {
            
            InitializeComponent();
            label16.Text = "f(x1,x2)=2*x1*x1 - 2*x1*x2 + 3*x2*x2 + x1 - 3*x2";
            label15.Text = "f(x1,x2)=x1*x1 + x2*x2 + 7*x2*x2 - 10*x1 - 15*x2";
            label19.Text = "5*x1 + 13*x2 - 51 <= 0\n15*x1 + 7*x2 - 107 <= 0";
            clear();
        }

        public void clear() 
        {
            len = 0; len2 = 0;
            richTextBox1.Text = "МЕТОД НСП:\n";
            richTextBox2.Text = "МЕТОД ЦПС:\n";
            x1.Enabled = true;
            x2.Enabled = true;
            eps.Enabled = true;
            x1.Text = "2";
            x2.Text = "2";
            eps.Text = "0,00001";
        }

        void text() 
        {
            float x1=(point[len - 1].X -(float)x_mid[0]);
            x1=x1/ma;
            float x2=(point[len - 1].Y -(float)x_mid[1]);
            x2 /= ma;
            f = 2 * x1 * x1 - 2 * x1 * x2 + 3 * x2 * x2 + x1 - 3 * x2;           
            richTextBox1.Text += "Точка минимума:\t" + "x1= " + x1.ToString() + "\tx2=" + x2.ToString() + "\n";
            richTextBox1.Text += "Значение функциив точке min: f(x1,x2) = " + f.ToString() + "\n";
            richTextBox1.Text += "Количество базовых шагов, проделанных методом: " + (len/3).ToString();                       
        }

        void text1()
        {
            float x1 = (point2[len2 - 1].X -(float)x_mid[0]);
            x1 /= ma;
            float x2 = (point2[len2 - 1].Y -(float)x_mid[1]);
            x2 /= ma;
            f2 = 2 * x1 * x1 - 2 * x1 * x2 + 3 * x2 * x2 + x1 - 3 * x2;
            richTextBox2.Text += "Точка минимума:\t" + "x1= " + x1.ToString() + "\tx2=" + x2.ToString() + "\n";
            richTextBox2.Text += "Значение функции в точке min: f(x1,x2) = " + f2.ToString() + "\n";
            richTextBox2.Text += "Количество базовых шагов, проделанных методом: " + (len2/2).ToString();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
          
            try
            {
                pogr = double.Parse(eps.Text); 
                x[0] = double.Parse(x1.Text);
                x[1] = double.Parse(x2.Text);

                if (pogr > 0.1)
                {
                    MessageBox.Show("Критерий остановки не должен превышать 0.1");
                    eps.Text = "0,00001";
                    clear();
                    return;
                }
                
            }
            catch (Exception)
            {
                MessageBox.Show("Неверные входные данные. Повторите снова.");
                clear();                
                return;
            }

            nsm(pogr, x, 0, 0, 0);
            text();
            x[0] = double.Parse(x1.Text);
            x[1] = double.Parse(x2.Text);
            cps(pogr, x, 1, 0, 0);
            text1();
            x1.Enabled = false;
            x2.Enabled = false;
            eps.Enabled=false;
        }

        public void tochka (double xxx1,double xxx2,int key)         
        {
            if (key==0)
            {
                point[len].X = (float)((xxx1*ma + x_mid[0]));
                point[len].Y = (float)((xxx2*ma + x_mid[1]));
                len++;
            }

            if (key == 1)
            {
                point2[len2].X = (float)((xxx1*ma + x_mid[0]));
                point2[len2].Y = (float)((xxx2*ma + x_mid[1]));
                len2++;   
            }

            if (key == 2)
            {
                point3[len3].X = (float)((xxx1*ma + x_mid[0]*1.5));
                point3[len3].Y = (float)((xxx2*ma + x_mid[1]*1.5));
                len3++;
            }
        }

        void grad(double[] d, double[] x)
        {
            d[0] = (-1) * (4 * x[0] - 2 * x[1] + 1);
            d[1] = (-1) * (-2 * x[0] + 9 * x[1] - 3);
        }

        public double [] nsm(double pogr, double[] x,int cps,int shtraf, double mu)
        {
            int i;
            double epselum = 5.0, e_gold = pogr / razmer, j=0;
            pogr = double.Parse(eps.Text);
            double[] y = new double[razmer];
            double[] d = new double[razmer];

            while (pogr < epselum)
            {
                for (i = 0; i < razmer; i++)
                    y[i] = x[i];                                   
                
                tochka(x[0],x[1],cps);
            
                //predvaritelnii shag CPS
                grad(d, y);
                j = qwerty.nach(x, d, e_gold,shtraf,mu,tb1,tb2);
                for (i = 0; i < razmer; i++)
                  x[i] = x[i] + j * d[i];
                tochka(x[0], x[1], cps);
                
                epselum = Math.Sqrt(qwerty.proverka(x, y));
                }
            return x;
        }

        public double[] cps(double pogr, double[] x, int cps, int shtraf, double mu)
        {
            int i;
            double epselum = 5.0, e_gold = pogr / razmer, j = 0;
            pogr = double.Parse(eps.Text);
            double[] y = new double[razmer];
            double[][] d = new double[2][];
            for (int q = 0; q < 2; q++) d[q] = new double[2];
            d[0][0] = 1; d[0][1] = 0; d[1][0] = 0; d[1][1] = 1;
            while (pogr < epselum)
            {
                for (i = 0; i < razmer; i++)
                    y[i] = x[i];
                tochka(x[0], x[1], cps);
                //predvaritelnii shag CPS
                for (i = 0; i < razmer; i++)
                {
                    j = qwerty.nach(x, d[i], e_gold, shtraf, mu, tb1, tb2);
                    x[i] = x[i] + j;
                    tochka(x[0], x[1], cps);
                }
                epselum = Math.Sqrt(qwerty.proverka(x, y));
            }
            return x;
        }

       
        private void button2_Click(object sender, EventArgs e)
        {
            clear();
        }

   
        private void button3_Click(object sender, EventArgs e)
        {
            double[] re = new double[2];
            double[] ru = new double[2];           
            try
            {
                re[0] = double.Parse(textBox7.Text);
                re[1] = double.Parse(textBox8.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Неверные входные данные.");
                textBox7.Text="5";
                textBox8.Text = "5";
                return;
            }
            qwerty.alfa(re);
            if (Math.Abs(re[0]) <= 1 && Math.Abs(re[1]) <= 1) re[0] = 2;
            //if (re[0] == 1 && re[1] == 1) { point3[0].X = 1 * ma + (float)(x_mid[0] * 1.5); point3[0].Y = 1 * ma + (float)(x_mid[0] * 1.5); point3[1].X = 1 / 2 * ma + (float)(x_mid[0] * 1.5); point3[1].Y = 1 * ma + (float)(x_mid[0] * 1.5); len3 += 2; }
            //if (re[0] == 0 && re[1] == 0) { point3[0].X = 0 * ma + (float)(x_mid[0] * 1.5); point3[0].Y = 0 * ma + (float)(x_mid[0] * 1.5); point3[1].X = 1 / 2 * ma + (float)(x_mid[0] * 1.5); point3[1].Y = 1*ma + (float)(x_mid[0] * 1.5); len3 += 2; }
            while (Math.Abs(qwerty.alfa(re)) > pogr)
            {       
                pogr = double.Parse(eps.Text);
                ru = cps(pogr, re, 2,1,mu);
                re = ru;
                if (Math.Abs(qwerty.alfa(re)) < pogr) break;
                mu = mu * 10;
                pogr = double.Parse(eps.Text);
            }
            double fvr;
            fvr=qwerty.function(ru,ru,0,1,0,ru,ru);
            richTextBox3.Text = "Точка минимумума\nс учетом допустимой области:\n";
            richTextBox3.Text+="x1=" +ru[0].ToString() + "\n"+"x2=" + ru[1].ToString()+"\n\nf(x1,x2)= "+fvr.ToString ();
            textBox7.Enabled = false;
            textBox8.Enabled = false;
            button3.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            len3 = 0;
            mu = 0.1;
            textBox7.Enabled = true;
            textBox8.Enabled = true;
            button3.Enabled = true;
            richTextBox3.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            eps.Text = "0,00001";
            x1.Text = "2";
            x2.Text = "2";
        }


    }
}
                          

