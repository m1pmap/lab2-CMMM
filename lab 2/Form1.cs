using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Antlr.Runtime;
using NCalc;

namespace lab_2
{
    public partial class Form1 : Form
    {
        public double a;
        public double b;
        public double eps = 0.0001;
        public double answer;

        string myFunction;

        public double f(double x, string function)
        {
            Expression y = new Expression(function);
            y.Parameters["x"] = x;
            object res = y.Evaluate();
            return Convert.ToDouble(res);
        }

        static double derivative(double x, string function, double h)
        {
            Expression y = new Expression(function);
            y.Parameters["x"] = x;

            double fx = (double)y.Evaluate();

            y.Parameters["x"] = x - h;
            double fxMinusH = (double)y.Evaluate();

            return (fx - fxMinusH) / h;
        }

        public double MH(double a, double b, double eps)
        {
            double x0;
            double x1 = a;
            double x2 = b;
            int i = 1;
            answer = 0;

            try
            {
                while (a < b)
                {
                    chart1.Series[0].Points.AddXY(a, f(a, myFunction));
                    a += 0.1;
                }

                while (Math.Abs(x2 - x1) > eps)
                {
                    x0 = x1;
                    x1 = x2;
                    x2 = x1 - ((x0 - x1) * f(x1, myFunction)) / (f(x0, myFunction) - f(x1, myFunction));

                    chart1.Series.Add("Хорда " + i.ToString());
                    chart1.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chart1.Series[i].Points.AddXY(b, f(b, myFunction));
                    chart1.Series[i].Points.AddXY(x2, f(x2, myFunction));
                    chart1.Series[i].BorderWidth = 3;
                    i++;
                }

                answer = x2;
            }
            catch
            {
                MessageBox.Show("Функция введена неверно");
            }
            return answer;
        }

        public double MN(double a, double b, double eps)
        {
            double x1 = a;
            double x0 = a + 2 * eps;
            double x = a;
            int i = 1;
            answer = 0;


            try
            {
                while (a < b)
                {
                    chart1.Series[0].Points.AddXY(a, f(a, myFunction));
                    a += 0.1;
                }

                while (Math.Abs(x0 - x1) > eps)
                {
                    chart1.Series.Add("Касательная " + i.ToString());
                    chart1.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chart1.Series[i].Points.AddXY(x1, f(x1, myFunction));

                    x0 = x1;
                    x1 = x0 - (f(x0, myFunction) / derivative(x0, myFunction, eps));

                    chart1.Series[i].Points.AddXY(x1, 0);
                    chart1.Series[i].BorderWidth = 3;
                    i++;
                }
                answer = x1;
            }
            catch
            {
                MessageBox.Show("Функция введена неверно!");
            }
            return answer;
        }

        public double MDSH(double a, double b, double eps)
        {
            double c;
            double x1 = a;
            double x2 = b;
            int i = 1;
            answer = 0;

            try
            {
                while (a < b)
                {
                    chart1.Series[0].Points.AddXY(a, f(a, myFunction));
                    a += 0.1;
                }

                while (Math.Abs(x2 - x1) > eps)
                {
                    c = (x1 + x2) / 2.0;
                    if (f(x1, myFunction) * f(c, myFunction) > 0)
                        x1 = c;
                    else
                        x2 = c;

                    chart1.Series.Add(i.ToString());
                    chart1.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chart1.Series[i].Points.AddXY(c, f(c, myFunction));
                    chart1.Series[i].Points.AddXY(c, 0);
                    chart1.Series[i].BorderDashStyle = ChartDashStyle.Dash;
                    i++;

                    chart1.Series.Add(i.ToString());
                    chart1.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chart1.Series[i].Points.AddXY(c, f(c, myFunction));
                    chart1.Series[i].Points.AddXY(0, f(c, myFunction));
                    chart1.Series[i].BorderDashStyle = ChartDashStyle.Dash;
                    i++;

                    chart1.Legends.Clear();
                }

                answer = (x1 + x2) / 2;
            }
            catch
            {
                MessageBox.Show("Функция введена неверно");
            }
            return answer;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = "Sqrt(1 - 0.4*Pow(x, 2)) - Asin(x)";
            textBox3.Text = "0";
            textBox4.Text = "1";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = "3*x - 4*Log(x, Exp(1)) - 5";
            textBox3.Text = "0,1";
            textBox4.Text = "2";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = "Exp(x) - Exp(-x) - 2";
            textBox3.Text = "-1";
            textBox4.Text = "2";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Update();
            chart1.Series.Clear();
            chart1.Series.Add("График функции");
            chart1.Series[0].BorderWidth = 5;
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            myFunction = textBox1.Text;
            a = Convert.ToDouble(textBox3.Text);
            b = Convert.ToDouble(textBox4.Text);

            if (radioButton1.Checked == true)
            {
                textBox2.Text = MH(a, b, eps).ToString();
            }
            if (radioButton2.Checked == true)
            {
                textBox2.Text = MN(a, b, eps).ToString();
            }
            if (radioButton3.Checked == true)
            {
                textBox2.Text = MDSH(a, b, eps).ToString();
            }
        }
    }
}
