using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SumaContaMT
{
    public partial class Form1 : Form
    {
        int minimo;
        int maximo;
        double veces;
        double media_des;
        double[] suma_des;
        double des_max;
        public class Resultado
        {
            public int dia;
            public long sanos;
            public long infectados;
            public long importados;
            public long curados;
            public long desinmunizados;
            public long muertos;
            public double recorrido_individuo_activo;
            public double contactos_de_riesgo;
            public double media_dias_infectado;
            public double R0;
            public long contagios_cercania;
            public long contagios_clusters;
            public long infectados_total_clusters;
            public long curados_total_clusters;
            public long muertos_total_clusters;
            public long enfermos;
            public long hospitalizados;
            public long infectados_ambientales;
            public int series;

            public Resultado(
                int dia,
                long sanos,
                long infectados,
                long importados,
                long curados,
                long desinmunizados,
                long muertos,
                double recorrido_individuo_activo,
                double contactos_de_riesgo,
                double media_dias_infectado,
                double R0,
                long contagios_cercania,
                long contagios_clusters,
                long infectados_total_clusters,
                long curados_total_clusters,
                long muertos_total_clusters,
                long enfermos,
                long hospitalizados,
                long infectados_ambientales,
                int series)
            {
                this.dia = dia;
                this.sanos = sanos;
                this.infectados = infectados;
                this.importados = importados;
                this.curados = curados;
                this.desinmunizados = desinmunizados;
                this.muertos = muertos;
                this.recorrido_individuo_activo = recorrido_individuo_activo;
                this.contactos_de_riesgo = contactos_de_riesgo;
                this.media_dias_infectado = media_dias_infectado;
                this.R0 = R0;
                this.contagios_cercania = contagios_cercania;
                this.contagios_clusters = contagios_clusters;
                this.infectados_total_clusters = infectados_total_clusters;
                this.curados_total_clusters = curados_total_clusters;
                this.muertos_total_clusters = muertos_total_clusters;
                this.enfermos = enfermos;
                this.hospitalizados = hospitalizados;
                this.infectados_ambientales = infectados_ambientales;
                this.series = series;
            }
        }
        bool cancelar;
        public Form1()
        {
            InitializeComponent();
        }
        private void Borra_fuentes_Click(object sender, EventArgs e)
        {
            lista.Items.Clear();
            cuenta.Text = lista.Items.Count.ToString();
        }
        private void Sel_fuente_Click(object sender, EventArgs e)
        {
            OpenFileDialog leefichero = new OpenFileDialog
            {
                Filter = "LOG (*.log)|*.log|All files (*.*)|*.*",
                CheckFileExists = true,
                Multiselect = true
            };
            if (leefichero.ShowDialog() == DialogResult.OK)
            {
                lista.Items.Clear();
                foreach (string fichero in leefichero.FileNames)
                {
                    lista.Items.Add(fichero);
                }
                cuenta.Text = lista.Items.Count.ToString();
            }
        }
        private void Add_fuentes_Click(object sender, EventArgs e)
        {
            OpenFileDialog leefichero = new OpenFileDialog
            {
                Filter = "LOG (*.log)|*.log|All files (*.*)|*.*",
                CheckFileExists = true,
                Multiselect = true
            };
            if (leefichero.ShowDialog() == DialogResult.OK)
            {
                foreach (string fichero in leefichero.FileNames)
                {
                    lista.Items.Add(fichero);
                }
                cuenta.Text = lista.Items.Count.ToString();
            }
        }
        private void Sel_salida_Click(object sender, EventArgs e)
        {
            SaveFileDialog ficheroescritura = new SaveFileDialog()
            {
                Filter = "TXT (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 1
            };
            ficheroescritura.RestoreDirectory = ficheroescritura.OverwritePrompt = false;
            ficheroescritura.CheckPathExists = true;
            if (ficheroescritura.ShowDialog() == DialogResult.OK)
            {
                senda_salida.Text = ficheroescritura.FileName;
            }
        }
        private void B_ejecuta_Click(object sender, EventArgs e)
        {
            int fuentes = lista.Items.Count;
            if (fuentes == 0)
            {
                MessageBox.Show("No hay fuentes", "Sumar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (senda_salida.Text.Trim().Length == 0)
            {
                MessageBox.Show("No hay fichero de salida", "Sumar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            minimo = Convert.ToInt32(dias_min.Text);
            maximo = Convert.ToInt32(dias_max.Text);
            if (maximo == 0) maximo = 9999;
            if (maximo < minimo)
            {
                MessageBox.Show("El máximo debe ser igual o mayor al mínimo", "Sumar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            veces = Convert.ToDouble(des_maxima.Text);
            cancelar = false;
            string fichero;
            List<Resultado>[] resultados = new List<Resultado>[fuentes];
            int n = 0;
            for (int hilo = 0; hilo < fuentes; hilo++)
            {
                fichero = lista.Items[hilo].ToString();
                resultados[n] = ImportaResultado(fichero);
                if (cancelar) break;
                if (resultados[n] != null && resultados[n].Count >= minimo && resultados[n].Count <= maximo)
                {
                    n++;
                }
            }
            if (cancelar)
            {
                MessageBox.Show("Proceso cancelado", "Sumar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                List<Resultado> media = Media(resultados);
                int ns;
                if (veces > 0)
                {
                    List<Resultado>[] depurados = Elimina(resultados, media, veces);
                    media = Media(depurados);
                    ns = 0;
                    for (int i = 0; i < depurados.Length; i++)
                    {
                        if (depurados[i] != null && depurados[i].Count > 0)
                        {
                            ns++;
                        }
                    }
                }
                else
                {
                    ns = 0;
                    for (int i = 0; i < resultados.Length; i++)
                    {
                        if (resultados[i] != null && resultados[i].Count > 0)
                        {
                            ns++;
                        }
                    }
                }
                SalidaValoresMedios(senda_salida.Text, media, resultados);
                MessageBox.Show(string.Format("Sumados {0} ficheros", ns), "Sumar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private List<Resultado>[] Elimina(List<Resultado>[] resultados, List<Resultado> media, double veces)
        {
            List<Resultado>[] nuevos = new List<Resultado>[resultados.Length];
            double des;
            media_des = 0;
            suma_des = new double[resultados.Length];
            Resultado r;
            Resultado ra;
            for (int i = 0; i < resultados.Length; i++)
            {
                suma_des[i] = 0;
                for (int j = 0; j < resultados[i].Count; j++)
                {
                    r = resultados[i].ElementAt(j);
                    ra = media.ElementAt(j);
                    des = r.infectados - ra.infectados;
                    suma_des[i] += des * des;

                }
                suma_des[i] = Math.Sqrt(suma_des[i]) / resultados[i].Count;
                media_des += suma_des[i];
            }
            media_des /= resultados.Length;
            des_max = media_des * veces;
            int n = 0;
            for (int i = 0; i < resultados.Length; i++)
            {
                if (suma_des[i] < des_max)
                {
                    nuevos[n++] = resultados[i];
                }
            }

            // Borra el resto

            for (int i = n; i < resultados.Length; i++)
            {
                nuevos[i] = null;
            }
            return nuevos;
        }
        private List<Resultado> ImportaResultado(string fichero)
        {
            string s;
            string[] sd;
            FileStream fr;
            StreamReader sr;
            if (string.IsNullOrEmpty(fichero) || !File.Exists(fichero))
            {
                MessageBox.Show("No se encuentra el fichero " + fichero, "Importar resultados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cancelar = true;
                return null;
            }
            try
            {
                fr = new FileStream(fichero, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                sr = new StreamReader(fr);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error al leer el fichero " + fichero + Environment.NewLine + e.Message, "Importar resultados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cancelar = true;
                return null;
            }
            List<Resultado> resultado = new List<Resultado>();
            int cuenta_lineas = 0;
            int primera_linea = -1;
            int series = 0;
            while (true && !sr.EndOfStream)
            {
                s = sr.ReadLine();
                cuenta_lineas++;
                if (s.Equals("Valores al final del día"))
                {
                    series++;
                    primera_linea = cuenta_lineas;
                }
            }
            if (primera_linea == -1)
            {
                return null;
            }
            sr.Close();
            fr = new FileStream(fichero, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            sr = new StreamReader(fr);

            // Saltar líneas

            for (int i = 0; i < primera_linea; i++)
            {
                sr.ReadLine();
            }
            sr.ReadLine();
            if (sr.EndOfStream)
            {
                return null;
            }
            sr.ReadLine();
            if (sr.EndOfStream)
            {
                return null;
            }
            sr.ReadLine();
            int dia;
            long sanos;
            long infectados;
            long importados;
            long curados;
            long desinmunizados;
            long muertos;
            double recorrido_individuo_activo;
            double contactos_de_riesgo;
            double media_dias_infectado;
            double R0;
            long contagios_cercania;
            long contagios_clusters;
            long infectados_total_clusters;
            long curados_total_clusters;
            long muertos_total_clusters;
            long enfermos;
            long hospitalizados;
            long infectados_ambientales;
            int cuantos;
            while (true && !sr.EndOfStream)
            {
                s = sr.ReadLine().Trim();
                if (s.Length == 0) break;
                if (s.StartsWith("--- ---------- ")) break;
                sd = s.Split(' ');
                cuantos = 0;
                for (int i = 0; i < sd.Length; i++)
                {
                    if (sd[i].Trim().Length > 0)
                    {
                        if (cuantos != i) sd[cuantos] = sd[i];
                        cuantos++;
                    }
                }
                if (cuantos != 19)
                {
                    MessageBox.Show(string.Format("Número incorrecto de campos: {0} {1}", cuantos, s), "Importar resultados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                dia = Convert.ToInt32(sd[0]);
                sanos = Convert.ToInt64(sd[1]);
                infectados = Convert.ToInt64(sd[2]);
                importados = Convert.ToInt64(sd[3]);
                curados = Convert.ToInt64(sd[4]);
                desinmunizados = Convert.ToInt64(sd[5]);
                muertos = Convert.ToInt64(sd[6]);
                recorrido_individuo_activo = Convert.ToDouble(sd[7]);
                contactos_de_riesgo = Convert.ToDouble(sd[8]);
                media_dias_infectado = Convert.ToDouble(sd[9]);
                R0 = Convert.ToDouble(sd[10]);
                contagios_cercania = Convert.ToInt64(sd[11]);
                contagios_clusters = Convert.ToInt64(sd[12]);
                infectados_total_clusters = Convert.ToInt64(sd[13]);
                curados_total_clusters = Convert.ToInt64(sd[14]);
                muertos_total_clusters = Convert.ToInt64(sd[15]);
                enfermos = Convert.ToInt64(sd[16]);
                hospitalizados = Convert.ToInt64(sd[17]);
                infectados_ambientales = Convert.ToInt64(sd[18]);
                resultado.Add(new Resultado(dia, sanos, infectados, importados, curados, desinmunizados, muertos, recorrido_individuo_activo, contactos_de_riesgo, media_dias_infectado, R0, contagios_cercania, contagios_clusters, infectados_total_clusters, curados_total_clusters, muertos_total_clusters, enfermos, hospitalizados, infectados_ambientales, series));
            }
            return resultado;
        }
        private List<Resultado> Media(List<Resultado>[] resultados)
        {
            if (resultados == null || resultados[0] == null) return null;
            int datos;
            long sanos;
            long infectados;
            long importados;
            long curados;
            long desinmunizados;
            long muertos;
            double recorrido_individuo_activo;
            double contactos_de_riesgo;
            double media_dias_infectado;
            double R0;
            long contagios_cercania;
            long contagios_clusters;
            long infectados_total_clusters;
            long curados_total_clusters;
            long muertos_total_clusters;
            long enfermos;
            long hospitalizados;
            long infectados_ambientales;
            Resultado r;
            List<Resultado> medio = new List<Resultado>();
            int max_dias = resultados[0].Count;
            for (int i = 0; i < resultados.Length; i++)
            {
                if (resultados[i] != null && max_dias < resultados[i].Count)
                {
                    max_dias = resultados[i].Count;
                }
            }
            for (int i = 0; i < max_dias; i++)
            {
                datos = 0;
                sanos = 0;
                infectados = 0;
                importados = 0;
                curados = 0;
                desinmunizados = 0;
                muertos = 0;
                recorrido_individuo_activo = 0;
                contactos_de_riesgo = 0;
                media_dias_infectado = 0;
                R0 = 0;
                contagios_cercania = 0;
                contagios_clusters = 0;
                infectados_total_clusters = 0;
                curados_total_clusters = 0;
                muertos_total_clusters = 0;
                enfermos = 0;
                hospitalizados = 0;
                infectados_ambientales = 0;
                for (int n = 0; n < resultados.Length; n++)
                {
                    if (resultados[n] == null || resultados[n].Count <= i) continue;
                    r = resultados[n].ElementAt(i);
                    datos++;
                    sanos += r.sanos;
                    infectados += r.infectados;
                    importados += r.importados;
                    curados += r.curados;
                    desinmunizados += r.desinmunizados;
                    muertos += r.muertos;
                    recorrido_individuo_activo += r.recorrido_individuo_activo;
                    contactos_de_riesgo += r.contactos_de_riesgo;
                    media_dias_infectado += r.media_dias_infectado;
                    R0 += r.R0;
                    contagios_cercania += r.contagios_cercania;
                    contagios_clusters += r.contagios_clusters;
                    infectados_total_clusters += r.infectados_total_clusters;
                    curados_total_clusters += r.curados_total_clusters;
                    muertos_total_clusters += r.muertos_total_clusters;
                    enfermos += r.enfermos;
                    hospitalizados += r.hospitalizados;
                    infectados_ambientales += r.infectados_ambientales;
                }
                if (datos == 0)
                {
                    datos = 1;
                }
                medio.Add(new Resultado(
                    datos,
                    (long)(sanos / datos),
                    (long)(infectados / datos),
                    (long)(importados / datos),
                    (long)(curados / datos),
                    (long)(desinmunizados / datos),
                    (long)(muertos / datos),
                    (double)(recorrido_individuo_activo / datos),
                    (double)(contactos_de_riesgo / datos),
                    (double)(media_dias_infectado / datos),
                    (double)(R0 / datos),
                    (long)(contagios_cercania / datos),
                    (long)(contagios_clusters / datos),
                    (long)(infectados_total_clusters / datos),
                    (long)(curados_total_clusters / datos),
                    (long)(muertos_total_clusters / datos),
                    (long)(enfermos / datos),
                    (long)(hospitalizados / datos),
                    (long)(infectados_ambientales / datos),
                    0)
                );
            }
            return medio;
        }
        private void SalidaValoresMedios(string fichero, List<Resultado> media, List<Resultado>[] resultados)
        {
            int res = resultados.Length;
            FileStream fw = new FileStream(fichero, FileMode.Append, FileAccess.Write, FileShare.Read);
            StreamWriter sw = new StreamWriter(fw);
            sw.WriteLine("Fichero                                                      Desv.Media Ser");
            sw.WriteLine("------------------------------------------------------------ ---------- --- --");
            for (int n = 0; n < res; n++)
            {
                sw.WriteLine(string.Format("{0,-60} {1,10:f2} {2,3:N0} {3}", lista.Items[n].ToString(), suma_des[n], resultados[n].ElementAt(0).series, suma_des[n] < des_max ? "ok" : ""));
            }
            sw.WriteLine("------------------------------------------------------------ ---------- --- --");
            sw.WriteLine(string.Format("                                          {0,2:N0} x {1,10:f2}  = {2,10:f2}", (int)veces, media_des, des_max));
            sw.WriteLine("------------------------------------------------------------ ---------- --- --");
            sw.WriteLine();
            sw.WriteLine("                                                                       Recorrido  Contactos Media días            ------ Afectados ---- ----------  Clusters -----------");
            sw.WriteLine("Día     Sanos  Infectados Importados    Curados Desinmuniz    Muertos     Ida     críticos  infectados         R0   Cercania   Clusters Infectados    Curados    Muertos   Enfermos Hospitaliz  Ambiental");
            sw.WriteLine("--- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------");
            Resultado r;
            for (int n = 0; n < media.Count; n++)
            {
                r = media.ElementAt(n);
                sw.WriteLine(string.Format(
                    "{0,3} {1,10} {2,10} {3,10} {4,10} {5,10} {6,10} {7,10:f2} {8,10:f2} {9,10:f1} {10,10:f2} {11,10} {12,10} {13,10} {14,10} {15,10} {16,10} {17,10} {18,10}",
                    n + 1,
                    r.sanos,
                    r.infectados,
                    r.importados,
                    r.curados,
                    r.desinmunizados,
                    r.muertos,
                    r.recorrido_individuo_activo,
                    r.contactos_de_riesgo,
                    r.media_dias_infectado,
                    r.R0,
                    r.contagios_cercania,
                    r.contagios_clusters,
                    r.infectados_total_clusters,
                    r.curados_total_clusters,
                    r.muertos_total_clusters,
                    r.enfermos,
                    r.hospitalizados,
                    r.infectados_ambientales));
            }
            sw.WriteLine("--- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------");
            sw.WriteLine("Día     Sanos  Infectados Importados    Curados Desinmuniz    Muertos     Ida     críticos  infectados         R0   Cercania   Clusters Infectados    Curados    Muertos   Enfermos Hospitaliz  Ambiental");
            sw.WriteLine();
            sw.Close();
        }
    }
}
