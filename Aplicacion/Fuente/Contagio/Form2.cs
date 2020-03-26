using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Contagio
{
    public partial class Form2 : Form
    {
        public Form1 principal;
        public double radio;
        private Bitmap fuente;
        private const int ancho_lienzo = 890;
        private const int alto_lienzo = 890;
        private const int izq_lienzo = 4;
        private const int sup_lienzo = 50;
        private const int margen_dcha = 22;
        private const int margen_sup = 51;
        private double escala = 1.0;
        private int dia;
        private readonly Font letra = new Font("Arial", 10);
        public bool cancelar;
        private class Par
        {
            public string grupo;
            public int indice;
            public Par(string grupo, int indice)
            {
                this.grupo = grupo;
                this.indice = indice;
            }
        }
        public Form2()
        {
            InitializeComponent();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            Text = string.Format("{0} [{1}]", principal.TITULO, principal.PREFIJO);
            BotonCancelar(true);
            lienzo.SetBounds(izq_lienzo, sup_lienzo, ancho_lienzo, alto_lienzo);
            SetBounds(Screen.PrimaryScreen.WorkingArea.Width - (izq_lienzo + ancho_lienzo + margen_dcha), 0, izq_lienzo + ancho_lienzo + margen_dcha, alto_lienzo + margen_sup + sup_lienzo);
            Grafico(false);
        }
        public void ActualizaTitulo()
        {
            Text = string.Format("{0} [{1}]", principal.TITULO, principal.PREFIJO);
        }
        public void ActualizaDia(int dia)
        {
            this.dia = dia;
            dia_actual.Text = string.Format("Día : {0}", dia);
        }
        private void Lienzo_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.Black), 0, 0, lienzo.Width, lienzo.Height);
            if (fuente != null) g.DrawImage(fuente, 0, 0, lienzo.Width, lienzo.Height);
        }
        public void Grafico(bool solo_puntos)
        {
            escala = (ancho_lienzo - 4) / (radio + radio);
            fuente = new Bitmap(ancho_lienzo, alto_lienzo);
            int i_x;
            int i_y;
            for (i_x = 0; i_x < ancho_lienzo; i_x++)
            {
                for (i_y = 0; i_y < alto_lienzo; i_y++)
                {
                    fuente.SetPixel(i_x, i_y, Color.Black);
                }
            }
            List<Par> nombre = new List<Par>();
            int[] testigo = null;
            if (principal.mostrar_recorridos.Checked && !solo_puntos)
            {
                testigo = new int[principal.grupos.Count];
                for (int i = 0; i < principal.grupos.Count; i++)
                {
                    testigo[i] = -1;
                    nombre.Add(new Par(principal.grupos.ElementAt(i).grupo, i));
                }
            }
            Par par;
            int i_grupo = 0;
            Color estado;
            int n = 0;
            foreach (Form1.Individuo indi in principal.individuos)
            {
                if (principal.mostrar_recorridos.Checked && testigo != null && !solo_puntos)
                {
                    par = nombre.Find(x => x.grupo == indi.grupo);
                    i_grupo = par.indice;
                }
                i_x = (int)((indi.x + radio) * escala);
                i_y = (int)((indi.y + radio) * escala);
                switch (indi.estado)
                {
                    case -1:
                        // Muerto

                        estado = Color.Red;
                        break;
                    case 0:
                        // Sano

                        estado = Color.Gray;
                        if (principal.mostrar_recorridos.Checked && testigo != null && testigo[i_grupo] == -1 && !solo_puntos)
                        {
                            testigo[i_grupo] = n;
                        }
                        break;
                    case 1:
                        // Infectado

                        estado = Color.Yellow;
                        break;
                    case 2:
                        // Curado

                        estado = Color.Green;
                        break;
                    default:
                        // Desinmunizado

                        estado = Color.Purple;
                        break;
                }
                if (i_x >= 0 && i_x < ancho_lienzo - 1 && i_y >= 0 && i_y < alto_lienzo - 1)
                {
                    fuente.SetPixel(i_x, i_y, estado);
                    if (principal.puntos_gordos.Checked)
                    {
                        fuente.SetPixel(i_x + 1, i_y, estado);
                        fuente.SetPixel(i_x, i_y + 1, estado);
                        fuente.SetPixel(i_x + 1, i_y + 1, estado);
                    }
                }
                n++;
            }
            Graphics g = Graphics.FromImage(fuente);

            // Día en curso

            g.DrawString(string.Format("{0}", dia), letra, Brushes.White, new Point(10, 4));

            // Referencias de las distancias de contacto y longitud de los pasos

            int dc = (int)(principal.CONTACTO * escala);
            int dp = (int)(principal.LON_10PASOS * escala);
            int dm = dc > dp ? dc : dp;
            g.DrawEllipse(Pens.Fuchsia, 70 + 4, 4 + (dm - dc) / 2, dc, dc);
            g.DrawEllipse(Pens.LightBlue, 70 + 8 + dc, 4 + (dm - dp) / 2, dp, dp);

            // Número de individuos segun su estados

            g.DrawString(string.Format("{0}", principal.infectados), letra, Brushes.Yellow, new Point(lienzo.Width - 100, 0));
            g.DrawString(string.Format("{0}", principal.curados), letra, Brushes.Green, new Point(10, lienzo.Height - 40));
            g.DrawString(string.Format("{0}", principal.desinmunizados), letra, Brushes.Purple, new Point(10, lienzo.Height - 20));
            g.DrawString(string.Format("{0}", principal.muertos), letra, Brushes.Red, new Point(lienzo.Width - 100, lienzo.Height - 20));

            // Recorridos ilustrativos

            if (principal.mostrar_recorridos.Checked && testigo != null && !solo_puntos)
            {
                int x1;
                int y1;
                int x2;
                int y2;
                for (int ti = 0; ti < testigo.Length; ti++)
                {
                    if (testigo[ti] != -1)
                    {
                        Form1.Individuo indi = principal.individuos.ElementAt(testigo[ti]);
                        if (indi.pasos_dia > 0 && indi.pasos != null)
                        {
                            for (int i = 1; i < indi.pasos_dia; i++)
                            {
                                x1 = (int)((indi.pasos[i - 1, 0] + radio) * escala);
                                y1 = (int)((indi.pasos[i - 1, 1] + radio) * escala);
                                x2 = (int)((indi.pasos[i, 0] + radio) * escala);
                                y2 = (int)((indi.pasos[i, 1] + radio) * escala);
                                if (x1 >= 0 && x1 < ancho_lienzo - 1 && y1 >= 0 && y1 < alto_lienzo - 1 && x2 >= 0 && x2 < ancho_lienzo - 1 && y2 >= 0 && y2 < alto_lienzo - 1)
                                {
                                    g.DrawLine(Pens.LightBlue, x1, y1, x2, y2);
                                }
                            }
                            x1 = (int)((indi.pasos[0, 0] + radio) * escala) - 2;
                            if (x1 < 0) x1 = 0;
                            y1 = (int)((indi.pasos[0, 1] + radio) * escala) - 2;
                            if (y1 < 0) y1 = 0;
                            g.DrawEllipse(Pens.White, x1, y1, 4, 4);
                        }
                    }
                }
            }
            if (principal.salvar_imagenes.Checked)
            {
                string carpeta = string.Format(@"{0}:\{1}\{2}", principal.U_SALIDAS, principal.CARPETA_SALIDAS, principal.PREFIJO);
                if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
                fuente.Save(string.Format(@"{0}\gr{1:D3}.png", carpeta, dia), System.Drawing.Imaging.ImageFormat.Png);
            }
            lienzo.Refresh();
        }
        private void B_cancelar_Click(object sender, EventArgs e)
        {
            BotonCancelar(false);
        }
        public void BotonCancelar(bool que)
        {
            cancelar = !que;
            b_cancelar.Enabled = que;
            principal.BotonCancelar(que);
        }
    }
}
