using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimulaTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void B_calcula_Click(object sender, EventArgs e)
        {
            Calcular();
        }
        private void Calcular()
        {
            double pre = Convert.ToDouble(prevalencia.Text) / 100;
            double sen = Convert.ToDouble(sensibilidad.Text) / 100;
            double espe = Convert.ToDouble(especificidad.Text) / 100;
            long num_simulaciones = Convert.ToInt32(simulaciones.Text);
            long num_muestra = Convert.ToInt32(muestra.Text);
            double ancho_his = Convert.ToDouble(ancho.Text.Replace('.', ','));
            int n_histo = (int)(100.0 / ancho_his) + 1;
            long[] histo = new long[n_histo];
            int ih;
            int max_ih = -1;
            int min_ih = int.MaxValue;
            long pv_cm;
            long pf_cm;
            long nv_cm;
            long nf_cm;
            long pv_c = 0;
            long pf_c = 0;
            long nv_c = 0;
            long nf_c = 0;
            long tt = 0;
            pv.Text = string.Format("{0:N0}", pv_c);
            pf.Text = string.Format("{0:N0}", pf_c);
            nv.Text = string.Format("{0:N0}", nv_c);
            nf.Text = string.Format("{0:N0}", nf_c);
            positivos.Text = string.Format("{0:N0}", pv_c + pf_c);
            Random azar;
            azar = new Random(12345);
            bool pre_real;
            double pm;
            cancelar = false;
            b_cancela.Enabled = true;
            b_calcula.Enabled = prevalencia.Enabled = sensibilidad.Enabled = especificidad.Enabled = simulaciones.Enabled = muestra.Enabled = ancho.Enabled = false;
            for (long i = 0; i < num_simulaciones; i++)
            {
                pv_cm = 0;
                pf_cm = 0;
                nv_cm = 0;
                nf_cm = 0;
                for (long j = 0; j < num_muestra; j++)
                {
                    // Prevalencia real

                    pre_real = azar.NextDouble() < pre ? true : false;
                    if (pre_real)
                    {
                        // Sensibilidad

                        if (azar.NextDouble() < sen)
                        {
                            // Positivo verdadero

                            pv_cm++;
                        }
                        else
                        {
                            // Negativo falso (pre_real es true)

                            nf_cm++;
                        }
                    }
                    else
                    {
                        // Especificidad

                        if (azar.NextDouble() < espe)
                        {
                            nv_cm++;
                        }
                        else
                        {
                            pf_cm++;
                        }
                    }
                }
                pv_c += pv_cm;
                pf_c += pf_cm;
                nv_c += nv_cm;
                nf_c += nf_cm;
                pm = 100.0 * (pv_cm + pf_cm) / num_muestra;
                ih = (int)(pm / ancho_his);
                histo[ih]++;
                if (ih < min_ih) min_ih = ih;
                if (ih > max_ih) max_ih = ih;
                if (i % 100 == 0)
                {
                    contador.Text = string.Format("{0:N0}", i + 1);
                    pv.Text = string.Format("{0:f2}", 100.0 * pv_cm / num_muestra);
                    pf.Text = string.Format("{0:f2}", 100.0 * pf_cm / num_muestra);
                    nv.Text = string.Format("{0:f2}", 100.0 * nv_cm / num_muestra);
                    nf.Text = string.Format("{0:f2}", 100.0 * nf_cm / num_muestra);
                    positivos.Text = string.Format("{0:f2}", pm);
                    Grafico(histo, min_ih, max_ih, ancho_his, i);
                    Application.DoEvents();
                }
                tt += num_muestra;
                if (cancelar) break;
            }
            contador.Text = string.Format("{0:N0}", num_simulaciones);
            pv.Text = string.Format("{0:f2}", 100.0 * pv_c / tt);
            pf.Text = string.Format("{0:f2}", 100.0 * pf_c / tt);
            nv.Text = string.Format("{0:f2}", 100.0 * nv_c / tt);
            nf.Text = string.Format("{0:f2}", 100.0 * nf_c / tt);
            pm = 100.0 * (pv_c + pf_c) / tt;
            positivos.Text = string.Format("{0:f2}", pm);
            Application.DoEvents();
            if (!Directory.Exists(@"C:\Contagio")) Directory.CreateDirectory(@"C:\Contagio");
            string F_CASO = string.Format(@"C:\Contagio\Probailidad_test_{0}_{1}_{2}.csv", prevalencia.Text, sensibilidad.Text, especificidad.Text);
            FileStream fw = new FileStream(F_CASO, FileMode.Create, FileAccess.Write, FileShare.Read);
            StreamWriter sw = new StreamWriter(fw);
            long v_max = -1;
            max_ih++;
            for (int i = 0; i < max_ih; i++)
            {
                if (v_max < histo[i]) v_max = histo[i];
            }
            sw.WriteLine("{0:f3};", 100.0 * v_max / num_simulaciones);
            double x = 0;
            for (int i = 0; i < max_ih; i++)
            {
                sw.WriteLine("{0:f3};{1:f3}", x + ancho_his / 2, 100.0 * histo[i] / num_simulaciones);
                x += ancho_his;
            }
            sw.Close();
            Console.Beep();
            b_cancela.Enabled = false;
            b_calcula.Enabled = prevalencia.Enabled = sensibilidad.Enabled = especificidad.Enabled = simulaciones.Enabled = muestra.Enabled = ancho.Enabled = true;
            MessageBox.Show("Simulación terminada");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            grafico.Image = null;
            b_calcula.Focus();
        }
        private void Grafico(long[] histo, int min_ih, int max_ih, double ancho_his, long num_simulaciones)
        {
            Bitmap img_grafico = new Bitmap(grafico.Width, grafico.Height);
            Graphics g = Graphics.FromImage(img_grafico);
            Brush brocha = Brushes.White;
            g.FillRectangle(brocha, 0, 0, grafico.Width, grafico.Height);
            int nRE = histo.Length;
            double medio_ancho = ancho_his / 2;
            double[] x = new double[nRE];
            double[] y = new double[nRE];
            double xmin = min_ih * ancho_his + medio_ancho;
            double xmax = max_ih * ancho_his + medio_ancho;
            double ymin = double.MaxValue;
            double ymax = double.MinValue;
            int imy = -1;
            for (int i = min_ih; i < max_ih; i++)
            {
                x[i] = i * ancho_his + medio_ancho;
                y[i] = 100.0 * histo[i] / (num_simulaciones + 1);
                if (y[i] < ymin) ymin = y[i];
                if (y[i] > ymax)
                {
                    imy = i;
                    ymax = y[i];
                }
            }
            if (imy == -1) return;
            Brush azul = Brushes.Blue;
            int margen = 60;
            int mediomargen = margen / 2;
            double fx = (grafico.Width - margen) / (xmax - xmin);
            double fy;
            if (ymin == double.MaxValue || ymax == double.MinValue || ymax == ymin)
            {
                fy = 1.0d;
            }
            else
            {
                fy = (grafico.Height - margen) / (ymax - ymin);
            }
            int xi;
            int an = (int)(fx * ancho_his);
            int al;
            int yi;
            for (int i = min_ih; i < max_ih; i++)
            {
                xi = mediomargen + (int)(fx * (x[i] - xmin));
                al = (int)(fy * (y[i] - ymin));
                yi = grafico.Height - mediomargen - al;
                if (y[i] != -1) g.FillRectangle(azul, xi, yi, an, al);
            }
            Font fuente = new Font("Arial", 10);
            string s = string.Format("{0:f2}", xmin);
            g.DrawString(s, fuente, Brushes.Black, 0, grafico.Height - 20);
            s = string.Format("{0:f2}", x[imy]);
            g.DrawString(s, fuente, Brushes.Black, mediomargen + (int)(fx * (x[imy] - xmin)), grafico.Height - 20);
            s = string.Format("{0:f2}", xmax);
            g.DrawString(s, fuente, Brushes.Black, grafico.Width - 44, grafico.Height - 20);
            s = string.Format("{0:f2}", ymax);
            g.DrawString(s, fuente, Brushes.Black, 0, 0);
            grafico.Image = img_grafico;
            Application.DoEvents();
        }
        bool cancelar;
        private void B_cancela_Click(object sender, EventArgs e)
        {
            cancelar = true;
            b_cancela.Enabled = false;
            Application.DoEvents();
        }
    }
}
